using System.Net;

namespace CRM_api.Services.Helper.Reminder_Helper
{
    public static class SMSHelper
    {
        public static void SendSMS(string MobileNo, string Message, string Templateid)
        {

            string smsurl = "http://sms.messageindia.in/v2/sendSMS?username=Kafinsec&message=" + Message + "&sendername=KAGRUP&smstype=TRANS&numbers=" + MobileNo + "&apikey=c5fcf3d2-b775-4d6f-8be2-dce3b5fe76c0&peid=1201159592535386414&templateid=" + Templateid + "";
            WebRequest request = HttpWebRequest.Create(smsurl);
            if (smsurl.StartsWith("HTTPS", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            }
            
            try
            {

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
