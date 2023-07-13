using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
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
        private readonly IMGainRepository _mGainRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public BusinessDashboardService(IUserMasterRepository userMasterRepository, IInsuranceClientRepository insuranceClientRepository, IStocksRepository stocksRepository, IMutualfundRepository mutualfundRepository, IMapper mapper, IConfiguration configuration, IMGainRepository mGainRepository)
        {
            _userMasterRepository = userMasterRepository;
            _insuranceClientRepository = insuranceClientRepository;
            _stocksRepository = stocksRepository;
            _mutualfundRepository = mutualfundRepository;
            _mapper = mapper;
            _configuration = configuration;
            _mGainRepository = mGainRepository;
        }

        #region Client Current Investment Snapshot
        public async Task<ResponseDto<ClientReportDto<ClientCurrentInvSnapshotDto>>> GetClientCurrentInvSnapshotAsync(string search, SortingParams sortingParams)
        {
            var category = await _userMasterRepository.GetCategoryByName(CategoryConstant.customer);
            var clients = await _userMasterRepository.GetUsersByCategoryId(category.CatId, search, sortingParams);
            List<ClientReportDto<ClientCurrentInvSnapshotDto>> clientCurrentInvSnapshots = new List<ClientReportDto<ClientCurrentInvSnapshotDto>>();

            foreach (var client in clients.Values)
            {
                var clientReport = new ClientReportDto<ClientCurrentInvSnapshotDto>();
                clientReport.UserName = client.UserName;
                clientReport.UserEmail = client.UserEmail;
                clientReport.UserMobile = client.UserMobile;
                var clientCurrentSnapshot = new ClientCurrentInvSnapshotDto();

                var lifeInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.Life.ToString());
                clientCurrentSnapshot.LI = await _insuranceClientRepository.GetInsDetailsByUserId(client.UserId, lifeInsId);

                var generalInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.General.ToString());
                clientCurrentSnapshot.GI = await _insuranceClientRepository.GetInsDetailsByUserId(client.UserId, generalInsId);

                clientCurrentSnapshot.Stocks = await _stocksRepository.GetStockDataByUserName(client.UserName);

                var mgainDetails = await _mGainRepository.GetMGainDetailsByUserId(client.UserId);
                clientCurrentSnapshot.MGain = mgainDetails.Sum(x => x.MgainInvamt);

                clientCurrentSnapshot.MutualFunds = await _mutualfundRepository.GetMFTransactionByUserId(client.UserId);

                clientReport.ClientInvSnapshot = clientCurrentSnapshot;

                clientCurrentInvSnapshots.Add(clientReport);
            }

            var pagination = _mapper.Map<PaginationDto>(clients.Pagination);

            var responseSnapshot = new ResponseDto<ClientReportDto<ClientCurrentInvSnapshotDto>>()
            {
                Values = clientCurrentInvSnapshots,
                Pagination = pagination
            };

            return responseSnapshot;
        }
        #endregion

        #region Client Monthly Transaction Snapshot
        public async Task<ResponseDto<ClientReportDto<ClientMonthlyTransSnapshotDto>>> GetClientMonthlyTransSnapshotAsync(int? month, int? year, string search, SortingParams sortingParams)
        {
            var category = await _userMasterRepository.GetCategoryByName(CategoryConstant.customer);
            var clients = await _userMasterRepository.GetUsersByCategoryId(category.CatId, search, sortingParams);
            List<ClientReportDto<ClientMonthlyTransSnapshotDto>> clientMonthlyTransSnapshots = new List<ClientReportDto<ClientMonthlyTransSnapshotDto>>();

            foreach (var client in clients.Values)
            {
                var clientReport = new ClientReportDto<ClientMonthlyTransSnapshotDto>();
                clientReport.UserName = client.UserName;
                clientReport.UserEmail = client.UserEmail;
                clientReport.UserMobile = client.UserMobile;
                var clientMonthlyTransSnapshot = new ClientMonthlyTransSnapshotDto();

                var lifeInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.Life.ToString());
                clientMonthlyTransSnapshot.LIPremium = await _insuranceClientRepository.GetInsPremiumAmountByUserId(month, year, client.UserId, lifeInsId);

                var generalInsId = await _insuranceClientRepository.GetSubInsTypeIdByName(SubInvType.General.ToString());
                clientMonthlyTransSnapshot.GIPremium = await _insuranceClientRepository.GetInsPremiumAmountByUserId(month, year, client.UserId, generalInsId);

                var stockData = await _stocksRepository.GetCurrentStockDataByUserName(month, year, client.UserName);
                if (stockData.Count > 0)
                {
                    clientMonthlyTransSnapshot.Trading = Math.Round((decimal)stockData.Where(x => x.StType.Equals("B")).Sum(x => x.StNetcostvalue), 2);

                    clientMonthlyTransSnapshot.Delivery = Math.Round((decimal)stockData.Where(x => x.StType.Equals("S")).Sum(x => x.StNetcostvalue), 2);
                }

                var mfTransactions = await _mutualfundRepository.GetMFTransactionSIPLumpsumByUserId(month, year, client.UserId);
                if (mfTransactions.Count > 0)
                {
                    clientMonthlyTransSnapshot.MFSip = mfTransactions.Where(x => x.Transactiontype == "PIP").Sum(x => x.Invamount);
                    clientMonthlyTransSnapshot.MFLumpsum = mfTransactions.Where(x => x.Transactiontype == "PIP (SIP)").Sum(x => x.Invamount);
                }

                clientReport.ClientInvSnapshot = clientMonthlyTransSnapshot;

                clientMonthlyTransSnapshots.Add(clientReport);
            }

            var pagination = _mapper.Map<PaginationDto>(clients.Pagination);

            var responseSnapshot = new ResponseDto<ClientReportDto<ClientMonthlyTransSnapshotDto>>()
            {
                Values = clientMonthlyTransSnapshots,
                Pagination = pagination
            };

            return responseSnapshot;
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
