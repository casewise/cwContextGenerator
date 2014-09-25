using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cwContextGenerator.ProgramManager;

namespace cwContextGenerator
{
    class CwProgramManager
    {

        public CwProgramManagerOptions options = new CwProgramManagerOptions();
        public CwProgramManager(CwProgramManagerOptions _options){
            this.options = _options;
        }
    }
}
