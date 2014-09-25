using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Casewise.GraphAPI.API;
using Microsoft.Windows.Controls.Ribbon;

namespace cwContextGenerator.ViewModels.MenuItem
{
    class CwRibbonApplicationSplitRootMenuItem : RibbonApplicationSplitMenuItem
    {
        public List<cwLightModel> _lightModels = new List<cwLightModel>();
        public Func<cwLightModel, int> MenuItemCommand;

        public CwRibbonApplicationSplitRootMenuItem(List<cwLightModel> lightModels, string name, string header, Func<cwLightModel, int> menuItemCommand)
        {
            this._lightModels = lightModels;
            this.Header = header;
            this.Name = name;
            this.MenuItemCommand = menuItemCommand;
            this.setItems();
        }

        public void setItems()
        {
            this.Items.Clear();
            foreach (var lightModel in this._lightModels)
            {
                CwRibbonApplicationSplitModelMenuItem modelItem = new CwRibbonApplicationSplitModelMenuItem(lightModel, this.MenuItemCommand);
                this.Items.Add(modelItem);
            }
        }
    }
}