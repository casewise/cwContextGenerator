using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using Casewise.GraphAPI.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.Data
{
    public static class CwDiagramDataLoader
    {
        /// <summary>
        /// get all shapes on a diagram
        /// </summary>
        /// <param name="diagramId">DIAGRAMID</param>
        /// <param name="model">model</param>
        /// <returns></returns>
        public static List<cwLightObject> GetAllShapesForSelectedDiagrams(List<int> diagramIdList, cwLightModel model)
        {
            string[] propertiesToLoad = new string[] { "SEQUENCE", "X", "Y", "WIDTH", "HEIGHT", "DIAGRAMID", "TABLENUMBER", "OBJECTID" };
            cwLightNodeObjectType node = model.GetObjectTypeNode("SHAPE");
            node.addPropertiesToSelect(propertiesToLoad);
            node.addAttributeForFilterAND("DIAGRAMID", diagramIdList, Casewise.GraphAPI.Definitions.EnumCWPropertyFilterOperator.IN);
            node.preloadLightObjects();
            return node.usedOTLightObjects;
        }


        public static List<cwLightObject> GetAllJoinerForSelectedDiagrams(List<int> diagramIdList, cwLightModel model)
        {
            string[] propertiesToLoad = new string[] { "SEQUENCE", "DIAGRAMID", "FROMSEQUENCE", "TOSEQUENCE", "TABLENUMBER", "OBJECTID" };
            cwLightNodeObjectType node = model.GetObjectTypeNode("JOINER");
            node.addPropertiesToSelect(propertiesToLoad);
            node.addAttributeForFilterAND("DIAGRAMID", diagramIdList, Casewise.GraphAPI.Definitions.EnumCWPropertyFilterOperator.IN);
            node.preloadLightObjects();

            return node.usedOTLightObjects;
        }
    }
}
