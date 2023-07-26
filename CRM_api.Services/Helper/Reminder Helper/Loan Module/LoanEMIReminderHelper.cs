using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CRM_api.Services.Helper.Reminder_Helper.Loan_Module
{
    public static class LoanEMIReminderHelper
    {
        public static async void LoanEMIReminder(IConfiguration configuration, ILoanMasterRepository loanMasterRepository, FirebaseMessaging firebaseMessaging)
        {
            var loans = loanMasterRepository.GetLoanDetailsForEMIReminder();

            foreach (var loan in loans)
            {
                string reminderMessage = $"Dear {loan.TblUserMaster.UserName},\n\n" + $"We hope this message finds you well. We would like to remind you of the upcoming payment for your Loan EMI, which is due on {loan.MaturityDate.Value.ToString("D")}.";
                var subject = "Friendly Reminder: Upcoming Loan EMI";

                if (loan.IsEmailReminder is true && loan.TblUserMaster.UserEmail is not null)
                {
                    var body = new BodyBuilder();
                    body.TextBody = reminderMessage;

                    EmailHelper.SendMailAsync(configuration, loan.TblUserMaster.UserEmail, subject, body);
                }

                if (loan.IsSmsReminder is true && loan.TblUserMaster.UserMobile is not null)
                {
                    string templateid = "1207162209381681204";

                    SMSHelper.SendSMS(loan.TblUserMaster.UserMobile, reminderMessage, templateid);
                }

                if (loan.IsNotification is true && loan.TblUserMaster.UserDeviceid is not null)
                    await NotificationHelper.SendNotification(firebaseMessaging, reminderMessage, subject, loan.TblUserMaster.UserDeviceid);

                loan.MaturityDate = loan.MaturityDate.Value.AddMonths(1);

                await loanMasterRepository.UpdateLoanDetail(loan, true);
            }
        }
    }
}
