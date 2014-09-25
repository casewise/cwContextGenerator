using cwContextGenerator.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwContextGenerator.GUI
{
    public partial class LauncherCreateItemPopup : Form
    {
        public bool hasBeenCanceled = false;
        private ApplicationCore core = null;

        public LauncherCreateItemPopup(ApplicationCore c)
        {
            InitializeComponent();
            this.core = c;
            this.Icon = Properties.Resources.Casewise_Icon;
            this.ActiveControl = this.textBoxConfiguration;
            ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the bOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bOK_Click(object sender, EventArgs e)
        {
            string newName = this.GetNewConfigurationName();
            bool canCreate = this.core.CanConfigurationBeCreated(newName);
            if (canCreate)
            {
                this.hasBeenCanceled = false;
                this.Close();
            }
            else
            {
                this.labelWarning.Visible = true;
                this.textBoxConfiguration.SelectAll();
                this.textBoxConfiguration.Focus();
            }
        }

        public string GetNewConfigurationName()
        {
            return this.textBoxConfiguration.Text;
        }

        /// <summary>
        /// Handles the Click event of the buttonCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.hasBeenCanceled = true;
            this.Close();
        }

        private void textBoxConfiguration_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    this.bOK_Click(null, null);
                    break;
                case (char)Keys.Escape:
                    this.buttonCancel_Click(null, null);
                    break;
                default:
                    break;
            }
        }


    }
}
