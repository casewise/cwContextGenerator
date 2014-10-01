using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using cwContextGenerator.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{

    public class ContextPackage
    {
        public cwLightObject ContextObject { get; set; }
        public List<CwShape> RelatedShapes { get; set; }
        public List<cwLightObject> RelatedObjects { get; set; }
        public ContextPackage()
        {
            ContextObject = null;
            RelatedShapes = new List<CwShape>();
            RelatedObjects = new List<cwLightObject>();
        }
    }


    public class CwDiagramContextManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CwDiagramContextManager));
        private static string PropertyTypeRootLevel = "ROOTLEVEL";
        private static string PropertyTypeAtName = "ATNAME";
        private static string PropertyTypeAtScriptName = "ATSCRIPTNAME";
        public string[] PropertiesToBySelected
        {
            get { return new string[] { "NAME", PropertyTypeRootLevel, PropertyTypeAtName, PropertyTypeAtScriptName }; }
        }

        private CwContextMataModelManager ContextMetaModel { get; set; }
        private cwLightModel SelectedModel { get; set; }
        private ConfigurationRootNode Config { get; set; }
        private Dictionary<int, cwLightObject> ApprovedLightDiagramsById { get; set; }

        public CwDiagramContextManager(cwLightModel model, ConfigurationRootNode config)
        {
            this.SelectedModel = model;
            this.Config = config;
            this.ContextMetaModel = new CwContextMataModelManager(model);
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
                    this.CreateContextHierarchy(diagram, diagramContext);
                }
            }
        }

        private Dictionary<int, cwLightObject> GetLightDiagramById()
        {
            Dictionary<int, cwLightObject> lightdiagramsById = Config.GetDiagramNode().usedOTLightObjectsByID;
            return lightdiagramsById;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="diagramContext"></param>
        private void CreateContextHierarchy(CwDiagram diagram, DiagramContext diagramContext)
        {
            List<CwShape> parentShapes = new List<CwShape>();
            parentShapes = diagramContext.GetShapesByObject(diagram.Parent);

            foreach (CwShape parentShape in parentShapes)
            {
                ContextPackage newContextObject = CreateContext(parentShape, diagram, this.Config);

                foreach (ConfigurationObjectNode childNode in Config.ChildrenNodes)
                {
                    CreateContextHierarchyRec(parentShape, childNode, newContextObject.ContextObject, diagram);
                }
            }
        }

        private void CreateContextHierarchyRec(CwShape parentShape, ConfigurationObjectNode node, cwLightObject newContextObject, CwDiagram diagram)
        {
            ContextPackage contextObject = CreateContext(parentShape, node, newContextObject, diagram);
            foreach (ConfigurationObjectNode childNode in node.ChildrenNodes)
            {
                foreach (CwShape shape in contextObject.RelatedShapes)
                {
                    CreateContextHierarchyRec(shape, childNode, contextObject.ContextObject, diagram);
                }
            }
        }

        /// <summary>
        /// Name convention  
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="sourceObjectName"></param>
        /// <param name="readingMode"></param>
        /// <returns></returns>
        private string SetContextObjectName(CwDiagram diagram, cwLightObject sourceObjectName, ReadingMode readingMode)
        {
            return diagram.Type.ToString() + "_" + diagram.ToString() + "_" + sourceObjectName.ToString() + "_" + readingMode.ToString();
        }

        /// <summary>
        /// root level
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="diagram"></param>
        /// <returns></returns>
        private ContextPackage CreateContext(CwShape shape, CwDiagram diagram, ConfigurationRootNode rootNode)
        {
            cwLightObject parentObject = shape.GetObject(this.SelectedModel);

            string contextObjectName = SetContextObjectName(diagram, parentObject, Config.ReadingMode);

            int newContextObjectId = this.ContextMetaModel.ContextOT.createObjectWithName(contextObjectName);

            cwLightNodeObjectType contextOTNode = this.ContextMetaModel.ContextOT.getNewNode();
            contextOTNode.addPropertiesToSelect(PropertiesToBySelected);
            contextOTNode.preloadLightObjects();

            cwLightObject newContextObject = contextOTNode.usedOTLightObjectsByID[newContextObjectId];
            newContextObject.properties[PropertyTypeRootLevel].Value = new CwPropertyBoolean(true);
            newContextObject.properties["NAME"].Value = contextObjectName + "_" + newContextObjectId.ToString();

            newContextObject.updatePropertiesInModel();
            newContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextEndWith, parentObject);

            cwLightObject pathObject = this.ContextMetaModel.ContextPathOT.getObjectByID(rootNode.ConfigurationId);
            newContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextPartOfPath, pathObject);

            newContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextDiscribesDiagram, diagram.CmObject);

            ContextPackage contextPackage = new ContextPackage();
            contextPackage.ContextObject = newContextObject;
            //contextPackage.RelatedShapes = targetShapesForTargetObjects;
            return contextPackage;
        }

        /// <summary>
        /// child level
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="childNode"></param>
        /// <param name="parentContextObject"></param>
        /// <param name="diagram"></param>
        /// <returns></returns>
        private ContextPackage CreateContext(CwShape shape, ConfigurationObjectNode childNode, cwLightObject parentContextObject, CwDiagram diagram)
        {
            cwLightNodeAssociationType atNode = childNode.GetNode();
            List<cwLightObject> targetObjects = atNode.GetAllTargetObjects();
            cwLightAssociationType associationType = atNode.AssociationType;
            cwLightObjectType targetObjectType = associationType.Target;
            cwLightObject parentObject = shape.GetObject(this.SelectedModel);

            List<cwLightObject> endObjects = new List<cwLightObject>();
            List<CwShape> targetShapes = new List<CwShape>();
            switch (childNode.ReadingMode)
            {
                case ReadingMode.Includes:
                    shape.ChildrenShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;

                case ReadingMode.IsIncludedIn:
                    shape.ParentsShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;

                case ReadingMode.LinkedTo:
                    List<CwShape> shapesLinkedByJoiner = new List<CwShape>();
                    shape.ToShapesByIntersectionId.TryGetValue(associationType.Intersection.ID, out targetShapes);
                    break;
                default:
                    break;
            }
            ContextPackage contextPackage = new ContextPackage();
            this.GetTargetObjectsOnDiagram(targetShapes, targetObjects, ref contextPackage);
            endObjects = contextPackage.RelatedObjects;
            List<CwShape> targetShapesForTargetObjects = contextPackage.RelatedShapes;

            cwLightObject newContextObject = null;

            if (endObjects.Count > 0)
            {
                string contextObjectName = this.SetContextObjectName(diagram, parentObject, childNode.ReadingMode);

                int newContextObjectId = this.ContextMetaModel.ContextOT.createObjectWithName(contextObjectName);
                cwLightNodeObjectType contextOTNode = this.ContextMetaModel.ContextOT.getNewNode();
                contextOTNode.addPropertiesToSelect(PropertiesToBySelected);
                contextOTNode.preloadLightObjects();

                newContextObject = contextOTNode.usedOTLightObjectsByID[newContextObjectId];
                newContextObject.properties[PropertyTypeAtName].Value = associationType.ToString();
                newContextObject.properties[PropertyTypeAtScriptName].Value = associationType.ScriptName;
                newContextObject.properties["NAME"].Value = contextObjectName + "_" + newContextObjectId.ToString();
                newContextObject.updatePropertiesInModel();

                parentContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextToContext, newContextObject);
                newContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextStartFrom, parentObject);
                foreach (cwLightObject endObject in endObjects)
                {
                    newContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextEndWith, endObject);
                }
            }

            contextPackage.ContextObject = newContextObject;
            contextPackage.RelatedShapes = targetShapesForTargetObjects;
            return contextPackage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetShapes"></param>
        /// <param name="targetObjects"></param>
        /// <returns></returns>
        private void GetTargetObjectsOnDiagram(List<CwShape> targetShapes, List<cwLightObject> targetObjects, ref ContextPackage contextPackage)
        {
            List<cwLightObject> targetObjectsOnDiagram = new List<cwLightObject>();
            List<CwShape> targetShapesForTargetObjects = new List<CwShape>();

            foreach (CwShape targetShape in targetShapes)
            {
                cwLightObject childObject = targetShape.GetObject(this.SelectedModel);
                if (targetObjects.Contains(childObject))
                {
                    targetObjectsOnDiagram.Add(childObject);
                    targetShapesForTargetObjects.Add(targetShape);
                }
            }
            contextPackage.RelatedObjects = targetObjectsOnDiagram;
            contextPackage.RelatedShapes = targetShapesForTargetObjects;
            return;
        }

       
    }
}
