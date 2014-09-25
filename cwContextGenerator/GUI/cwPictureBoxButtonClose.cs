using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.PSF;

namespace cwContextGenerator.GUI
{
    class cwPictureBoxButtonClose : cwPictureBoxButton
    {
        public cwPictureBoxButtonClose(Launcher mainGUI)
            : base(mainGUI)
        {
            BackgroundImage = Properties.Resources.image_option_close_32x32;
            this.Click += new EventHandler(cwPictureBoxClose_Click);
            Visible = false;
        }

        private void cwPictureBoxClose_Click(object sender, EventArgs e)
        {
            mainGUI.CloseConnection();
            Application.Exit();
        }
    }
}
