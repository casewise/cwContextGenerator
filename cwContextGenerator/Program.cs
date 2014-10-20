using cwContextGenerator.Core;
using cwContextGenerator.GUI;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwContextGenerator
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    // GUI
                    ApplicationCore core = new ApplicationCore();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Launcher mf = new Launcher(core);
                    Application.Run(mf);
                    return core.ReturnValue;
                }
                else
                {
                    // BATCH
                    BatchManager batch = new BatchManager(args);
                    return batch.ExecuteOperation();
                }
            }
            catch (Exception e)
            {
                log.Debug(e.ToString());
               // MessageBox.Show(e.ToString());
                return -1;
            }
        }


    }
}
