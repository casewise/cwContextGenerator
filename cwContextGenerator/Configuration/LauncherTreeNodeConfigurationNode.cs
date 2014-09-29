using Casewise.GraphAPI.API;
using Casewise.GraphAPI.PSF;
using cwContextGenerator.Core;
using cwContextGenerator.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwContextGenerator.Configuration
{
    public class LauncherTreeNodeConfigurationNode : LauncherTreeNodeObjectNode
    {
        public EditModeGUI GUI { get; private set; }
        public ConfigurationRootNode config { get; private set; }

        cwPSFPropertyBoxComboBoxTemplate bTemplate { get; set; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LauncherTreeNodeConfigurationNode"/> class.
        /// </summary>
        public LauncherTreeNodeConfigurationNode(ApplicationCore _c, ConfigurationRootNode _config)
            : base(_c, _config)
        {
            this.CreateContextMenu();
            this.config = _config;
        }
        #endregion

        /// <summary>
        /// Sets the GUI.
        /// </summary>
        /// <param name="gui">The GUI.</param>
        public void SetGUI(EditModeGUI gui)
        {
            this.GUI = gui;
        }

        ///// <summary>
        ///// Creates the context menu.
        ///// </summary>
        public override void CreateContextMenu()
        {
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            ToolStripItem addItem = this.ContextMenuStrip.Items.Add("Add child");

            addItem.Click += addItem_Click;
        }

        /// <summary>
        /// Sets the properties boxes.
        /// </summary>
        /// <param name="panel">The panel.</param>
        protected override void SetPropertiesBoxes(cwPSFTableLayoutPropertiesBoxes panel)
        {
            base.SetPropertiesBoxes(panel);
            if (this.bTemplate == null)
            {
                this.bTemplate = new cwPSFPropertyBoxComboBoxTemplate(null, "Template", string.Empty, string.Empty, this.Core.SelectedModel);
                this.bAt.disable();
                this.bReadingPath.disable();

                this.bFilter.loadNodes(this.Core.SelectedModel.getObjectTypeByScriptName("DIAGRAM"));
            }
            panel.addPropertyBox(this.bTemplate);
        }

        /// <summary>
        /// Handles the Changed event of the bOt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected override void bOt_Changed(object sender, EventArgs e)
        {
            cwLightObjectType ot = this.bOt.getSelectedObjectType();
            this.bName.Text = ot.ToString();
        }

        /// <summary>
        /// Setups the configuration object.
        /// </summary>
        /// <param name="node">The node.</param>
        public void SetupConfigurationObject(ConfigurationRootNode node)
        {
            if (this.bName != null)
            {
                base.SetupConfigurationObject(node);
                node.DiagramId = this.bTemplate.ToString();
            }
            else
            {
                node.Name = this.Text;
            }
        }

        /// <summary>
        /// Loads from configuration object.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void LoadFromConfigurationObject(ConfigurationObjectNode node)
        {
            base.LoadFromConfigurationObject(node);
            this.bAt.disable();
            this.bReadingPath.disable();
            ConfigurationRootNode n = node as ConfigurationRootNode;
            this.bTemplate.setValue(n.DiagramId);
        }

    }
}
