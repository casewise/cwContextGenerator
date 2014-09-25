using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.PSF;

namespace cwContextGenerator.GUI
{
    class cwPictureBoxButtonBack : cwPictureBoxButton
    {
        public cwPictureBoxButtonBack(Launcher mainGUI)
            : base(mainGUI)
        {
            BackgroundImage = Properties.Resources.image_option_prev_32x32;
            this.Click += new EventHandler(cwPictureBoxButtonBack_Click);
            Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the cwPictureBoxButtonBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cwPictureBoxButtonBack_Click(object sender, EventArgs e)
        {
            this.mainGUI.GetCore()._selectedModel = null;
            this.mainGUI.DisplayEnabledModels();
        }

    }
}
