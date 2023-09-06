using AutoMapper;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;

namespace CRM_api.Services.Services.Business_Module.MutualFunds_Module
{
    public class MutualfundDashBoardService : IMutalfundDashBoardService
    {
        private readonly IMutualfundDashBoardRepository _mutualfundDashBoardRepository;
        private readonly IMutualfundRepository _mutualfundRepository;
        private readonly IMapper _mapper;

        public MutualfundDashBoardService(IMutualfundDashBoardRepository mutualfundDashBoardRepository, IMutualfundRepository mutualfundRepository, IMapper mapper)
        {
            _mutualfundDashBoardRepository = mutualfundDashBoardRepository;
            _mutualfundRepository = mutualfundRepository;
            _mapper = mapper;
        }

        #region Get Top 10 Scheme By InvestmentWise
        public async Task<List<TopTenSchemeDto>> GetTopTenSchemeByInvestmentAsync()
        {
            var schemes = await _mutualfundDashBoardRepository.GetTopTenSchemeByInvestmentWise();
            var mapSchemes = _mapper.Map<List<TopTenSchemeDto>>(schemes);

            return mapSchemes;
        }
        #endregion

        #region Get Mf Holding Summary Month Wise
        public async Task<List<HoldingChartReportDto>> GetMFHoldingSummaryAsync(DateTime? fromDate, DateTime? toDate)
        {
            var mfTransaction = await _mutualfundDashBoardRepository.GetMFInSpecificDateForExistUser(toDate);
            var monthDiffrence = (12 * (toDate.Value.Year - fromDate.Value.Year) + toDate.Value.Month - fromDate.Value.Month) + 1;
            var allScheme = await _mutualfundRepository.GetAllMFScheme();
            List<HoldingChartReportDto> userCounts = new List<HoldingChartReportDto>();

            for (var i = 0; i < monthDiffrence; i++)
            {
                HoldingChartReportDto userCountDto = new HoldingChartReportDto();
                var date = fromDate.Value.AddMonths(i);
                var lastDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
                decimal currentAmount = 0;

                userCountDto.Month = date.ToString("MMM-yyyy");
                var currentMonthData = mfTransaction.Where(x => x.Date <= lastDay).ToList();
                userCountDto.UserCount = currentMonthData.GroupBy(x => x.Username).Count();

                var schemewiseData = currentMonthData.GroupBy(x => x.Schemename).ToList();

                foreach (var schemeData in schemewiseData)
                {
                    var purchaseTransaction = schemeData.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                    var purchaseUnit = Math.Round((decimal)purchaseTransaction.Sum(x => x.Noofunit), 3);

                    var redemptionTransaction = schemeData.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                    var redemptionUnit = Math.Round((decimal)redemptionTransaction.Sum(x => x.Noofunit), 3);

                    var balanceUnit = purchaseUnit - redemptionUnit;
                    var scheme = allScheme.FirstOrDefault(x => x.SchemeName.ToLower().Equals(schemeData.Key.Replace("  ", " ").ToLower()));
                    if (scheme is not null)
                        currentAmount += Math.Round(balanceUnit * Convert.ToDecimal(scheme.NetAssetValue), 3);
                    else
                        currentAmount += 0;
                }
                userCountDto.CurrentValue = currentAmount;

                userCounts.Add(userCountDto);
            }

            return userCounts;
        }
        #endregion

        #region Get All Mutual Fund Summary For Different Time
        public async Task<List<TimeWiseMutualFundSummaryDto>> GetMFSummaryTimeWiseAsync()
        {
            var mfTransaction = await _mutualfundDashBoardRepository.GetAllMFTransaction();
            List<TimeWiseMutualFundSummaryDto> timeWiseMutualFunds = new List<TimeWiseMutualFundSummaryDto>();

            var today = DateTime.Now.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfQuarter = new DateTime(today.Year, (today.Month - 1) / 3 * 3 + 1, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            // Filter stock data for different durations
            var yearDataList = mfTransaction.Where(s => s.Date.Value.Year == today.Year).ToList();
            var quarterDataList = yearDataList.Where(s => s.Date.Value.Date >= startOfQuarter.Date && s.Date.Value <= startOfQuarter.AddMonths(2)).ToList();
            var monthDataList = quarterDataList.Where(s => s.Date.Value.Month == startOfMonth.Month).ToList();
            var weekDataList = monthDataList.Where(s => s.Date.Value.Date >= startOfWeek.Date && s.Date.Value.Date < startOfWeek.AddDays(7).Date).ToList();
            var todayDataList = weekDataList.Where(s => s.Date.Value.Date == today.Date).ToList();

            timeWiseMutualFunds.Add(await GetTimeWiseMutualFund(todayDataList, "Today"));
            timeWiseMutualFunds.Add(await GetTimeWiseMutualFund(weekDataList, "This Week"));
            timeWiseMutualFunds.Add(await GetTimeWiseMutualFund(monthDataList, "This Month"));
            timeWiseMutualFunds.Add(await GetTimeWiseMutualFund(quarterDataList, "This Quarter"));
            timeWiseMutualFunds.Add(await GetTimeWiseMutualFund(yearDataList, "This Year"));
            timeWiseMutualFunds.Add(await GetTimeWiseMutualFund(mfTransaction, "All Time"));

            return timeWiseMutualFunds;
        }
        #endregion

        #region Get MF Data Time Wise
        private async Task<TimeWiseMutualFundSummaryDto> GetTimeWiseMutualFund(List<vw_Mftransaction> tblMftransactions, string? duration)
        {
            TimeWiseMutualFundSummaryDto timeWiseMutualFundSummary = new TimeWiseMutualFundSummaryDto();

            timeWiseMutualFundSummary.Duration = duration;
            var sipTransaction = tblMftransactions.Where(x => x.Transactiontype.Equals("PIP (SIP)")).AsQueryable();
            timeWiseMutualFundSummary.SIP = sipTransaction.GroupBy(x => x.Username).Count();
            timeWiseMutualFundSummary.SIPAmount = sipTransaction.Sum(x => x.Invamount);

            var lumpSumpTransaction = tblMftransactions.Where(x => x.Transactiontype.Equals("PIP")).AsQueryable();
            timeWiseMutualFundSummary.LumpSump = lumpSumpTransaction.DistinctBy(x => x.Username).Count();
            timeWiseMutualFundSummary.LumpsumpAmount = lumpSumpTransaction.Sum(x => x.Invamount);

            var redeemptionTransaction = tblMftransactions.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale").AsQueryable();
            timeWiseMutualFundSummary.Redeemption = redeemptionTransaction.DistinctBy(x => x.Username).Count();
            timeWiseMutualFundSummary.RedeemptionAmount = redeemptionTransaction.Sum(x => x.Invamount);

            return timeWiseMutualFundSummary;
        }
        #endregion
    }
}
