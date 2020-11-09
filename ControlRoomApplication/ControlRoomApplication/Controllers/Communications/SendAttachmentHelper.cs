/// A Lifesaver(tm): https://colinmackay.scot/2011/11/10/sending-more-than-a-basic-email-with-amazon-ses/
/// Modified for running in .NET version > .NET 4.5

using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace ControlRoomApplication.Controllers.Communications
{
    public static class SendAttachmentHelper
    {
        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly ConstructorInfo _mailWriterConstructor;
        private static readonly MethodInfo _sendMethod;
        private static readonly MethodInfo _closeMethod;

        static SendAttachmentHelper()
        {
            Assembly systemAssembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = systemAssembly.GetType("System.Net.Mail.MailWriter");

            _mailWriterConstructor = mailWriterType.GetConstructor(Flags, null, new[] { typeof(Stream) }, null);

            _sendMethod = typeof(MailMessage).GetMethod("Send", Flags);

            _closeMethod = mailWriterType.GetMethod("Close", Flags);
        }

        public static MemoryStream ConvertMailMessageToMemoryStream(MailMessage message)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                object mailWriter = _mailWriterConstructor.Invoke(new object[] { memoryStream });

                _sendMethod.Invoke(message, Flags, null, new[] { mailWriter, true, true }, null);
                _closeMethod.Invoke(mailWriter, Flags, null, new object[] { }, null);

                return memoryStream;
            }
        }       
    }
}
