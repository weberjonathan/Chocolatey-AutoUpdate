using CandyShop.Packages;
using CandyShop.Packages.Chocolatey;
using CandyShop.Packages.Winget;
using CandyShop.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CandyShop
{
    public class CandyShopApplicationContext : ApplicationContext
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private IPackageManager packageManager = new WingetManager(); // TODO allow choice

        public CandyShopApplicationContext()
        {
            // determine silent mode
            List<string> args = new List<string>(Environment.GetCommandLineArgs());
            LaunchInBackground = args.Find(s => s.Equals("--background") ||
                                                s.Equals("-b")) != null;

            // launch with form or in tray
            if (LaunchInBackground)
            {
                NotifyIcon icon = CreateAndShowNotifyIcon();
                GetOutdatedAndShowNotification(icon);
            }
            else
            {
                BuildForm().Show();
            }
        }

        public bool LaunchInBackground { get; }

        private NotifyIcon CreateAndShowNotifyIcon()
        {
            // create context menu
            ToolStripItem exitItem = new ToolStripMenuItem
            {
                Text = "Exit",
            };
            exitItem.Click += new EventHandler((sender, e) => { ExitThread(); });

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(exitItem);

            // Initialize Tray Icon
            NotifyIcon rtn = new NotifyIcon()
            {
                // Icon = Resources.AppIcon,
                Icon = Resources.IconNew,
                Visible = true,
                ContextMenuStrip = contextMenu
            };

            // make sure tray icon is removed on application exit
            Application.ApplicationExit += new EventHandler((sender, e) =>
            {
                if (rtn != null) rtn.Visible = false;
            });

            return rtn;
        }

        private async void GetOutdatedAndShowNotification(NotifyIcon icon)
        {
            List<IPackage> packages = null;

            // obtain outdated packages
            try
            {
                packages = await packageManager.GetOutdatedAsync();
            }
            catch (ChocolateyException)
            {
                icon.BalloonTipIcon = ToolTipIcon.Error;
                icon.Text = Application.ProductName;
                icon.BalloonTipTitle = $"Candy Shop";
                icon.BalloonTipText = Properties.strings.Err_CheckOutdated;
                icon.ShowBalloonTip(2000);
                Environment.Exit(0);
            }

            // show notification and create click handlers
            if (packages.Count > 0)
            {
                icon.BalloonTipClicked += new EventHandler((sender, e) =>
                {
                    BuildFormWith(packages).Show();
                });

                icon.MouseClick += new MouseEventHandler((sender, e) =>
                {
                    if (e.Button.Equals(MouseButtons.Left))
                    {
                        BuildFormWith(packages).Show();
                    }
                });

                icon.BalloonTipIcon = ToolTipIcon.Info;
                icon.Text = Application.ProductName;
                icon.BalloonTipTitle = $"{packages.Count} package{(packages.Count == 1 ? " is" : "s are")} outdated.";
                icon.BalloonTipText = $"To upgrade click here or the tray icon later.";
                icon.ShowBalloonTip(2000);
            }
            else
            {
                // terminate, bc app was launched silently but no packages are outdated and thus nothing is to be done
                Environment.Exit(0);
            }
        }

        private CandyShopForm BuildForm()
        {
            CandyShopForm rtn = new CandyShopForm();
            RegisterFormHandlers(rtn);
            return rtn;
        }

        private CandyShopForm BuildFormWith(List<IPackage> outdatedPackages)
        {
            CandyShopForm rtn = new CandyShopForm(packageManager, outdatedPackages);
            RegisterFormHandlers(rtn);
            return rtn;
        }

        private void RegisterFormHandlers(CandyShopForm form)
        {
            form.FormClosed += new FormClosedEventHandler((sender, e) =>
            {
                form.ShowInTaskbar = false;
                form.Hide();

                if (form.DialogResult.Equals(DialogResult.OK))
                {
                    LaunchUpgradeConsole(form.SelectedPackages);
                }
                else
                {
                    if (!LaunchInBackground)
                    {
                        ExitThread();
                    }
                }
            });
        }

        private void LaunchUpgradeConsole(List<IPackage> packages)
        {
            // setup watcher for desktop shortcuts
            Queue<string> shortcuts = new Queue<string>();
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.BeginInit();
                
                watcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                watcher.Filter = "*.lnk";
                watcher.IncludeSubdirectories = false;
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.InternalBufferSize = 65536; // TODO test; incurs performance penalty so remove if not useful
                watcher.EnableRaisingEvents = true;
                watcher.Created += new FileSystemEventHandler((sender, e) =>
                {
                    shortcuts.Enqueue(e.FullPath);
                });
                
                watcher.EndInit();

                // upgrade
                AllocConsole();
                Console.CursorVisible = false;

                try
                {
                    packageManager.Upgrade(packages);
                }
                catch (ChocolateyException e)
                {
                    MessageBox.Show(
                        $"An error occurred while executing Chocolatey: \"{e.Message}\"",
                        $"{Application.ProductName} Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    ExitThread();
                }
            }

            // display results
            IntPtr handle = GetConsoleWindow();
            if (!IntPtr.Zero.Equals(handle))
            {
                SetForegroundWindow(handle);
            }
            Console.CursorVisible = false;
            Console.Write("\nPress any key to continue... ");
            Console.ReadKey();

            // remove shortcuts
            if (shortcuts.Count > 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.Append($"During the upgrade process {shortcuts.Count} new desktop shortcut(s) were created:\n\n");
                foreach (string shortcut in shortcuts)
                {
                    msg.Append($"- {Path.GetFileNameWithoutExtension(shortcut)}\n");
                }
                msg.Append($"\nDo you want to delete all {shortcuts.Count} shortcut(s)?");

                DialogResult result = MessageBox.Show(
                    msg.ToString(),
                    Application.ProductName,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                if (result.Equals(DialogResult.Yes))
                {
                    while (shortcuts.Count > 0)
                    {
                        string shortcut = shortcuts.Dequeue();
                        try
                        {
                            File.Delete(shortcut);
                        }
                        catch (IOException)
                        {
                            // TODO
                        }
                    }
                }
            }

            // exit
            ExitThread();
        }
    }
}
