﻿using Casewise.GraphAPI.API;
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
        public ConfigurationRootNode Config { get; private set; }

        private cwPSFPropertyBoxComboBoxTemplate bTemplate { get; set; }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LauncherTreeNodeConfigurationNode"/> class.
        /// </summary>
        public LauncherTreeNodeConfigurationNode(ApplicationCore core, ConfigurationRootNode config)
            : base(core, config)
        {
            this.CreateContextMenu();
            this.Config = config;
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
            this.bFilter = new cwPSFPropertyBoxFilterProperties(null, "Diagram Filtre (ET)", string.Empty, string.Empty);

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
                node.DiagramId = Convert.ToInt32(this.bTemplate.ToString());

            }
            else
            {
                node.Name = this.Text;
            }
        }

        //public bool CheckConfiguration(ConfigurationRootNode node)
        //{
        //    bool approved = true;
        //    if (this.bName == null)
        //    {
        //        this.LoadFromConfigurationObject(node);
        //    }
        //    if (this.bTemplate == null)
        //    {
        //        approved = false;
        //        this.GUI.appendError("Sélectionner obligatoirement un [Diagram Template].");
        //    }
        //    return approved;
        //}

        /// <summary>
        /// Loads from configuration object.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void LoadFromConfigurationObject(ConfigurationObjectNode config)
        {
            base.LoadFromConfigurationObject(config);
            this.bAt.disable();
            this.bReadingPath.disable();
            ConfigurationRootNode n = config as ConfigurationRootNode;
            this.bTemplate.setValue(n.DiagramId);
        }

    }
}
