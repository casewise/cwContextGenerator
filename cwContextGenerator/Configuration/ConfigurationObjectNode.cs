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
   // public enum ReadingMode { _NONE_, INCLUDES, IS_INCLUDED_IN, IS_LINK_WITH_JOINER }
    public enum ReadingMode { _NONE_, Includes, IsIncludedIn, IsLinkWithJoiner }

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


        public void SetModel(cwLightModel model)
        {
            this.Model = model;
        }
        /// <summary>
        /// Adds the child node.
        /// </summary>
        /// <param name="n">The n.</param>
        public void AddChildNode(ConfigurationObjectNode node)
        {
            this.ChildrenNodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public cwLightNodeAssociationType GetNode()
        {
            cwLightObjectType sourceOt = this.Model.ObjectTypeManager.GetObjectTypeByScriptName(this.ObjectTypeScriptName);
            cwLightAssociationType at = sourceOt.getAssociationTypeByScriptName(this.AssociationTypeScriptName);
            cwLightNodeAssociationType node = new cwLightNodeAssociationType(sourceOt, at);

            node.selectedPropertiesScriptName = new string[] { "ID", "NAME" }.ToList();
            node.attributeFiltersKeep = this.Filters;
            node.preloadLightObjects_Rec();
            return node;
        }

         
        /// <summary>
        /// Get Association Type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public cwLightAssociationType GetAssociationType(cwLightModel model)
        {
            cwLightObjectType sourceOt = model.getObjectTypeByScriptName(this.ObjectTypeScriptName);
            cwLightAssociationType at = sourceOt.getAssociationTypeByScriptName(this.AssociationTypeScriptName);
            return at;
        }

        /// <summary>
        /// get target object type
        /// </summary>
        /// <returns></returns>
        //public cwLightObjectType GetTargetObjectType(cwLightModel model)
        //{
        //    return this.GetAssociationType(model).Target;
        //}


        /// <summary>
        /// get target object type
        /// </summary>
        /// <returns></returns>
        //public cwLightObjectType GetTargetObjectType()
        //{
        //    return this.GetAssociationType(this.Model).Target;
        //}
    }
}
