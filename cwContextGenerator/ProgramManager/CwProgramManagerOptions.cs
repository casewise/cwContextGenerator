using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.ProgramManager
{
    class CwProgramManagerOptions
    {
        /// <summary>
        /// executingAssembly
        /// </summary>
        public Assembly executingAssembly = null;
        /// <summary>
        /// warningText
        /// </summary>
        public String warningText;
        /// <summary>
        /// applicationName
        /// </summary>
        public String applicationName;
        /// <summary>
        /// applicationLogo
        /// </summary>
        public Bitmap applicationLogo;
        /// <summary>
        /// applicationObjectTypeScriptName
        /// </summary>
        public String applicationObjectTypeScriptName;
        /// <summary>
        /// startUpGUIText
        /// </summary>
        public String startUpGUIText;
        /// <summary>
        /// itemIcon
        /// </summary>
        //public Bitmap itemIcon;
        /// <summary>
        /// itemIconAnimated
        /// </summary>
        //public Bitmap itemIconAnimated;

        /// <summary>
        /// helpURL
        /// </summary>
        public string helpURL = "http://www.casewise.com/";

        /// <summary>
        /// addItemTooltipMessage
        /// </summary>
        //public string addItemTooltipMessage = Properties.Resources.LAUNCHER_OPTIONS_TOOLTIP_ADD_ITEM;

        /// <summary>
        /// Initializes a new instance of the <see cref="cwProgramManagerOptions"/> class.
        /// </summary>
        public CwProgramManagerOptions()
        { 
        }
    }
}
