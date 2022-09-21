using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using API.BusinessLogic;
using API.Context;
using API.Abstraction;

namespace API.Helper
{
    public class TimedTasks
    {
        private const string mailFrom = "connect@matrixwebtech.com";
        private const string mailPassword = "Letstry123#";
        private const string mailTo = "sajalde@gmail.com";
        private const string mailCC = "kuntal.bose01@gmail.com";
        private readonly DBContext _db;


        public TimedTasks(DBContext db)
        {
            this._db = db;
        }
        public void applicationInfoMail()
        {
            string Subject = "Application Log";
            string Body = "Please find attachment.";

            try
            {

                string strMailBody = string.Empty;
               
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(new MailAddress(mailTo));
                msg.CC.Add(mailCC);
                msg.Subject = Subject + "" + System.DateTime.Now.ToString();

                strMailBody = Body;


                msg.Body = strMailBody;
                msg.IsBodyHtml = true;

                DirectoryInfo directoryInfo = new DirectoryInfo("logs");
                string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                foreach (var file in directoryInfo.GetFiles())
                {
                    if (file.Name == "internal-nlog.txt" || file.Name == "nlog-all-" + date + ".log" || file.Name == "nlog-own-" + date + ".log")
                    {
                        msg.Attachments.Add(new Attachment(file.FullName));
                    }
                }

                IEmail email = new EmailClientGmail(msg);
                email.SendMail();
                

            }
            catch (Exception)
            {

            }
        }

     
    }
}
