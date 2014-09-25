using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.MetaModel;
using Casewise.GraphAPI.API.Model;
using Microsoft.Windows.Controls.Ribbon;

namespace cwContextGenerator.ViewModels.MenuItem
{
    class CwRibbonApplicationSplitModelMenuItem : RibbonApplicationSplitMenuItem
    {
        private static BitmapImage ItemIcon = Tool.GetImage(@"../Resources/Images/Model.png");

        private cwLightModel _model;
        public Func<cwLightModel, int> MenuItemCommand;

        public CwRibbonApplicationSplitModelMenuItem(cwLightModel model, Func<cwLightModel, int> menuItemCommand)
        {
            this._model = model;
            this.Name = model.FileName;
            this.Header = model.ToString();
            this.ImageSource = ItemIcon;
            this.SetItems();
            this.MenuItemCommand = menuItemCommand;
            this.Click += new RoutedEventHandler(CanActivateModel);
        }

        public void CanActivateModel(object sender, RoutedEventArgs e)
        {
            //e.OriginalSource
            //this.EnableMetaModelForContextGenerator(this._model);
           //;
            //   enableModel
            this.MenuItemCommand(this._model);
        }

        public void CanActivateModel(object p, CanExecuteRoutedEventArgs e)
        {

            // return true;
        }

        
        private void SetItems()
        {
            //if (this._model.hasObjectTypeByScriptName(Tool.ObjectTypeNameInCm))
            //{
            //    var objectType = this._model.ObjectTypeManager.GetObjectTypeByScriptName(Tool.ObjectTypeNameInCm);
            //    List<cwLightObject> flyWeightAllObjects = objectType.getAllObjects(new string[] { "NAME", "ID", "UNIQUEIDENTIFIER" });
            //    foreach (cwLightObject flyWeightObject in flyWeightAllObjects)
            //    {
            //        this.Items.Add(new CwRibbonApplicationConfigurationMenuItem(flyWeightObject));
            //    }
            //}
        }
    }
}
