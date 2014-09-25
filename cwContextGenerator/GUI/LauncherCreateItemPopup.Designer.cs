namespace cwContextGenerator.GUI
{
    partial class LauncherCreateItemPopup
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelData = new System.Windows.Forms.Panel();
            this.labelWarning = new System.Windows.Forms.Label();
            this.labelConfiguration = new System.Windows.Forms.Label();
            this.textBoxConfiguration = new System.Windows.Forms.TextBox();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.labelChooseName = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.panelData.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.panelData);
            this.panelMain.Controls.Add(this.panelTitle);
            this.panelMain.Controls.Add(this.buttonCancel);
            this.panelMain.Controls.Add(this.buttonOK);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(2, 2);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(464, 291);
            this.panelMain.TabIndex = 0;
            // 
            // panelData
            // 
            this.panelData.Controls.Add(this.labelWarning);
            this.panelData.Controls.Add(this.labelConfiguration);
            this.panelData.Controls.Add(this.textBoxConfiguration);
            this.panelData.Location = new System.Drawing.Point(15, 43);
            this.panelData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(432, 204);
            this.panelData.TabIndex = 5;
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(3, 69);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(391, 18);
            this.labelWarning.TabIndex = 2;
            this.labelWarning.Text = "Impossible de créer une configuration. Le nom existe déjà.";
            this.labelWarning.Visible = false;
            // 
            // labelConfiguration
            // 
            this.labelConfiguration.AutoSize = true;
            this.labelConfiguration.Location = new System.Drawing.Point(3, 23);
            this.labelConfiguration.Name = "labelConfiguration";
            this.labelConfiguration.Size = new System.Drawing.Size(45, 18);
            this.labelConfiguration.TabIndex = 1;
            this.labelConfiguration.Text = "Nom :";
            // 
            // textBoxConfiguration
            // 
            this.textBoxConfiguration.Location = new System.Drawing.Point(54, 20);
            this.textBoxConfiguration.Name = "textBoxConfiguration";
            this.textBoxConfiguration.Size = new System.Drawing.Size(375, 23);
            this.textBoxConfiguration.TabIndex = 0;
            this.textBoxConfiguration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxConfiguration_KeyPress);
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panelTitle.Controls.Add(this.labelChooseName);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(464, 34);
            this.panelTitle.TabIndex = 4;
            // 
            // labelChooseName
            // 
            this.labelChooseName.BackColor = System.Drawing.Color.White;
            this.labelChooseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelChooseName.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChooseName.ForeColor = System.Drawing.Color.MediumBlue;
            this.labelChooseName.Location = new System.Drawing.Point(0, 0);
            this.labelChooseName.Name = "labelChooseName";
            this.labelChooseName.Size = new System.Drawing.Size(464, 34);
            this.labelChooseName.TabIndex = 4;
            this.labelChooseName.Text = "Configuration";
            this.labelChooseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.DarkRed;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancel.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(333, 255);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(114, 28);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Annuler";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.OliveDrab;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOK.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(15, 255);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(114, 28);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // LauncherCreateItemPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(468, 295);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LauncherCreateItemPopup";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "createItemPopup";
            this.panelMain.ResumeLayout(false);
            this.panelData.ResumeLayout(false);
            this.panelData.PerformLayout();
            this.panelTitle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label labelChooseName;
        private System.Windows.Forms.Panel panelData;
        private System.Windows.Forms.TextBox textBoxConfiguration;
        private System.Windows.Forms.Label labelConfiguration;
        private System.Windows.Forms.Label labelWarning;

    }
}