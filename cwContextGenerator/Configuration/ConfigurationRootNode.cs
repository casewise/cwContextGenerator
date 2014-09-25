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
        public string diagramId { get; set; }

        public ConfigurationRootNode()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRootNode"/> class.
        /// </summary>
        public ConfigurationRootNode(cwLightModel m)
            : base(m)
        {
        }

        public new cwLightNodeObjectType GetNode()
        {
            cwLightObjectType ot = this.Model.ObjectTypeManager.GetObjectTypeByScriptName(this.otScriptname);
            cwLightNodeObjectType node = new cwLightNodeObjectType(ot);

            return node;
        }

        public cwLightNodeObjectType GetDiagramNode()
        {
            cwLightObjectType ot = this.Model.ObjectTypeManager.GetObjectTypeByScriptName("DIAGRAM");
            cwLightNodeObjectType node = new cwLightNodeObjectType(ot);

            return node;
        }


    }
}
