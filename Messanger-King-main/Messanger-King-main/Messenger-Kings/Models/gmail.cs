using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Messenger_Kings.Models
{
    public class GmailClass
    {
        public void Gmail(string subject, string body, string email)
        {


            



            string from = "codeshedding@gmail.com";

            using (MailMessage mail = new MailMessage(from, email))

            {

                mail.Subject = subject;

                mail.Body = body;
                
                



                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";

                smtp.EnableSsl = true;

                NetworkCredential networkCredential = new NetworkCredential(from, "Thir3yearproject2020!@$!");

                smtp.UseDefaultCredentials = true;
               
                smtp.Credentials = networkCredential;

                smtp.Port = 587;

                smtp.Send(mail);

            }


        }
    }
}