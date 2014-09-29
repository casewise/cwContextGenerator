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
        public cwLightPropertyType Type{get;private set;}
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

        //    this.Type = diagramData.properties["TYPE"].Value;
            this._model = model;
        }
    }
}