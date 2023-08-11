namespace CRM_api.DataAccess.Helper
{
    public static class BusinessConstants
    {
        public static string LifeIns = "life";
        public static string GeneralIns = "general";
        public static string MF = "mutual funds";
        public static string Mgain = "mgain";
        public static string Loan = "loan";
        public static string Stocks = "stocks";
        public static string PortfolioTransfer = "portfolio transfer";
        public static string Brokerage = "brokerage";
        public static string KAGroup = "ka group";
        public static string Journal = "journal";

        public enum UserLevel
        {
            Basic,
            Silver,
            Gold,
            Platinum,
            Diamond
        }
}
}
