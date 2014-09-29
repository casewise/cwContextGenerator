using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
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
    public partial class Launcher : Form
    {
        internal cwPictureBoxButtonClose optionClose = null;
        internal cwPictureBoxButtonOptions optionOptions = null;
        internal cwPictureBoxButtonBack optionBack = null;
        internal cwPictureBoxLogo pictureBoxLogo = null;
        private ApplicationCore Core = null;

        public Launcher(ApplicationCore core)
        {
            InitializeComponent();
            this.Core = core;

            this.Icon = Properties.Resources.Casewise_Icon;
            this.optionClose = new cwPictureBoxButtonClose(this);
            this.optionOptions = new cwPictureBoxButtonOptions(this);
            this.optionBack = new cwPictureBoxButtonBack(this);
            DoubleBuffered = true;

            flowLayoutPanelOptions.Controls.Add(optionClose);
            flowLayoutPanelOptions.Controls.Add(optionOptions);
            flowLayoutPanelOptions.Controls.Add(optionBack);
            flowLayoutPanelOptions.AutoSize = true;
            flowLayoutPanelItems.Refresh();

            pictureBoxLogo = new cwPictureBoxLogo(this, this.splitContainerMain);
            this.splitContainerMain.Panel1.Controls.Add(this.pictureBoxLogo);
            pictureBoxLogo.Image = Properties.Resources.casewise_logo_h100;
            pictureBoxLogo.WaitOnLoad = true;
            pictureBoxLogo.Visible = true;
            optionClose.Visible = true;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            this.Core.closeConnection();
        }

        /// <summary>
        /// Handles the MouseUp event of the Launcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void Launcher_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            resize = false;
        }

        /// <summary>
        /// Handles the MouseDown event of the Launcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void Launcher_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                bool left = e.X < Padding.Left;
                bool right = e.X > Size.Width - Padding.Right;
                bool top = e.Y < Padding.Top;
                bool bottom = e.Y > Size.Height - Padding.Bottom;

                bool topLeft = top && left;
                bool bottomRight = bottom && right;
                bool topRight = top && right;
                bool bottomLeft = bottom && left;

                if (bottom || right)
                {
                    Cursor.Current = Cursors.SizeAll;
                    setInitialResizePoint(e);
                }

            }
        }

        private Point initialResizePoint = new Point();
        private bool resize = true;
        private const int formMinimumSizeWidth = 608;
        private const int formMinimumSizeHeight = 368;

        /// <summary>
        /// Resizes the form.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void resizeForm(MouseEventArgs e)
        {
            if (0.Equals(initialResizePoint.X) && 0.Equals(initialResizePoint.Y))
            {
                return;
            }

            SuspendLayout();
            flowLayoutPanelItems.SuspendLayout();
            flowLayoutPanelOptions.SuspendLayout();
            Point currentPoint = new Point(e.X, e.Y);
            currentPoint.Offset(initialResizePoint);

            int w = Size.Width + currentPoint.X;
            int h = Size.Height + currentPoint.Y;
            if (w < formMinimumSizeWidth)
            {
                w = formMinimumSizeWidth;
            }
            if (h < formMinimumSizeHeight)
            {
                h = formMinimumSizeHeight;
            }
            this.Size = new Size(w, h);
            setInitialResizePoint(e);

            pictureBoxLogo.Location = new Point(Size.Width / 2 - pictureBoxLogo.Width / 2, pictureBoxLogo.Location.Y);
            optionOptions.Refresh();
            optionBack.Refresh();
            optionClose.Refresh();
            flowLayoutPanelOptions.Refresh();
            flowLayoutPanelOptions.ResumeLayout();
            flowLayoutPanelItems.Refresh();
            flowLayoutPanelItems.ResumeLayout();
            Refresh();
            ResumeLayout();
        }

        /// <summary>
        /// Handles the MouseMove event of the Launcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void Launcher_MouseMove(object sender, MouseEventArgs e)
        {
            if (resize && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                resizeForm(e);
            }
        }

        /// <summary>
        /// Sets the initial resize point.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void setInitialResizePoint(MouseEventArgs e)
        {
            initialResizePoint = new Point(-e.X, -e.Y);
            resize = true;
        }

        /// <summary>
        /// Clears the flow layout panel.
        /// </summary>
        public void clearFlowLayoutPanel()
        {
            flowLayoutPanelItems.SuspendLayout();
            flowLayoutPanelItems.Controls.Clear();
            flowLayoutPanelItems.VerticalScroll.Value = 0;
            flowLayoutPanelItems.HorizontalScroll.Value = 0;
            flowLayoutPanelItems.Refresh();
            flowLayoutPanelItems.ResumeLayout();
        }

        /// <summary>
        /// Handles the Shown event of the Launcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Launcher_Shown(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.Core.LoadModels();
            this.DisplayEnabledModels();
            this.ResumeLayout();
        }

        /// <summary>
        /// Displays the configurations.
        /// </summary>
        /// <param name="m">The m.</param>
        public void DisplayConfigurations(cwLightModel m)
        {
            this.clearFlowLayoutPanel();
            List<cwLightObject> configurations = this.GetCore().GetConfigurations(m);
            optionOptions.Visible = true;
            if (!0.Equals(configurations.Count))
            {
                configurations.Sort();
                for (int i = 0; i < configurations.Count; i++)
                {
                    cwButtonItem itemButton = new cwButtonItem(this, configurations[i]);
                    flowLayoutPanelItems.Controls.Add(itemButton);
                }
            }
        }

        /// <summary>
        /// Displays the enabled models.
        /// </summary>
        public void DisplayEnabledModels()
        {
            this.optionBack.Visible = false;
            this.clearFlowLayoutPanel();
            List<cwLightModel> enabled = this.Core._enabledModels;
            List<cwLightModel> notEnabled = this.Core._notEnabledModels;
            if (notEnabled.Count > 0)
            {
                optionOptions.Visible = true;
            }

            if (!0.Equals(enabled.Count))
            {
                enabled.Sort();
                for (int i = 0; i < enabled.Count; ++i)
                {
                    cwButtonModel modelButton = new cwButtonModel(this, enabled[i]);
                    flowLayoutPanelItems.Controls.Add(modelButton);
                }
            }
        }

        /// <summary>
        /// Gets the core.
        /// </summary>
        /// <returns></returns>
        public ApplicationCore GetCore()
        {
            return this.Core;
        }

        /// <summary>
        /// Displays the create form.
        /// </summary>
        /// <param name="m">The m.</param>
        public void DisplayCreateForm(cwLightModel m)
        {
            LauncherCreateItemPopup form = new LauncherCreateItemPopup(this.GetCore());
            cwLightObjectType ot = m.getObjectTypeByScriptName(ApplicationCore.CONTEXTPATH_OT);
            // create new window
            if (!form.hasBeenCanceled)
            {
                string newName = form.GetNewConfigurationName();
                cwLightObject o = this.Core.CreateConfiguration(ot, newName);
                // reload gui for configurations
                this.DisplayConfigurations(m);
                this.LoadConfigurationInGUI(o);
            }
        }

        /// <summary>
        /// Loads the configuration in GUI.
        /// </summary>
        /// <param name="configObject">The configuration.</param>
        public void LoadConfigurationInGUI(cwLightObject configObject)
        {
            // create new window
            EditModeGUI gui = new EditModeGUI(configObject, this.Core);
            // build configuration node from configuration object
            Configuration.ConfigurationRootNode root = this.Core.GetConfigurationNodeFromDescription(configObject);
            configObject = this.Core.GetConfigurationObjectFromId(configObject.ID); // update de l'objet
            Configuration.LauncherTreeNodeConfigurationNode node = this.Core.BuildTreeNode(root);
            node.SetGUI(gui);
            node.ExpandAll();
            gui.addRootNode(node);
            gui.ShowDialog();
        }


    }
}
