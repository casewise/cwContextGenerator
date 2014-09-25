using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casewise.GraphAPI.API;

namespace cwContextGenerator.DataAnalysis
{
    class CwJoiner
    {
        private const int ConnectorTableNumberId = 9225;
        private const int HierarchyLineTableNumberId = 9;

        private int fromShapeId;
        private int toShapeId;
        private int tableNumberId;
        private int objectId;

        public int FromShapeId
        {
            get { return this.fromShapeId; }
        }

        public int ToShapeId{

            get { return this.toShapeId; }
        }

        public int IntersectionId {
            get { return this.tableNumberId; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public CwJoiner(cwLightObject data)
        {
            this.fromShapeId = Convert.ToInt32(data.properties["FROMSEQUENCE"].Value);
            this.toShapeId = Convert.ToInt32(data.properties["TOSEQUENCE"].Value);
            this.tableNumberId = Convert.ToInt32(data.properties["TABLENUMBER"].Value);
            this.objectId = Convert.ToInt32(data.properties["OBJECTID"].Value);
        }
        //public cwLightAssociationType AssociationType;

        //public cwLightAssociationType getAssociationType(cwLightModel model, ShapeManager shapeManager)
        //{
        //    //Dictionary<int, Dictionary<int, cwLightAssociationType>> associationTypeByIntersectionAndSourceObjectId = new Dictionary<int,Dictionary<int,cwLightAssociationType>>();
        //    //foreach (cwLightObjectType ot in model.getObjectTypes())
        //    //{
        //    //    if (ot.isIntersectionObjectType)
        //    //    {
        //    //       // ot.isIntersectionObjectType

        //    //    }
        //    //}
        //    cwLightObjectType otSource = shapeManager.GetShapeObjectByShapeId(this._fromShapeId).GetObjectType();
        //    SortedDictionary<cwLightObjectType, List<cwLightAssociationType>> associationTypesByTargetObjectType = otSource.getAssociationTypesByTargetObjectType();
        //    List<cwLightAssociationType> assocationTypes = associationTypesByTargetObjectType[shapeManager.GetShapeObjectByShapeId(this._toShapeId).GetObjectType()];

        //    foreach (cwLightAssociationType at in assocationTypes)
        //    {
        //        if (at.Intersection.ID == this._tableNumberId)
        //        {
        //            return at;
        //        }
        //    }
        //    return null;
        //}
        /// <summary>
        /// if the joiner is an association type on the diagram
        /// </summary>
        //public bool IsAssicationTypeLine
        //{
        //    get { return (this._tableNumberId != ConnectorTableNumberId && this._tableNumberId != HierarchyLineTableNumberId); }
        //}

        ///// <summary>
        ///// if the joiner is a connector on the diagram
        ///// </summary>
        //public bool IsConnector
        //{
        //    get { return this._tableNumberId == ConnectorTableNumberId; }
        //}
    }
}