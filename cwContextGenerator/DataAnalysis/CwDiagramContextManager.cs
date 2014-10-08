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
    public class CwDiagramContextManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CwDiagramContextManager));

        private CwContextMataModelManager ContextMetaModel { get; set; }
        private cwLightModel SelectedModel { get; set; }
        private ConfigurationRootNode Config { get; set; }
        private Dictionary<int, cwLightObject> ApprovedLightDiagramsById { get; set; }
        public Dictionary<int, CwDiagramContext> DiagramContextByDiagramId { get; private set; }

        public CwDiagramContextManager(cwLightModel model, ConfigurationRootNode config)
        {
            this.SelectedModel = model;
            this.Config = config;
            this.ContextMetaModel = new CwContextMataModelManager(model);
            this.ApprovedLightDiagramsById = new Dictionary<int, cwLightObject>();

            this.GetApprovedLightDiagramsById();
            this.GetDiagramContextsFromDataStore();
        }

        private void GetApprovedLightDiagramsById()
        {
            Dictionary<int, cwLightObject> lightDiagramById = this.GetLightDiagramById();
            this.CheckDiagram(lightDiagramById);
        }

        private Dictionary<int, cwLightObject> GetLightDiagramById()
        {
            Dictionary<int, cwLightObject> lightdiagramsById = Config.GetDiagramNode().usedOTLightObjectsByID;
            return lightdiagramsById;
        }

        private void GetDiagramContextsFromDataStore()
        {
            CwDiagramContextDataStore contextDataStore = new CwDiagramContextDataStore(this.ApprovedLightDiagramsById.Keys.ToList(), this.SelectedModel);
            this.DiagramContextByDiagramId = contextDataStore.DiagramContextByDiagramId;
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

        public void CreateContextHierarchy()
        {
            foreach (var d in this.ApprovedLightDiagramsById)
            {
                int diagramId = d.Key;
                cwLightObject approvedDiagram = d.Value;
                if (!this.DiagramContextByDiagramId.ContainsKey(diagramId))
                {
                    continue;
                }
                else
                {
                    CwDiagram diagram = new CwDiagram(approvedDiagram, this.SelectedModel);
                    CwDiagramContext diagramContext = this.DiagramContextByDiagramId[diagramId];
                    this.CreateContextHierarchy(diagram, diagramContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="diagramContext"></param>
        /// 
        private void CreateContextHierarchy(CwDiagram diagram, CwDiagramContext diagramContext)
        {
            try
            {
                List<CwShape> parentShapes = new List<CwShape>();
                parentShapes = diagramContext.GetShapesByObject(diagram.Parent);

                cwLightObject parentObject = diagram.Parent;
                foreach (CwShape parentShape in parentShapes)
                {
                    int level = 0;

                    CwContextObjectRootLevel rootContextObject = new CwContextObjectRootLevel(level, parentObject, parentShape, ContextMetaModel, this.Config, diagram);
                    foreach (ConfigurationObjectNode childNode in Config.ChildrenNodes)
                    {
                        CreateContextHierarchyRec(rootContextObject.Level, parentObject, parentShape, childNode, rootContextObject.ContextContainer, diagram);
                    }
                }
            }
            catch (Exception e)
            {
                string message = String.Format("{0} {1} {2}",
                    "Les contextes du diagramme", diagram.ToString(),
                    "n'ont pas été générés correctement");
                log.Error(message, e);
            }
        }

        private void CreateContextHierarchyRec(int count, cwLightObject fromObject, CwShape fromShape, ConfigurationObjectNode node, cwLightObject parentContextObject, CwDiagram diagram)
        {
            count += 1;
            CwContextObject contextObject = new CwContextObject(count, fromObject, fromShape, ContextMetaModel, node, parentContextObject, diagram);
         
            foreach (ConfigurationObjectNode childNode in node.ChildrenNodes)
            {
                foreach (CwShape toShape in contextObject.ToShapes)
                {
                    cwLightObject toObject = this.GetCwLightObjectByShape(toShape);
                    CreateContextHierarchyRec(count, toObject, toShape, childNode, contextObject.ContextContainer, diagram);
                }
            }
        }

        private cwLightObject GetCwLightObjectByShape(CwShape shape)
        {
            return this.SelectedModel.getObjectTypeByID(shape.ObjectTypeId).getObjectByID(shape.ObjectId);
        }
    }
}