using cwContextGenerator.Core;
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
        private  ConfigurationObjectNode Config;
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
        public LauncherTreeNodeObjectNode(ApplicationCore core, ConfigurationObjectNode config)
            : this()
        {
            this.Core = core;
            this.Config = config;
            if (!string.IsNullOrEmpty(this.Config.Name))
            {
                this.Text = this.Config.Name;
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
            ToolStripItem douplicateItem = this.ContextMenuStrip.Items.Add("Duplicate node");
            //ToolStripItem cloneItem = this.ContextMenuStrip.Items.Add("Copy");
            //ToolStripItem pasteItem = this.ContextMenuStrip.Items.Add("Paste as child");
            ToolStripItem deleteItem = this.ContextMenuStrip.Items.Add("Delete");
           
           
            addItem.Click += addItem_Click;
           // cloneItem.Click += cloneItem_Click;
            douplicateItem.Click += DuplicateItem_Click;
          //  pasteItem.Click += pasteItem_Click;
            deleteItem.Click += deleteItem_Click;
        }

        /// <summary>
        /// Copy Node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private LauncherTreeNodeObjectNode RecCopyNode(LauncherTreeNodeObjectNode node)
        {
            LauncherTreeNodeObjectNode copy = new LauncherTreeNodeObjectNode(node.Core, node.Config);

            foreach (LauncherTreeNodeObjectNode childNode in node.GetChildren())
            {
                LauncherTreeNodeObjectNode copyChild = RecCopyNode(childNode);
                copy.Nodes.Add(copyChild);
            }
            return copy;
        }

        private void DuplicateItem_Click(object sender, EventArgs e)
        {
            LauncherTreeNodeObjectNode copy = RecCopyNode(this);
           
            this.Parent.Nodes.Add(copy);
        }

        /// <summary>
        /// Handles the Click event of the deleteItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void deleteItem_Click(object sender, EventArgs e)
        {
            this.deleteNode();
        }

        /// <summary>
        /// Deletes the node.
        /// </summary>
        public void deleteNode()
        {
            DialogResult r = MessageBox.Show("Voulez-vous vraiment supprimer ce noeud ?", "Delete Node ?", MessageBoxButtons.OKCancel);
            if (r.Equals(DialogResult.OK))
            {
                this.Remove();
            }
        }

        /// <summary>
        /// Handles the Click event of the pasteItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pasteItem_Click(object sender, EventArgs e)
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
        private void cloneItem_Click(object sender, EventArgs e)
        {
            LauncherTreeNodeObjectNode copy = this.CloneNode();
            this.Core.copiedNode = copy;
        }

        private LauncherTreeNodeObjectNode CloneNode()
        {
            LauncherTreeNodeObjectNode copy = new LauncherTreeNodeObjectNode();
            this.SetupConfigurationObject(this.Config);
            ConfigurationObjectNode copyConfig = ConfigurationObjectNode.Copy(this.Config);
            copy.setCore(this.Core);
            copy.Config = copyConfig;
            copy.Text = this.Text;

            List<LauncherTreeNodeObjectNode> children = this.GetChildren();
            foreach (LauncherTreeNodeObjectNode n in children)
            {
                copy.Nodes.Add(n.CloneNode());
            }
            return copy;
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
            ConfigurationObjectNode config = new ConfigurationObjectNode(this.Core.SelectedModel);
            if (this.bName != null)
            {
                cwLightObjectType target = this.bAt.getSelectedTargetObjectType();
                if (target != null)
                {
                    config.ObjectTypeScriptName = target.ScriptName;
                    config.Name = target.ToString();
                }
                else
                {
                    cwLightObjectType ot = this.bOt.getSelectedObjectType();
                    if (ot != null)
                    {
                        config.ObjectTypeScriptName = ot.ScriptName;
                        config.Name = ot.ToString();
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
                this.LoadFromConfigurationObject(this.Config);
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
                this.bReadingPath = new cwPSFPropertyBoxComboBox(null, "Mode de lecture", string.Empty, string.Empty, Enum.GetNames(typeof(ReadingMode)));

                this.bOt.loadNodes(this.Core.SelectedModel);
                this.bReadingPath.setValue(ReadingMode._None_.ToString());

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
                node.Name = this.Text;
                node.ObjectTypeScriptName = this.bOt.ToString();
                node.AssociationTypeScriptName = this.bAt.ToString();
                node.Filters = this.bFilter.getDataGrid().GetAttributesFiltered();

                node.ReadingMode = (ReadingMode)Enum.Parse(typeof(ReadingMode), this.bReadingPath.getValue().ToString());
              //  this.Config = node;
            }
            
        }

        /// <summary>
        /// Loads from configuration object.
        /// </summary>
        /// <param name="node">The node.</param>
        protected virtual void LoadFromConfigurationObject(ConfigurationObjectNode node)
        {
            this.bName.setValue(node.Name);
            this.bOt.setValue(node.ObjectTypeScriptName);
            this.bAt.setValue(node.AssociationTypeScriptName);
            foreach (var attributesKeep in node.Filters)
            {
                foreach (cwLightNodePropertyFilter attributeFilter in attributesKeep.Value)
                {
                    this.bFilter.getDataGrid().AddDataGridRow(attributeFilter.OperatorString, attributesKeep.Key, attributeFilter.Value);
                }
            }
            this.bReadingPath.setValue(node.ReadingMode.ToString());

            this.bOt.enable();
            this.bAt.enable();
            this.bReadingPath.enable();
        }
    }
}
