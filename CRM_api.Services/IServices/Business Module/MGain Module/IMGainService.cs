using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.MGain_Module
{
    public interface IMGainService
    {
        Task<MGainResponseDto<MGainDetailsDto>> GetAllMGainDetailsAsync(int? currencyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams, int? mgainCompanyId);
        Task<List<MGainPaymentDto>> GetPaymentByMgainIdAsync(int mGainId);
        Task<string> MGainAggrementAsync(int mGainId);
        Task<MGainPDFResponseDto> GenerateMGainAggrementAsync(int id, string htmlContent);
        Task<MGainPDFResponseDto> MGainPaymentReceipt(int id);
        Task<(MGainNCmonthlyTotalDto, string)> GetNonCumulativeMonthlyReportAsync(int month, int year, int? schemeId, decimal? tds, bool? isPayment, DateTime? crEntryDate, string? crNarration, string? searchingParams, SortingParams sortingParams, bool? isSendSMS, int? companyId, bool isJournal = false, string jvNaration = null, DateTime? jvEntryDate = null);
        Task<List<MGainValuationDto>> GetValuationReportByUserIdAsync(int UserId);
        Task<MGainPDFResponseDto> ValuationReportPDF(List<MGainValuationDto> mGainValuations);
        Task<MGainTotalInterestPaidDto<MGainUserInterestPaidDto>> GetMonthWiseInterestPaidAsync(int month, int year, string? searchingParams, SortingParams sortingParams);
        Task<MGainCumulativeReportDto> GetMgGainCumulativeInterestReportAsync(int fromYear, int toYear, int? schemeId, string? search, SortingParams sortingParams);
        Task<MGainTenYearReportDto> GetMGain10YearsInterestDetailsAsync(string userName, int schemeId, DateTime invDate, decimal mGainAmount, string mGainType);
        Task<InterestCertificateDto> GetMGainIntertestCertificateAsync(int userId, int financialYearId);
        Task<MGainLedgerDto> GetMGainInterestLedgerAsync(int userId, int financialYearId);
        Task<List<PlotMasterDto>> GetPlotsByProjectIdAsync(int projectId, int? plotId, int mgainId);
        Task<List<MGainCurrancyDto>> GetAllCurrenciesAsync();
        Task<TblMgaindetail> AddMGainDetailAsync(AddMGainDetailsDto addMGainDetails);
        Task<int> AddPaymentDetailsAsync(List<AddMGainPaymentDto> paymentDtos);
        Task<(int, string)> UpdateMGainDetailsAsync(UpdateMGainDetailsDto updateMGainDetails);
        Task<int> UpdateMGainPaymentAsync(List<UpdateMGainPaymentDto> updateMGainPayment);
        Task<int> DeleteMGainPaymentAsync(int id);
        Task<MGainPDFResponseDto> MGainPdfFiles(int id, string file);
        Task<int> AddMgainPlotDetailsAsync(List<AddMGainPlotDetailsDto> plotDto);
        Task<int> DeleteMgainPlotDetailsAsync(int id);
        Task<IList<MGainPlotDetailsDto>> GetMGainPlotDetails(int mgainId);
        Task<int> AddMgainRedemptionRequestAsync(AddMgainRedemptionRequestsDto redemptionDto);
        Task<int> UpdateMgainRedemptionRequestAsync(UpdateMgainRedemptionRequestsDto redemptionDto);
        Task<int> DeleteMgainRedemptionRequestAsync(int id, string? reason);
        Task<ResponseDto<MgainRedemptionRequestDto>> GetAllMgainRedemptionRequestAsync(string? searchingParams, SortingParams sortingParams);
        Task<MgainRedemptionRequestDto> GetMgainRedemptionRequestByIdAsync(int id);
        Task<ResponseDto<MGainDetailsDto>> GetMGainListByClientId(int ClientId);
        Task<(double, decimal, decimal)> GetMGainRedemptionAsync(int mgainId, DateTime entryDate);
    }
}
