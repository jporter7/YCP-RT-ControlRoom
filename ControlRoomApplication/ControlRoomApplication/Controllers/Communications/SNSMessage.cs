using System;
using System.Collections.Generic;
using ControlRoomApplication.Entities;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Controllers.Communications;

namespace ControlRoomApplication.Controllers
{
    class SNSMessage
    {
        public static void sendMessage(User user, MessageTypeEnum type)
        {

            if (user.first_name == "control")
            {
                return;
            }

            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(AWSConstants.SNS_ACCESS_KEY, AWSConstants.SNS_SECRET_ACCESS_KEY, Amazon.RegionEndpoint.USEast1);

            AmazonSimpleEmailServiceClient snsEmailClient = new AmazonSimpleEmailServiceClient(AWSConstants.SNS_ACCESS_KEY, AWSConstants.SNS_SECRET_ACCESS_KEY, Amazon.RegionEndpoint.USEast1);

            PublishRequest pubRequest = new PublishRequest();

            // get the message that corresponds to the type of notification
            pubRequest.Message = MessageTypeExtension.GetDescription(type);

            // sending sms message
            if (user._Notification_Type == NotificationTypeEnum.SMS || user._Notification_Type == NotificationTypeEnum.ALL)
            {
                // we need to have +1 on the beginning of the number in order to send
                if (user.phone_number.Substring(0, 2) != "+1")
                {
                    pubRequest.PhoneNumber = "+1" + user.phone_number;
                }
                else
                {
                    pubRequest.PhoneNumber = user.phone_number;
                }

                PublishResponse pubResponse = snsClient.Publish(pubRequest);
                Console.WriteLine(pubResponse.MessageId);
            }

            // sending email
            if (user._Notification_Type == NotificationTypeEnum.EMAIL || user._Notification_Type == NotificationTypeEnum.ALL)
            {
                sendEmail(user);
            }
        }

        public static void sendEmail(User user)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = EmailPartConstants.senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { user.email_address }
                    },
                    Message = new Message
                    {
                        Subject = new Content(EmailPartConstants.Subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = EmailPartConstants.html
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = EmailPartConstants.Text
                            }
                        }
                    }
                };
                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var response = client.SendEmail(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine($"Error: {e}");
                }
            }
        }
    }
}
                
            
        
    
