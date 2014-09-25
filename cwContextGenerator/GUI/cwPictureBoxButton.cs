using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.PSF;

namespace cwContextGenerator.GUI
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="GuiType">The type of the UI type.</typeparam>
    /// <typeparam name="RootNodeType">The type of the oot node type.</typeparam>
    /// <typeparam name="CreateItemType">The type of the reate item type.</typeparam>
    public class cwPictureBoxButton : PictureBox
    {
        /// <summary>
        /// mainGUI
        /// </summary>
        protected Launcher mainGUI = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="cwPictureBoxButton&lt;GuiType, RootNodeType, CreateItemType&gt;"/> class.
        /// </summary>
        /// <param name="mainGUI">The main GUI.</param>
        public cwPictureBoxButton(Launcher mainGUI)
        {
            this.mainGUI = mainGUI;
            this.MouseEnter += new System.EventHandler(this.cwPictureBoxButton_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.cwPictureBoxButton_MouseLeave);
            Size = new System.Drawing.Size(22, 22);
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        /// <summary>
        /// Handles the MouseEnter event of the cwPictureBoxButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cwPictureBoxButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Handles the MouseLeave event of the cwPictureBoxButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cwPictureBoxButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

    }
}
