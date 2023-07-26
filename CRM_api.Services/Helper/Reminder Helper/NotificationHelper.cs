using FirebaseAdmin.Messaging;

namespace CRM_api.Services.Helper.Reminder_Helper
{
    public static class NotificationHelper
    {
        public static async Task SendNotification(FirebaseMessaging firebaseMessaging, string? body, string? title, string? deviceToken)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Token = deviceToken
            };

             await firebaseMessaging.SendAsync(message);
        }
    }
}