using Casewise.GraphAPI.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casewise.GraphAPI.PSF;
using System.Windows.Forms;
using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;

namespace cwContextGenerator.Configuration
{
    public enum READING_PATH { _NONE_, INCLUDES, IS_INCLUDED_IN, IS_LINK_WITH_JOINER }

    public class ConfigurationObjectNode
    {
        public string name { get; set; }
        public string otScriptname { get; set; }
        public string atScriptname { get; set; }
        public Dictionary<string, List<cwLightNodePropertyFilter>> filters { get; set; }
        public List<ConfigurationObjectNode> ChildrenNodes { get; set; }
        public READING_PATH readingMode { get; set; }

        protected cwLightModel Model { get; set; }

        public ConfigurationObjectNode()
        {
            this.name = "Child Node";
            this.ChildrenNodes = new List<ConfigurationObjectNode>();
            this.filters = new Dictionary<string, List<cwLightNodePropertyFilter>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectNode"/> class.
        /// </summary>
        public ConfigurationObjectNode(cwLightModel m)
            : this()
        {
            this.Model = m;
        }

        /// <summary>
        /// Sets the model for all nodes.
        /// </summary>
        /// <param name="m">The m.</param>
        public void SetModelForAllNodes(cwLightModel m)
        {
            this.Model = m;
            foreach (ConfigurationObjectNode node in this.ChildrenNodes)
            {
                node.SetModelForAllNodes(m);
            }
        }

        /// <summary>
        /// Adds the child node.
        /// </summary>
        /// <param name="n">The n.</param>
        public void AddChildNode(ConfigurationObjectNode n)
        {
            this.ChildrenNodes.Add(n);
        }

        public cwLightNodeAssociationType GetNode()
        {
            cwLightObjectType sourceOt = this.Model.ObjectTypeManager.GetObjectTypeByScriptName(this.otScriptname);
            cwLightAssociationType at = sourceOt.getAssociationTypeByScriptName(atScriptname);
            cwLightNodeAssociationType node = new cwLightNodeAssociationType(sourceOt, at);

            node.selectedPropertiesScriptName = new string[] {"ID", "NAME" }.ToList();
            node.attributeFiltersKeep = this.filters;

            return node;
        }

    }
}
