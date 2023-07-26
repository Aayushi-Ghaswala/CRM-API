using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CRM_api.Services.Helper.Reminder_Helper.LI_GI_Module
{
    public static class InsPremiumReminderHelper
    {
        public static async void InsPremimumReminder(IConfiguration configuration, IInsuranceClientRepository insuranceClientRepository, FirebaseMessaging firebaseMessaging)
        {
            var insClients = insuranceClientRepository.GetInsClientsForInsPremiumReminder();

            foreach (var insClient in insClients)
            {
                string reminderMessage = $"Dear {insClient.InsUsername},\n\n" + $"We hope this message finds you well. We would like to remind you of the upcoming payment for your insurance policy premium, which is due on {insClient.InsPremiumRmdDate.Value.ToString("D")}. As a valued policyholder, we want to ensure that your coverage remains active and uninterrupted.\n\n" +
                                                 $"Maintaining an active insurance policy is crucial for safeguarding your assets, protecting your loved ones, and providing peace of mind. By timely paying your premium.";
                var subject = "Friendly Reminder: Upcoming Insurance Premium Payment";

                if (insClient.IsEmailReminder is true && insClient.InsEmail is not null)
                {
                    var body = new BodyBuilder();
                    body.TextBody = reminderMessage;

                    EmailHelper.SendMailAsync(configuration, insClient.InsEmail, subject, body);
                }

                if (insClient.IsSmsReminder is true && insClient.InsMobile is not null)
                {
                    string templateid = "1207162209381681204";

                    SMSHelper.SendSMS(insClient.InsMobile, reminderMessage, templateid);
                }

                if (insClient.IsNotification is true && insClient.TblUserMaster.UserDeviceid is not null)
                    await NotificationHelper.SendNotification(firebaseMessaging, reminderMessage, subject, insClient.TblUserMaster.UserDeviceid);

                switch (insClient.InsFrequency.ToLower())
                {
                    case "yearly":
                        insClient.InsPremiumRmdDate = insClient.InsPremiumRmdDate.Value.AddYears(1);
                        break;
                    case "monthly":
                        insClient.InsPremiumRmdDate = insClient.InsPremiumRmdDate.Value.AddMonths(1);
                        break;
                    case "quarterly":
                        insClient.InsPremiumRmdDate = insClient.InsPremiumRmdDate.Value.AddMonths(3);
                        break;
                }

                await insuranceClientRepository.UpdateInsuranceClientDetail(insClient, true);
            }
        }
    }
}
