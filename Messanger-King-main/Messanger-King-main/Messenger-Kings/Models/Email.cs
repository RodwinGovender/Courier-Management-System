using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger_Kings.Models
{
    public class Email
    {
        //overload with what you want to show in the email.
        public void SendConfirmation(string email, string Name, int id, DateTime deliveryDate, double total)
        {
            try
            {
                var myMessage = new SendGridMessage
                {
                    From = new EmailAddress("codeshedding@gmail.com", "Messenger Kings")
                };

                myMessage.AddTo(email);
                string subject = "Successful Booking";
                string body = (
                    "Dear " + Name + "<br/>"
                    + "<br/>"
                    + "You have successfully booked for delivery by Messenger King Courier... "
                    + "<br/>"
                    + "<br/>" + "Your ordder number is: " + id
                    + "," + " your item(s) requested delivery for " + deliveryDate
                    + "<br/>" + "The total cost is:" + total.ToString("C") +
                    "<br/>" +
                    "<br/>" +
                    "<br/>" +

                    //"Sincerely Yours, " +
                    "<br/>" +
                    "Messenger Kings Courier Management");

                myMessage.Subject = subject;
                myMessage.HtmlContent = body;

                var transportWeb = new SendGrid.SendGridClient("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");

                transportWeb.SendEmailAsync(myMessage);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }

        }

    }
}