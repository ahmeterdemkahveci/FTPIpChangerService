using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FtpIpChangerService
{
    public partial class Scheduler : ServiceBase
    {
        Runner ftpService = null;

        public static string FTPUser = "FTPUser";

        public static string password = "68861299";

        public Scheduler()
        {
            InitializeComponent();

        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            ftpService = new Runner();
            ftpService.CheckFtpServerSituation(FTPUser,password);
            

        }

        protected override void OnStop()
        {
        }
    }
}
