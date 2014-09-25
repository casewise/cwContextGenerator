using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class ShapeCouple
    {
        public CwShape Ancestor { get; set; }
        public CwShape Descendant { get; set; }

        public bool AreFamilly
        {
            get
            {
                if (this.Ancestor != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ShapeCouple(CwShape shapeA, CwShape shapeB)
        {
            if (shapeA.Includes(shapeB))
            {
                this.Ancestor = shapeA;
                this.Descendant = shapeB;
            }
            else if (shapeB.Includes(shapeA))
            {
                this.Ancestor = shapeB;
                this.Descendant = shapeA;
            }
        }
    }
}
