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
        /// Copies the specified c.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        //public static ConfigurationObjectNode Copy(ConfigurationObjectNode c)
        //{
        //    ConfigurationObjectNode newNode = new ConfigurationObjectNode();
        //    newNode.Name = string.IsNullOrEmpty(c.Name) ? string.Empty : c.Name.Clone().ToString();
        //    newNode.ObjectTypeScriptName = string.IsNullOrEmpty(c.ObjectTypeScriptName) ? string.Empty : c.ObjectTypeScriptName.Clone().ToString();
        //    newNode.AssociationTypeScriptName = string.IsNullOrEmpty(c.AssociationTypeScriptName) ? string.Empty : c.AssociationTypeScriptName.Clone().ToString();
        //    Dictionary<string, List<cwLightNodePropertyFilter>> filter = new Dictionary<string, List<cwLightNodePropertyFilter>>();
        //    foreach (var v in c.Filters)
        //    {
        //        string pScript = v.Key.Clone().ToString();
        //        filter[pScript] = new List<cwLightNodePropertyFilter>();
        //        foreach (cwLightNodePropertyFilter f in v.Value)
        //        {
        //            filter[pScript].Add(new cwLightNodePropertyFilter(f.Value, f.Operator));
        //        }
        //    }
        //    newNode.Filters = filter;

        //    List<ConfigurationObjectNode> children = new List<ConfigurationObjectNode>();
        //    foreach (ConfigurationObjectNode n in c.ChildrenNodes)
        //    {
        //        children.Add(ConfigurationObjectNode.Copy(n));
        //    }
        //    newNode.ChildrenNodes = children;
        //    newNode.ReadingMode = c.ReadingMode;

        //    newNode.Model = c.Model;

        //    return newNode;
        //}

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public cwLightNodeAssociationType GetNode()
        //{
        //    cwLightObjectType sourceOt = this.Model.ObjectTypeManager.GetObjectTypeByScriptName(this.ObjectTypeScriptName);
        //    cwLightAssociationType at = sourceOt.getAssociationTypeByScriptName(this.AssociationTypeScriptName);
        //    cwLightNodeAssociationType node = new cwLightNodeAssociationType(sourceOt, at);

        //    node.selectedPropertiesScriptName = new string[] { "ID", "NAME" }.ToList();
        //    node.attributeFiltersKeep = this.Filters;

        //    node.preloadLightObjects();
        //    //node.preloadLightObjects_Rec();

        //    return node;
        //}


        public cwLightNodeAssociationType GetNode()
        {
            cwLightNodeObjectType OTNode = this.Model.GetObjectTypeNode(this.ObjectTypeScriptName); ;

            OTNode.addPropertiesToSelect(new string[] { "ID", "NAME" });
            cwLightNodeAssociationType ATNode = OTNode.createAssociationNode(this.AssociationTypeScriptName);
            ATNode.attributeFiltersKeep = this.Filters;
            OTNode.preloadLightObjects_Rec();
            return ATNode;
        }


        public cwLightNodeObjectType GetTargetObjectTypeNode()
        {
            cwLightObjectType sourceOt = this.Model.getObjectTypeByScriptName(this.ObjectTypeScriptName);
            cwLightAssociationType at = sourceOt.getAssociationTypeByScriptName(this.AssociationTypeScriptName);
            cwLightNodeObjectType OTTargetNode = this.Model.GetObjectTypeNode(at.Target.ScriptName);
            OTTargetNode.addPropertiesToSelect(new string[] { "ID", "NAME" });
            OTTargetNode.attributeFiltersKeep = this.Filters;
            OTTargetNode.preloadLightObjects();
            return OTTargetNode;
        }
    }
}
