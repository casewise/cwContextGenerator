﻿using cwContextGenerator.Core;
using cwContextGenerator.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Casewise.GraphAPI.PSF;
using Casewise.GraphAPI.API;
using Casewise.GraphAPI.Definitions;

namespace cwContextGenerator.Configuration
{
    public class LauncherTreeNodeObjectNode : TreeNode
    {
        ConfigurationObjectNode config;
        protected ApplicationCore Core = null;

        bool configNeedToBeLoaded = false;

        protected cwPSFPropertyBoxString bName { get; set; }
        protected cwPSFPropertyBoxComboBoxObjectType bOt { get; set; }
        protected cwPSFPropertyBoxComboBoxAssociationType bAt { get; set; }
        protected cwPSFPropertyBoxFilterProperties bFilter { get; set; }
        protected cwPSFPropertyBoxComboBox bReadingPath { get; set; }


        #region Constructors
        public LauncherTreeNodeObjectNode()
            : base()
        {
            this.Text = "Child node";
            this.CreateContextMenu();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LauncherTreeNodeObjectNode"/> class.
        /// </summary>
        public LauncherTreeNodeObjectNode(ApplicationCore c, ConfigurationObjectNode _config)
            : this()
        {
            this.Core = c;
            this.config = _config;
            if (!string.IsNullOrEmpty(this.config.name))
            {
                this.Text = this.config.name;
            }
        }
        #endregion

        public EditModeGUI GetGui()
        {
            LauncherTreeNodeConfigurationNode node = this.TreeView.Nodes[0] as LauncherTreeNodeConfigurationNode;
            if (node != null)
            {
                return node.GUI;
            }
            return null;
        }

        /// <summary>
        /// Creates the context menu.
        /// </summary>
        public virtual void CreateContextMenu()
        {
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            ToolStripItem addItem = this.ContextMenuStrip.Items.Add("Add child");
            ToolStripItem cloneItem = this.ContextMenuStrip.Items.Add("Copy");
            ToolStripItem pasteItem = this.ContextMenuStrip.Items.Add("Paste as child");
            ToolStripItem deleteItem = this.ContextMenuStrip.Items.Add("Delete");

            addItem.Click += addItem_Click;
            cloneItem.Click += cloneItem_Click;
            pasteItem.Click += pasteItem_Click;
            deleteItem.Click += deleteItem_Click;
        }

        void deleteItem_Click(object sender, EventArgs e)
        {
            this.Remove();
        }

        /// <summary>
        /// Handles the Click event of the pasteItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void pasteItem_Click(object sender, EventArgs e)
        {
            LauncherTreeNodeObjectNode copy = this.Core.copiedNode;
            this.Nodes.Add(copy);
            this.Core.copiedNode = null;
        }

        /// <summary>
        /// Handles the Click event of the cloneItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void cloneItem_Click(object sender, EventArgs e)
        {
            LauncherTreeNodeObjectNode copy = this.Clone() as LauncherTreeNodeObjectNode;
            copy.setCore(this.Core);

            this.Core.copiedNode = copy;
        }

        /// <summary>
        /// Sets the core.
        /// </summary>
        /// <param name="c">The c.</param>
        private void setCore(ApplicationCore c)
        {
            this.Core = c;
            List<LauncherTreeNodeObjectNode> children = this.GetChildren();
            foreach (LauncherTreeNodeObjectNode child in children)
            {
                child.setCore(c);
            }
        }

        /// <summary>
        /// Handles the Click event of the addItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void addItem_Click(object sender, EventArgs e)
        {
            ConfigurationObjectNode config = new ConfigurationObjectNode(this.Core._selectedModel);
            if (this.bName != null)
            {
                cwLightObjectType target = this.bAt.getSelectedTargetObjectType();
                if (target != null)
                {
                    config.otScriptname = target.ScriptName;
                    config.name = target.ToString();
                }
                else
                {
                    cwLightObjectType ot = this.bOt.getSelectedObjectType();
                    if (ot != null)
                    {
                        config.otScriptname = ot.ScriptName;
                        config.name = ot.ToString();
                    }
                }
            }
            this.Nodes.Add(new LauncherTreeNodeObjectNode(this.Core, config));
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns></returns>
        public List<LauncherTreeNodeObjectNode> GetChildren()
        {
            return LauncherTreeNodeObjectNode.GetChildrenNodes(this);
        }

        /// <summary>
        /// Gets the children nodes.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public static List<LauncherTreeNodeObjectNode> GetChildrenNodes(TreeNode root)
        {
            List<LauncherTreeNodeObjectNode> children = new List<LauncherTreeNodeObjectNode>();
            if (root is LauncherTreeNodeConfigurationNode || root is LauncherTreeNodeObjectNode)
            {
                foreach (TreeNode n in root.Nodes)
                {
                    LauncherTreeNodeObjectNode node = n as LauncherTreeNodeObjectNode;
                    children.Add(node);
                }
            }
            return children;
        }

        /// <summary>
        /// Displays the options.
        /// </summary>
        public void DisplayOptions()
        {
            EditModeGUI gui = this.GetGui();
            cwPSFTableLayoutPropertiesBoxes options = gui.GetPanelOptions();

            options.SuspendLayout();
            this.SetPropertiesBoxes(options);
            if (this.configNeedToBeLoaded)
            {
                this.LoadFromConfigurationObject(this.config);
                this.configNeedToBeLoaded = false;
            }
            options.ResumeLayout();
        }

        /// <summary>
        /// Sets the properties boxes.
        /// </summary>
        /// <param name="panel">The panel.</param>
        protected virtual void SetPropertiesBoxes(cwPSFTableLayoutPropertiesBoxes panel)
        {
            if (this.bName == null)
            {
                this.configNeedToBeLoaded = true;
                this.bName = new cwPSFPropertyBoxString(null, "Nom", string.Empty, string.Empty);
                this.bName.Text = this.Text;
                this.bName.TextChanged += bName_TextChanged;
                this.bOt = new cwPSFPropertyBoxComboBoxObjectType(null, "Objet Type", string.Empty, string.Empty, null);
                this.bAt = new cwPSFPropertyBoxComboBoxAssociationType(null, "Association Type", string.Empty, string.Empty, null);
                this.bFilter = new cwPSFPropertyBoxFilterProperties(null, "Filtre (ET)", string.Empty, string.Empty);
                this.bReadingPath = new cwPSFPropertyBoxComboBox(null, "Mode de lecture", string.Empty, string.Empty, Enum.GetNames(typeof(READING_PATH)));

                this.bOt.loadNodes(this.Core._selectedModel);
                this.bReadingPath.setValue(READING_PATH._NONE_.ToString());

                this.bOt.checkBoxChanged(this.bOt_Changed);
                this.bAt.checkBoxChanged(this.bAt_Changed);
            }
            panel.Reset();
            panel.addPropertyBox(this.bName);
            panel.addPropertyBox(this.bOt);
            panel.addPropertyBox(this.bReadingPath);
            panel.addPropertyBox(this.bAt);
            panel.addPropertyBox(this.bFilter);
        }


        /// <summary>
        /// Handles the Changed event of the bOt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void bOt_Changed(object sender, EventArgs e)
        {
            cwLightObjectType ot = this.bOt.getSelectedObjectType();
            this.bAt.loadNodes(ot);
            this.bAt.enable();

            if (this.bAt.getSelectedTargetObjectType() == null)
            {
                this.bFilter.loadNodes(ot);
            }

            this.bName.Text = ot.ToString();
        }

        /// <summary>
        /// Handles the Changed event of the bAt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void bAt_Changed(object sender, EventArgs e)
        {
            cwLightObjectType targetOt = this.bAt.getSelectedTargetObjectType();
            this.bFilter.loadNodes(targetOt);

            cwLightObjectType ot = this.bOt.getSelectedObjectType();
            if (ot != null && targetOt == null)
            {
                this.bFilter.loadNodes(ot);
            }

            if (targetOt != null)
            {
                this.bName.Text = targetOt.ToString() + " <-- " + ot.ToString();
            }
            else
            {
                if (ot != null)
                {
                    this.bName.Text = ot.ToString();
                }
                else
                {
                    this.bName.Text = "Child Node";
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the bName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void bName_TextChanged(object sender, EventArgs e)
        {
            this.Text = bName.Text;
        }

        /// <summary>
        /// Setups the configuration object.
        /// </summary>
        /// <param name="node">The node.</param>
        public void SetupConfigurationObject(ConfigurationObjectNode node)
        {
            if (this.bName != null)
            {
                node.name = this.Text;
                node.otScriptname = this.bOt.ToString();
                node.atScriptname = this.bAt.ToString();
                node.filters = this.bFilter.getDataGrid().GetAttributesFiltered();

                node.readingMode = (READING_PATH)Enum.Parse(typeof(READING_PATH), this.bReadingPath.getValue().ToString());
            }
        }

        /// <summary>
        /// Loads from configuration object.
        /// </summary>
        /// <param name="node">The node.</param>
        protected virtual void LoadFromConfigurationObject(ConfigurationObjectNode node)
        {
            this.bName.setValue(node.name);
            this.bOt.setValue(node.otScriptname);
            this.bAt.setValue(node.atScriptname);
            foreach (var attributesKeep in node.filters)
            {
                foreach (cwLightNodePropertyFilter attributeFilter in attributesKeep.Value)
                {
                    this.bFilter.getDataGrid().AddDataGridRow(attributeFilter.OperatorString, attributesKeep.Key, attributeFilter.Value);
                }
            }
            this.bReadingPath.setValue(node.readingMode.ToString());

            this.bOt.enable();
            this.bAt.enable();
            this.bReadingPath.enable();
        }



    }
}
