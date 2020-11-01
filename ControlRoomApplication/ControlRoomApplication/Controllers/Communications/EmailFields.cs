using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications
{
    public class EmailFields
    {
        public static string Sender { get; set; }
        public static string Subject { get; set; }
        public static string Text { get; set; }
        public static string Html { get; set; }

        public EmailFields(string sender, string subject, string text, string html)
        {
            Sender = sender;
            Subject = subject;
            Text = text;
            Html = html;
        }

        public static void setSender(string sender)
        {
            Sender = sender;
        }
        public static void setSubject(string subject)
        {
            Subject = subject;
        }
        public static void setText(string text)
        {
            Text = text;
        }
        public static void setHtml(string html)
        {
            Html = html;
        }
    }
}
