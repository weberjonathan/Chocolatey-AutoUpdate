using CandyShop.Packages;
using CandyShop.Packages.Chocolatey;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CandyShop
{
    public partial class InstalledPage : UserControl
    {
        private List<IPackage> _Packages = new List<IPackage>();
        private Dictionary<string, string> PackageDetailsCache = new Dictionary<string, string>();

        public InstalledPage()
        {
            InitializeComponent();
            // SplitContainer.Panel2Collapsed = true;

            this.Resize += new EventHandler((sender, e) =>
            {
                TextSearch.Size = new System.Drawing.Size(CheckHideSuffixed.Location.X - 20, TextSearch.Height);
            });

            LstPackages.Resize += new EventHandler((sender, e) =>
            {
                int availWidth = LstPackages.Width - LstPackages.Margin.Left - LstPackages.Margin.Right - SystemInformation.VerticalScrollBarWidth;

                LstPackages.Columns[0].Width = (int)Math.Floor(availWidth * .6);
                LstPackages.Columns[1].Width = (int)Math.Floor(availWidth * .4);
            });
        }

        public List<IPackage> Packages {
            get => _Packages;
            set {
                _Packages = value;
                foreach (ChocolateyPackage pckg in value)
                {
                    if (!(CheckHideSuffixed.Checked && pckg.HasSuffix))
                    {
                        LstPackages.Items.Add(PackageToListView(pckg));
                    }
                }

                if (LstPackages.Items.Count > 0)
                {
                    LstPackages.Items[0].Selected = true;
                }
            }
        }

        public IPackage SelectedPackage {
            get {
                if (LstPackages.SelectedItems.Count > 0)
                {
                    string pName = LstPackages.SelectedItems[0].Text;
                    return Packages.Find(p => p.Name.Equals(pName));
                }
                else
                {
                    return null;
                }
            }
        }

        public IPackageManager PackageManager { get; set; }

        private async void LstPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedPackage == null) return;
            if (PackageManager == null) return; // TODO log warning?

            TxtDetails.Text = "Loading ...";

            string selectedPackageName = SelectedPackage.Name;
            string details;
            if (!PackageDetailsCache.TryGetValue(selectedPackageName, out details))
            {
                try
                {
                    details = await PackageManager.GetInfoAsync(SelectedPackage);
                    if (!PackageDetailsCache.ContainsKey(selectedPackageName))
                    {
                        PackageDetailsCache.Add(selectedPackageName, details);
                    }
                }
                catch (ChocolateyException)
                {
                    details = Properties.strings.Form_Err_GetInfo;
                }
            }

            // check if package whose info was waited on is still selected
            if (SelectedPackage != null)
            {
                if (SelectedPackage.Name.Equals(selectedPackageName))
                {
                    TxtDetails.Text = details;
                }
            }
        }

        private void CheckHideSuffixed_CheckedChanged(object sender, EventArgs e)
        {
            SyncListView();
        }

        private void TextSearch_TextChanged(object sender, EventArgs e)
        {
            SyncListView();
        }

        private void TextSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter) && LstPackages.Items.Count > 0)
            {
                LstPackages.Items[0].Selected = true;
            }
        }

        private ListViewItem PackageToListView(ChocolateyPackage pckg)
        {
            ListViewItem rtn = new ListViewItem(new string[]
            {
                pckg.Name,
                pckg.Version
            });

            return rtn;
        }

        private void InsertPackageInListView(ChocolateyPackage package)
        {
            int latestPossibleIndex = _Packages.IndexOf(package);
            ListViewItem lastVisibilePackage = null;

            // find package that is supposed to be directly above it
            for (int j = 0; j < latestPossibleIndex; j++)
            {
                ListViewItem previousPackage = LstPackages.FindItemWithText(_Packages[j].Name);
                if (previousPackage != null)
                {
                    lastVisibilePackage = previousPackage;
                }
            }

            // insert
            int index = 0;
            if (lastVisibilePackage != null)
            {
                index = LstPackages.Items.IndexOf(lastVisibilePackage) + 1;
            }

            LstPackages.Items.Insert(index, PackageToListView(package));
        }

        private void SyncListView()
        {
            string filterName = TextSearch.Text;
            bool hideSuffixed = CheckHideSuffixed.Checked;

            foreach (ChocolateyPackage package in _Packages)
            {
                bool packageAllowed = true;
                
                // determine whether package should be displayed
                if (hideSuffixed && package.HasSuffix)
                {
                    packageAllowed = false;
                }

                if (!String.IsNullOrEmpty(filterName) && !package.Name.Contains(filterName))
                {
                    packageAllowed = false;
                }
                
                // determine whether it is displayed
                ListViewItem listviewItem = LstPackages.FindItemWithText(package.Name);
                if (listviewItem == null)
                {
                    if (packageAllowed)
                    {
                        InsertPackageInListView(package);
                    }
                }
                else
                {
                    if (!packageAllowed)
                    {
                        listviewItem.Remove();
                    }
                }
            }
        }
    }
}
