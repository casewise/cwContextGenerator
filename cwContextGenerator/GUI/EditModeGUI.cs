using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Casewise.GraphAPI.PSF;
using Casewise.GraphAPI.Exceptions;
using System.Reflection;
using log4net;
using Casewise.GraphAPI.API;
using log4net.Core;
using System.Configuration;
using cwContextGenerator.Configuration;
using cwContextGenerator.Core;

namespace cwContextGenerator.GUI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EditModeGUI : Form
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(EditModeGUI));

        private cwLightObject _cmObject = null;
        private cwLightModel _model = null;
        private ApplicationCore _core = null;
        private LauncherTreeNodeConfigurationNode _rootNode = null;



        /// <summary>
        /// Initializes a new instance of the <see cref="cwEditModeGUI"/> class.
        /// </summary>
        /// <param name="_model">The _model.</param>
        /// <param name="options">The options.</param>
        public EditModeGUI(cwLightObject configObject, ApplicationCore core)
        {
            InitializeComponent();
            this._cmObject = configObject;
            this._model = configObject.GetObjectType().Model;
            this._core = core;
            this.Icon = Properties.Resources.Casewise_Icon;
            log.Debug("Edit mode GUI Created");
        }

        /// <summary>
        /// Appends the info.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="color">The color.</param>
        public void appendInfo(string message, Color? color = null)
        {
            this.richTextBoxDebug.SelectionStart = richTextBoxDebug.TextLength;
            this.richTextBoxDebug.SelectionLength = 0;
            this.richTextBoxDebug.SelectionColor = color.HasValue ? color.Value : Color.Black;
            this.richTextBoxDebug.AppendText(message + "\n");
            this.richTextBoxDebug.ScrollToCaret();
            this.richTextBoxDebug.Refresh();
            this.richTextBoxDebug.SelectionColor = richTextBoxDebug.ForeColor;
            log.Info(message);
        }

        /// <summary>
        /// Appends the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void appendError(string message)
        {
            this.richTextBoxDebug.AppendText(message);
            this.richTextBoxDebug.ScrollToCaret();
            this.richTextBoxDebug.Refresh();
            log.Error(message);
        }

        /// <summary>
        /// Appends the error.
        /// </summary>
        /// <param name="e">The e.</param>
        public void appendError(Exception e)
        {
            this.richTextBoxDebug.AppendText(e.Message);
            this.richTextBoxDebug.ScrollToCaret();
            this.richTextBoxDebug.Refresh();
            this.appendError(e.ToString());
        }

        /// <summary>
        /// Suspends the layout.
        /// </summary>
        public void suspendLayout()
        {
            this.groupBoxDetails.SuspendLayout();
            this.SuspendLayout();
        }

        /// <summary>
        /// Resumes the layout.
        /// </summary>
        public void resumeLayout()
        {
            this.ResumeLayout();
            this.groupBoxDetails.ResumeLayout();
        }

        /// <summary>
        /// Adds the root node.
        /// </summary>
        /// <param name="rootTreeNode">The root tree node.</param>
        public void addRootNode(LauncherTreeNodeConfigurationNode rootTreeNode)
        {
            this.treeViewConfigurations.Nodes.Add(rootTreeNode);
            this._rootNode = rootTreeNode;
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveConfiguration();
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        private void SaveConfiguration()
        {
            Cursor.Current = Cursors.WaitCursor;
            this._core.SaveConfiguration(this._rootNode, this._cmObject);
            this.appendInfo("Configuration sauvegardée !");
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles the Click event of the executeToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ExecuteConfiguration();
        }

        private void ExecuteConfiguration()
        {
            Cursor.Current = Cursors.WaitCursor;
            this.appendInfo("Exécution de la configuration");
            cwLightObject obj = this._core.GetConfigurationObjectFromId(this._cmObject.ID);
            ConfigurationRootNode root = this._core.GetConfigurationNodeFromDescription(obj);
            this._core.GenerateContextTree(root);
            this.appendInfo("Fin de l'opération");
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Handles the Click event of the treeViewConfigurations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void treeViewConfigurations_Click(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right) || e.Button.Equals(MouseButtons.Left))
            {
                Point p = new Point(e.X, e.Y);
                TreeNode node = this.treeViewConfigurations.GetNodeAt(p);
                if (node != null)
                {
                    this.treeViewConfigurations.SelectedNode = node;
                    if (e.Button.Equals(MouseButtons.Right))
                    {
                        if (node is LauncherTreeNodeObjectNode && !(node is LauncherTreeNodeConfigurationNode))
                        {
                            // display context menu
                            if (this._core.copiedNode == null)
                            {
                                node.ContextMenuStrip.Items[2].Enabled = false;
                            }
                            else
                            {
                                node.ContextMenuStrip.Items[2].Enabled = true;
                            }
                        }
                        node.ContextMenuStrip.Show(p);
                    }
                    else
                    {
                        // display gui
                        LauncherTreeNodeObjectNode n = node as LauncherTreeNodeObjectNode;
                        n.DisplayOptions();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the panel options.
        /// </summary>
        /// <returns></returns>
        public cwPSFTableLayoutPropertiesBoxes GetPanelOptions()
        {
            return this.tableLayoutOptions;
        }

        /// <summary>
        /// Handles the KeyDown event of the EditModeGUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void EditModeGUI_KeyDown(object sender, KeyEventArgs e)
        {
            LauncherTreeNodeObjectNode node = this.treeViewConfigurations.SelectedNode as LauncherTreeNodeObjectNode;
            if (node != null)
            {
                switch (e.KeyData)
                {
                    case Keys.Space:
                        node.DisplayOptions();
                        break;

                    case Keys.Delete:
                        if (!(node is LauncherTreeNodeConfigurationNode))
                        {
                            node.deleteNode();
                        }
                        break;

                    default:
                        if (e.KeyData == (Keys.Control | Keys.S))
                        {
                            this.SaveConfiguration();
                        }
                        else if (e.KeyData == (Keys.Control | Keys.R))
                        {
                            this.ExecuteConfiguration();
                        }
                        break;
                }
            }
        }
    }
}
