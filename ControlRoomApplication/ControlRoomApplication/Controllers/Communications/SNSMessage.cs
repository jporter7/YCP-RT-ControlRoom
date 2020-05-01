using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlRoomApplication.Entities;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace ControlRoomApplication.Controllers
{
    class SNSMessage
    {
        public static void sendMessage(User user, MessageTypeEnum type)
        {
            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(AWSConstants.SNS_ACCESS_KEY, AWSConstants.SNS_SECRET_ACCESS_KEY, Amazon.RegionEndpoint.USEast1);
           
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


            }

            // sending email
            if (user._Notification_Type == NotificationTypeEnum.EMAIL || user._Notification_Type == NotificationTypeEnum.ALL)
            {
                
            }


                // add optional MessageAttributes, for example:
                //   pubRequest.MessageAttributes.Add("AWS.SNS.SMS.SenderID", new MessageAttributeValue
                //      { StringValue = "SenderId", DataType = "String" });
                PublishResponse pubResponse = snsClient.Publish(pubRequest);
            Console.WriteLine(pubResponse.MessageId);
        }
    }
}
