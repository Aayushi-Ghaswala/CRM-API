using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.IServices.Sales_Module;
using static CRM_api.Services.Helper.ConstantValue.InvesmentTypeConstant;

namespace CRM_api.Services.Services.Sales_Module
{
    public class SalesDashboardService : ISalesDashboardService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IConversationHistoryRepository _conversationHistoryRepository;
        private readonly IMutualfundRepository _mutualfundRepository;
        private readonly IMapper _mapper;

        public SalesDashboardService(IUserMasterRepository userMasterRepository, IMapper mapper, ILeadRepository leadRepository, IConversationHistoryRepository conversationHistoryRepository, IMeetingRepository meetingRepository, IMutualfundRepository mutualfundRepository)
        {
            _userMasterRepository = userMasterRepository;
            _mapper = mapper;
            _leadRepository = leadRepository;
            _conversationHistoryRepository = conversationHistoryRepository;
            _meetingRepository = meetingRepository;
            _mutualfundRepository = mutualfundRepository;
        }

        #region Get User wise Lead and Meeting
        public async Task<List<SalesDashboardDto>> GetUserwiseLeadAndMeetingAsync(int? userId, int? campaignId)
        {
            var date = DateTime.Now.AddMonths(-2);
            DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var leads = await _leadRepository.GetUserwiseLeads(userId, campaignId, date);
            var meetings = await _meetingRepository.GetUserWiseMeetings(userId, date);
            var users = await _userMasterRepository.GetUserByParentId(userId, date);
            var salesDashboardDtoList = new List<SalesDashboardDto>();

            if (leads.Count > 0)
            {
                var salesDashboardDto = new SalesDashboardDto();
                salesDashboardDto.Type = "Leads";
                salesDashboardDto.Week = leads.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                salesDashboardDto.Month = leads.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year).Count();
                salesDashboardDto.Quarter = leads.Count();
                salesDashboardDtoList.Add(salesDashboardDto);
            }
            if (meetings.Count > 0)
            {
                var salesDashboardDto = new SalesDashboardDto();
                salesDashboardDto.Type = "Meetings";
                salesDashboardDto.Week = meetings.Where(x => x.DateOfMeeting >= startOfWeek && x.DateOfMeeting <= endOfWeek).Count();
                salesDashboardDto.Month = meetings.Where(x => x.DateOfMeeting.Month == DateTime.Now.Month && x.DateOfMeeting.Year == DateTime.Now.Year).Count();
                salesDashboardDto.Quarter = meetings.Count();
                salesDashboardDtoList.Add(salesDashboardDto);
            }
            if (users.Count > 0)
            {
                var salesDashboardDto = new SalesDashboardDto();
                salesDashboardDto.Type = "New Clients";
                salesDashboardDto.Week = users.Where(x => x.UserDoj.Value >= startOfWeek && x.UserDoj.Value <= endOfWeek).Count();
                salesDashboardDto.Month = users.Where(x => x.UserDoj.Value.Month == DateTime.Now.Month && x.UserDoj.Value.Year == DateTime.Now.Year).Count();
                salesDashboardDto.Quarter = users.Count();
                salesDashboardDtoList.Add(salesDashboardDto);
            }

            return salesDashboardDtoList;
        }
        #endregion

        #region Get User Wise New Client Count
        public async Task<List<SalesDashboardDto>> GetUserWiseNewClientCountAsync(int? userId)
        {
            var date = DateTime.Now.AddMonths(-2);
            DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var leads = await _leadRepository.GetUserwiseLeads(userId, null, date);
            var sortingParam = new SortingParams()
            {
                SortBy = "id"
            };
            var investmentTypes = await _leadRepository.GetInvestmentTypes(null, sortingParam);
            var salesDashboardDtoList = new List<SalesDashboardDto>();

            if (leads.Count > 0)
            {
                var mfClients = leads.Where(x => x.InterestedIn.Split(',').Any(x => x == investmentTypes.Values.First(x => x.InvestmentName == InvesmentType.MutualFund.ToString()).Id.ToString())).ToList();
                var salesMFDashboardDto = new SalesDashboardDto();
                salesMFDashboardDto.Type = "Mutual Fund";
                if (mfClients.Count > 0)
                {
                    salesMFDashboardDto.Week = mfClients.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    salesMFDashboardDto.Month = mfClients.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year).Count();
                    salesMFDashboardDto.Quarter = mfClients.Count();
                }
                salesDashboardDtoList.Add(salesMFDashboardDto);

                var stockClients = leads.Where(x => x.InterestedIn.Split(',').Any(x => x == investmentTypes.Values.First(x => x.InvestmentName == InvesmentType.MutualFund.ToString()).Id.ToString())).ToList();
                var salesStockDashboardDto = new SalesDashboardDto();
                salesStockDashboardDto.Type = "Stocks";
                if (stockClients.Count > 0)
                {
                    salesStockDashboardDto.Week = stockClients.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    salesStockDashboardDto.Month = stockClients.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year).Count();
                    salesStockDashboardDto.Quarter = stockClients.Count();
                }
                salesDashboardDtoList.Add(salesStockDashboardDto);

                var mgainClients = leads.Where(x => x.InterestedIn.Split(',').Any(x => x == investmentTypes.Values.First(x => x.InvestmentName == InvesmentType.MutualFund.ToString()).Id.ToString())).ToList();
                var salesMGainDashboardDto = new SalesDashboardDto();
                salesMGainDashboardDto.Type = "MGain";
                if (mgainClients.Count > 0)
                {
                    salesMGainDashboardDto.Week = mgainClients.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    salesMGainDashboardDto.Month = mgainClients.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year).Count();
                    salesMGainDashboardDto.Quarter = mgainClients.Count();
                }
                salesDashboardDtoList.Add(salesMGainDashboardDto);

                var insuranceClients = leads.Where(x => x.InterestedIn.Split(',').Any(x => x == investmentTypes.Values.First(x => x.InvestmentName == InvesmentType.MutualFund.ToString()).Id.ToString())).ToList();
                var salesInsDashboardDto = new SalesDashboardDto();
                salesInsDashboardDto.Type = "Insurance";
                if (insuranceClients.Count > 0)
                {
                    salesInsDashboardDto.Week = insuranceClients.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    salesInsDashboardDto.Month = insuranceClients.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year).Count();
                    salesInsDashboardDto.Quarter = insuranceClients.Count();
                }
                salesDashboardDtoList.Add(salesInsDashboardDto);

                var realEstateClients = leads.Where(x => x.InterestedIn.Split(',').Any(x => x == investmentTypes.Values.First(x => x.InvestmentName == InvesmentType.MutualFund.ToString()).Id.ToString())).ToList();
                var salesDashboardDto = new SalesDashboardDto();
                salesDashboardDto.Type = "Real Estate";
                if (realEstateClients.Count > 0)
                {
                    salesDashboardDto.Week = realEstateClients.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek).Count();
                    salesDashboardDto.Month = realEstateClients.Where(x => x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year).Count();
                    salesDashboardDto.Quarter = realEstateClients.Count();
                }
                salesDashboardDtoList.Add(salesDashboardDto);
            }

            return salesDashboardDtoList;
        }
        #endregion

        #region Get User Wise Call/Meeting Schedule Count
        public async Task<List<MeetingScheduleDto>> GetUserWiseMeetingScheduleCountAsync(int? userId)
        {
            var meetings = await _meetingRepository.GetUserWiseMeetingsSchedule(userId);
            var meetingScheduleDtoList = new List<MeetingScheduleDto>();
            var meetingScheduleDto = new MeetingScheduleDto();
            meetingScheduleDto.WeekDay = string.Concat("Today", " ", $"({DateTime.Now.ToShortDateString()})");

            if (meetings.Count > 0)
            {
                meetingScheduleDto.Meetings = meetings.Where(x => x.DateOfMeeting.Date == DateTime.Now.Date && x.Mode != "Telephonic").Count();
                meetingScheduleDto.Calls = meetings.Where(x => x.DateOfMeeting.Date == DateTime.Now.Date && x.Mode == "Telephonic").Count();
            }
            meetingScheduleDtoList.Add(meetingScheduleDto);
            var date = DateTime.Now;

            for (int i = 0; i < 6; i++)
            {
                date = date.AddDays(1);
                var meetingSchedule = new MeetingScheduleDto();
                if (i == 0)
                    meetingSchedule.WeekDay = string.Concat("Tomorrow", " ", $"({date.ToShortDateString()})");
                else
                    meetingSchedule.WeekDay = string.Concat(date.DayOfWeek.ToString(), " ", $"({date.ToShortDateString()})");

                if (meetings.Count > 0)
                {
                    meetingSchedule.Meetings = meetings.Where(x => x.DateOfMeeting.Date == date.Date && x.Mode != "Telephonic").Count();
                    meetingSchedule.Calls = meetings.Where(x => x.DateOfMeeting.Date == date.Date && x.Mode == "Telephonic").Count();
                }
                meetingScheduleDtoList.Add(meetingSchedule);

            }

            return meetingScheduleDtoList;
        }
        #endregion

        #region Get Meeting Wise Conversation History 
        public async Task<ResponseDto<ConversationHistoryDto>> GetLeadWiseConversationHistoryAsync(int leadId, string? search, SortingParams sortingParams)
        {
            var conversationHistories = await _conversationHistoryRepository.GetLeadWiseConversionHistory(leadId, search, sortingParams);
            var mapConversationHistories = _mapper.Map<ResponseDto<ConversationHistoryDto>>(conversationHistories);

            return mapConversationHistories;
        }
        #endregion

        #region Get Lead Summary Chart By Date Range
        public async Task<LeadSummaryChartDto> GetLeadSummaryChartAsync(int? assignTo, DateTime fromDate, DateTime toDate)
        {
            var leads = await _leadRepository.GetLeadsByDateRange(assignTo, fromDate, toDate);
            var userIds = leads.Item1.Select(x => x.UserId).ToList();
            var mfSchemes = await _mutualfundRepository.GetAllMFScheme();
            var mfTransactions = await _mutualfundRepository.GetMFTransactionsByUserIds(userIds, fromDate, toDate);
            var newUserClientCounts = new List<NewUserClientCountDto>();
            var leadUserMFSummaryDtos = new List<LeadUserMFSummaryDto>();
            var monthdiffrence = (12 * (toDate.Year - fromDate.Year) + toDate.Month - fromDate.Month) + 1;

            for (int i = 0; i < monthdiffrence; i++)
            {
                var date = fromDate.AddMonths(i);
                var newUserCount = leads.Item1.Where(x => x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year).Count();
                var newClientCount = leads.Item2.Where(x => x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year).Count();
                var newUserClientCount = new NewUserClientCountDto(date.ToString("MMM-yyyy"), newUserCount, newClientCount);
                newUserClientCounts.Add(newUserClientCount);

                var monthWiseMFTransactions = mfTransactions.Where(x => x.Date.Value.Month == date.Month).ToList();
                var schemeWiseMFTransactions = monthWiseMFTransactions.GroupBy(x => x.Schemename).ToList();
                decimal? currValue = 0;
                decimal? sipAmount = 0;

                foreach (var schemeWiseMFTransaction in schemeWiseMFTransactions)
                {
                    decimal? nav = 0;
                    var sipPurchaseUnit = schemeWiseMFTransaction.Where(x => x.Transactiontype == "PIP (SIP)").Sum(x => x.Noofunit);

                    var purchase = schemeWiseMFTransaction.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                    var totalPurchaseUnits = purchase.Sum(x => x.Noofunit);

                    var redeem = schemeWiseMFTransaction.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                    var redemptionUnit = redeem.Sum(x => x.Noofunit);

                    var balanceUnit = totalPurchaseUnits - redemptionUnit;
                    var scheme = mfSchemes.FirstOrDefault(x => x.SchemeName.ToLower().Equals(schemeWiseMFTransaction.Key.Replace("  ", " ").ToLower()));
                    if (scheme is not null)
                        nav = Convert.ToDecimal(scheme.NetAssetValue);
                    else
                        nav = 0;

                    currValue += Math.Round((decimal)(balanceUnit * nav), 2);
                    sipAmount += Math.Round((decimal)(sipPurchaseUnit * nav), 2);
                }

                var leadUserMFSummaryDto = new LeadUserMFSummaryDto(date.ToString("MMM-yyyy"), currValue, sipAmount);
                leadUserMFSummaryDtos.Add(leadUserMFSummaryDto);
            }

            var leadChartSummary = new LeadSummaryChartDto(newUserClientCounts, leadUserMFSummaryDtos);

            return leadChartSummary;
        }
        #endregion


    }
}
