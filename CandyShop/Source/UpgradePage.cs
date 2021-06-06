using CandyShop.Packages;
using CandyShop.Packages.Chocolatey;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CandyShop
{
    public partial class UpgradePage : UserControl
    {
        private List<IPackage> _OutdatedPackages = new List<IPackage>();

        public UpgradePage()
        {
            InitializeComponent();

            LstPackages.ItemChecked += LstPackages_ItemChecked;
            LstPackages.Resize += LstPackages_Resize;
            BtnUpgradeAll.Click += new EventHandler((sender, e) => { UpgradeAllClick?.Invoke(this, e); });
            BtnUpgradeSelected.Click += new EventHandler((sender, e) => { UpgradeSelectedClick?.Invoke(this, e); });
            BtnCancel.Click += new EventHandler((sender, e) => { CancelClick?.Invoke(this, e); });
        }

        public event EventHandler UpgradeAllClick;

        public event EventHandler UpgradeSelectedClick;

        public event EventHandler CancelClick;

        public bool ShowAdminWarning {
            get {
                return PanelTop.Visible;
            }
            set {
                PanelTop.Visible = value;
            }
        }

        public List<IPackage> OutdatedPackages {
            get => _OutdatedPackages;
            set {
                _OutdatedPackages = value;

                LblLoading.Visible = false;

                if (value.Count > 0)
                {
                    BtnUpgradeSelected.Enabled = true;
                    BtnUpgradeAll.Enabled = true;

                    foreach (IPackage pckg in value)
                    {
                        ListViewItem item;
                        if (pckg is ChocolateyPackage)
                        {
                            ChocolateyPackage chocoPackage = (ChocolateyPackage) pckg;
                            item = new ListViewItem(new string[]
                            {
                                chocoPackage.Name,
                                chocoPackage.Version,
                                chocoPackage.AvailableVersion,
                                chocoPackage.Pinned.ToString()
                            });
                        }
                        else
                        {
                            item = new ListViewItem(new string[]
                            {
                                pckg.Name,
                                pckg.Version,
                                pckg.AvailableVersion,
                            });
                        }

                        LstPackages.Items.Add(item);
                    }
                }

                CheckNormalAndMetaItems();
                LstPackages_Resize(this, EventArgs.Empty);
            }
        }

        public List<IPackage> SelectedPackages {
            get {
                List<IPackage> rtn = new List<IPackage>();
                foreach (ListViewItem item in LstPackages.CheckedItems)
                {
                    rtn.Add(FindPackageByName(item.Text));
                }
                return rtn;
            }
        }


        public void CheckAllItems()
        {
            foreach (ListViewItem item in LstPackages.Items)
            {
                item.Checked = true;
            }
        }

        public void CheckNormalAndMetaItems()
        {
            // method only relevant if data source is of Chocolatey
            foreach (ListViewItem item in LstPackages.Items)
            {
                IPackage pckg = FindPackageByName(item.Text);

                if (pckg is ChocolateyPackage)
                {
                    ChocolateyPackage chocoPackage = (ChocolateyPackage)pckg;
                    item.Checked = !(chocoPackage.HasMetaPackage && chocoPackage.HasSuffix);
                }
            }
        }

        public void UncheckAllItems()
        {
            foreach (ListViewItem item in LstPackages.Items)
            {
                item.Checked = false;
            }
        }

        private void LstPackages_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            BtnUpgradeSelected.Text = $"Upgrade ({LstPackages.CheckedItems.Count})";
        }

        private void LstPackages_Resize(object sender, EventArgs e)
        {
            if (OutdatedPackages.Count > 0)
            {
                int pinnedWidth = OutdatedPackages[0] is ChocolateyPackage ? 60 : 0;
                int availWidth = LstPackages.Width - pinnedWidth - LstPackages.Margin.Left - LstPackages.Margin.Right - SystemInformation.VerticalScrollBarWidth;

                LstPackages.Columns[0].Width = (int)Math.Floor(availWidth * .4);
                LstPackages.Columns[1].Width = (int)Math.Floor(availWidth * .3);
                LstPackages.Columns[2].Width = (int)Math.Floor(availWidth * .3);
                LstPackages.Columns[3].Width = pinnedWidth;
            }
            else
            {
                // TODO
            }
        }

        // TODO why no dict?
        private IPackage FindPackageByName(string name)
        {
            return _OutdatedPackages.Find(pckg => pckg.Name.Equals(name));
        }
    }
}
