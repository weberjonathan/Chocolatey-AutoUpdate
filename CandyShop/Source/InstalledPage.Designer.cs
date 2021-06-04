namespace CandyShop
{
    partial class InstalledPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LstPackages = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colVer = new System.Windows.Forms.ColumnHeader();
            this.PanelTop = new System.Windows.Forms.Panel();
            this.TextSearch = new System.Windows.Forms.TextBox();
            this.CheckHideSuffixed = new System.Windows.Forms.CheckBox();
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.TxtDetails = new System.Windows.Forms.TextBox();
            this.PanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // LstPackages
            // 
            this.LstPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colVer});
            this.LstPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LstPackages.FullRowSelect = true;
            this.LstPackages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LstPackages.HideSelection = false;
            this.LstPackages.Location = new System.Drawing.Point(0, 0);
            this.LstPackages.Name = "LstPackages";
            this.LstPackages.Size = new System.Drawing.Size(600, 250);
            this.LstPackages.TabIndex = 0;
            this.LstPackages.UseCompatibleStateImageBehavior = false;
            this.LstPackages.View = System.Windows.Forms.View.Details;
            this.LstPackages.SelectedIndexChanged += new System.EventHandler(this.LstPackages_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            // 
            // colVer
            // 
            this.colVer.Text = "Version";
            // 
            // PanelTop
            // 
            this.PanelTop.Controls.Add(this.TextSearch);
            this.PanelTop.Controls.Add(this.CheckHideSuffixed);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(600, 44);
            this.PanelTop.TabIndex = 1;
            // 
            // TextSearch
            // 
            this.TextSearch.Location = new System.Drawing.Point(0, 13);
            this.TextSearch.Name = "TextSearch";
            this.TextSearch.PlaceholderText = "Search";
            this.TextSearch.Size = new System.Drawing.Size(371, 23);
            this.TextSearch.TabIndex = 1;
            this.TextSearch.TextChanged += new System.EventHandler(this.TextSearch_TextChanged);
            this.TextSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextSearch_KeyDown);
            // 
            // CheckHideSuffixed
            // 
            this.CheckHideSuffixed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckHideSuffixed.AutoSize = true;
            this.CheckHideSuffixed.Checked = true;
            this.CheckHideSuffixed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckHideSuffixed.Location = new System.Drawing.Point(440, 15);
            this.CheckHideSuffixed.Name = "CheckHideSuffixed";
            this.CheckHideSuffixed.Size = new System.Drawing.Size(148, 19);
            this.CheckHideSuffixed.TabIndex = 0;
            this.CheckHideSuffixed.Text = "&Hide suffixed packages";
            this.CheckHideSuffixed.UseVisualStyleBackColor = true;
            this.CheckHideSuffixed.CheckedChanged += new System.EventHandler(this.CheckHideSuffixed_CheckedChanged);
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer.Location = new System.Drawing.Point(0, 44);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.LstPackages);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.TxtDetails);
            this.SplitContainer.Size = new System.Drawing.Size(600, 408);
            this.SplitContainer.SplitterDistance = 250;
            this.SplitContainer.TabIndex = 2;
            this.SplitContainer.Text = "splitContainer1";
            // 
            // TxtDetails
            // 
            this.TxtDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtDetails.Location = new System.Drawing.Point(0, 0);
            this.TxtDetails.Multiline = true;
            this.TxtDetails.Name = "TxtDetails";
            this.TxtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtDetails.Size = new System.Drawing.Size(600, 154);
            this.TxtDetails.TabIndex = 0;
            this.TxtDetails.WordWrap = false;
            // 
            // InstalledPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SplitContainer);
            this.Controls.Add(this.PanelTop);
            this.Name = "InstalledPage";
            this.Size = new System.Drawing.Size(600, 452);
            this.PanelTop.ResumeLayout(false);
            this.PanelTop.PerformLayout();
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            this.SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView LstPackages;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colVer;
        private System.Windows.Forms.Panel PanelTop;
        private System.Windows.Forms.SplitContainer SplitContainer;
        private System.Windows.Forms.TextBox TxtDetails;
        private System.Windows.Forms.CheckBox CheckHideSuffixed;
        private System.Windows.Forms.TextBox TextSearch;
    }
}
