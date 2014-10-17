using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public interface ICwShapeRelationship
    {
        int ShapeId { get; }

        Dictionary<int, List<CwShape>> ChildrenShapesByObjectTypeId { get; set; }
        Dictionary<int, List<CwShape>> DescendantsShapesByObjectTypeId { get; set; }
        Dictionary<int, List<CwShape>> AncestorsShapesByObjectTypeId { get; set; }       
        Dictionary<int, List<CwShape>> ParentsShapesByObjectTypeId { get; set; }
        Dictionary<int, List<CwShape>> ToShapesByIntersectionId { get; set; }

        bool Includes(CwShape insideItem);
    }
}
