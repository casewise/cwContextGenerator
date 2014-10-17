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

    
    public class CwContextObjectRootLevel : CwContextObject
    {
        public ConfigurationRootNode RootConfig { get; set; }

        protected override string Name
        {
            get
            {
                return String.Format("{0}_{1}_L{2}_{3}_RootLevel_{4}",
                                    Diagram.Type.ToString(),
                                    Diagram.ToString(),
                                    Level.ToString(),
                                    FromObject.ToString(),
                                    this.Id.ToString());
            }
            set { }
        }


        #region Constructor


        public CwContextObjectRootLevel(ConfigurationRootNode rootNode, CwContextObjectParameters parameters):base(parameters)
        {
            this.RootConfig = rootNode;
            this.Create();
            //update 
            this.UpdateProperties();
            this.UpdateAssociations();
        }

        #endregion

        #region update related data
        protected override void UpdateProperties()
        {
            //update root level
            this.ContextContainer.properties[PropertyTypeRootLevel].Value = new CwPropertyBoolean(true);
            //update name
            this.ContextContainer.properties[PropertyTypeName].Value = this.Name;

            this.ContextContainer.updatePropertiesInModel();
        }

        protected override void UpdateAssociations()
        {
            //create associations
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextEndWith, this.FromObject);
            cwLightObject pathObject = this.ContextMetaModel.ContextPathOT.getObjectByID(RootConfig.ConfigurationId);
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextPartOfPath, pathObject);
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextDiscribesDiagram, this.Diagram.CmObject);
        }
        #endregion
    }
}