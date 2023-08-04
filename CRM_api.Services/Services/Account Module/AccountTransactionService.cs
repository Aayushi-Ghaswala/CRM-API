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
        public async Task<(ResponseDto<AccountTransactionDto>, Dictionary<string, decimal?>)> GetAccountTransactionAsync(int? companyId, int? financialYearId, string filterString, string? searchingParams, SortingParams sortingParams)
        {
            var accountTransaction = await _accountTransactionRepository.GetAccountTransaction(companyId, financialYearId, filterString, searchingParams, sortingParams);
            var mapAccountTransaction = _mapper.Map<ResponseDto<AccountTransactionDto>>(accountTransaction.Item1);

            foreach (var transaction in mapAccountTransaction.Values)
            {
                var investment = await _accountTransactionRepository.GetInvestmentType(transaction.DocParticulars);

                if (investment is not null)
                {
                    var mapInvestment = _mapper.Map<InvestmentTypeDto>(investment);
                    transaction.investmentType = mapInvestment;
                }

                if (transaction.TransactionType is not null)
                {
                    var payment = await _accountTransactionRepository.GetPaymentTypebyName(transaction.TransactionType);

                    if (payment is not null)
                    {
                        var mapPayment = _mapper.Map<PaymentTypeDto>(payment);
                        transaction.TblPaymentType = mapPayment;
                    }
                }

                if (transaction.Debit != null || transaction.Debit != 0)
                {
                    var creditTransaction = _accountTransactionRepository.GetAccountTransactionByDocNo(transaction.DocNo, transaction.Debit, transaction.Credit).Result.FirstOrDefault();

                    if( creditTransaction is not null)
                        transaction.CreditAccount = _mapper.Map<AccountMasterDto>(creditTransaction.TblAccountMaster);
                }
                else
                {
                    var debitTransaction = _accountTransactionRepository.GetAccountTransactionByDocNo(transaction.DocNo, transaction.Debit, transaction.Credit).Result.FirstOrDefault();

                    if (debitTransaction is not null )
                        transaction.DebitAccount = _mapper.Map<AccountMasterDto>(debitTransaction.TblAccountMaster);
                }
            }
            Dictionary<string, decimal?> total = new Dictionary<string, decimal?>();
            total.Add("debit", accountTransaction.Item2);
            total.Add("credit", accountTransaction.Item3);

            return (mapAccountTransaction, total);
        }
        #endregion

        #region Get Payment type
        public async Task<ResponseDto<PaymentTypeDto>> GetPaymentTypesAsync(string? search, SortingParams sortingParams)
        {
            var pyamentTypes = await _accountTransactionRepository.GetPaymentType(search, sortingParams);
            return _mapper.Map<ResponseDto<PaymentTypeDto>>(pyamentTypes);
        }
        #endregion

        #region Get Company And Account wise Account Transaction
        public async Task<(List<AccountTransactionDto>, Dictionary<string, decimal?>)> GetCompanyAndAccountWiseTransactionAsync(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams)
        {
            var transactions = await _accountTransactionRepository.GetCompanyAndAccountWiseTransaction(companyId, accountId, startDate, endDate, search, sortingParams);
            var mappedTransactions = _mapper.Map<List<AccountTransactionDto>>(transactions);

            var totalDebit = mappedTransactions.Sum(x => x.Debit);
            var totalCredit = mappedTransactions.Sum(x => x.Credit);

            Dictionary<string, decimal?> total = new Dictionary<string, decimal?>();
            total.Add("totalDebit", totalDebit);
            total.Add("totalCredit", totalCredit);

            if (totalDebit >= totalCredit)
                total.Add("totalClosingDebit", totalDebit - totalCredit);
            else
                total.Add("totalClosingCredit", totalCredit - totalDebit);

            return (mappedTransactions, total);

        }
        #endregion

        #region Add Account Transaction
        public async Task<int> AddAccountTransactionAsync(AddAccountTransactionDto addAccountTransaction)
        {
            var accountTransactions = new List<TblAccountTransaction>();
            var mapAccountTransaction = _mapper.Map<TblAccountTransaction>(addAccountTransaction);
            mapAccountTransaction.Accountid = addAccountTransaction.DebitAccountId;
            accountTransactions.Add(mapAccountTransaction);
            var creditAccountTransaction = new TblAccountTransaction()
            {
                DocDate = mapAccountTransaction.DocDate,
                DocNo = mapAccountTransaction.DocNo,
                DocType = mapAccountTransaction.DocType,
                DocParticulars = mapAccountTransaction.DocParticulars,
                DocUserid = mapAccountTransaction.DocUserid,
                Accountid = addAccountTransaction.CreditAccountId,
                Credit = mapAccountTransaction.Debit,
                Companyid = mapAccountTransaction.Companyid,
                TransactionType = mapAccountTransaction.TransactionType,
                Currencyid = mapAccountTransaction.Currencyid
            };
            accountTransactions.Add(creditAccountTransaction);

            return await _accountTransactionRepository.AddAccountTransaction(accountTransactions);
        }
        #endregion

        #region Update Account Transaction
        public async Task<int> UpdateAccountTransactionAsync(UpdateAccountTransactionDto updateAccountTransaction)
        {
            var accTransaction = await _accountTransactionRepository.GetAccountTransactionById(updateAccountTransaction.Id);
            if (accTransaction is null) return 0;

            var accountTransactions = new List<TblAccountTransaction>();
            var mapAccountTransaction = _mapper.Map<TblAccountTransaction>(updateAccountTransaction);
            if (mapAccountTransaction.Debit is not null && mapAccountTransaction.Debit != 0)
                mapAccountTransaction.Accountid = updateAccountTransaction.DebitAccountId;
            else
                mapAccountTransaction.Accountid = updateAccountTransaction.CreditAccountId;

            accountTransactions.Add(mapAccountTransaction);
            var transaction = _accountTransactionRepository.GetAccountTransactionByDocNo(accTransaction.DocNo, accTransaction.Debit, accTransaction.Credit).Result.FirstOrDefault();
            if (transaction is not null)
            {
                if (mapAccountTransaction.Debit is not null && mapAccountTransaction.Debit != 0)
                {
                    transaction.DocDate = mapAccountTransaction.DocDate;
                    transaction.DocNo = mapAccountTransaction.DocNo;
                    transaction.DocType = mapAccountTransaction.DocType;
                    transaction.DocParticulars = mapAccountTransaction.DocParticulars;
                    transaction.DocUserid = mapAccountTransaction.DocUserid;
                    transaction.Accountid = updateAccountTransaction.CreditAccountId;
                    transaction.Credit = mapAccountTransaction.Debit;
                    transaction.Companyid = mapAccountTransaction.Companyid;
                    transaction.TransactionType = mapAccountTransaction.TransactionType;
                    transaction.Currencyid = mapAccountTransaction.Currencyid;
                    accountTransactions.Add(transaction);
                }
                else
                {
                    transaction.DocDate = mapAccountTransaction.DocDate;
                    transaction.DocNo = mapAccountTransaction.DocNo;
                    transaction.DocType = mapAccountTransaction.DocType;
                    transaction.DocParticulars = mapAccountTransaction.DocParticulars;
                    transaction.DocUserid = mapAccountTransaction.DocUserid;
                    transaction.Accountid = updateAccountTransaction.DebitAccountId;
                    transaction.Debit = mapAccountTransaction.Credit;
                    transaction.Companyid = mapAccountTransaction.Companyid;
                    transaction.TransactionType = mapAccountTransaction.TransactionType;
                    transaction.Currencyid = mapAccountTransaction.Currencyid;
                    accountTransactions.Add(transaction);
                }
            }
            return await _accountTransactionRepository.UpdateAccountTransaction(accountTransactions);
        }
        #endregion

        #region Delete Account Transaction
        public async Task<int> DeleteAccountTransactionAsync(string? docNo)
        {
            List<TblAccountTransaction> accountTransactions = await _accountTransactionRepository.GetAccountTransactionByDocNo(docNo, null, null);
            return await _accountTransactionRepository.DeleteAccountTransaction(accountTransactions);
        }
        #endregion
    }
}
