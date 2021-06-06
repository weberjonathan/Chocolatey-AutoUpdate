using CandyShop.Packages;
using CandyShop.Packages.Chocolatey;
using CandyShop.Packages.Winget;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace CandyShop
{
    public partial class CandyShopForm : Form
    {
        private const string TASKNAME = "CandyShopLaunch";
        private bool WindowsTaskExists; // TODO create Context or properties class that contains information like this

        private IPackageManager _PackageManager;

        public CandyShopForm()
        {
            _PackageManager = new WingetManager(); // TODO allow choice

            InitializeComponent();
            if (!(_PackageManager is ChocolateyProcess))
            {
                MenuEditSelectRelevant.Visible = false;
                MenuHelpMetaPackages.Visible = false;
            }


            GetInstalledAsync();
            GetOutdatedAsync();
        }

        public CandyShopForm(IPackageManager packageManager, List<IPackage> outdatedPackages)
        {
            InitializeComponent();
            GetInstalledAsync();
            this._PackageManager = packageManager;
            UpgradePage.OutdatedPackages = outdatedPackages;
            // TODO implement refresh that prompts on new outdated packages
        }

        public List<IPackage> SelectedPackages { get; set; }

        private void ChocoAutoUpdateForm_Load(object sender, EventArgs e)
        {
            // register handlers
            UpgradePage.UpgradeAllClick += UpgradePage_UpgradeAllClick;
            UpgradePage.UpgradeSelectedClick += UpgradePage_UpgradeSelectedClick;
            UpgradePage.CancelClick += UpgradePage_CancelClick;

            // display admin warning or not
            if (HasAdminPrivileges())
            {
                UpgradePage.ShowAdminWarning = false;
                this.Text = $"{Application.ProductName} v{Application.ProductVersion}";
            }
            else
            {
                UpgradePage.ShowAdminWarning = true;
                this.Text = $"{Application.ProductName} v{Application.ProductVersion} (no administrator privileges)";
            }

            // check task entry or not
            using (TaskService ts = new TaskService())
            {
                WindowsTaskExists = ts.GetTask(TASKNAME) != null;
            }
            MenuExtrasCreateTask.Checked = WindowsTaskExists;

            this.Activate();
        }

        private void MenuEditSelectAll_Click(object sender, EventArgs e)
        {
            UpgradePage.CheckAllItems();
        }

        private void MenuEditSelectRelevant_Click(object sender, EventArgs e)
        {
            UpgradePage.CheckNormalAndMetaItems();
        }

        private void MenuEditDeselectAll_Click(object sender, EventArgs e)
        {
            UpgradePage.UncheckAllItems();
        }

        private void MenuExtrasCreateTask_Click(object sender, EventArgs e)
        {
            if (!HasAdminPrivileges())
            {
                ShowErrorDialog(Properties.strings.Form_Err_RequireAdmin);
                return;
            }

            using (TaskService ts = new TaskService())
            {
                if (WindowsTaskExists)
                {
                    // remove task
                    Task task = ts.GetTask(TASKNAME);
                    if (task != null)
                    {
                        ts.RootFolder.DeleteTask(TASKNAME);
                    }

                    WindowsTaskExists = false;
                }
                else
                {
                    // create task
                    string executable = Process.GetCurrentProcess().MainModule.FileName;
                    string dir = Directory.GetParent(executable).FullName;

                    TaskDefinition definition = TaskService.Instance.NewTask();
                    definition.RegistrationInfo.Description = "Launch CandyShop with elevated privileges to display outdated packages on login.";
                    definition.Principal.LogonType = TaskLogonType.InteractiveToken;

                    LogonTrigger trigger = new LogonTrigger();

                    definition.Triggers.Add(trigger);
                    definition.Actions.Add(executable, "--background", dir);
                    definition.Principal.RunLevel = TaskRunLevel.Highest;

                    TaskService.Instance.RootFolder.RegisterTaskDefinition(TASKNAME, definition);

                    WindowsTaskExists = true;
                }
            }
        }

        private void MenuHelpGithub_Click(object sender, EventArgs e)
        {
            launchUrl("https://github.com/weberjonathan/CandyShop");
        }

        private void MenuHelpLicense_Click(object sender, EventArgs e)
        {
            using (LicenseForm form = new LicenseForm())
            {
                form.ShowDialog();
            }
        }

        private void MenuHelpMetaPackages_Click(object sender, EventArgs e)
        {
            launchUrl("https://docs.chocolatey.org/en-us/faqs#what-is-the-difference-between-packages-no-suffix-as-compared-to.install.portable");
        }

        private void UpgradePage_UpgradeAllClick(object sender, EventArgs e)
        {
            SelectedPackages = UpgradePage.OutdatedPackages;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UpgradePage_UpgradeSelectedClick(object sender, EventArgs e)
        {
            SelectedPackages = UpgradePage.SelectedPackages;
            if (SelectedPackages.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UpgradePage_CancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void GetOutdatedAsync()
        {
            try
            {
                List<IPackage> packages = await _PackageManager.GetOutdatedAsync();
                UpgradePage.OutdatedPackages = packages;
            }
            catch (ChocolateyException)
            {
                ShowErrorDialog(Properties.strings.Err_CheckOutdated);
            }
        }

        private async void GetInstalledAsync()
        {
            try
            {
                List<IPackage> packages = await _PackageManager.GetInstalledAsync();
                InstalledPage.Packages = packages;
            }
            catch (ChocolateyException)
            {
                ShowErrorDialog(Properties.strings.Form_Err_ListInstalled);
            }
        }

        private bool HasAdminPrivileges()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void ShowErrorDialog(string msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void launchUrl(string url)
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = $"/c start {url}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(info);
        }
    }
}
