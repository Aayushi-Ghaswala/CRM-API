using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Account_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Account_Module;

namespace CRM_api.Services.Services.Account_Module
{
    public class AccountTransactionservice : IAccountTransactionservice
    {
        private readonly IAccountTransactionRepository _accountTransactionRepository;
        private readonly IMapper _mapper;

        public AccountTransactionservice(IAccountTransactionRepository accountTransactionRepository, IMapper mapper)
        {
            _accountTransactionRepository = accountTransactionRepository;
            _mapper = mapper;
        }

        #region Get Transaction Doc No
        public async Task<string> GetTransactionDocNoAsync(string? filterString, string? docNo = null)
        {
            string? newDocNo = null;
            string? year = null;

            var currYear = DateTime.Now.Year;

            if (DateTime.Now.Month >= 4)
            {
                year = currYear + "-" + (currYear + 1).ToString().Substring((currYear + 1).ToString().Length - 2);
            }
            else
            {
                year = (currYear - 1).ToString() + "-" + currYear.ToString().Substring(currYear.ToString().Length - 2);
            }

            if (docNo is null)
            {
                var transaction = await _accountTransactionRepository.GetLastAccountTrasaction(filterString, year);

                if (transaction is not null)
                {
                    var no = transaction.DocNo.Split('/');
                    var number = (Convert.ToInt32(no.Last()) + 1).ToString().PadLeft(5, '0');

                    newDocNo = year + "/" + no.Skip(1).First() + "/" + number;
                }
                else
                {
                    if (filterString == "Sales")
                        newDocNo = year + "/SEL/00001";
                    else if (filterString == "Payment")
                        newDocNo = year + "/PMT/00001";
                    else if (filterString == "Receipt")
                        newDocNo = year + "/RCT/00001";
                    else if (filterString == "Journal")
                        newDocNo = year + "/JOR/00001";
                    else if (filterString == "Contra")
                        newDocNo = year + "/COR/00001";
                    else if (filterString == "Purchase")
                        newDocNo = year + "/PUR/00001";
                }
            }
            else if (docNo is not null)
            {
                var no = docNo.Split('/');
                var number = (Convert.ToInt32(no.Last()) + 1).ToString().PadLeft(5, '0');

                newDocNo = year + "/" + no.Skip(1).First() + "/" + number;
            }

            return newDocNo;
        }
        #endregion

        #region Get Account Transaction
        public async Task<ResponseDto<AccountTransactionDto>> GetAccountTransactionAsync(string filterString, string? searchingParams, SortingParams sortingParams)
        {
            var accountTransaction = await _accountTransactionRepository.GetAccountTransaction(filterString, searchingParams, sortingParams);
            var mapAccountTransaction = _mapper.Map<ResponseDto<AccountTransactionDto>>(accountTransaction);

            foreach (var transaction in mapAccountTransaction.Values)
            {
                var investment = await _accountTransactionRepository.GetInvestmentType(transaction.DocParticulars);

                if (investment is not null)
                {
                    var mapInvestment = _mapper.Map<InvestmentTypeDto>(investment);
                    transaction.investmentType = mapInvestment;
                }
            }
            return mapAccountTransaction;
        }
        #endregion

        #region Add Account Transaction
        public async Task<int> AddAccountTransactionAsync(AddAccountTransactionDto addAccountTransaction)
        {
            var mapAccountTransaction = _mapper.Map<TblAccountTransaction>(addAccountTransaction);
            return await _accountTransactionRepository.AddAccountTransaction(mapAccountTransaction);
        }
        #endregion

        #region Update Account Transaction
        public async Task<int> UpdateAccountTransactionAsync(UpdateAccountTransactionDto updateAccountTransaction)
        {
            var mapAccountTransaction = _mapper.Map<TblAccountTransaction>(updateAccountTransaction);
            return await _accountTransactionRepository.UpdateAccountTransaction(mapAccountTransaction);
        }
        #endregion
    }
}
