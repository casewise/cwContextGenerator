﻿using System;
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
using log4net;

namespace cwContextGenerator.Core
{
    public class ApplicationCore
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ApplicationCore));

        internal static string CONTEXTPATH_OT = "CWCONTEXTPATH";
        internal static string DIAGRAMTEMPLATE_PT = "DIAGRAMUUID";
        internal static string ATNAME_PT = "ATNAME";
        internal static string ATSCRIPTNAME_PT = "ATSCRIPTNAME";

        cwConnection Connection { get; set; }
        List<cwLightModel> AllModels { get; set; }
        public List<cwLightModel> NotEnabledModels { get; private set; }
        public List<cwLightModel> EnabledModels { get; private set; }
        public cwLightModel SelectedModel { get; set; }

        public LauncherTreeNodeObjectNode CopiedNode {get;set;}

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
            log4net.Config.XmlConfigurator.Configure();
            CwFrameworkLoader.RegisterAssemblyResolveToCasewiseBin();
            if (!string.IsNullOrEmpty(user))
            {
                try
                {
                    this.Connection = new cwConnection(database, user, password);
                }
                catch (Exception e)
                {
                    log.Error(e);
                    this.ReturnValue = -2;
                    return;
                }
            }
            else
            {
                this.Connection = new cwConnection();
            }
            log.Debug("Connection open. Loading models...");

            List<cwLightModel> allmodels = this.Connection.getModels();
            this.AllModels = new List<cwLightModel>();
            foreach (cwLightModel m in allmodels)
            {
                this.AllModels.Add(this.Connection.getModel(m.FileName));
            }

            this.NotEnabledModels = new List<cwLightModel>();
            this.EnabledModels = new List<cwLightModel>();
            log.Debug("Models loaded");
        }
        #endregion

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void closeConnection()
        {
            log.Debug("Closing connection");
            if (this.Connection != null)
            {
                this.Connection.CloseConnections();
            }
            log.Debug("Connection closed");
        }

        /// <summary>
        /// Sets the model from filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void SetModelFromFilename(string filename)
        {
            log.Debug("Select model " + filename + " for cwContextGenerator operation");
            cwLightModel model = this.AllModels.Find(m => m.FileName.Equals(filename));
            if (model == null)
            {
                this.ReturnValue = -3;
                log.Error("Model " + filename + " does not exist in repository.");
                return;
            }
            if (this.EnabledModels.Exists(mod => mod.FileName.Equals(filename)))
            {
                this.ReturnValue = -4;
                log.Error("Model " + filename + " is not enabled for cwContextGenerator");
                return;
            }

            this.SelectedModel = model;
            this.SelectedModel.loadLightModelContent();

        }

        /// <summary>
        /// Sets the model as enabled.
        /// </summary>
        /// <param name="m">The m.</param>
        public void SetModelAsEnabled(cwLightModel m)
        {
            int i = 0;
            for (i = 0; i < this.NotEnabledModels.Count; i++)
            {
                if (m.FileName.Equals(this.NotEnabledModels[i].FileName))
                {
                    break;
                }
            }
            this.NotEnabledModels.RemoveAt(i);
            this.EnabledModels.Add(m);
        }

        /// <summary>
        /// Gets the configuration object from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public cwLightObject GetConfigurationObjectFromId(int id)
        {

            log.Debug("Getting configuration object with id = " + id);

           
           //this.SelectedModel.loadLightModelContent();
            cwLightModel m = this.SelectedModel;

            cwLightObjectType ot = m.getObjectTypeByScriptName(CONTEXTPATH_OT);
            cwLightObject o = ot.getObjectByID(id);
            if (o == null)
            {
                this.ReturnValue = -5;
                log.Error("Impossible to get configuration object with id = " + id);
                return null;
            }
            log.Debug("Configuration with id = " + id + " loaded");
            return o;
        }

        /// <summary>
        /// Enables the model.
        /// </summary>
        /// <param name="m">The m.</param>
        public void EnableModel(cwLightModel m)
        {
            log.Debug("Activating model " + m.FileName + " for cwContextGenerator operation");
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();
            Stream stream = assembly.GetManifestResourceStream("cwContextGenerator.Resources.ContextGeneratorMetaModel.xml");
            XmlReader reader = XmlReader.Create(stream);
            CasewiseProfessionalServices.Data.cwMetaModelManager manager = new CasewiseProfessionalServices.Data.cwMetaModelManager(Tool.GetModelFromLightModel(m));
            manager.UpdateMetaModel(reader);
            m.loadLightModelContent();
            log.Debug("Meta model updated for model " + m.FileName);
        }

        /// <summary>
        /// Loads the models.
        /// </summary>
        public void LoadModels()
        {
            this.NotEnabledModels.Clear();
            this.AllModels.ForEach(m =>
            {
                m.loadLightModelContent();
                if (!m.hasObjectTypeByScriptName(ApplicationCore.CONTEXTPATH_OT))
                {
                    this.NotEnabledModels.Add(m);
                }
                else
                {
                    this.EnabledModels.Add(m);
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
            log.Debug("Create new configuration \"" + newName + "\"");
            cwLightObject o = ot.createUsingNameAndGet(newName);
            // ajouter les éléments nécessaire au root node
            ConfigurationRootNode root = new ConfigurationRootNode(this.SelectedModel, o.ID);
            root.Name = newName;

            StringBuilder serialize = this.SerializeConfiguration(root);
            
            o.getProperty<CwPropertyMemo>("DESCRIPTION").Value = serialize.ToString();
            o.updatePropertiesInModel();
            log.Debug("Configuration created");
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
            if (root.ConfigurationId == 0)
            {
                root.ConfigurationId = o.ID;
            }
            cwLightModel model = o.GetObjectType().Model;
            root.SetModelForAllNodes(model);
            return root;
        }

        /// <summary>
        /// Builds the tree node.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public LauncherTreeNodeConfigurationNode BuildTreeNode(ConfigurationRootNode config)
        {
            log.Debug("Create ui treenode from configuration");
            LauncherTreeNodeConfigurationNode root = new LauncherTreeNodeConfigurationNode(this, config);
            List<ConfigurationObjectNode> children = config.ChildrenNodes;
            foreach (ConfigurationObjectNode child in children)
            {
                LauncherTreeNodeObjectNode n = new LauncherTreeNodeObjectNode(this, child);
             //   n.LoadFromConfigurationObject(child);
                this.BrowseNodeToLoadConfiguration(n, child);
                root.Nodes.Add(n);
            }
            log.Debug("Treenodes created");
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

        public bool CheckConfiguration(LauncherTreeNodeConfigurationNode rootNode)
        {
            // ConfigurationRootNode config = new ConfigurationRootNode(this.SelectedModel, o.ID);
            return rootNode.CheckConfiguration();
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        public void SaveConfiguration(LauncherTreeNodeConfigurationNode rootNode, cwLightObject o)
        {

            log.Debug("Creating configuration from ui treenode");
            ConfigurationRootNode config = new ConfigurationRootNode(this.SelectedModel, o.ID);

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
            log.Debug("Configuration written in model");
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
                DateTime start = DateTime.Now;
               // this.SelectedModel = this.Connection.getModel(this.SelectedModel.FileName);
                //this.SelectedModel.loadLightModelContent();
                log.Debug("Start operation");

                CwDiagramContextManager diagramContextManager = new CwDiagramContextManager(this.SelectedModel, config);
                diagramContextManager.CreateContextHierarchy();
                diagramContextManager.SetLog();

                // appel du context manager
                log.Debug("Operation done ! Duration : " + DateTime.Now.Subtract(start).ToString());
            }
            catch (Exception e)
            {
                this.ReturnValue = 1;
                log.Error("Error occured during operation.", e);
                return;
            }
        }
        #endregion
    }
}
