using Casewise.GraphAPI.API;
using cwContextGenerator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.Core
{
    public class BatchManager
    {
        ApplicationCore Core { get; set; }

        public string User { get; private set; }
        public string Password { get; private set; }
        public string Database { get; private set; }
        public string ModelFilename { get; private set; }
        public int ConfigurationId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchManager"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public BatchManager(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string[] _arg = args[i].Split('=');
                switch (_arg[0])
                {
                    case "-model":
                        this.ModelFilename = _arg[1];
                        break;
                    case "-configuration":
                        this.ConfigurationId = Convert.ToInt32(_arg[1]);
                        break;
                    case "-user":
                        this.User = _arg[1];
                        break;
                    case "-password":
                        this.Password = _arg[1];
                        break;
                    case "-database":
                        this.Database = _arg[1];
                        break;
                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <returns></returns>
        public int ExecuteOperation()
        {
            ApplicationCore core = new ApplicationCore(this.User, this.Password, this.Database);
            if (core.ReturnValue != 0)
            {
                return core.ReturnValue;
            }
            core.SetModelFromFilename(this.ModelFilename);
            if (core.ReturnValue != 0)
            {
                return core.ReturnValue;
            }
            cwLightObject config = core.GetConfigurationObjectFromId(this.ConfigurationId);
            if (core.ReturnValue != 0)
            {
                return core.ReturnValue;
            }
            ConfigurationRootNode node = core.GetConfigurationNodeFromDescription(config);
            core.GenerateContextTree(node);

            core.closeConnection();
            return core.ReturnValue;
        }


    }
}
