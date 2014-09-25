using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class DiagramContext
    {
        private List<cwLightObject> AllShapes = new List<cwLightObject>();
        private List<cwLightObject> AllJoiners = new List<cwLightObject>();

        private List<CwShape> shapeList = new List<CwShape>();
        public int DiagramId { get; set; }

        public Dictionary<int, CwShape> ShapesById { get; set; }

        private Dictionary<int, Dictionary<int, List<CwShape>>> AllShapesByObjectTypeAndObjectId
        {
            get
            {
                return this.SetShapesByObjectTypeAndObjectIdDictionary(this.shapeList);
            }
            set { }
        }

        public Dictionary<int, List<CwShape>> ShapesByObjectTypeId
        {
            get
            {
                return DiagramContext.SetShapesByObjectTypeId(this.shapeList);
            }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagramId"></param>
        public DiagramContext(int diagramId)
        {
            this.DiagramId = diagramId;
            this.ShapesById = new Dictionary<int, CwShape>();
            this.AllShapesByObjectTypeAndObjectId = new Dictionary<int, Dictionary<int, List<CwShape>>>();
        }

        public DiagramContext(int diagramId, List<cwLightObject> allJoiners)
            : this(diagramId)
        {
            this.AllJoiners = allJoiners;
        }

        public DiagramContext(int diagramId, List<cwLightObject> allJoiners, List<cwLightObject> allShapes)
            : this(diagramId, allJoiners)
        {
            this.AllShapes = allShapes;
        }

        public void LoadShapesDirectory()
        {
            this.AnalyzeAllShapes();
            this.AnalyzeAllJoiners();
        }

        private void AnalyzeAllShapes()
        {
            //init
            this.InitShapesDirectory();

            //fill shapes' descendants 
            //fill shapes' children
            //fill shapes' parent
            this.FillShapesDescendants();
            this.FillShapesChildren();
            this.FilShapesParents();
        }

        private void InitShapesDirectory()
        {
            Dictionary<int, CwShape> shapes = new Dictionary<int, CwShape>();
            for (int i = 0; i < this.AllShapes.Count; i++)
            {
                foreach (cwLightObject shapeData in this.AllShapes)
                {
                    CwShape shape = new CwShape(shapeData);
                    if (!shapes.ContainsKey(shape.ShapeId))
                    {
                        shapes[shape.ShapeId] = shape;
                    }
                }
            }
            this.ShapesById = shapes;
            shapeList = this.ShapesById.Values.ToList();
        }

        private Dictionary<int, Dictionary<int, List<CwShape>>> SetShapesByObjectTypeAndObjectIdDictionary(List<CwShape> shapes)
        {
            Dictionary<int, Dictionary<int, List<CwShape>>> shapesByObjectTypeAndObjectIdDictionary = new Dictionary<int, Dictionary<int, List<CwShape>>>();
            foreach (CwShape shape in shapes)
            {
                int otId = shape.ObjectTypeId;
                int objectId = shape.ObjectId;

                if (!shapesByObjectTypeAndObjectIdDictionary.ContainsKey(otId))
                {
                    shapesByObjectTypeAndObjectIdDictionary[otId] = new Dictionary<int, List<CwShape>>();
                }
                if (!shapesByObjectTypeAndObjectIdDictionary[otId].ContainsKey(objectId))
                {
                    shapesByObjectTypeAndObjectIdDictionary[otId][objectId] = new List<CwShape>();
                }

                shapesByObjectTypeAndObjectIdDictionary[otId][objectId].Add(shape);
            }
            return shapesByObjectTypeAndObjectIdDictionary;
        }

        private static Dictionary<int, List<CwShape>> SetShapesByObjectTypeId(List<CwShape> shapes)
        {
            Dictionary<int, List<CwShape>> shapesByObjectTypeId = new Dictionary<int, List<CwShape>>();
            foreach (CwShape shape in shapes)
            {
                if (!shapesByObjectTypeId.ContainsKey(shape.ObjectTypeId))
                {
                    shapesByObjectTypeId[shape.ObjectTypeId] = new List<CwShape>();
                } shapesByObjectTypeId[shape.ObjectTypeId].Add(shape);
            }
            return shapesByObjectTypeId;
        }
        /// <summary>
        /// fill shapes' desendants
        /// </summary>
        private void FillShapesDescendants()
        {
            for (int i = 0; i < this.shapeList.Count; i++)
            {
                for (int j = i + 1; j < shapeList.Count; j++)
                {
                    CwShape shapeA = shapeList[i];
                    CwShape shapeB = shapeList[j];

                    ShapeCouple shapeCouple = new ShapeCouple(shapeA, shapeB);

                    //check if two shapes have including relation 
                    //shapeCouple. CoupleId is the acendant shape id
                    if (shapeCouple.AreFamilly)
                    {
                        this.ShapesById[shapeCouple.Ancestor.ShapeId].Descendants.Add(shapeCouple.Descendant);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillShapesChildren()
        {
            foreach (var shape in this.ShapesById)
            {
                CwShape ancestor = shape.Value;

                List<CwShape> descendantsOfAllDescendants = new List<CwShape>();
                foreach (CwShape descendant in ancestor.Descendants)
                {
                    List<CwShape> descendantsOfADescentdant = descendant.Descendants;
                    IEnumerable<CwShape> union = descendantsOfAllDescendants.Union(descendantsOfADescentdant, new ShapeComparator());
                    descendantsOfAllDescendants = union.ToList();

                    //if (this.ShapesById.ContainsKey(descendant.ShapeId))
                    //{
                    //    // List<CwShape> descendantsOfADescentdant = this.ShapesById[descendant.ShapeId].Descendants;       
                    //}

                }
                // implémentent l'interface générique IEqualityComparer<T>. 
                var result = ancestor.Descendants.Except(descendantsOfAllDescendants).ToList();
                //  ascendantShape.Children = result.ToList();
                ancestor.Children = result.ToList();
                ancestor.ChildrenShapesByObjectTypeId = DiagramContext.SetShapesByObjectTypeId(ancestor.Children);
            }
        }

        /// <summary>
        /// when you know a shape's children, then you get the parent of each child
        /// </summary>
        private void FilShapesParents()
        {
            foreach (var shape in this.ShapesById)
            {
                CwShape ancestor = shape.Value;
                foreach (CwShape child in ancestor.Children)
                {
                    var key = child.ShapeId;
                    this.ShapesById[key].Parents.Add(ancestor);
                }
            }

            foreach (var shape in this.ShapesById)
            {
                shape.Value.ParentsShapesByObjectTypeId = DiagramContext.SetShapesByObjectTypeId(shape.Value.Parents);
            }
        }

        /// <summary>
        /// Toshapes by joiner data
        /// </summary>
        private void AnalyzeAllJoiners()
        {
            Dictionary<int, Dictionary<int, List<int>>> toShapesByFromShapeAndIntersectionId = new Dictionary<int, Dictionary<int, List<int>>>();
            //init Dictionary
            foreach (cwLightObject joinerData in this.AllJoiners)
            {
                CwJoiner joiner = new CwJoiner(joinerData);

                if (!toShapesByFromShapeAndIntersectionId.ContainsKey(joiner.FromShapeId))
                {
                    toShapesByFromShapeAndIntersectionId[joiner.FromShapeId] = new Dictionary<int, List<int>>();
                }
                if (!toShapesByFromShapeAndIntersectionId[joiner.FromShapeId].ContainsKey(joiner.IntersectionId))
                {
                    toShapesByFromShapeAndIntersectionId[joiner.FromShapeId][joiner.IntersectionId] = new List<int>();
                }
                toShapesByFromShapeAndIntersectionId[joiner.FromShapeId][joiner.IntersectionId].Add(joiner.ToShapeId);
            }

            //fill to shapes by intersection id
            foreach (var fromShape in toShapesByFromShapeAndIntersectionId)
            {
                this.ShapesById[fromShape.Key].ToShapesByIntersectionId = toShapesByFromShapeAndIntersectionId[fromShape.Key];
            }
        }

        public List<CwShape> GetShapesByObject(cwLightObject cwObject)
        {
            List<CwShape> shapes = new List<CwShape>();
            this.GetShapesByObjectType(cwObject.GetObjectType()).TryGetValue(cwObject.ID, out shapes);
            return shapes;
        }

        private Dictionary<int, List<CwShape>> GetShapesByObjectType(cwLightObjectType cwObjectType)
        {
            Dictionary<int, Dictionary<int, List<CwShape>>> shapesByObjectTypeAndObjectId = this.AllShapesByObjectTypeAndObjectId;
            
            Dictionary<int, List<CwShape>> shapeByObjectId = new Dictionary<int, List<CwShape>>();
            shapesByObjectTypeAndObjectId.TryGetValue(cwObjectType.ID, out shapeByObjectId);
            return shapeByObjectId;
        }
    }
}
