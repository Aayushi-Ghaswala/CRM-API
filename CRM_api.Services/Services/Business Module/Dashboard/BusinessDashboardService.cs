using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.Business_Module.Dashboard;
using Microsoft.Extensions.Configuration;
using MimeKit;
using static CRM_api.Services.Helper.ConstantValue.SubInvTypeConstant;

namespace CRM_api.Services.Services.Business_Module.Dashboard
{
    public class BusinessDashboardService : IBusinessDashboardService
    {
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IInsuranceClientRepository _insuranceClientRepository;
        private readonly IStocksRepository _stocksRepository;
        private readonly IMutualfundRepository _mutualfundRepository;
        private readonly IConfiguration _configuration;

        public BusinessDashboardService(IUserMasterRepository userMasterRepository, IInsuranceClientRepository insuranceClientRepository, IStocksRepository stocksRepository, IMutualfundRepository mutualfundRepository, IConfiguration configuration)
        {
            _userMasterRepository = userMasterRepository;
            _insuranceClientRepository = insuranceClientRepository;
            _stocksRepository = stocksRepository;
            _mutualfundRepository = mutualfundRepository;
            _configuration = configuration;
        }

        #region Client Current Investment Snapshot
        public async Task<List<ClientReportDto<ClientCurrentInvSnapshotDto>>> GetClientCurrentInvSnapshotAsync(int? userId, bool? isZero, string search)
        {
            var category = await _userMasterRepository.GetCategoryByName(CategoryConstant.customer);
            List<TblUserMaster> clients = new List<TblUserMaster>();

            if (userId is null)
                clients = await _userMasterRepository.GetUsersByCategoryId(category.CatId, null, null, search, true);
            else
            {
                var client = await _userMasterRepository.GetUserMasterbyId((int)userId, null, null, true);
                clients.Add(client);
            }

            List<ClientReportDto<ClientCurrentInvSnapshotDto>> clientCurrentInvSnapshots = new List<ClientReportDto<ClientCurrentInvSnapshotDto>>();

            foreach (var client in clients)
            {
                var clientReport = new ClientReportDto<ClientCurrentInvSnapshotDto>();
                clientReport.UserName = client.UserName;
                clientReport.UserEmail = client.UserEmail;
                clientReport.UserMobile = client.UserMobile;
                var clientCurrentSnapshot = new ClientCurrentInvSnapshotDto();

                if (client.TblInsuranceclients.Count() < 0)
                {
                    var lifeInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.Life.ToString());
                    clientCurrentSnapshot.LI = client.TblInsuranceclients.Where(x => x.InsUserid == userId && x.IsDeleted != true && x.InvSubtype == lifeInsId)
                                                                         .Sum(x => x.InsAmount);

                    var generalInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.General.ToString());
                    clientCurrentSnapshot.GI = client.TblInsuranceclients.Where(x => x.InsUserid == userId && x.IsDeleted != true && x.InvSubtype == generalInsId)
                                                                         .Sum(x => x.InsAmount);
                }

                var stocks = await _stocksRepository.GetStockDataByUserName(client.UserName, null, null);
                if (stocks.Count > 0)
                {
                    var totalPurchase = stocks.Where(s => s.StType.Equals("B")).Sum(x => x.StNetcostvalue);
                    var totalSale = stocks.Where(s => s.StType.Equals("S")).Sum(x => x.StNetcostvalue);
                    clientCurrentSnapshot.Stocks = totalPurchase - totalSale;
                }

                if (client.TblMgaindetails.Count() < 0)
                    clientCurrentSnapshot.MGain = client.TblMgaindetails.Sum(x => x.MgainInvamt);

                if (client.TblMftransactions is not null)
                {
                    var redemptionUnit = client.TblMftransactions.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                    var redemAmount = redemptionUnit.Sum(x => x.Invamount);

                    var TotalpurchaseUnit = client.TblMftransactions.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                    var purchaseAmount = TotalpurchaseUnit.Sum(x => x.Invamount);

                    clientCurrentSnapshot.MutualFunds = purchaseAmount - redemAmount;
                }

                clientReport.ClientInvSnapshot = clientCurrentSnapshot;

                if (isZero is true)
                {
                    if (clientCurrentSnapshot.MGain == 0 && clientCurrentSnapshot.Assets == 0 && clientCurrentSnapshot.Stocks == 0
                                && clientCurrentSnapshot.MutualFunds == 0 && clientCurrentSnapshot.GI == 0 && clientCurrentSnapshot.LI == 0)
                        clientCurrentInvSnapshots.Add(clientReport);
                }
                else
                {
                    if (clientCurrentSnapshot.MGain != 0 || clientCurrentSnapshot.Assets != 0 || clientCurrentSnapshot.Stocks != 0
                                || clientCurrentSnapshot.MutualFunds != 0 || clientCurrentSnapshot.GI != 0 || clientCurrentSnapshot.LI != 0)
                        clientCurrentInvSnapshots.Add(clientReport);
                }
            }

            return clientCurrentInvSnapshots;
        }
        #endregion

        #region Client Monthly Transaction Snapshot
        public async Task<List<ClientReportDto<ClientMonthlyTransSnapshotDto>>> GetClientMonthlyTransSnapshotAsync(int? userId, int? month, int? year, bool? isZero, string search)
        {
            var clients = new List<TblUserMaster>();
            var category = await _userMasterRepository.GetCategoryByName(CategoryConstant.customer);
            List<ClientReportDto<ClientMonthlyTransSnapshotDto>> clientMonthlyTransSnapshots = new List<ClientReportDto<ClientMonthlyTransSnapshotDto>>();

            if (userId is not null)
            {
                var client = await _userMasterRepository.GetUserMasterbyId((int)userId, month, year);
                clients.Add(client);
            }
            else
                clients = await _userMasterRepository.GetUsersByCategoryId(category.CatId, month, year, search);

            foreach (var client in clients)
            {
                var clientReport = new ClientReportDto<ClientMonthlyTransSnapshotDto>();
                clientReport.UserName = client.UserName;
                clientReport.UserEmail = client.UserEmail;
                clientReport.UserMobile = client.UserMobile;
                var clientMonthlyTransSnapshot = new ClientMonthlyTransSnapshotDto();
                if (client.TblInsuranceclients.Count > 0)
                {
                    var lifeInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.Life.ToString());
                    var generalInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.General.ToString());

                    clientMonthlyTransSnapshot.LIPremium = client.TblInsuranceclients.Where(x => x.InvSubtype == lifeInsId)
                                                            .Sum(x => x.PremiumAmount);

                    clientMonthlyTransSnapshot.GIPremium = client.TblInsuranceclients.Where(x => x.InvSubtype == generalInsId)
                                                            .Sum(x => x.PremiumAmount);
                }

                if (client.TblMftransactions.Count > 0)
                {
                    clientMonthlyTransSnapshot.MFSip = client.TblMftransactions.Where(x => x.Transactiontype == "PIP").Sum(x => x.Invamount);
                    clientMonthlyTransSnapshot.MFLumpsum = client.TblMftransactions.Where(x => x.Transactiontype == "PIP (SIP)").Sum(x => x.Invamount);
                }


                var stockData = await _stocksRepository.GetCurrentStockDataByUserName(month, year, client.UserName);
                if (stockData.Count > 0)
                {
                    clientMonthlyTransSnapshot.Trading = Math.Round((decimal)stockData.Where(x => x.StType.Equals("B")).Sum(x => x.StNetcostvalue), 2);

                    clientMonthlyTransSnapshot.Delivery = Math.Round((decimal)stockData.Where(x => x.StType.Equals("S")).Sum(x => x.StNetcostvalue), 2);
                }

                clientReport.ClientInvSnapshot = clientMonthlyTransSnapshot;

                if (isZero is true)
                {
                    if (clientMonthlyTransSnapshot.MFSip == 0 && clientMonthlyTransSnapshot.Trading == 0 && clientMonthlyTransSnapshot.MFLumpsum == 0
                                && clientMonthlyTransSnapshot.Delivery == 0 && clientMonthlyTransSnapshot.GIPremium == 0
                                && clientMonthlyTransSnapshot.LIPremium == 0)
                        clientMonthlyTransSnapshots.Add(clientReport);
                }
                else
                {
                    if (clientMonthlyTransSnapshot.MFSip != 0 || clientMonthlyTransSnapshot.Trading != 0 || clientMonthlyTransSnapshot.MFLumpsum != 0
                                || clientMonthlyTransSnapshot.Delivery != 0 || clientMonthlyTransSnapshot.GIPremium != 0
                                || clientMonthlyTransSnapshot.LIPremium != 0)
                        clientMonthlyTransSnapshots.Add(clientReport);
                }

            }

            return clientMonthlyTransSnapshots;
        }
        #endregion

        #region Get Monthly Chart
        public async Task<MFMonthlyChartDto> GetMonthlyChartAsync()
        {
            var monthlyChartDto = new MFMonthlyChartDto();
            var mfTransactions = await _mutualfundRepository.GetMonthlyMFTransactionSIPLumpsum();
            monthlyChartDto.MFSipAmount = Math.Round((decimal)mfTransactions.Where(x => x.Transactiontype.Equals("PIP (SIP)")).Sum(x => x.Invamount), 2);
            monthlyChartDto.MFLumpsumAmount = Math.Round((decimal)mfTransactions.Where(x => x.Transactiontype.Equals("PIP")).Sum(x => x.Invamount), 2);

            return monthlyChartDto;
        }
        #endregion

        #region Send Client Current Investment Snapshots Email
        public int SendCurrentInvSnapshotEmailAsync(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto)
        {
            if (clientReportDto.UserEmail is null)
                return 0;

            var subject = "Your Current Investment Snapshots";
            var message = "Dear " + clientReportDto.UserName + ",\n\n" +
            "I hope this email finds you well. I wanted to provide you with an update on your current investment portfolio and share the latest snapshots for your reference.\n\n" +
            $"- Stock: {clientReportDto.ClientInvSnapshot.Stocks}\n" +
            $"- Mutual Funds: {clientReportDto.ClientInvSnapshot.MutualFunds}\n" +
            $"- MGain: {clientReportDto.ClientInvSnapshot.MGain}\n" +
            $"- Assets: {clientReportDto.ClientInvSnapshot.Assets}\n" +
            $"- Life Insurance: {clientReportDto.ClientInvSnapshot.LI}\n" +
            $"- General Insurance: {clientReportDto.ClientInvSnapshot.GI}\n\n" +
            "Please take some time to review the information and let me know if you have any questions or concerns. I am here to assist you and provide any additional clarification you may need.\n\n" +
            "As your trusted financial advisor, it is my priority to ensure that you stay informed and have a clear understanding of your investment portfolio. I encourage you to regularly review your investments and reach out to me if you have any updates to your financial goals or if you would like to discuss any adjustments to your portfolio strategy.\n\n" +
            "Thank you for your continued trust and partnership. I look forward to speaking with you soon.\n\n" +
            "Best regards,\n\n" +
            "KAGroup";
            var body = new BodyBuilder();
            body.TextBody = message;
            var email = "dippanchal02@gmail.com";

            EmailHelper.SendMailAsync(_configuration, email, subject, body);

            return 1;
        }
        #endregion

        #region Send Client Current Investment Snapshots SMS
        public int SendCurrentInvSnapshotSMSAsync(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto)
        {
            if (clientReportDto.UserMobile is null) return 0;

            var message = "Dear " + clientReportDto.UserName + ",\n" +
               "Your current investment snapshots:\n" +
               $"- Stock: {clientReportDto.ClientInvSnapshot.Stocks}\n" +
               $"- Mutual Funds: {clientReportDto.ClientInvSnapshot.MutualFunds}\n" +
               $"- MGain: {clientReportDto.ClientInvSnapshot.MGain}\n" +
               $"- Assets: {clientReportDto.ClientInvSnapshot.Assets}\n" +
               $"- Life Insurance: {clientReportDto.ClientInvSnapshot.LI}\n" +
               $"- General Insurance: {clientReportDto.ClientInvSnapshot.GI}\n\n" +
               "Best regards,\n" +
               "KAGroup";
            var mobile = "9081470774";

            SMSHelper.SendSMS(mobile, message, "");

            return 1;
        }
        #endregion

        #region Send Client Monthly Transaction Snapshots Email
        public int SendMonthlyTransSnapshotEmailAsync(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto)
        {
            if (clientReportDto.UserEmail is null)
                return 0;

            var subject = "Your Monthly Transaction Snapshots";
            var message = "Dear " + clientReportDto.UserName + ",\n\n" +
            "I hope this email finds you well. I wanted to provide you with an update on your monthly transaction portfolio and share the latest snapshots for your reference.\n\n" +
            $"- Average Trading: {clientReportDto.ClientInvSnapshot.Trading}\n" +
            $"- Mutual Fund SIP: {clientReportDto.ClientInvSnapshot.MFSip}\n" +
            $"- Average Delivery: {clientReportDto.ClientInvSnapshot.Delivery}\n" +
            $"- Life Insurance Premium: {clientReportDto.ClientInvSnapshot.LIPremium}\n" +
            $"- General Insurance Premium: {clientReportDto.ClientInvSnapshot.GIPremium}\n\n" +
            "Please take some time to review the information and let me know if you have any questions or concerns. I am here to assist you and provide any additional clarification you may need.\n\n" +
            "As your trusted financial advisor, it is my priority to ensure that you stay informed and have a clear understanding of your monthly transaction portfolio. I encourage you to regularly review your investments and reach out to me if you have any updates to your financial goals or if you would like to discuss any adjustments to your portfolio strategy.\n\n" +
            "Thank you for your continued trust and partnership. I look forward to speaking with you soon.\n\n" +
            "Best regards,\n\n" +
            "KAGroup";
            var body = new BodyBuilder();
            body.TextBody = message;
            var email = "dippanchal02@gmail.com";

            EmailHelper.SendMailAsync(_configuration, email, subject, body);

            return 1;
        }
        #endregion

        #region Send Client Monthly Transaction Snapshots SMS
        public int SendMonthlyTransSnapshotSMSAsync(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto)
        {
            if (clientReportDto.UserMobile is null) return 0;

            var message = "Dear " + clientReportDto.UserName + ",\n" +
               "Your Monthly Transaction snapshots:\n" +
               $"- Average Trading: {clientReportDto.ClientInvSnapshot.Trading}\n" +
               $"- Mutual Fund SIP: {clientReportDto.ClientInvSnapshot.MFSip}\n" +
               $"- Average Delivery: {clientReportDto.ClientInvSnapshot.Delivery}\n" +
               $"- Life Insurance Premium: {clientReportDto.ClientInvSnapshot.LIPremium}\n" +
               $"- General Insurance Premium: {clientReportDto.ClientInvSnapshot.GIPremium}\n\n" +
               "Best regards,\n" +
               "KAGroup";
            var mobile = "9081470774";

            SMSHelper.SendSMS(mobile, message, "");

            return 1;
        }
        #endregion
    }
}