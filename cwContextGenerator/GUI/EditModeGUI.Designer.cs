using Casewise.GraphAPI.PSF;
namespace cwContextGenerator.GUI
{
    public partial class EditModeGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditModeGUI));
            this.richTextBoxDebug = new System.Windows.Forms.RichTextBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerTop = new System.Windows.Forms.SplitContainer();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker5 = new System.ComponentModel.BackgroundWorker();
            this.treeViewConfigurations = new System.Windows.Forms.TreeView();
            this.groupBoxDesigns = new System.Windows.Forms.GroupBox();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelOptionValue = new System.Windows.Forms.Label();
            this.labelOptionName = new System.Windows.Forms.Label();
            this.tableLayoutOptions = new Casewise.GraphAPI.PSF.cwPSFTableLayoutPropertiesBoxes();
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            this.menuStripTop.SuspendLayout();
            this.groupBoxDesigns.SuspendLayout();
            this.panelOptions.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxDebug
            // 
            this.richTextBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDebug.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxDebug.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxDebug.Name = "richTextBoxDebug";
            this.richTextBoxDebug.Size = new System.Drawing.Size(935, 80);
            this.richTextBoxDebug.TabIndex = 3;
            this.richTextBoxDebug.Text = "";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 26);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(11);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTop);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(9);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.richTextBoxDebug);
            this.splitContainerMain.Size = new System.Drawing.Size(935, 539);
            this.splitContainerMain.SplitterDistance = 455;
            this.splitContainerMain.TabIndex = 4;
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(9, 9);
            this.splitContainerTop.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerTop.Name = "splitContainerTop";
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.groupBoxDesigns);
            // 
            // splitContainerTop.Panel2
            // 
            this.splitContainerTop.Panel2.Controls.Add(this.groupBoxDetails);
            this.splitContainerTop.Size = new System.Drawing.Size(917, 437);
            this.splitContainerTop.SplitterDistance = 300;
            this.splitContainerTop.TabIndex = 3;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.menuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.executeToolStripMenuItem});
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStripTop.Size = new System.Drawing.Size(935, 26);
            this.menuStripTop.TabIndex = 5;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(64, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(66, 22);
            this.executeToolStripMenuItem.Text = "Execute";
            this.executeToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // treeViewConfigurations
            // 
            this.treeViewConfigurations.AllowDrop = true;
            this.treeViewConfigurations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewConfigurations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewConfigurations.HideSelection = false;
            this.treeViewConfigurations.Location = new System.Drawing.Point(4, 19);
            this.treeViewConfigurations.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewConfigurations.Name = "treeViewConfigurations";
            this.treeViewConfigurations.Size = new System.Drawing.Size(292, 414);
            this.treeViewConfigurations.TabIndex = 0;
            this.treeViewConfigurations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditModeGUI_KeyDown);
            this.treeViewConfigurations.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewConfigurations_Click);
            // 
            // groupBoxDesigns
            // 
            this.groupBoxDesigns.BackColor = System.Drawing.Color.White;
            this.groupBoxDesigns.Controls.Add(this.treeViewConfigurations);
            this.groupBoxDesigns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDesigns.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDesigns.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxDesigns.Name = "groupBoxDesigns";
            this.groupBoxDesigns.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxDesigns.Size = new System.Drawing.Size(300, 437);
            this.groupBoxDesigns.TabIndex = 0;
            this.groupBoxDesigns.TabStop = false;
            this.groupBoxDesigns.Text = "Configurations";
            // 
            // panelOptions
            // 
            this.panelOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelOptions.Controls.Add(this.tableLayoutOptions);
            this.panelOptions.Controls.Add(this.tableLayoutPanel);
            this.panelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptions.Location = new System.Drawing.Point(11, 28);
            this.panelOptions.Margin = new System.Windows.Forms.Padding(4);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(591, 396);
            this.panelOptions.TabIndex = 2;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.Controls.Add(this.labelOptionName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelOptionValue, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(591, 28);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelOptionValue
            // 
            this.labelOptionValue.AutoSize = true;
            this.labelOptionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOptionValue.Location = new System.Drawing.Point(181, 1);
            this.labelOptionValue.Name = "labelOptionValue";
            this.labelOptionValue.Size = new System.Drawing.Size(406, 26);
            this.labelOptionValue.TabIndex = 1;
            this.labelOptionValue.Text = "Valeurs";
            this.labelOptionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOptionName
            // 
            this.labelOptionName.AutoSize = true;
            this.labelOptionName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOptionName.Location = new System.Drawing.Point(4, 1);
            this.labelOptionName.Name = "labelOptionName";
            this.labelOptionName.Size = new System.Drawing.Size(170, 26);
            this.labelOptionName.TabIndex = 0;
            this.labelOptionName.Text = "Options";
            this.labelOptionName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutOptions
            // 
            this.tableLayoutOptions.AutoScroll = true;
            this.tableLayoutOptions.AutoSize = true;
            this.tableLayoutOptions.BackColor = System.Drawing.Color.White;
            this.tableLayoutOptions.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutOptions.ColumnCount = 2;
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutOptions.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutOptions.Name = "tableLayoutOptions";
            this.tableLayoutOptions.RowCount = 1;
            this.tableLayoutOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutOptions.Size = new System.Drawing.Size(591, 368);
            this.tableLayoutOptions.TabIndex = 1;
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.AutoSize = true;
            this.groupBoxDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxDetails.BackColor = System.Drawing.Color.White;
            this.groupBoxDetails.Controls.Add(this.panelOptions);
            this.groupBoxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDetails.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDetails.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Padding = new System.Windows.Forms.Padding(11, 13, 11, 13);
            this.groupBoxDetails.Size = new System.Drawing.Size(613, 437);
            this.groupBoxDetails.TabIndex = 1;
            this.groupBoxDetails.TabStop = false;
            this.groupBoxDetails.Text = "Options";
            // 
            // EditModeGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(106F, 106F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(935, 565);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStripTop);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripTop;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EditModeGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "cwEditModeGUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel2.ResumeLayout(false);
            this.splitContainerTop.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.menuStripTop.ResumeLayout(false);
            this.menuStripTop.PerformLayout();
            this.groupBoxDesigns.ResumeLayout(false);
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBoxDetails.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Provide the debug/info information
        /// </summary>
        private System.Windows.Forms.RichTextBox richTextBoxDebug;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        /// <summary>
        /// splitContainerTop
        /// </summary>
        protected System.Windows.Forms.SplitContainer splitContainerTop;
        private System.Windows.Forms.MenuStrip menuStripTop;
        /// <summary>
        /// save menu
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private System.ComponentModel.BackgroundWorker backgroundWorker5;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        protected System.Windows.Forms.GroupBox groupBoxDesigns;
        public System.Windows.Forms.TreeView treeViewConfigurations;
        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Panel panelOptions;
        private cwPSFTableLayoutPropertiesBoxes tableLayoutOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelOptionName;
        private System.Windows.Forms.Label labelOptionValue;
    }
}