using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using cwContextGenerator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class CwDiagramContextManager
    {

        private cwLightModel SelectedModel { get; set; }
        private ConfigurationRootNode Config { get; set; }
        private Dictionary<int, cwLightObject> ApprovedLightDiagramsById { get; set; }


        public CwDiagramContextManager(cwLightModel model, ConfigurationRootNode config)
        {
            this.SelectedModel = model;
            this.Config = config;
            this.ApprovedLightDiagramsById = new Dictionary<int, cwLightObject>();
        }

        public void DoAnalysis()
        {
            Dictionary<int, cwLightObject> lightDiagramById = this.GetLightDiagramById();

            this.CheckDiagram(lightDiagramById);

            DiagramContextDataStore contextDataStore = new DiagramContextDataStore(this.ApprovedLightDiagramsById.Keys.ToList(), this.SelectedModel);

            foreach (var d in this.ApprovedLightDiagramsById)
            {
                int diagramId = d.Key;
                cwLightObject approvedDiagram = d.Value;
                if (!contextDataStore.DiagramContextByDiagramId.ContainsKey(diagramId))
                {
                    continue;
                }
                else
                {
                    CwDiagram diagram = new CwDiagram(approvedDiagram, this.SelectedModel);
                    DiagramContext diagramContext = contextDataStore.DiagramContextByDiagramId[diagramId];
                    this.CreateContextObjects(diagram, diagramContext);
                }
            }
        }

        private Dictionary<int, cwLightObject> GetLightDiagramById()
        {
            Dictionary<int, cwLightObject> lightdiagramsById = Config.GetDiagramNode().usedOTLightObjectsByID;
            return lightdiagramsById;
        }

        public cwLightObject CreateContext(CwShape shape, CwDiagram diagram)
        {
            cwLightObjectType contextOT = this.SelectedModel.getObjectTypeByScriptName("CWCONTEXTNODE");
            cwLightAssociationType atContextEndWith = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT");
            cwLightObject parentObject = shape.GetObject(this.SelectedModel);
            string contextObjectName = diagram.Type.ToString() + diagram.ToString() + parentObject.properties["NAME"] + shape.ShapeId.ToString() +DateTime.Now;

            cwLightObject newContextObject = contextOT.createUsingNameAndGet(contextObjectName);
            newContextObject.AssociateToWithTransaction(atContextEndWith, parentObject);
            return newContextObject;
        }

        public cwLightObject CreateContext(CwShape shape, ConfigurationObjectNode childNode, cwLightObject parentContextObject, CwDiagram diagram)
        {
            cwLightObjectType contextOT = this.SelectedModel.getObjectTypeByScriptName("CWCONTEXTNODE");
            cwLightAssociationType atContextToContext = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCONTEXTNODEFATHEROFFORWARDTOCWCONTEXTNODE");
            cwLightAssociationType atContextStartFrom = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASSTARTOBJECTFORWARDTOANYOBJECT");
            cwLightAssociationType atContextEndWith = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT");

            Dictionary<int, cwLightObject> targetObjects = childNode.GetNode().usedOTLightObjectsByID;
            cwLightObjectType targetObjectType = childNode.GetAssociationType().Target;
            cwLightObject parentObject = shape.GetObject(this.SelectedModel);

            string contextObjectName = diagram.Type.ToString() + diagram.ToString() + parentObject.properties["NAME"] + shape.ShapeId.ToString()+DateTime.Now;

            cwLightObject newContextObject = contextOT.createUsingNameAndGet(contextObjectName);
            parentContextObject.AssociateToWithTransaction(atContextToContext, newContextObject);
            newContextObject.AssociateToWithTransaction(atContextStartFrom, parentObject);

            switch (childNode.ReadingMode)
            {
                case ReadingMode.Includes:
                    List<CwShape> childShapes = new List<CwShape>();
                    shape.ChildrenShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out childShapes);

                    List<cwLightObject> endObjects = new List<cwLightObject>();
                    foreach (CwShape childShape in childShapes)
                    {
                        cwLightObject childObject = childShape.GetObject(this.SelectedModel);
                        if (targetObjects.ContainsKey(childObject.ID))
                        {
                            endObjects.Add(childObject);
                        }
                    }

                    foreach (cwLightObject endObject in endObjects)
                    {
                        newContextObject.AssociateToWithTransaction(atContextEndWith, endObject);
                    }

                    break;
                case ReadingMode.IsIncludedIn:
                    List<CwShape> parentShapes = new List<CwShape>();
                    shape.ParentsShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out parentShapes);

                    break;
                case ReadingMode.IsLinkWithJoiner:

                    break;
                default:
                    break;
            }

            return newContextObject;
        }

        private void CheckDiagram(Dictionary<int, cwLightObject> lightDiagramById)
        {
            foreach (var d in lightDiagramById)
            {
                int diagramId = d.Key;
                cwLightObject lightDiagram = d.Value;

                int parentObjectId = Convert.ToInt32(lightDiagram.properties["OBJECTID"].Value);
                if (parentObjectId != 0)
                {
                    this.ApprovedLightDiagramsById[diagramId] = lightDiagram;
                }
            }
        }


        public void CreateContextObjects(CwDiagram diagram, DiagramContext diagramContext)
        {
            List<CwShape> parentShapes = new List<CwShape>();
            parentShapes = diagramContext.GetShapesByObject(diagram.Parent);

            foreach (CwShape parentShape in parentShapes)
            {
                cwLightObject newContextObject = CreateContext(parentShape, diagram);

                foreach (ConfigurationObjectNode childNode in Config.ChildrenNodes)
                {
                    CreateContext(parentShape, childNode, newContextObject, diagram);
                }
            }
        }
        //public void CreateContextObjects(CwDiagram diagram, DiagramContext diagramContext)
        //{
        //    List<CwShape> parentShapes = new List<CwShape>();
        //    parentShapes = diagramContext.GetShapesByObject(diagram.Parent);

        //    foreach (CwShape parentShape in parentShapes)
        //    {
        //        cwLightObject parentObjectContext;
        //        parentObjectContext = this.CreateContextObject(parentShape);

        //        List<ConfigurationObjectNode> childrenNode = Config.ChildrenNodes;
        //        foreach (ConfigurationObjectNode childNode in childrenNode)
        //        {
        //            cwLightNodeAssociationType associationTypeNoode = childNode.GetNode();
        //            Dictionary<int, cwLightObject> targetObjectsById = associationTypeNoode.usedOTLightObjectsByID;

        //            cwLightAssociationType associationType = associationTypeNoode.AssociationType;
        //            //cwLightAssociationType associationType = childNode.GetAssociationType(this.SelectedModel);
        //            cwLightObjectType targetObjectType = associationType.Target;

        //            //todo : targetObjectType.targetObjectType.getObjectsByFilter <=childNode Filters                    
        //            List<CwShape> shapesOfTargetObjects = new List<CwShape>();
        //            diagramContext.ShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out shapesOfTargetObjects);

        //            switch (childNode.ReadingMode)
        //            {
        //                case ReadingMode.Includes:

        //                    Dictionary<int, List<CwShape>> includesShapes = parentShape.ChildrenShapesByObjectTypeId;
        //                    List<CwShape> includesTargetShapes = new List<CwShape>();

        //                    includesShapes.TryGetValue(targetObjectType.ID, out includesTargetShapes);
        //                    if (includesTargetShapes == null)
        //                    {
        //                        continue;
        //                    }

        //                    var res = includesTargetShapes.Union(shapesOfTargetObjects, new ShapeComparator());
        //                    if (res.ToList() != null)
        //                    {
        //                        CreateContextObject(parentObjectContext, associationType, includesTargetShapes);
        //                    }

        //                    break;
        //                case ReadingMode.IsIncludedIn:
        //                    Dictionary<int, List<CwShape>> containerShapes = parentShape.ParentsShapesByObjectTypeId;
        //                    List<CwShape> containerTargetShapes = new List<CwShape>();
        //                    containerShapes.TryGetValue(targetObjectType.ID, out containerTargetShapes);
        //                    if (containerTargetShapes == null)
        //                    {
        //                        continue;
        //                    }

        //                    res = containerTargetShapes.Union(shapesOfTargetObjects, new ShapeComparator());
        //                    if (res.ToList() != null)
        //                    {
        //                        CreateContextObject(parentObjectContext, associationType, containerTargetShapes);
        //                    }
        //                    break;
        //                case ReadingMode.IsLinkWithJoiner:
        //                    Dictionary<int, List<int>> toShapesByIntersectaionId = parentShape.ToShapesByIntersectionId;
        //                    List<int> toShapesId = new List<int>();
        //                    List<CwShape> toShapes = new List<CwShape>();

        //                    toShapesByIntersectaionId.TryGetValue(associationType.Intersection.ID, out toShapesId);
        //                    if (toShapes == null)
        //                    {
        //                        continue;
        //                    }

        //                    foreach (int toShapeId in toShapesId)
        //                    {
        //                        CwShape shape = null;
        //                        diagramContext.ShapesById.TryGetValue(toShapeId, out shape);
        //                        if (shape != null)
        //                        {
        //                            toShapes.Add(shape);
        //                        }
        //                    }

        //                    if (toShapes.Count != 0)
        //                    {
        //                        CreateContextObject(parentObjectContext, associationType, toShapes);
        //                    }

        //                    break;
        //                default:

        //                    break;
        //            }
        //        }
        //    }
        //}

        //public cwLightObject CreateContextObject(cwLightObject parentContextObject, cwLightAssociationType at, List<CwShape> childrenShapes)
        //{
        //    cwLightObjectType contextOT = this.SelectedModel.getObjectTypeByScriptName("CWCONTEXTNODE");
        //    cwLightAssociationType atContextToContext = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCONTEXTNODEFATHEROFFORWARDTOCWCONTEXTNODE");
        //    cwLightAssociationType atContextStartFrom = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASSTARTOBJECTFORWARDTOANYOBJECT");
        //    cwLightAssociationType atContextEndWith = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT");

        //    cwLightObject childContextObject = null;
        //    if (childrenShapes.Count > 0)
        //    {
        //        childContextObject = contextOT.createUsingNameAndGet("Context");
        //    }
        //    parentContextObject.AssociateToWithTransaction(atContextToContext, childContextObject, this.SelectedModel);

        //    foreach (CwShape childShape in childrenShapes)
        //    {
        //        cwLightObject childOnObject = childShape.GetObject(this.SelectedModel);
        //        childContextObject.AssociateToWithTransaction(atContextEndWith, childOnObject, this.SelectedModel);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Create context object for parent shape (the root shape) 
        ///// </summary>
        ///// <param name="parentShape"></param>
        ///// <returns></returns>
        //public cwLightObject CreateContextObject(CwShape parentShape)
        //{
        //    return this.CreateContextObject(parentShape, this.SelectedModel);
        //}

        //public cwLightObject CreateContextObject(CwShape parentShape, cwLightModel model)
        //{
        //    cwLightObjectType contextOT = model.getObjectTypeByScriptName("CWCONTEXTNODE");
        //    cwLightAssociationType atContextEndWith = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT");

        //    cwLightObject parentOnObject = parentShape.GetObject(model);
        //    //Dictionary<string, CwProperty> sourceProperties = new Dictionary<string, CwProperty>();

        //    cwLightObject parentContextObject = contextOT.createUsingNameAndGet(parentOnObject.Text + "_" + parentOnObject.ID + "_" + DateTime.Now);

        //    parentContextObject.AssociateToWithTransaction(atContextEndWith, parentOnObject, model);
        //    return parentContextObject;
        //}
    }
}
