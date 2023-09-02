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
            var mfTransaction = await _mutualfundDashBoardRepository.GetMFInSpecificDateForExistUser(fromDate, toDate);
            var monthDiffrence = (12 * (toDate.Value.Year - fromDate.Value.Year) + toDate.Value.Month - fromDate.Value.Month) + 1;
            var allScheme = await _mutualfundRepository.GetAllMFScheme();
            List<HoldingChartReportDto> userCounts = new List<HoldingChartReportDto>();

            for (var i = 0; i < monthDiffrence; i++)
            {
                HoldingChartReportDto userCountDto = new HoldingChartReportDto();
                var date = fromDate.Value.AddMonths(i);
                decimal currentAmount = 0;

                userCountDto.Month = date.ToString("MMM-yy");
                var currentMonthData = mfTransaction.Where(x => x.Date.Value.Month == date.Month && x.Date.Value.Year == date.Year).ToList();
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

            TimeWiseMutualFundSummaryDto todayMutualFundSummary = new TimeWiseMutualFundSummaryDto();
            TimeWiseMutualFundSummaryDto currentWeekMutualFundSummary = new TimeWiseMutualFundSummaryDto();
            TimeWiseMutualFundSummaryDto currentMonthMutualFundSummary = new TimeWiseMutualFundSummaryDto();
            TimeWiseMutualFundSummaryDto currentQuarterMutualFundSummary = new TimeWiseMutualFundSummaryDto();
            TimeWiseMutualFundSummaryDto currentYearMutualFundSummary = new TimeWiseMutualFundSummaryDto();
            TimeWiseMutualFundSummaryDto allTimeMutualFundSummary = new TimeWiseMutualFundSummaryDto();

            var curDate = DateTime.Now.Date;
            todayMutualFundSummary.Duration = "Today";
            var todayTransaction = mfTransaction.Where(x => x.Date == curDate).ToList();
            if (todayTransaction.Count() > 0)
            {
                todayMutualFundSummary = await GetTimeWiseMutualFund(todayTransaction, todayMutualFundSummary.Duration);
                timeWiseMutualFunds.Add(todayMutualFundSummary);
            }
            else
                timeWiseMutualFunds.Add(todayMutualFundSummary);

            var weekStartDate = DateTime.Now.AddDays(-(curDate.DayOfWeek - DayOfWeek.Monday));
            var weekEndDate = weekStartDate.AddDays(6);
            var currentWeekTransaction = mfTransaction.Where(x => x.Date >= weekStartDate && x.Date <= weekEndDate).ToList();
            currentWeekMutualFundSummary.Duration = "This Week";
            if(currentWeekTransaction.Count() > 0)
                currentWeekMutualFundSummary = await GetTimeWiseMutualFund(currentWeekTransaction, currentWeekMutualFundSummary.Duration);

            timeWiseMutualFunds.Add(currentWeekMutualFundSummary);

            var thisMonthTransaction = mfTransaction.Where(x => x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();
            currentMonthMutualFundSummary.Duration = "This Month";
            if (thisMonthTransaction.Count() > 0)
                currentMonthMutualFundSummary = await GetTimeWiseMutualFund(thisMonthTransaction, currentMonthMutualFundSummary.Duration);
                
            timeWiseMutualFunds.Add(currentMonthMutualFundSummary);

            DateTime quarterStartDate = new DateTime(curDate.Year, 3 * ((curDate.Month - 1) / 3 + 1) - 2, 1);
            DateTime quarterEndDate = new DateTime(curDate.Year, 3 * ((curDate.Month - 1) / 3 + 1) + 1, 1).AddDays(-1);
            var currQuarterTransaction = mfTransaction.Where(x => x.Date >= quarterStartDate && x.Date <= quarterEndDate).ToList();
            currentQuarterMutualFundSummary.Duration = "This Quarter";
            if (currQuarterTransaction.Count() > 0)
                currentQuarterMutualFundSummary = await GetTimeWiseMutualFund(currQuarterTransaction, currentQuarterMutualFundSummary.Duration);
                
            timeWiseMutualFunds.Add(currentQuarterMutualFundSummary);

            var currYearTransaction = mfTransaction.Where(x => x.Date.Value.Year == DateTime.Now.Year).ToList();
            currentYearMutualFundSummary.Duration = "This Year";
            if (currYearTransaction.Count() > 0)
                currentYearMutualFundSummary = await GetTimeWiseMutualFund(currYearTransaction, currentYearMutualFundSummary.Duration);
                
            timeWiseMutualFunds.Add(currentYearMutualFundSummary);

            allTimeMutualFundSummary.Duration = "All Time";
            allTimeMutualFundSummary = await GetTimeWiseMutualFund(mfTransaction, allTimeMutualFundSummary.Duration);
            timeWiseMutualFunds.Add(allTimeMutualFundSummary);

            return timeWiseMutualFunds;
        }
        #endregion

        #region Get MF Data Time Wise
        private async Task<TimeWiseMutualFundSummaryDto> GetTimeWiseMutualFund(List<TblMftransaction> tblMftransactions, string? duration)
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
