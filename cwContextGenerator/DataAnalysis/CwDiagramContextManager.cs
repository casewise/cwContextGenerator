using Casewise.GraphAPI.API;
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

        private cwLightModel SelectedModel;
        private ConfigurationRootNode config;
        public CwDiagramContextManager(cwLightModel model, ConfigurationRootNode config)
        {

            this.SelectedModel = model;
            this.config = config;

        }
        public void DoLater()
        {

            config.SetModel(this.SelectedModel);

            Dictionary<int, cwLightObject> lightdiagramsById = config.GetDiagramNode().usedOTLightObjectsByID;

            //diagram data
            CwDiagramDataStore diagramDataStore = new CwDiagramDataStore(config.Filters, this.SelectedModel);
            //data on diagram
            DiagramContextDataStore contextDataStore = new DiagramContextDataStore(diagramDataStore.DiagramIds, this.SelectedModel);

            foreach (int diagramId in diagramDataStore.DiagramIds)
            {
                if (!contextDataStore.DiagramContextByDiagramId.ContainsKey(diagramId))
                {
                    continue;
                }

                CwDiagram diagram = diagramDataStore.FilteredDiagrams[diagramId];

                DiagramContext diagramContext = contextDataStore.DiagramContextByDiagramId[diagramId];

                List<CwShape> parentShapes = new List<CwShape>();
                cwLightObject parentObjectContext;
                parentShapes = diagramContext.GetShapesByObject(diagram.Parent);
                if (parentShapes == null)
                {
                    // parent object must have at least one shape on the diagram
                    continue;
                }
                else
                {
                    parentObjectContext = this.CreateContextObject(parentShapes.FirstOrDefault());
                }

                List<ConfigurationObjectNode> childrenNode = config.ChildrenNodes;
                foreach (ConfigurationObjectNode childNode in childrenNode)
                {
                    cwLightAssociationType associationType = childNode.GetAssociationType(this.SelectedModel);
                    cwLightObjectType targetObjectType = associationType.Target;

                    //todo : targetObjectType.targetObjectType.getObjectsByFilter <=childNode Filters                    
                    List<CwShape> targeObjectOnDiagram = new List<CwShape>();
                    diagramContext.ShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targeObjectOnDiagram);

                    switch (childNode.ReadingMode)
                    {
                        case ReadingMode.Includes:
                            foreach (CwShape parentShape in parentShapes)
                            {
                                Dictionary<int, List<CwShape>> includesShapes = parentShape.ChildrenShapesByObjectTypeId;
                                List<CwShape> includesTargetShapes = new List<CwShape>();

                                includesShapes.TryGetValue(targetObjectType.ID, out includesTargetShapes);
                                if (includesTargetShapes == null)
                                {
                                    continue;
                                }

                                var res = includesTargetShapes.Union(targeObjectOnDiagram, new ShapeComparator());
                                if (res.ToList() != null)
                                {
                                    this.CreateContextObject(parentObjectContext, associationType, includesTargetShapes);
                                }
                            }
                            break;
                        case ReadingMode.IsIncludedIn:
                            foreach (CwShape parentShape in parentShapes)
                            {
                                Dictionary<int, List<CwShape>> containerShapes = parentShape.ParentsShapesByObjectTypeId;
                                List<CwShape> containerTargetShapes = new List<CwShape>();
                                containerShapes.TryGetValue(targetObjectType.ID, out containerTargetShapes);
                                if (containerTargetShapes == null)
                                {
                                    continue;
                                }

                                var res = containerTargetShapes.Union(targeObjectOnDiagram, new ShapeComparator());
                                if (res.ToList() != null)
                                {
                                    this.CreateContextObject(parentObjectContext, associationType, containerTargetShapes);
                                }
                            }
                            break;
                        case ReadingMode.IsLinkWithJoiner:
                            foreach (CwShape parentShape in parentShapes)
                            {
                                Dictionary<int, List<int>> toShapesByIntersectaionId = parentShape.ToShapesByIntersectionId;
                                List<int> toShapesId = new List<int>();
                                List<CwShape> toShapes = new List<CwShape>();

                                toShapesByIntersectaionId.TryGetValue(associationType.Intersection.ID, out toShapesId);
                                if (toShapes == null)
                                {
                                    continue;
                                }

                                foreach (int toShapeId in toShapesId)
                                {
                                    CwShape shape = null;
                                    diagramContext.ShapesById.TryGetValue(toShapeId, out shape);
                                    if (shape != null)
                                    {
                                        toShapes.Add(shape);
                                    }
                                }

                                if (toShapes.Count != 0)
                                {
                                    this.CreateContextObject(parentObjectContext, associationType, toShapes);
                                }
                            }
                            break;
                        default:

                            break;
                    }
                }
            }
        }
        public cwLightObject CreateContextObject(cwLightObject parentContextObject, cwLightAssociationType at, List<CwShape> childrenShapes)
        {
            cwLightObjectType contextOT = this.SelectedModel.getObjectTypeByScriptName("CWCONTEXTNODE");
            cwLightAssociationType atContextToContext = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCONTEXTNODEFATHEROFFORWARDTOCWCONTEXTNODE");
            cwLightAssociationType atContextStartFrom = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASSTARTOBJECTFORWARDTOANYOBJECT");
            cwLightAssociationType atContextEndWith = contextOT.getAssociationTypeByScriptName("CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT");

            cwLightObject childContextObject = null;
            if (childrenShapes.Count > 0)
            {
                childContextObject = contextOT.createUsingNameAndGet("Context");
            }
            parentContextObject.AssociateToWithTransaction(atContextToContext, childContextObject, this.SelectedModel);

            foreach (CwShape childShape in childrenShapes)
            {
                cwLightObject childOnObject = childShape.GetOnObject(this.SelectedModel);
                childContextObject.AssociateToWithTransaction(atContextEndWith, childOnObject, this.SelectedModel);
            }
            return null;
        }

        public cwLightObject CreateContextObject(CwShape parentShape)
        {
            return this.CreateContextObject(parentShape, this.SelectedModel);
        }

        public cwLightObject CreateContextObject(CwShape parentShape, cwLightModel model)
        {
            cwLightObjectType contextOT = model.getObjectTypeByScriptName("CWCONTEXTNODE");
            cwLightObject parentOnObject = parentShape.GetOnObject(model);
            Dictionary<string, CwProperty> sourceProperties = new Dictionary<string, CwProperty>();
            cwLightObject parentContextObject = contextOT.createUsingNameAndGet(parentOnObject.Text + "_" + parentOnObject.ID + "_" + DateTime.Now);
            return parentContextObject;
        }
    }
}
