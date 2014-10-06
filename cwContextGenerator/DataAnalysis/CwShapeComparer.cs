using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cwContextGenerator.Data;
namespace cwContextGenerator.DataAnalysis
{
    class ShapeComparator : IEqualityComparer<CwShape>
    {
        public bool Equals(CwShape x, CwShape y)
        {
            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the shape' properties are equal. 
            return x.ShapeId == y.ShapeId;
        }

        public int GetHashCode(CwShape shape)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(shape, null)) return 0;

            //Get hash code for the ShapeId _instance if it is not null. 
            int hashShapeId = shape.ShapeId.GetHashCode();
            return hashShapeId;
        }
    }
}
