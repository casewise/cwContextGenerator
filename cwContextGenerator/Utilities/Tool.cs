using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator
{
    static class Tool
    {
        public static CasewiseProfessionalServices.Data.cwModel GetModelFromLightModel(cwLightModel m)
        {
            CasewiseProfessionalServices.Data.cwConnection conn = new CasewiseProfessionalServices.Data.cwConnection();
            conn.LoadModels();
            return conn.Models[m.FileName];
        }
    }
}
