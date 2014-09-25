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
        public string DiagramId { get; set; }

        public ConfigurationRootNode()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRootNode"/> class.
        /// </summary>
        public ConfigurationRootNode(cwLightModel model)
            : base(model)
        {
        }

        public new cwLightNodeObjectType GetNode()
        {
            cwLightObjectType ot = this.Model.getObjectTypeByScriptName(this.ObjectTypeScriptName);
            cwLightNodeObjectType node = new cwLightNodeObjectType(ot);
            return node;
        }

        public cwLightNodeObjectType GetDiagramNode()
        {
            cwLightObjectType ot = this.Model.getObjectTypeByScriptName("DIAGRAM");
            cwLightNodeObjectType node = new cwLightNodeObjectType(ot);

            node.selectedPropertiesScriptName = new string[] { "ID", "NAME" }.ToList();
            node.attributeFiltersKeep = this.Filters;

            return node;
        }

    }
}
