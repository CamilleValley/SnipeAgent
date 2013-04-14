using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using System.Net;

namespace UltimateSniper_Services
{
    public class ServiceEmail
    {
        public static void SendEmail(User user, string emailTitle, string emailBody)
        {
            Mail email = new Mail("Snipe Agent", GeneralSettings.Default.ContactEmailAddress, user.UserName, user.UserEmailAddress, emailTitle, emailBody, true, false);
            email.Send();
        }

        public static void SendAdminEmail(string emailTitle, string emailBody)
        {
            Mail email = new Mail("Snipe Agent", GeneralSettings.Default.ContactEmailAddress, "Admin", GeneralSettings.Default.ContactEmailAddress, emailTitle, emailBody, true, true);
            email.Send();
        }

        public static void SendContactEmail(string emailAddress, string emailTitle, string emailBody)
        {
            Mail email = new Mail("Free form contact", emailAddress, "Admin", GeneralSettings.Default.ContactEmailAddress, emailTitle, emailBody, true, false);
            email.Send();
        }

        public static void SendContactEmail(User user, string emailTitle, string emailBody)
        {
            bool highP = false;

            if (user.HasOptions()) highP = true;

            Mail email = new Mail(user.UserName, user.UserEmailAddress, "Admin", GeneralSettings.Default.ContactEmailAddress, emailTitle, emailBody, true, highP);
            email.Send();
        }
    }

    public class Mail
    {
        private System.Net.Mail.MailMessage _mail = new System.Net.Mail.MailMessage();
        public Mail(string fromName, string fromMail, string toName, string toMail, string subject, string body, bool isHtml, bool highPriority)
        {
            this._mail.Subject = subject;
            this._mail.Body = body;
            this._mail.From = new System.Net.Mail.MailAddress(fromMail, fromName);
            this._mail.To.Add(new System.Net.Mail.MailAddress(toMail, toName));
            this._mail.IsBodyHtml = isHtml;
            this._mail.BodyEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            this._mail.SubjectEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            if (highPriority) this._mail.Priority = System.Net.Mail.MailPriority.High;
        }
        public bool Send()
        {
            try
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ServiceParametersHelper.SMTPAddress()); // Mettre le serveur smtp ici

                //to authenticate we set the username and password properites on the SmtpClient
                smtp.Credentials = new NetworkCredential("contact@snipeagent.com", "cams1982");

                smtp.Send(this._mail);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
