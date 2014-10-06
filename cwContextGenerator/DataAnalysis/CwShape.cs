using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casewise.GraphAPI.API;

namespace cwContextGenerator.DataAnalysis
{
    public class CwShape : ICwShape
    {
        #region datafields
        private float x;
        private float y;
        private float width;
        private float height;
        private float xPlusWidth;
        private float yPlusHeight;
        private int shapeId;
        private int objectTypeId;
        private int objectId;
        private const int ACaseInGrid = 131072;
        #endregion

        #region relation fields
        private List<CwShape> descendants = new List<CwShape>();
        private List<CwShape> parents = new List<CwShape>();
        private List<CwShape> children = new List<CwShape>();
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public int ObjectTypeId
        {
            get { return this.objectTypeId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ObjectId
        {
            get { return this.objectId; }
        }

        public int ShapeId
        {
            get { return this.shapeId; }
          //  set { this.shapeId = value; }
        }

        public List<CwShape> Descendants
        {
            get { return this.descendants; }
            set { this.descendants = value; }
        }

        public List<CwShape> Parents
        {
            get { return this.parents; }
            set { this.parents = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<CwShape> Children
        {
            get { return this.children; }
            set { this.children = value; }
        }

        public Dictionary<int, List<CwShape>> ChildrenShapesByObjectTypeId { get; set; }
        public Dictionary<int, List<CwShape>> ParentsShapesByObjectTypeId { get; set; }

        public Dictionary<int, List<CwShape>> ToShapesByIntersectionId { get; set; }

        /// <summary>
        /// Constructor Shape on a diagram
        /// </summary>
        /// <param name="shapeData"></param>
        public CwShape(cwLightObject shapeData)
        {
            this.shapeId = Convert.ToInt32(shapeData.properties["SEQUENCE"].Value);
            this.objectTypeId = Convert.ToInt32(shapeData.properties["TABLENUMBER"].Value);
            this.objectId = Convert.ToInt32(shapeData.properties["OBJECTID"].Value);
            this.x = Convert.ToSingle(shapeData.properties["X"].Value) / ACaseInGrid;
            this.y = -Convert.ToSingle(shapeData.properties["Y"].Value) / ACaseInGrid;
            this.width = Convert.ToSingle(shapeData.properties["WIDTH"].Value) / ACaseInGrid;
            this.height = Convert.ToSingle(shapeData.properties["HEIGHT"].Value) / ACaseInGrid;
            this.xPlusWidth = (this.x + width);
            this.yPlusHeight = (this.y + height);
        }

        /// <summary>
        /// Implementation of Interface IIncludable
        /// </summary>
        /// <param name="container"></param>
        /// <param name="insideItem"></param>
        /// <returns></returns>
        public bool Includes(CwShape insideItem)
        {
            // bool includes = false;
            if (this.x < insideItem.x
                && this.y < insideItem.y
                && this.xPlusWidth > insideItem.xPlusWidth
                && this.yPlusHeight > insideItem.yPlusHeight
                && this.width > insideItem.width
                && this.height > insideItem.height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get shape On object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public cwLightObject GetObject(cwLightModel model)
        {
            return model.getObjectTypeByID(this.ObjectTypeId).getObjectByID(this.ObjectId);
        }

        public cwLightObjectType GetObjectType(cwLightModel model)
        {
            return model.getObjectTypeByID(this.ObjectTypeId);
        }
    }
}
