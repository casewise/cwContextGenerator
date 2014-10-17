using Casewise.GraphAPI.API;
using cwContextGenerator.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.Compare
{
    public class CwObjectShapeMapping
    {
        public cwLightObject SourceObject { get; set; }
        public List<CwShape> TargetShapes { get; set; }
        public bool NoMapping
        {
            get
            {
                if (this.SourceObject != null & this.TargetShapes.Count != 0)
                {
                    return false;
                }
                else { return true; }
            }
        }

        public CwObjectShapeMapping()
        {
            this.TargetShapes = new List<CwShape>();
        }
    }
}
