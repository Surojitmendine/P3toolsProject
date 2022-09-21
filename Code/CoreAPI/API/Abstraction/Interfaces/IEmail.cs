using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Abstraction
{
    interface IEmail
    {
        void SendMail();

    }

}
