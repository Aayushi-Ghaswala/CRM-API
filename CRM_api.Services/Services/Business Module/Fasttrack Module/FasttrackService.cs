using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Account_Module;
using CRM_api.DataAccess.IRepositories.Business_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.IServices.Account_Module;
using CRM_api.Services.IServices.Business_Module.Fasttrack_Module;
using static CRM_api.Services.Helper.ConstantValue.MGainAccountPaymentConstant;

namespace CRM_api.Services.Services.Business_Module.Fasttrack_Module
{
    public class FasttrackService : IFasttrackService
    {
        private readonly IFasttrackRepository _fasttrackRepository;
        private readonly IAccountTransactionservice _accountTransactionservice;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountTransactionRepository _accountTransactionRepository;
        private readonly IMapper _mapper;

        public FasttrackService(IFasttrackRepository fasttrackRepository, IMapper mapper, IAccountTransactionservice accountTransactionservice, IAccountRepository accountRepository, IAccountTransactionRepository accountTransactionRepository)
        {
            _fasttrackRepository = fasttrackRepository;
            _mapper = mapper;
            _accountTransactionservice = accountTransactionservice;
            _accountRepository = accountRepository;
            _accountTransactionRepository = accountTransactionRepository;
        }

        #region Get sub Inv types
        public async Task<ResponseDto<FasttrackInvTypeResponseDto>> GetFtSubInvTypesAsync(int? invTypeId, string? search, SortingParams sortingParams)
        {
            var result = await _fasttrackRepository.GetFtSubInvTypes(invTypeId, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<FasttrackInvTypeResponseDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get Inv types
        public async Task<ResponseDto<FasttrackInvTypeResponseDto>> GetFtInvTypesAsync(int? userId, string? search, SortingParams sortingParams)
        {
            var result = await _fasttrackRepository.GetFtInvTypes(userId, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<FasttrackInvTypeResponseDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get fasttrack usernames
        public async Task<ResponseDto<UserNameDto>> GetFtUsernameAsync(int? typeId, int? subTypeId, string? search, SortingParams sortingParams)
        {
            var result = await _fasttrackRepository.GetFtUsername(typeId, subTypeId, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<UserNameDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get fasttrack ledger report
        public async Task<GoldPointResponseDto<FasttrackLedgerDto>> GetFasttrackLedgerReportAsync(int? userId, int? typeId, int? subTypeId, string? search, SortingParams sortingParams)
        {
            var result = await _fasttrackRepository.GetFasttrackLedgerReport(userId, typeId, subTypeId, search, sortingParams);
            var mappedResult = _mapper.Map<GoldPointResponseDto<FasttrackLedgerDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get fasttrack commission view
        public async Task<ResponseDto<FasttrackResponseDto>> GetFasttrackCommissionViewAsync(DateTime date, string? search, SortingParams sortingParams)
        {
            var result = await _fasttrackRepository.GetFasttrackCommissionView(date, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<FasttrackResponseDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Release fasttrack commission
        public async Task<(int, string)> ReleaseCommissionAsync(DateTime date, bool isTruncate)
        {
            var result = await _fasttrackRepository.ReleaseCommission(date, isTruncate);
            if (result.Item1 > 0 && result.Item3 is not null && result.Item3.Count > 0)
            {
                var transactions = await _accountTransactionRepository.GetAccountTransactionByDate(date);
                if (transactions.Count > 0)
                {
                    await _accountTransactionRepository.DeleteAccountTransaction(transactions);
                }

                List<TblAccountTransaction> fasttrackTransactions = new List<TblAccountTransaction>();
                string docNo = null;

                foreach (var ledger in result.Item3)
                {
                    var userAccountId = await _accountRepository.GetAccountByUserId((int)ledger.UserId);
                    if (userAccountId > 0)
                    {
                        docNo = await _accountTransactionservice.GetTransactionDocNoAsync(MGainPayment.Journal.ToString(), docNo);
                        var debitTransaction = new TblAccountTransaction(date, "Fasttrack Commission", MGainPayment.Journal.ToString(), docNo, ledger.Commission, 0, ledger.UserId, userAccountId, 0, 0, null, 1);
                        var creditTransaction = new TblAccountTransaction(date, "Fasttrack Commission", MGainPayment.Journal.ToString(), docNo, 0, ledger.Commission, ledger.UserId, 831, 0, 0, null, 1);
                        fasttrackTransactions.Add(creditTransaction);
                        fasttrackTransactions.Add(debitTransaction);
                    }
                }
                var transactionResult = await _accountTransactionRepository.AddAccountTransaction(fasttrackTransactions);
                if (transactionResult <= 0)
                    return (transactionResult, "Unable to add account transaction entry");
            }
            return (result.Item1, result.Item2);
        }
        #endregion

        #region Get Fasttrack Benefits
        public async Task<List<FasttrackBenefitsResponseDto>> GetFasttrackBenefitsAsync()
        {
            var fasttrackBenefits = await _fasttrackRepository.GetFasttrackBenefits();
            return _mapper.Map<List<FasttrackBenefitsResponseDto>>(fasttrackBenefits);
        }
        #endregion

        #region Get Fasttrack Schemes
        public async Task<List<FasttrackSchemeResponseDto>> GetFasttrackSchemesAsync()
        {
            var fasttrackSchemes = await _fasttrackRepository.GetFasttrackSchemes();
            return _mapper.Map<List<FasttrackSchemeResponseDto>>(fasttrackSchemes);
        }
        #endregion

        #region Get Fasttrack Level Commissions
        public async Task<List<FasttrackLevelCommissionResponseDto>> GetFasttrackLevelCommissionAsync()
        {
            var fasttrackLevelCommission = await _fasttrackRepository.GetFasttrackLevelCommission();
            return _mapper.Map<List<FasttrackLevelCommissionResponseDto>>(fasttrackLevelCommission);
        }
        #endregion

        #region Add Fasttrack Benefits
        public async Task<int> AddFasttrackBenefitsAsync(AddFasttrackBenefitsDto addFasttrackBenefits)
        {
            var mapFasttrackBenefits = _mapper.Map<TblFasttrackBenefits>(addFasttrackBenefits);
            return await _fasttrackRepository.AddFasttrackBenefit(mapFasttrackBenefits);
        }
        #endregion

        #region Add Fasttrack Benefits
        public async Task<int> UpdateFasttrackBenefitsAsync(UpdateFasttrackBenefitsDto updateFasttrackBenefits)
        {
            var mapFasttrackBenefits = _mapper.Map<TblFasttrackBenefits>(updateFasttrackBenefits);
            return await _fasttrackRepository.UpdateFasttrackBenefit(mapFasttrackBenefits);
        }
        #endregion

        #region Update fasttrack scheme
        public async Task<int> UpdateFasttrackSchemeAsync(UpdateFasttrackSchemeDto updateFasttrackSchemeDto)
        {
            var scheme = _mapper.Map<TblFasttrackSchemeMaster>(updateFasttrackSchemeDto);
            return await _fasttrackRepository.UpdateFasttrackScheme(scheme);
        }
        #endregion

        #region Update fasttrack levels commission
        public async Task<int> UpdateFasttrackLevelsCommissionAsync(UpdateFasttrackLevelCommissionDto updateFasttrackLevelCommissionDto)
        {
            var levelCommission = _mapper.Map<TblFasttrackLevelCommission>(updateFasttrackLevelCommissionDto);
            return await _fasttrackRepository.UpdateFasttrackLevelsCommission(levelCommission);
        }
        #endregion
    }
}