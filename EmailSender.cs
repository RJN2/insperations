using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RAJAR_Application.HelpClasses
{
    public class EmailSender
    {
        /// <summary>
        /// This method send out the emails to users and returns the status of the process.
        /// </summary>
        /// <param name="UserName">The user logged in as.</param>
        /// <param name="UserEmail">Single Email address of receipent or multiple email address seperated by semi colon(;)</param>
        /// <param name="Subject">Subject for email</param>
        /// <param name="EmailType">Email body</param>
        public void SendEmailToUser(String UserName, String UserEmail, string Subject, string Body)
        {
            String FromAddress = "";
            String FromName = "";
            try
            {
                FromAddress = ConfigurationManager.AppSettings["EmailFrom"].ToString();
            }
            catch
            {
                FromAddress = "donotreply@ipsos.com";
            }
            try
            {
                FromName = ConfigurationManager.AppSettings["EmailFromName"].ToString();
            }
            catch
            {
                FromName = "Ipsos Admin";
            }

            String[] TargetEmail = new String[] { UserEmail };
            String[] EmptyStringArray = new String[] { };

            Boolean Success = this.Email(FromAddress, FromName, TargetEmail, EmptyStringArray, EmptyStringArray, Subject, Body, EmptyStringArray);
        }

        private String _strEmailFrom = String.Empty;
        private String _strEmailFromNameToDisplay = String.Empty;
        private String[] _strEmailTo;
        private String[] _strEmailCC;
        private String[] _strEmailBCC;
        private String _strEmailSubject = "";
        private String _strEmailMessageBody = "";
        private String[] _strFilename;


        /// <summary>
        /// This class sends an email
        /// Be aware that the Addresses / File paths are an Array of Strings
        /// </summary>
        /// <param name="strEmailFrom">String Email Address From</param>
        /// <param name="strEmailFromNameToDisplay">String Name to Display From</param>
        /// <param name="strEmailTo">Array of strings with the email addresses to send to</param>
        /// <param name="strEmailCC">Array of strings with the email addresses to send CC if any</param>
        /// <param name="strEmailBCC">Array of strings with the email addresses to send BCC if any</param>
        /// <param name="strEmailSubject">String Email Subject</param>
        /// <param name="strEmailMessageBody">String Message</param>
        /// <param name="strFilename">Array of strings with the path of the files</param>
        /// <returns></returns>
        public Boolean Email(String strEmailFrom, String strEmailFromNameToDisplay, String[] strEmailTo, String[] strEmailCC, String[] strEmailBCC, String strEmailSubject, String strEmailMessageBody, String[] strFilename)
        {
            Boolean Success = true;
            _strEmailFrom = strEmailFrom;
            _strEmailFromNameToDisplay = strEmailFromNameToDisplay;
            _strEmailTo = strEmailTo;
            _strEmailCC = strEmailCC;
            _strEmailBCC = strEmailBCC;
            _strEmailSubject = strEmailSubject;
            _strEmailMessageBody = strEmailMessageBody;
            _strFilename = strFilename;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(_strEmailFrom, _strEmailFromNameToDisplay);
                message.From = fromAddress;

                foreach (String strAddress in _strEmailTo)
                {
                    try { message.To.Add(strAddress); }
                    catch (Exception) { }
                }
                foreach (String strAddress in _strEmailCC)
                {
                    try { message.CC.Add(strAddress); }
                    catch (Exception) { }
                }
                foreach (String strAddress in _strEmailBCC)
                {
                    try { message.Bcc.Add(strAddress); }
                    catch (Exception) { }
                }

                message.Subject = _strEmailSubject;
                message.IsBodyHtml = true;
                message.Body = _strEmailMessageBody;

                foreach (String strAttachments in _strFilename)
                {
                    try { message.Attachments.Add(new Attachment(@"" + strAttachments)); }
                    catch (Exception) { }
                }

                smtpClient.Port = 25;

                try
                {
                    smtpClient.Host = ConfigurationManager.AppSettings["SMTPHost"];
                }
                catch
                {
                    //This is our pmta server.
                    smtpClient.Host = "pmta.extranet.iext";
                }

                try
                {
                    smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"], ConfigurationManager.AppSettings["SMTPPassword"]);
                }
                catch { }

                //Page.Server.ClearError();
                smtpClient.Send(message);
            }
            catch { Success = false; }
            return Success;
        }
    }
}