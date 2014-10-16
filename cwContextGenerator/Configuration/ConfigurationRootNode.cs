using Casewise.GraphAPI.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casewise.GraphAPI.PSF;
using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;

namespace cwContextGenerator.Configuration
{
    public class ConfigurationRootNode : ConfigurationObjectNode
    {
        public int DiagramId { get; set; }
        public int ConfigurationId { get; set; }
        public ConfigurationRootNode()
            : base()
        {
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRootNode"/> class.
        /// </summary>
        public ConfigurationRootNode(cwLightModel model, int id)
            : base(model)
        {
            this.ConfigurationId = id;
        }

        /// <summary>
        /// get object type
        /// </summary>
        /// <returns></returns>
        public new cwLightNodeObjectType GetNode()
        {
            cwLightNodeObjectType node = this.Model.GetObjectTypeNode(this.ObjectTypeScriptName);
            node.selectedPropertiesScriptName = new string[] { "ID", "NAME" }.ToList();
            node.preloadLightObjects();
            return node;
        }

        /// <summary>
        /// get diagram node
        /// </summary>
        /// <returns></returns>
        public cwLightNodeObjectType GetDiagramNode()
        {
            cwLightObject template = this.GetTemplateByDiagramID(this.DiagramId);
            cwLightNodeObjectType node = this.Model.GetObjectTypeNode("DIAGRAM");
            CwPropertyLookup type = template.getPropertyLookup("TYPE");

            CwPropertyLookup diagrammer = template.getPropertyLookup("DIAGRAMMER");
            node.selectedPropertiesScriptName = new string[] { "ID", "NAME", "TYPE", "TABLENUMBER", "OBJECTID", "DIAGRAMMER", "TEMPLATE" }.ToList();
            node.attributeFiltersKeep = this.Filters;

            node.addAttributeForFilterAND("TYPE", type.UUID, Casewise.GraphAPI.Definitions.EnumCWPropertyFilterOperator.EQUAL);
            node.addAttributeForFilterAND("DIAGRAMMER", diagrammer.UUID, Casewise.GraphAPI.Definitions.EnumCWPropertyFilterOperator.EQUAL);
            node.addAttributeForFilterAND("TEMPLATE", false, Casewise.GraphAPI.Definitions.EnumCWPropertyFilterOperator.EQUAL);
            node.preloadLightObjects();
            return node;
        }

        private cwLightObject GetTemplateByDiagramID(int diagramId)
        {
            cwLightNodeObjectType node = this.Model.GetObjectTypeNode("DIAGRAM");
            node.selectedPropertiesScriptName = new string[] { "ID", "NAME", "TYPE", "DIAGRAMMER" }.ToList();
            node.addAttributeForFilterAND("ID", diagramId, Casewise.GraphAPI.Definitions.EnumCWPropertyFilterOperator.EQUAL);
            node.preloadLightObjects();
            return node.usedOTLightObjects.FirstOrDefault();
        }
    }
}
