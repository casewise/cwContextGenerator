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
using System.Web.Script.Serialization;

namespace cwContextGenerator.Configuration
{
    public enum ReadingMode { _None_, Includes, IsIncludedIn, LinkedTo, Directly_Includes, Directly_IsIncludedIn }

    public class ConfigurationObjectNode
    {
        public string Name { get; set; }
        public string ObjectTypeScriptName { get; set; }
        public string AssociationTypeScriptName { get; set; }
        public Dictionary<string, List<cwLightNodePropertyFilter>> Filters { get; set; }
        public List<ConfigurationObjectNode> ChildrenNodes { get; set; }
        public ReadingMode ReadingMode { get; set; }

        protected cwLightModel Model { get; set; }
        public ConfigurationObjectNode()
        {
            this.Name = "Child Node";
            this.ChildrenNodes = new List<ConfigurationObjectNode>();
            this.Filters = new Dictionary<string, List<cwLightNodePropertyFilter>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectNode"/> class.
        /// </summary>
        public ConfigurationObjectNode(cwLightModel model)
            : this()
        {
            this.Model = model;
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
        public void AddChildNode(ConfigurationObjectNode node)
        {
            this.ChildrenNodes.Add(node);
        }

        public cwLightNodeAssociationType GetNode()
        {
            cwLightNodeObjectType OTNode = this.Model.GetObjectTypeNode(this.ObjectTypeScriptName); ;

            OTNode.addPropertiesToSelect(new string[] { "ID", "NAME" });
            cwLightNodeAssociationType ATNode = OTNode.createAssociationNode(this.AssociationTypeScriptName);

            ATNode.addPropertiesToSelect(new string[] { "ID", "NAME" });
            ATNode.attributeFiltersKeep = this.Filters;
            OTNode.preloadLightObjects_Rec();
            return ATNode;
        }
    }
}
