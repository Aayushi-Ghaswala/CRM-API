using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CRM_api.Services.Helper.Reminder_Helper.LI_GI_Module
{
    public static class InsDueReminderHelper
    {
        public static async void InsDueHelper(IConfiguration configuration, IInsuranceClientRepository insuranceClientRepository)
        {
            var insClients = insuranceClientRepository.GetInsClientsForInsDueReminder();

            foreach (var insClient in insClients)
            {
                var templateId = "";
                var reminderMessage = "";

                var diff = insClient.InsDuedate.Value.Date.Subtract(DateTime.Now.Date).TotalDays;

                switch (diff)
                {
                    case 5:
                        reminderMessage = $"Dear Investors, Only 5 days Left!To continue the coverage and benefits of policy {insClient.InsPolicy}, renew today. From KA Group of Companies.";
                        templateId = "1207162209381681204";
                        break;
                    case 15:
                        reminderMessage = $"Dear Investors, Only 15 days left!To renew your policy {insClient.InsPolicy}. Your Premium amount is: {insClient.PremiumAmount} Please renew before expiry. From KA Group of Companies.";
                        templateId = "1207162209372436085";
                        break;
                    case 30:
                        reminderMessage = $"Dear Investor, We wish to remind you that your insurance policy no. {insClient.InsPolicy} is about to expire in 30 days. Your Premium amount is: {insClient.PremiumAmount} Please renew before expiry. From KA Group of Companies.";
                        templateId = "1207162209360055235";
                        break;
                }

                if (insClient.IsEmailReminder == true)
                {
                    if (insClient.InsEmail is not null)
                    {
                        var subject = "Friendly Reminder: Renewal Reminder for Insurance Policy";
                        var body = new BodyBuilder();
                        body.TextBody = reminderMessage;

                        EmailHelper.SendMailAsync(configuration, insClient.InsEmail, subject, body);
                    }
                }

                if (insClient.IsSmsReminder == true)
                {
                    if (insClient.InsMobile is not null)
                    {
                        SMSHelper.SendSMS(insClient.InsMobile, reminderMessage, templateId);
                    }
                }
            }
        }
    }
}
