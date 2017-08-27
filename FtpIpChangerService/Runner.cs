using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using NLog;


namespace FtpIpChangerService
{
    public class Runner
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /*private string  createLogFile()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string logFile = currentDirectory + "\\logFile.txt";

            if (!File.Exists(logFile))
            {
                File.Create(logFile);
            }
            return logFile;

        }*/
        private string  getIpAddress(NetworkInterfaceType _type)
        {
            logger.Warn(DateTime.Today.ToShortTimeString()+":"+"Service gets IpAddress");
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            logger.Warn(DateTime.Today.ToShortTimeString() + ":" + "IpAddress is-->"+output);
            return output;
        }

        public void checkFTPServerSituation(string ftpUser,string password)
        {
            string status = "";
            try
            {
                string wirelessIpAddress = getIpAddress(NetworkInterfaceType.Wireless80211);
                logger.Warn(DateTime.Today.ToShortTimeString() + ":" + "FTP Server Status is checked");
                FtpWebRequest requestDir = (FtpWebRequest) WebRequest.Create("ftp://" + wirelessIpAddress);
                requestDir.Credentials = new NetworkCredential(ftpUser, password);

                requestDir.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = (FtpWebResponse) requestDir.GetResponse();

            }
            catch (WebException exception)
            {
                logger.Error(DateTime.Today.ToShortTimeString() + ":" + "FTP Server Status is down!!!");
                sendEmail(status);
            } 
        }

        private void sendEmail(string status)
        {
            logger.Warn(DateTime.Today.ToShortTimeString() + ":" + "Mail is sending to receiver...");
        }
    }
}
