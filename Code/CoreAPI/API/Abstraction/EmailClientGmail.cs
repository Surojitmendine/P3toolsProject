using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Abstraction
{
    public class EmailClientGmail:IEmail
    {

        private const string mailFrom = "connect@matrixwebtech.com";
        private const string mailPassword = "Letstry123#";      

        private readonly NetworkCredential _loginInfo;

        private readonly MailMessage _msg;
        public EmailClientGmail(MailMessage Msg)
        {
            this._msg = Msg;
           this._loginInfo = new NetworkCredential(mailFrom, mailPassword);

        }

        public void SendMail()
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                //client.Port = 25;

                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = _loginInfo;
                client.Send(this._msg);
            }
            catch(Exception ex)
            {

            }

        }
    }
}
