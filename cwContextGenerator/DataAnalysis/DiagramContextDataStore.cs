using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cwContextGenerator.Data;

namespace cwContextGenerator.DataAnalysis
{
    /// <summary>
    /// 
    /// </summary>
    public class DiagramContextDataStore
    {
        private List<int> _diagramIdList;
        private cwLightModel _model;

        private List<cwLightObject> AllJoinersDataForSelectedDiagrams { get; set; }
        private List<cwLightObject> AllShapesDataForSelectedDiagrams { get; set; }

        private Dictionary<int, List<cwLightObject>> ShapesByDiagramId { get; set; }
        private Dictionary<int, List<cwLightObject>> JoinerByDiagramId { get; set; }

        public Dictionary<int, DiagramContext> DiagramContextByDiagramId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagramIdList"></param>
        /// <param name="model"></param>
        public DiagramContextDataStore(List<int> diagramIdList, cwLightModel model)
        {
            this._diagramIdList = diagramIdList;
            this._model = model;

            this.LoadData();
            this.DispatchData();

            this.FillDiagramContextData();
        }

        /// <summary>
        /// get data from the provider
        /// </summary>
        private void LoadData()
        {
            this.AllJoinersDataForSelectedDiagrams = CwDiagramDataLoader.GetAllJoinerForSelectedDiagrams(_diagramIdList, _model);
            this.AllShapesDataForSelectedDiagrams = CwDiagramDataLoader.GetAllShapesForSelectedDiagrams(_diagramIdList, _model);
        }

        /// <summary>
        /// dispatch data to diagram context use
        /// </summary>
        private void DispatchData()
        {
            this.ShapesByDiagramId = this.DispatchDiagramContextDataByDiagramId(this.AllShapesDataForSelectedDiagrams);
            this.JoinerByDiagramId = this.DispatchDiagramContextDataByDiagramId(this.AllJoinersDataForSelectedDiagrams);
        }


        /// <summary>
        /// create diagram context 
        ///and then pull into DiagramContextDictionary
        /// </summary>
        private void FillDiagramContextData()
        {
            Dictionary<int, DiagramContext> diagramContextDictionary = new Dictionary<int, DiagramContext>();
            foreach (int diagramId in this._diagramIdList)
            {  
                List<cwLightObject> allJoinersByDiagramId = new List<cwLightObject>();
                List<cwLightObject> allShapesByDiagramId = new List<cwLightObject>(); 
                bool hasJoiner = this.JoinerByDiagramId.ContainsKey(diagramId);
                bool hasShape = this.ShapesByDiagramId.ContainsKey(diagramId);

                if (hasJoiner)
                {
                    allJoinersByDiagramId = this.JoinerByDiagramId[diagramId];
                }

                if (hasShape)
                {
                    allShapesByDiagramId = this.ShapesByDiagramId[diagramId];
                }

                DiagramContext newDiagramContext = new DiagramContext(diagramId, allJoinersByDiagramId, allShapesByDiagramId);
                newDiagramContext.LoadShapesDirectory();

                if (hasJoiner || hasShape)
                {
                    diagramContextDictionary[diagramId] = newDiagramContext;
                }
            }

            this.DiagramContextByDiagramId = diagramContextDictionary;

        }

        /// <summary>
        /// dispatch data by diagramId
        /// </summary>
        /// <param name="selectedDiagramData"></param>
        /// <returns></returns>
        private Dictionary<int, List<cwLightObject>> DispatchDiagramContextDataByDiagramId(List<cwLightObject> selectedDiagramData)
        {
            Dictionary<int, List<cwLightObject>> dataByDiagramId = new Dictionary<int, List<cwLightObject>>();

            foreach (cwLightObject data in selectedDiagramData)
            {
                int diagramId = Convert.ToInt32(data.properties["DIAGRAMID"].Value);

                //seperate data by diagram id
                if (!dataByDiagramId.ContainsKey(diagramId))
                {
                    dataByDiagramId[diagramId] = new List<cwLightObject>();
                }

                dataByDiagramId[diagramId].Add(data);
            }

            return dataByDiagramId;
        }
    }
}
