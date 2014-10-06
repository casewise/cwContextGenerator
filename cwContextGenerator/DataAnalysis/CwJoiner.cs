using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casewise.GraphAPI.API;

namespace cwContextGenerator.DataAnalysis
{
    class CwJoiner
    {
        private const int ConnectorTableNumberId = 9225;
        private const int HierarchyLineTableNumberId = 9;

        private int fromShapeId;
        private int toShapeId;
        private int tableNumberId;
        private int objectId;

        public int FromShapeId
        {
            get { return this.fromShapeId; }
        }

        public int ToShapeId{

            get { return this.toShapeId; }
        }

        public int IntersectionId {
            get { return this.tableNumberId; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public CwJoiner(cwLightObject data)
        {
            this.fromShapeId = Convert.ToInt32(data.properties["FROMSEQUENCE"].Value);
            this.toShapeId = Convert.ToInt32(data.properties["TOSEQUENCE"].Value);
            this.tableNumberId = Convert.ToInt32(data.properties["TABLENUMBER"].Value);
            this.objectId = Convert.ToInt32(data.properties["OBJECTID"].Value);
        }

    }
}