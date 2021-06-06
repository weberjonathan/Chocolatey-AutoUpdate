using CandyShop.Packages;
using CandyShop.Packages.Chocolatey;
using CandyShop.Packages.Winget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace CandyShop
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            WingetManager packageManager = new WingetManager();
            List<IPackage> test = packageManager.GetInstalled();
            string abc = test[0].Name;
            string abc2 = ((WingetPackage) test[0]).Name;
            string abc3 = ((WingetPackage)test[0]).Id;

            // check if Chocolatey is in path
            try
            {
                ProcessStartInfo pi = new ProcessStartInfo("choco", "--version")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process p = Process.Start(pi);
                p.WaitForExit();
            }
            catch (Win32Exception)
            {
                MessageBox.Show(
                    "Error: An error occurred while starting the Chocolatey application. Please make sure it is installed and in PATH.",
                    Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // launch application
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CandyShopApplicationContext context = new CandyShopApplicationContext();
            Application.Run(context);
        }
    }
}
