using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2.Helpers
{
    public class SendMailHelper
    {
        public static SendMail Mail { get; set; }
        private SendMailHelper _Instance = null;
        public SendMailHelper Instance {
            get {
                if (_Instance == null)
                    _Instance = new SendMailHelper();
                return _Instance;
            }
        }

        public static bool SendMail(string toEmails, string emailText, string subject)
        {
            if (Mail == null)
                Mail = new SendMail();
            if (string.IsNullOrEmpty(toEmails)) return false;
            return Mail.SendaMail(toEmails, emailText, subject);
        }
    }
}