﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
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
        //Smpt server
        public const string HOTMAIL_SERVER = "smtp.live.com";
        //Connecting port
        public const int PORT = 465;


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
        private string getIpAddress(NetworkInterfaceType _type)
        {
            logger.Warn(DateTime.Today.ToShortTimeString() + ":" + "Service gets IpAddress");
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
            logger.Warn(DateTime.Today.ToShortTimeString() + ":" + "IpAddress is-->" + output);
            return output;
        }

        public void checkFTPServerSituation(string ftpUser, string password)
        {
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
                sendEmail(false);
            }
        }

        private void sendEmail(bool status)
        {
            logger.Warn(DateTime.Today.ToShortTimeString() + ":" + "Mail is sending to receiver...");
            if (!status)
            {
               var smtpClient = setEmailCredentials();

                MailMessage mailMessage = new MailMessage("tim_ahmet89@hotmail.com", "ahmeterdemkahveci@gmail.com");

                mailMessage.Subject = "FTPIPDetector";

                mailMessage.Body = "FTP Server Ip has been changed, please check when you arrive at home...";

                smtpClient.Send(mailMessage);
            }
        }

        private SmtpClient setEmailCredentials()
        {
            SmtpClient mailServer = new SmtpClient(HOTMAIL_SERVER);

            mailServer.EnableSsl = true;

            mailServer.Credentials = new NetworkCredential("tim_ahmet89@hotmail.com","AhmetErdem1");

            return mailServer;
        }
    }
}