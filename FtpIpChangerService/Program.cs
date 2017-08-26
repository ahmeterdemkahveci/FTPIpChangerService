using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FtpIpChangerService
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        static void Main()
        {

            if (Debugger.IsAttached)
            {
                Scheduler myScheduler = new Scheduler();
                myScheduler.OnDebug();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Scheduler()
                };
                ServiceBase.Run(ServicesToRun);

            }

        }
    }
}
