using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class Mailer: IMailer
    {
        string lodEmail;
        string lodPassword;

        public void Send(string email, Mail mail) // Mail: MailMessage
        {
            try
            {
                int portValue;
                string emailCompany = EmailCompany(lodEmail);
                string smtpName = SearchPortValueAndSmtpName(emailCompany, out portValue);
                SmtpClient client = new SmtpClient(smtpName, portValue);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(lodEmail, lodPassword);
                mail.From = new MailAddress(lodEmail);
                mail.To.Add(new MailAddress(email));
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mailer.Send: "+e.Message);
            }
        }

        string SearchPortValueAndSmtpName(string emailCompany, out int portValue)
        {
            try
            {
                if (emailCompany == "gmail.com") { portValue = 465; return "smtp.gmail.com"; }; // Gmail
                if (emailCompany == "mail.ru") { portValue = 465; return "smtp.mail.ru"; }; // Mail.ru
                if (emailCompany == "yandex.ru") { portValue = 465; return "smtp.yandex.ru"; }; // Yandex
                if (emailCompany == "rambler.ru") { portValue = 587; return "smtp.mail.rambler.ru"; } // Rambler
                if (emailCompany == "yahoo.com") { portValue = 465; return "smtp.mail.yahoo.com"; }; // Yahoo
                if (emailCompany == "hotmail.com") { portValue = 587; return "smtp.live.com"; }; //Hotmail
                if (emailCompany == "aol.com") { portValue = 587; return "smtp.aol.com"; }; //AOL 
            }
            catch (Exception e)
            {
                throw new Exception("Mailer.Send: " + e.Message);
            }
        }

        string EmailCompany(string email)
        {
            string[] split = email.Split(new Char[] { '@' });
            string emailCompany = split[split.Length];
            return emailCompany;
        }
    }
}
