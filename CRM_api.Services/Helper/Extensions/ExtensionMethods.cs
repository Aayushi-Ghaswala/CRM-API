namespace CRM_api.Services.Helper.Extensions
{
    public static class ExtensionMethods
    {
        public static string GetMFTransactionType(this string value)
        {
            if (value.Contains("SIP"))
                return "SIP";
            else if (value.Contains("Systematic Investment") || value.Contains("Systematic"))
                return "SIP";
            else if (value.Contains("Investment"))
                return "SIP";
            else if (value.Contains("Purchase") && !value.Contains("SIP"))
                return "Purchase";
            else if (value.Contains("Switch In") || value.Contains("Switch-In") || value.Contains("Switch Over In"))
                return "Purchase";
            else if (value.Contains("Switch Out") || value.Contains("Switch-Out") || value.Contains("Switch Over Out"))
                return "Sale";
            else if (value.Contains("Redemption"))
                return "Sale";
            else if (value.Contains("Transfer In") || value.Contains("Transfer-In"))
                return "Purchase";
            else if (value.Contains("Transfer Out") || value.Contains("Transfer-Out"))
                return "Sale";
            else if (value.Contains("Bonus"))
                return "Bonus";
            else if (value.Contains("Reinv"))
                return "Purchase";
            else if (value.Contains("Opening"))
                return "Purchase";
            else if (value.Contains("Transmission In") || value.Contains("Transmission-In"))
                return "Purchase";
            else if (value.Contains("Transmission Out") || value.Contains("Transmission-Out"))
                return "Sale";
            else
                return "NA";
        }
    }
}
