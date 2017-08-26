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

        public static string DirectoryPath = "directory";

        public static string FTPUser = "user";

        public static string password = "password";

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
            ftpService.WriteLog();
            bool ftpStatus = ftpService.checkFTPServerStatus(DirectoryPath,FTPUser,password);

        }

        protected override void OnStop()
        {
        }
    }
}
