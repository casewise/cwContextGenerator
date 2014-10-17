using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class CwDiagramContext
    {
        public int DiagramId { get; set; }
        public Dictionary<int, CwShape> ShapesById { get; set; }
        public Dictionary<int, List<CwShape>> ShapesByObjectTypeId { get; set; }
        public Dictionary<int, Dictionary<int, List<CwShape>>> ShapesByObjectTypeAndObjectId { get; set; }

        public CwDiagramContext(CwDiagramContextLaboratory diagramContextLaboratory)
        {
            this.DiagramId = diagramContextLaboratory.DiagramId;
            this.ShapesById = diagramContextLaboratory.ShapesById;
            this.ShapesByObjectTypeId = diagramContextLaboratory.ShapesByObjectTypeId;
            this.ShapesByObjectTypeAndObjectId = diagramContextLaboratory.ShapesByObjectTypeAndObjectId;
        }


        public List<CwShape> GetShapesByObject(cwLightObject cwObject)
        {
            List<CwShape> shapes = new List<CwShape>();
            this.GetShapesByObjectType(cwObject.GetObjectType()).TryGetValue(cwObject.ID, out shapes);
            return shapes;
        }

        private Dictionary<int, List<CwShape>> GetShapesByObjectType(cwLightObjectType cwObjectType)
        {
            Dictionary<int, Dictionary<int, List<CwShape>>> shapesByObjectTypeAndObjectId = this.ShapesByObjectTypeAndObjectId;

            Dictionary<int, List<CwShape>> shapeByObjectId = new Dictionary<int, List<CwShape>>();
            shapesByObjectTypeAndObjectId.TryGetValue(cwObjectType.ID, out shapeByObjectId);
            return shapeByObjectId;
        }
    }
}
