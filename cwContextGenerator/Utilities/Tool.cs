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
    public static class Tool
    {
        public static CasewiseProfessionalServices.Data.cwModel GetModelFromLightModel(cwLightModel m)
        {
            CasewiseProfessionalServices.Data.cwConnection connection = new CasewiseProfessionalServices.Data.cwConnection();
            connection.LoadModels();
            return connection.Models[m.FileName];
        }
    }
    

}