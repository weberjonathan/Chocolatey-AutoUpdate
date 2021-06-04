namespace CandyShop
{
    partial class CandyShopForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CandyShopForm));
            this.MainPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabUpgrade = new System.Windows.Forms.TabPage();
            this.UpgradePage = new CandyShop.UpgradePage();
            this.TabInstalled = new System.Windows.Forms.TabPage();
            this.InstalledPage = new CandyShop.InstalledPage();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.CandyShopMenu = new System.Windows.Forms.MenuStrip();
            this.MenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEditSelectRelevant = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEditDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExtras = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExtrasCreateTask = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelpGithub = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelpLicense = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelpMetaPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TabUpgrade.SuspendLayout();
            this.TabInstalled.SuspendLayout();
            this.CandyShopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.SystemColors.Window;
            this.MainPanel.Controls.Add(this.tabControl1);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 24);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(730, 510);
            this.MainPanel.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabUpgrade);
            this.tabControl1.Controls.Add(this.TabInstalled);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(730, 510);
            this.tabControl1.TabIndex = 3;
            // 
            // TabUpgrade
            // 
            this.TabUpgrade.Controls.Add(this.UpgradePage);
            this.TabUpgrade.Location = new System.Drawing.Point(4, 24);
            this.TabUpgrade.Name = "TabUpgrade";
            this.TabUpgrade.Padding = new System.Windows.Forms.Padding(3);
            this.TabUpgrade.Size = new System.Drawing.Size(722, 482);
            this.TabUpgrade.TabIndex = 0;
            this.TabUpgrade.Text = "Upgrade";
            this.TabUpgrade.UseVisualStyleBackColor = true;
            // 
            // UpgradePage
            // 
            this.UpgradePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpgradePage.Location = new System.Drawing.Point(3, 3);
            this.UpgradePage.Name = "UpgradePage";
            this.UpgradePage.ShowAdminWarning = true;
            this.UpgradePage.Size = new System.Drawing.Size(716, 476);
            this.UpgradePage.TabIndex = 2;
            // 
            // TabInstalled
            // 
            this.TabInstalled.Controls.Add(this.InstalledPage);
            this.TabInstalled.Location = new System.Drawing.Point(4, 24);
            this.TabInstalled.Name = "TabInstalled";
            this.TabInstalled.Padding = new System.Windows.Forms.Padding(3);
            this.TabInstalled.Size = new System.Drawing.Size(722, 482);
            this.TabInstalled.TabIndex = 1;
            this.TabInstalled.Text = "Installed";
            this.TabInstalled.UseVisualStyleBackColor = true;
            // 
            // InstalledPage
            // 
            this.InstalledPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InstalledPage.Location = new System.Drawing.Point(3, 3);
            this.InstalledPage.Name = "InstalledPage";
            this.InstalledPage.Size = new System.Drawing.Size(716, 476);
            this.InstalledPage.TabIndex = 0;
            // 
            // CandyShopMenu
            // 
            this.CandyShopMenu.BackColor = System.Drawing.SystemColors.Window;
            this.CandyShopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuEdit,
            this.MenuExtras,
            this.MenuHelp});
            this.CandyShopMenu.Location = new System.Drawing.Point(0, 0);
            this.CandyShopMenu.Name = "CandyShopMenu";
            this.CandyShopMenu.Size = new System.Drawing.Size(730, 24);
            this.CandyShopMenu.TabIndex = 3;
            this.CandyShopMenu.Text = "Menu";
            // 
            // MenuEdit
            // 
            this.MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuEditSelectAll,
            this.MenuEditSelectRelevant,
            this.MenuEditDeselectAll});
            this.MenuEdit.Name = "MenuEdit";
            this.MenuEdit.Size = new System.Drawing.Size(39, 20);
            this.MenuEdit.Text = "&Edit";
            // 
            // MenuEditSelectAll
            // 
            this.MenuEditSelectAll.Name = "MenuEditSelectAll";
            this.MenuEditSelectAll.Size = new System.Drawing.Size(251, 22);
            this.MenuEditSelectAll.Text = "&Select all";
            this.MenuEditSelectAll.Click += new System.EventHandler(this.MenuEditSelectAll_Click);
            // 
            // MenuEditSelectRelevant
            // 
            this.MenuEditSelectRelevant.Name = "MenuEditSelectRelevant";
            this.MenuEditSelectRelevant.Size = new System.Drawing.Size(251, 22);
            this.MenuEditSelectRelevant.Text = "Select &normal and meta packages";
            this.MenuEditSelectRelevant.Click += new System.EventHandler(this.MenuEditSelectRelevant_Click);
            // 
            // MenuEditDeselectAll
            // 
            this.MenuEditDeselectAll.Name = "MenuEditDeselectAll";
            this.MenuEditDeselectAll.Size = new System.Drawing.Size(251, 22);
            this.MenuEditDeselectAll.Text = "&Deselect all";
            this.MenuEditDeselectAll.Click += new System.EventHandler(this.MenuEditDeselectAll_Click);
            // 
            // MenuExtras
            // 
            this.MenuExtras.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuExtrasCreateTask});
            this.MenuExtras.Name = "MenuExtras";
            this.MenuExtras.Size = new System.Drawing.Size(50, 20);
            this.MenuExtras.Text = "&Extras";
            // 
            // MenuExtrasCreateTask
            // 
            this.MenuExtrasCreateTask.CheckOnClick = true;
            this.MenuExtrasCreateTask.Name = "MenuExtrasCreateTask";
            this.MenuExtrasCreateTask.Size = new System.Drawing.Size(344, 22);
            this.MenuExtrasCreateTask.Text = "&Display notification for outdated packages on login";
            this.MenuExtrasCreateTask.Click += new System.EventHandler(this.MenuExtrasCreateTask_Click);
            // 
            // MenuHelp
            // 
            this.MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHelpGithub,
            this.MenuHelpLicense,
            this.MenuHelpMetaPackages});
            this.MenuHelp.Name = "MenuHelp";
            this.MenuHelp.Size = new System.Drawing.Size(44, 20);
            this.MenuHelp.Text = "&Help";
            // 
            // MenuHelpGithub
            // 
            this.MenuHelpGithub.Name = "MenuHelpGithub";
            this.MenuHelpGithub.Size = new System.Drawing.Size(153, 22);
            this.MenuHelpGithub.Text = "&Github";
            this.MenuHelpGithub.Click += new System.EventHandler(this.MenuHelpGithub_Click);
            // 
            // MenuHelpLicense
            // 
            this.MenuHelpLicense.Name = "MenuHelpLicense";
            this.MenuHelpLicense.Size = new System.Drawing.Size(153, 22);
            this.MenuHelpLicense.Text = "&License";
            this.MenuHelpLicense.Click += new System.EventHandler(this.MenuHelpLicense_Click);
            // 
            // MenuHelpMetaPackages
            // 
            this.MenuHelpMetaPackages.Name = "MenuHelpMetaPackages";
            this.MenuHelpMetaPackages.Size = new System.Drawing.Size(153, 22);
            this.MenuHelpMetaPackages.Text = "&Meta packages";
            this.MenuHelpMetaPackages.Click += new System.EventHandler(this.MenuHelpMetaPackages_Click);
            // 
            // CandyShopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 534);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.CandyShopMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.CandyShopMenu;
            this.MaximizeBox = false;
            this.Name = "CandyShopForm";
            this.Text = "ChocoAutoUpdateForm";
            this.Load += new System.EventHandler(this.ChocoAutoUpdateForm_Load);
            this.MainPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TabUpgrade.ResumeLayout(false);
            this.TabInstalled.ResumeLayout(false);
            this.CandyShopMenu.ResumeLayout(false);
            this.CandyShopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private UpgradePage UpgradePage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabUpgrade;
        private System.Windows.Forms.TabPage TabInstalled;
        private InstalledPage InstalledPage;
        private System.Windows.Forms.MenuStrip CandyShopMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuExtras;
        private System.Windows.Forms.ToolStripMenuItem MenuExtrasCreateTask;
        private System.Windows.Forms.ToolStripMenuItem MenuHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuHelpGithub;
        private System.Windows.Forms.ToolStripMenuItem MenuHelpLicense;
        private System.Windows.Forms.ToolStripMenuItem MenuEdit;
        private System.Windows.Forms.ToolStripMenuItem MenuEditSelectAll;
        private System.Windows.Forms.ToolStripMenuItem MenuEditDeselectAll;
        private System.Windows.Forms.ToolStripMenuItem MenuEditSelectRelevant;
        private System.Windows.Forms.ToolStripMenuItem MenuHelpMetaPackages;
    }
}