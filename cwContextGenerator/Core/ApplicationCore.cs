using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casewise.GraphAPI.API;
using Casewise.GraphAPI;
using System.Reflection;
using System.IO;
using System.Xml;
using Casewise.GraphAPI.API.Graph;
using Casewise.GraphAPI.Definitions;
using cwContextGenerator.Configuration;
using System.Web.Script.Serialization;
using cwContextGenerator.DataAnalysis;

namespace cwContextGenerator.Core
{
    public enum DIAGRAM_NAVIGATION
    {
        Includes, IsIncludedIn, LinkedTo
    };

    public class ApplicationCore
    {
        internal static string CONTEXTPATH_OT = "CWCONTEXTPATH";
        internal static string DIAGRAMTEMPLATE_PT = "DIAGRAMUUID";
        internal static string ATNAME_PT = "ATNAME";
        internal static string ATSCRIPTNAME_PT = "ATSCRIPTNAME";

        cwConnection _connection { get; set; }
        List<cwLightModel> _allModels { get; set; }
        public List<cwLightModel> _notEnabledModels { get; private set; }
        public List<cwLightModel> _enabledModels { get; private set; }
        public cwLightModel SelectedModel { get; set; }

        public LauncherTreeNodeObjectNode copiedNode = null;

        /*
         Values are:
         * -1 = not started
         * -2 = wrong credentials
         * -3 = model does not exist
         * -4 = model not enabled
         * -5 = configuration does not exist
         * 0 = OK
         * 1 = error during execution
         */
        public Int32 ReturnValue { get; private set; }


        #region Constructors
        public ApplicationCore()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationCore"/> class.
        /// </summary>
        /// <param name="conn">The connection.</param>
        public ApplicationCore(string user, string password, string database)
        {
            CwFrameworkLoader.RegisterAssemblyResolveToCasewiseBin();
            if (!string.IsNullOrEmpty(user))
            {
                try
                {
                    this._connection = new cwConnection(database, user, password);
                }
                catch (Exception)
                {
                    this.ReturnValue = -2;
                    return;
                }
            }
            else
            {
                this._connection = new cwConnection();
            }
            this._allModels = this._connection.getModels();

            this._notEnabledModels = new List<cwLightModel>();
            this._enabledModels = new List<cwLightModel>();
        }
        #endregion

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void closeConnection()
        {
            if (this._connection != null)
            {
                this._connection.CloseConnections();
            }
        }

        /// <summary>
        /// Sets the model from filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void SetModelFromFilename(string filename)
        {
            cwLightModel model = this._allModels.Find(m => m.FileName.Equals(filename));
            if (model == null)
            {
                this.ReturnValue = -3;
                return;
            }
            if (this._enabledModels.Exists(mod => mod.FileName.Equals(filename)))
            {
                this.ReturnValue = -4;
                return;
            }
            this.SelectedModel = model;
        }

        /// <summary>
        /// Sets the model as enabled.
        /// </summary>
        /// <param name="m">The m.</param>
        public void SetModelAsEnabled(cwLightModel m)
        {
            int i = 0;
            for (i = 0; i < this._notEnabledModels.Count; i++)
            {
                if (m.FileName.Equals(this._notEnabledModels[i].FileName))
                {
                    break;
                }
            }
            this._notEnabledModels.RemoveAt(i);
            this._enabledModels.Add(m);
        }

        /// <summary>
        /// Gets the configuration object from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public cwLightObject GetConfigurationObjectFromId(int id)
        {
            cwLightModel m = this.SelectedModel;
            cwLightObjectType ot = m.getObjectTypeByScriptName(CONTEXTPATH_OT);
            cwLightObject o = ot.getObjectByID(id);
            if (o == null)
            {
                this.ReturnValue = -5;
            }
            return o;
        }

        /// <summary>
        /// Enables the model.
        /// </summary>
        /// <param name="m">The m.</param>
        public void EnableModel(cwLightModel m)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();
            Stream stream = assembly.GetManifestResourceStream("cwContextGenerator.Resources.ContextGeneratorMetaModel.xml");
            XmlReader reader = XmlReader.Create(stream);

            CasewiseProfessionalServices.Data.cwMetaModelManager manager = new CasewiseProfessionalServices.Data.cwMetaModelManager(Tool.GetModelFromLightModel(m));
            manager.UpdateMetaModel(reader);
            m.loadLightModelContent();
        }

        /// <summary>
        /// Loads the models.
        /// </summary>
        public void LoadModels()
        {
            this._notEnabledModels.Clear();
            this._allModels.ForEach(m =>
            {
                m.loadLightModelContent();
                if (!m.hasObjectTypeByScriptName(ApplicationCore.CONTEXTPATH_OT))
                {
                    this._notEnabledModels.Add(m);
                }
                else
                {
                    this._enabledModels.Add(m);
                }
            });
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        public List<cwLightObject> GetConfigurations(cwLightModel m)
        {
            cwLightObjectType ot = m.getObjectTypeByScriptName(ApplicationCore.CONTEXTPATH_OT);
            return ot.getAllObjects(new string[] { "NAME", "DESCRIPTION" });
        }

        /// <summary>
        /// Creates the configuration.
        /// </summary>
        /// <param name="ot">The ot.</param>
        public cwLightObject CreateConfiguration(cwLightObjectType ot, string newName)
        {
            // ajouter les éléments nécessaire au root node
            ConfigurationRootNode root = new ConfigurationRootNode(this.SelectedModel);
            root.Name = newName;

            StringBuilder serialize = this.SerializeConfiguration(root);
            cwLightObject o = ot.createUsingNameAndGet(newName);
            o.getProperty<CwPropertyMemo>("DESCRIPTION").Value = serialize.ToString();
            o.updatePropertiesInModel();
            return o;
        }

        /// <summary>
        /// Determines whether this instance [can configuration be created] the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool CanConfigurationBeCreated(string name)
        {
            cwLightModel m = this.SelectedModel;
            cwLightObjectType ot = m.getObjectTypeByScriptName(ApplicationCore.CONTEXTPATH_OT);
            return !ot.objectExistsByName(name);
        }

        #region Load
        /// <summary>
        /// Gets the configuration node from description.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public ConfigurationRootNode GetConfigurationNodeFromDescription(cwLightObject o)
        {
            CwPropertyMemo memo = o.getProperty<CwPropertyMemo>("DESCRIPTION");
            JavaScriptSerializer s = new JavaScriptSerializer();
            ConfigurationRootNode root = s.Deserialize(memo.DisplayValue, typeof(ConfigurationRootNode)) as ConfigurationRootNode;
            return root;
        }

        /// <summary>
        /// Builds the tree node.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public LauncherTreeNodeConfigurationNode BuildTreeNode(ConfigurationRootNode config)
        {
            LauncherTreeNodeConfigurationNode root = new LauncherTreeNodeConfigurationNode(this, config);
            List<ConfigurationObjectNode> children = config.ChildrenNodes;
            foreach (ConfigurationObjectNode child in children)
            {
                LauncherTreeNodeObjectNode n = new LauncherTreeNodeObjectNode(this, child);
                this.BrowseNodeToLoadConfiguration(n, child);
                root.Nodes.Add(n);
            }
            return root;
        }

        /// <summary>
        /// Browses the node to load configuration.
        /// </summary>
        /// <param name="father">The father.</param>
        /// <param name="config">The configuration.</param>
        private void BrowseNodeToLoadConfiguration(LauncherTreeNodeObjectNode father, ConfigurationObjectNode config)
        {
            foreach (ConfigurationObjectNode child in config.ChildrenNodes)
            {
                LauncherTreeNodeObjectNode n = new LauncherTreeNodeObjectNode(this, child);
                this.BrowseNodeToLoadConfiguration(n, child);
                father.Nodes.Add(n);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Serializes the configuration.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        private StringBuilder SerializeConfiguration(ConfigurationRootNode root)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            StringBuilder output = new StringBuilder();
            s.Serialize(root, output);
            return output;
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        public void SaveConfiguration(LauncherTreeNodeConfigurationNode rootNode, cwLightObject o)
        {
            ConfigurationRootNode config = new ConfigurationRootNode(this.SelectedModel);
            rootNode.SetupConfigurationObject(config);

            List<LauncherTreeNodeObjectNode> children = rootNode.GetChildren();
            foreach (LauncherTreeNodeObjectNode child in children)
            {
                ConfigurationObjectNode c = new ConfigurationObjectNode(this.SelectedModel);
                this.BrowseNodesToSaveConfiguration(child, c);
                config.AddChildNode(c);
            }

            StringBuilder xml = this.SerializeConfiguration(config);
            o.getProperty<CwPropertyMemo>("DESCRIPTION").Value = xml.ToString();
            o.updatePropertiesInModel();

            //config.ChildrenNodes[0].GetNode();
        }

        /// <summary>
        /// Browses the nodes to save configuration.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="config">The configuration.</param>
        private void BrowseNodesToSaveConfiguration(LauncherTreeNodeObjectNode node, ConfigurationObjectNode config)
        {
            node.SetupConfigurationObject(config);
            List<LauncherTreeNodeObjectNode> children = node.GetChildren();
            foreach (LauncherTreeNodeObjectNode child in children)
            {
                ConfigurationObjectNode c = new ConfigurationObjectNode(this.SelectedModel);
                this.BrowseNodesToSaveConfiguration(child, c);
                config.AddChildNode(c);
            }
        }
        #endregion

        #region Do
        /// <summary>
        /// Generates the context tree.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public void GenerateContextTree(ConfigurationRootNode config)
        {
            this.ReturnValue = -1;
            try
            {
                config.SetModel(this.SelectedModel);

                Dictionary<int, cwLightObject>  diagramNode = config.GetDiagramNode().usedOTLightObjectsByID;
   
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


                // appel du context manager
            }
            catch (Exception)
            {
                this.ReturnValue = 1;
            }
        }
        #endregion

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
