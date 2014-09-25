using Casewise.GraphAPI.API;
using Casewise.GraphAPI.Definitions;
using cwContextGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    class CwDiagramDataStore
    {
        public Dictionary<string, List<cwLightNodePropertyFilter>> Filters { get; set; }

        public Dictionary<int, cwLightObject> FilteredDiagramsData { get; set; }
        public Dictionary<int, CwDiagram> FilteredDiagrams { get; set; }
        private cwLightModel _model;

        public List<int> DiagramIds { get; private set; }
        public CwDiagramDataStore(Dictionary<string, List<cwLightNodePropertyFilter>> filters, cwLightModel model)
        {
            this._model = model;

            this.Filters = filters;

            this.FilteredDiagramsData = CwDiagramDataLoader.GetFilteredDiagrams(filters, model);
            this.FilteredDiagrams = new Dictionary<int, CwDiagram>();
            this.DispatchDiagramData();
        }

        /// <summary>
        /// dispatch the diagram data to diagram object
        /// </summary>
       private void DispatchDiagramData()
        {
            foreach (var diagramData in this.FilteredDiagramsData)
            {
                CwDiagram newDiagram = new CwDiagram(diagramData.Value, this._model);
                if (newDiagram.Parent != null)
                {
                    this.FilteredDiagrams[diagramData.Key] = newDiagram;
                }
                else
                {
                    //diagram without parent will not be analyzed
                }
            }
            this.DiagramIds = this.FilteredDiagrams.Keys.ToList();
        }
    }
}