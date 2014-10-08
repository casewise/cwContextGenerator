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
    public class CwRootLevelContextObject : CwContextObject
    {
        // public bool IsRootLevel { get { return true; } }

     
        public ConfigurationRootNode RootConfig { get; set; }

        public override string Name
        {
            get
            {

                return String.Format("{0}_{1}_{2}_{3}_{4}",
                                    Diagram.Type.ToString(),
                                    Diagram.ToString(),
                                    FromObject.ToString(),
                                    RootConfig.ReadingMode.ToString(),
                                    this.Id.ToString());
            }
            set { }
        }

        public CwRootLevelContextObject(cwLightObject fromObject, CwShape fromShape, CwContextMataModelManager contextMetaModel, ConfigurationRootNode rootConfigurationNode, CwDiagram diagram)
            : base(fromObject, fromShape, contextMetaModel, diagram)
        {
            this.RootConfig = rootConfigurationNode;
           
            //update 
            this.UpdateProperties();
            this.UpdateAssociations();
        }
       
        protected new void UpdateProperties()
        {
            this.ContextContainer.properties[PropertyTypeRootLevel].Value = new CwPropertyBoolean(true);
            //update name
            this.ContextContainer.properties["NAME"].Value = this.Name;

            this.ContextContainer.updatePropertiesInModel();
        }


        protected new void UpdateAssociations()
        {
            //create associations
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextEndWith, this.FromObject);
            cwLightObject pathObject = this.ContextMetaModel.ContextPathOT.getObjectByID(RootConfig.ConfigurationId);
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextPartOfPath, pathObject);
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextDiscribesDiagram, this.Diagram.CmObject);
        }
    }
}