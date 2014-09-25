using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.PSF;
using Casewise.GraphAPI.API;

using Microsoft.Win32;
using log4net;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace cwContextGenerator.GUI
{
    /// <summary>
    /// cwPictureBoxButtonOptions
    /// </summary>
    /// <typeparam name="GuiType">The type of the U i_ TYPE.</typeparam>
    /// <typeparam name="RootNodeType">The type of the OO t_ NODE.</typeparam>
    /// <typeparam name="CreateItemType">The type of the REAT e_ ITEM.</typeparam>
    public class cwPictureBoxButtonOptions : cwPictureBoxButton
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(cwPictureBoxButtonOptions));

        /// <summary>
        /// Initializes a new instance of the <see cref="cwPictureBoxButtonOptions&lt;GuiType, RootNodeType, CreateItemType&gt;"/> class.
        /// </summary>
        /// <param name="mainGUI">The main GUI.</param>
        public cwPictureBoxButtonOptions(Launcher mainGUI)
            : base(mainGUI)
        {
            BackgroundImage = Properties.Resources.image_option_add_32x32;
            this.MouseUp += new MouseEventHandler(cwPictureBoxButtonOptions_MouseUp);
            Visible = false;
        }

        /// <summary>
        /// Handles the MouseUp event of the cwPictureBoxButtonOptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void cwPictureBoxButtonOptions_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ContextMenuStrip menuStrip = new ContextMenuStrip();
                menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
                if (mainGUI.GetCore()._selectedModel == null)
                {
                    mainGUI.GetCore()._notEnabledModels.Sort();
                    for (var i = 0; i < mainGUI.GetCore()._notEnabledModels.Count; ++i)
                    {
                        cwLightModel model = mainGUI.GetCore()._notEnabledModels[i];
                        ToolStripItem modelItem = menuStrip.Items.Add(model.ToString());
                        modelItem.Click += (modelSender, args) => ctx_setupNewAModel_Click(model, args);
                        menuStrip.Items.Add(modelItem);
                    }
                }
                else
                {
                    ctx_createItem(mainGUI.GetCore()._selectedModel);
                }
                menuStrip.Show(this, new Point(e.X, e.Y));
            }
        }

        /// <summary>
        /// Ctx_creates the item.
        /// </summary>
        /// <param name="model">The model.</param>
        private void ctx_createItem(cwLightModel model)
        {
            try
            {
                this.mainGUI.DisplayCreateForm(model);
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }
        

        private void ctx_setupNewAModel_Click(cwLightModel model, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.mainGUI.GetCore().EnableModel(model);
                // remove model from list to enable
                this.mainGUI.GetCore().SetModelAsEnabled(model);
                // add cwButtonModel
                this.mainGUI.DisplayEnabledModels();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }

      
    }
}
