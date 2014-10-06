using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class CwDiagramContextLaboratory
    {
        private List<cwLightObject> AllShapes = new List<cwLightObject>();
        private List<cwLightObject> AllJoiners = new List<cwLightObject>();
        private List<CwShape> shapeList = new List<CwShape>();

        public int DiagramId { get; set; }
        public Dictionary<int, CwShape> ShapesById { get; set; }
        public Dictionary<int, Dictionary<int, List<CwShape>>> ShapesByObjectTypeAndObjectId
        {
            get
            {
                return CwDiagramContextLaboratory.ShapeListToShapeDictionaryByObjectTypeAndObjectId(this.shapeList);
            }
            set { }
        }
        public Dictionary<int, List<CwShape>> ShapesByObjectTypeId
        {
            get
            {
                return CwDiagramContextLaboratory.ShapesListToDictionaryByObjectTypeId(this.shapeList);
            }
            set { }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="diagramId"></param>
        public CwDiagramContextLaboratory(int diagramId)
        {
            this.DiagramId = diagramId;

            // relationship data container//shapes container
            this.ShapesById = new Dictionary<int, CwShape>();
            this.ShapesByObjectTypeAndObjectId = new Dictionary<int, Dictionary<int, List<CwShape>>>();

        }

        /// <summary>
        /// overridded Constructor
        /// </summary>
        /// <param name="diagramId"></param>
        /// <param name="allJoinersData"></param>
        /// <param name="allShapesData"></param>
        public CwDiagramContextLaboratory(int diagramId, List<cwLightObject> allJoinersData, List<cwLightObject> allShapesData)
            : this(diagramId)
        {
            // self data
            this.AllJoiners = allJoinersData;
            this.AllShapes = allShapesData;

            // if the data available, init shapes container
            this.InitShapesDirectory();
        }


        /// <summary>
        /// initilize the shapes directory 
        /// set a shape list
        /// </summary>
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
            this.shapeList = this.ShapesById.Values.ToList();
        }

        /// <summary>
        /// By analyzing the shapes data and joiners data,  
        /// determine the relationship with other shapes on the same diagram 
        /// </summary>
        public void AnalyzeShapesRelationship()
        {
            this.AnalyzeAllShapes();
            this.AnalyzeAllJoiners();
        }

        private void AnalyzeAllShapes()
        {
            //fill shapes' descendants 
            //fill shapes' children
            //fill shapes' parent
            this.FillShapesDescendants();
            this.FillShapesChildren();
            this.FilShapesParents();
        }


        /// <summary>
        /// turn shape list collection to shape dictionary collection by object type id and object id
        /// </summary>
        /// <param name="shapes">Shape list</param>
        /// <returns> Two dimension dictionary -> Shapes by object type id and object id </returns>
        private static Dictionary<int, Dictionary<int, List<CwShape>>> ShapeListToShapeDictionaryByObjectTypeAndObjectId(List<CwShape> shapes)
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


        /// <summary>
        /// Turn shape list collection to dictionary collection
        /// Key : object type id of a shape
        /// Value : CwShape
        /// </summary>
        /// <param name="shapes">Shape list</param>
        /// <returns> Shape Dictionary by object type id</returns>
        private static Dictionary<int, List<CwShape>> ShapesListToDictionaryByObjectTypeId(List<CwShape> shapes)
        {
            Dictionary<int, List<CwShape>> shapesByObjectTypeId = new Dictionary<int, List<CwShape>>();
            foreach (CwShape shape in shapes)
            {
                if (!shapesByObjectTypeId.ContainsKey(shape.ObjectTypeId))
                {
                    shapesByObjectTypeId[shape.ObjectTypeId] = new List<CwShape>();
                }
                shapesByObjectTypeId[shape.ObjectTypeId].Add(shape);
            }
            return shapesByObjectTypeId;
        }
        #region shape's relationship analysis
        /// <summary>
        //  shape's descendants
        /// </summary>
        private void FillShapesDescendants()
        {
            for (int i = 0; i < this.shapeList.Count; i++)
            {
                for (int j = i + 1; j < shapeList.Count; j++)
                {
                    CwShape shapeA = shapeList[i];
                    CwShape shapeB = shapeList[j];

                    CwShapeCouple shapeCouple = new CwShapeCouple(shapeA, shapeB);

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
        /// shape's children
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
                ancestor.ChildrenShapesByObjectTypeId = CwDiagramContextLaboratory.ShapesListToDictionaryByObjectTypeId(ancestor.Children);
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
                shape.Value.ParentsShapesByObjectTypeId = CwDiagramContextLaboratory.ShapesListToDictionaryByObjectTypeId(shape.Value.Parents);
            }
        }

        /// <summary>
        /// Toshapes by joiner data
        /// </summary>
        private void AnalyzeAllJoiners()
        {
            Dictionary<int, Dictionary<int, List<CwShape>>> toShapesByFromShapeAndIntersectionId = new Dictionary<int, Dictionary<int, List<CwShape>>>();

            //init Dictionary
            foreach (cwLightObject joinerData in this.AllJoiners)
            {
                CwJoiner joiner = new CwJoiner(joinerData);

                int fromId = joiner.FromShapeId;
                int toId = joiner.ToShapeId;
                int intersectionId = joiner.IntersectionId;
                CwShape fromShape = this.ShapesById[fromId];
                CwShape toShape = this.ShapesById[toId];


                this.FillToShapesByFromShapeAndIntersectionIdDictionary(ref toShapesByFromShapeAndIntersectionId, fromId, intersectionId, toShape);
                this.FillToShapesByFromShapeAndIntersectionIdDictionary(ref  toShapesByFromShapeAndIntersectionId, toId, intersectionId, fromShape);

                //fill to shapes by intersection id
                foreach (var fromItem in toShapesByFromShapeAndIntersectionId)
                {
                    this.ShapesById[fromItem.Key].ToShapesByIntersectionId = toShapesByFromShapeAndIntersectionId[fromItem.Key];
                }
            }
        }

        private void FillToShapesByFromShapeAndIntersectionIdDictionary(ref Dictionary<int, Dictionary<int, List<CwShape>>> toShapesByFromShapeAndIntersectionId, int fromShapeId, int intersectionId, CwShape toShape)
        {
            if (!toShapesByFromShapeAndIntersectionId.ContainsKey(fromShapeId))
            {
                toShapesByFromShapeAndIntersectionId[fromShapeId] = new Dictionary<int, List<CwShape>>();
            }

            if (!toShapesByFromShapeAndIntersectionId[fromShapeId].ContainsKey(intersectionId))
            {
                toShapesByFromShapeAndIntersectionId[fromShapeId][intersectionId] = new List<CwShape>();
            }

            toShapesByFromShapeAndIntersectionId[fromShapeId][intersectionId].Add(toShape);
        }
        #endregion
    }
}
