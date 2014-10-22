using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using cwContextGenerator.Configuration;
using cwContextGenerator.Logs;
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

        private Dictionary<int, cwLightObject> _approvedLightDiagramsById = new Dictionary<int, cwLightObject>();
        private CwContextMataModelManager _contextMetaModel { get; set; }
        private cwLightModel _selectedModel { get; set; }
        private ConfigurationRootNode _config { get; set; }

        public Dictionary<int, CwDiagramContext> DiagramContextByDiagramId { get; private set; }

        private cwLightObject RootContextObject { get; set; }

        public CwDiagramContextManager(cwLightModel model, ConfigurationRootNode config)
        {
            this._selectedModel = model;
            this._config = config;
            this._contextMetaModel = new CwContextMataModelManager(model);

            this.GetApprovedLightDiagramsById();
            this.GetDiagramContextsFromDataStore();
        }


        public void SetLog()
        {
            CwContextObjectInfo log = new CwContextObjectInfo(this.RootContextObject, this._selectedModel);
            log.SetLog();
        }

        private void GetApprovedLightDiagramsById()
        {
            Dictionary<int, cwLightObject> lightDiagramById = this.GetLightDiagramById();
            this.CheckDiagram(lightDiagramById);
        }

        private Dictionary<int, cwLightObject> GetLightDiagramById()
        {
            Dictionary<int, cwLightObject> lightdiagramsById = _config.GetDiagramNode().usedOTLightObjectsByID;
            return lightdiagramsById;
        }

        private void GetDiagramContextsFromDataStore()
        {
            CwDiagramContextDataStore contextDataStore = new CwDiagramContextDataStore(this._approvedLightDiagramsById.Keys.ToList(), this._selectedModel);
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
                    this._approvedLightDiagramsById[diagramId] = lightDiagram;
                }
            }
        }

        public void CreateContextHierarchy()
        {
            foreach (var d in this._approvedLightDiagramsById)
            {
                int diagramId = d.Key;
                cwLightObject approvedDiagram = d.Value;
                if (!this.DiagramContextByDiagramId.ContainsKey(diagramId))
                {
                    continue;
                }
                else
                {
                    CwDiagram diagram = new CwDiagram(approvedDiagram, this._selectedModel);
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

                    CwContextObjectParameters parameters = new CwContextObjectParameters
                    {
                        Level = level,
                        FromObject = parentObject,
                        FromShape = parentShape,
                        ContextMetaModel = this._contextMetaModel,
                        Diagram = diagram
                    };


                    CwContextObjectRootLevel rootContextObject = new CwContextObjectRootLevel(this._config, parameters);

                    this.RootContextObject = rootContextObject.ContextContainer;
                
                    foreach (ConfigurationObjectNode childNode in this._config.ChildrenNodes)
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
            CwContextObjectParameters parameters = new CwContextObjectParameters
            {
                Level = count,
                FromObject = fromObject,
                FromShape = fromShape,
                ContextMetaModel = this._contextMetaModel,
                Diagram = diagram
            };

            CwContextObject contextObject = new CwContextObject(node, parentContextObject, parameters);

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
            return this._selectedModel.getObjectTypeByID(shape.ObjectTypeId).getObjectByID(shape.ObjectId);
        }
    }
}