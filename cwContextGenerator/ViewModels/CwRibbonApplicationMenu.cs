using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.MetaModel;
using cwContextGenerator.ViewModels.MenuItem;
using Microsoft.Windows.Controls.Ribbon;
using System.IO;
using System.Reflection;
using System.Xml;

namespace cwContextGenerator.ViewModels
{
    public class CwRibbonApplicationMenu : RibbonApplicationMenu
    {
        private static string CONFIG_ACTIVATE_MODELS = "RootMenuForActivatingModel";
        private static string CONFIG_SELECT_MODELS = "RootMenuForSelectingModel";

        private List<cwLightModel> _flyWeightAllModels;
        private List<cwLightModel> _flyWeightEnabledModels = new List<cwLightModel>();
        private List<cwLightModel> _flyWeighNoneEnableModels = new List<cwLightModel>();
        private RibbonApplicationMenu _mainMenu;

        public CwRibbonApplicationMenu(RibbonApplicationMenu mainMenu, List<cwLightModel> flyWeightAllModels)
        {
            this._mainMenu = mainMenu;
            this._flyWeightAllModels = flyWeightAllModels;
            this.setItems();
        }
        
        private void setItems()
        {
            this._flyWeighNoneEnableModels.Clear();
            this._flyWeightEnabledModels.Clear();
            this.setModelRelatedMenuItems();
        }

        private void setModelRelatedMenuItems()
        {
            this.SeperateEnabledModelsAndNoneEnabledModels();
            CwRibbonApplicationSplitRootMenuItem enabledModelMenuItem = new CwRibbonApplicationSplitRootMenuItem(_flyWeightEnabledModels, CwRibbonApplicationMenu.CONFIG_SELECT_MODELS, "Selectionner un Modèle", SelectModel);
            enabledModelMenuItem.ToolTipDescription = "ToolTipDescription";
            CwRibbonApplicationSplitRootMenuItem activateModelmenuItem = new CwRibbonApplicationSplitRootMenuItem(_flyWeighNoneEnableModels, CwRibbonApplicationMenu.CONFIG_ACTIVATE_MODELS, "Activer un Modèle", EnableMetaModelForContextGenerator);
            enabledModelMenuItem.MinHeight = 300;
            this._mainMenu.Items.Add(enabledModelMenuItem);
            this._mainMenu.Items.Add(activateModelmenuItem);
        }

        private void SeperateEnabledModelsAndNoneEnabledModels()
        {
            foreach (var flyWeightModel in this._flyWeightAllModels)
            {
                flyWeightModel.loadLightModelContent();
                if (flyWeightModel.hasObjectTypeByScriptName("CWCONTEXTNODE"))
                {
                    this._flyWeightEnabledModels.Add(flyWeightModel);
                }
                else
                {
                    this._flyWeighNoneEnableModels.Add(flyWeightModel);
                }
            }
        }

        public int SelectModel(cwLightModel _model)
        {
            //do select model action
            return 0;
        }

        private void SetModelsInMenuItems()
        {
        }

        //activate the model
        public int EnableMetaModelForContextGenerator(cwLightModel _model)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();
            Stream stream = assembly.GetManifestResourceStream("cwContextGenerator.Resources.ContextGeneratorMetaModel.xml");
            XmlReader reader = XmlReader.Create(stream);

            CasewiseProfessionalServices.Data.cwMetaModelManager manager = new CasewiseProfessionalServices.Data.cwMetaModelManager(Tool.GetModelFromLightModel(_model));
            manager.UpdateMetaModel(reader);

            this.setItems();
            return 1;
        }


    }
}