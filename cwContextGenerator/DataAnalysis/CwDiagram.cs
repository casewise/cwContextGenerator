using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class CwDiagram
    {
        private int _tableNumber;
        private int _objectId;
        private cwLightModel _model;
        public string Type{get;private set;}
        private string name;
        public cwLightObject CmObject;
        public int ID { get; set; }
        public cwLightObject Parent
        {
            get
            {
                if (this._objectId != 0)
                {
                    cwLightObjectType parentObjectType = _model.getObjectTypeByID(this._tableNumber);
                    cwLightObject parentObject = parentObjectType.getObjectByID(this._objectId);
                    return parentObject;
                }
                return null;
            }
            set { }
        }

        public CwDiagram(cwLightObject diagramData, cwLightModel model)
        {
            this._tableNumber = Convert.ToInt32(diagramData.properties["TABLENUMBER"].Value);
            this._objectId = Convert.ToInt32(diagramData.properties["OBJECTID"].Value);
            this.Type = Convert.ToString(diagramData.properties["TYPE"].Value);
            this.name = Convert.ToString(diagramData.properties["NAME"].Value);
            this.ID = Convert.ToInt32(diagramData.properties["ID"].Value);
            this._model = model;
            this.CmObject = diagramData;
        }

        public override string ToString()
        {
            return this.name.ToString();
        }
    }
}