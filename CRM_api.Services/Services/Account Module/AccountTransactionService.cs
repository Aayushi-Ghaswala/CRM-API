using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Account_Module;
using CRM_api.DataAccess.IRepositories.Business_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Account_Module;
using static CRM_api.Services.Helper.ConstantValue.MGainAccountPaymentConstant;

namespace CRM_api.Services.Services.Account_Module
{
    public class AccountTransactionservice : IAccountTransactionservice
    {
        private readonly IAccountTransactionRepository _accountTransactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountTransactionservice(IAccountTransactionRepository accountTransactionRepository, IMapper mapper, IAccountRepository accountRepository)
        {
            _accountTransactionRepository = accountTransactionRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
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
                var transaction = _accountTransactionRepository.GetLastAccountTrasaction(filterString, year);

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

                    if (creditTransaction is not null)
                        transaction.CreditAccount = _mapper.Map<AccountMasterDto>(creditTransaction.TblAccountMaster);
                }
                else
                {
                    var debitTransaction = _accountTransactionRepository.GetAccountTransactionByDocNo(transaction.DocNo, transaction.Debit, transaction.Credit).Result.FirstOrDefault();

                    if (debitTransaction is not null)
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
        public async Task<(ResponseDto<AccountTransactionDto>, Dictionary<string, decimal?>)> GetCompanyAndAccountWiseTransactionAsync(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams, bool isBankBook = false)
        {
            var accounts = await _accountRepository.GetUserAccountById(accountId, companyId);
            decimal totalOpeningCredit = 0;
            decimal totalOpeningDebit = 0;
            if (accounts.Count > 0)
            {
                totalOpeningCredit = Convert.ToDecimal(accounts.Where(x => x.DebitCredit == "1").Sum(x => x.OpeningBalance));
                totalOpeningDebit = Convert.ToDecimal(accounts.Where(x => x.DebitCredit == "0").Sum(x => x.OpeningBalance));
                foreach (var account in accounts)
                {
                    var openingTransactions = await _accountTransactionRepository.GetCompanyAndAccountWiseTransaction(companyId, account.AccountId, (DateTime)account.OpeningBalanceDate, startDate, search, sortingParams, null, true);
                    if (openingTransactions.Count > 0)
                    {
                        totalOpeningCredit += Convert.ToDecimal(openingTransactions.Sum(x => x.Credit));
                        totalOpeningDebit += Convert.ToDecimal(openingTransactions.Sum(x => x.Debit));
                    }
                }
            }

            var openingBalanceTransaction = new AccountTransactionDto();
            openingBalanceTransaction.DocDate = startDate;
            openingBalanceTransaction.DocNo = "";
            openingBalanceTransaction.DocType = "";
            openingBalanceTransaction.DocParticulars = "Opening Balance";
            if (totalOpeningDebit >= totalOpeningCredit)
            {
                openingBalanceTransaction.Debit = totalOpeningDebit - totalOpeningCredit;
                openingBalanceTransaction.Credit = 0;
            }
            else
            {
                openingBalanceTransaction.Credit = totalOpeningCredit - totalOpeningDebit;
                openingBalanceTransaction.Debit = 0;
            }

            var transactions = await _accountTransactionRepository.GetCompanyAndAccountWiseTransaction(companyId, accountId, startDate, endDate, search, sortingParams);
            var mappedTransactions = _mapper.Map<List<AccountTransactionDto>>(transactions);
            mappedTransactions.Insert(0, openingBalanceTransaction);
            var newTransactionList = new List<AccountTransactionDto>();

            if (isBankBook)
            {
                var dateWiseGroupTransactions = mappedTransactions.GroupBy(x => x.DocDate.Value.Date);

                foreach (var transactionKey in dateWiseGroupTransactions)
                {
                    var totalDebitCreditTransaction = new AccountTransactionDto();
                    totalDebitCreditTransaction.DocNo = "";
                    totalDebitCreditTransaction.DocType = "";
                    totalDebitCreditTransaction.DocParticulars = "Total";
                    totalDebitCreditTransaction.Debit = transactionKey.Sum(x => x.Debit);
                    totalDebitCreditTransaction.Credit = transactionKey.Sum(x => x.Credit);

                    newTransactionList.AddRange(transactionKey.ToList());
                    newTransactionList.Add(totalDebitCreditTransaction);
                }
            }
            else
                newTransactionList.AddRange(mappedTransactions);


            double pageCount = Math.Ceiling(newTransactionList.Count() / sortingParams.PageSize);
            var paginatedData = SortingExtensions.ApplyPagination(newTransactionList.AsQueryable(), sortingParams.PageNumber, sortingParams.PageSize).ToList();

            foreach (var transaction in paginatedData)
            {
                if (transaction.DocParticulars is not "Opening Balance" && transaction.DocParticulars is not "Total")
                {
                    var opptransaction = await _accountTransactionRepository.GetAccountTransactionByDocNo(transaction.DocNo, transaction.Debit, transaction.Credit);
                    if (opptransaction.Count > 0 && opptransaction.First() is not null)
                        transaction.DebitAccount = _mapper.Map<AccountMasterDto>(opptransaction.First().TblAccountMaster);
                }
            }

            var responseTransactions = new ResponseDto<AccountTransactionDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            var totalDebit = mappedTransactions.Sum(x => x.Debit);
            var totalCredit = mappedTransactions.Sum(x => x.Credit);

            Dictionary<string, decimal?> total = new Dictionary<string, decimal?>();
            total.Add("totalDebit", totalDebit);
            total.Add("totalCredit", totalCredit);

            if (totalDebit >= totalCredit)
                total.Add("totalClosingDebit", totalDebit - totalCredit);
            else
                total.Add("totalClosingCredit", totalCredit - totalDebit);

            return (responseTransactions, total);

        }
        #endregion

        #region Get Company Wise Trial Balance
        public async Task<(List<TrialBalanceDto>, Dictionary<string, decimal>)> CalculateTrailBalanceByCompanyIdAsync(int companyId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams)
        {
            var accountsData = await _accountRepository.GetUserAccountById(null, companyId);
            var trialBalanceList = new List<TrialBalanceDto>();
            Dictionary<string, decimal> total = new Dictionary<string, decimal>();

            if (accountsData.Count > 0)
            {
                var accounts = accountsData.GroupBy(x => x.TblAccountGroupMaster.AccountGrpName);
                foreach (var accountKey in accounts)
                {
                    var trialBalance = new TrialBalanceDto();
                    var trialBalanceTransactionList = new List<TrialBalanceTrasactionDto>();

                    foreach (var account in accountKey)
                    {

                        decimal totalOpeningCredit = 0;
                        decimal totalOpeningDebit = 0;
                        if (account.DebitCredit == "0")
                        {
                            totalOpeningDebit = Convert.ToDecimal(account.OpeningBalance);
                            totalOpeningCredit = 0;
                        }
                        else
                        {
                            totalOpeningCredit = Convert.ToDecimal(account.OpeningBalance);
                            totalOpeningDebit = 0;
                        }

                        var openingTransactions = await _accountTransactionRepository.GetCompanyAndAccountWiseTransaction(companyId, account.AccountId, (DateTime)account.OpeningBalanceDate, endDate, search, sortingParams);
                        if (openingTransactions.Count > 0)
                        {
                            totalOpeningCredit += Convert.ToDecimal(openingTransactions.Sum(x => x.Credit));
                            totalOpeningDebit += Convert.ToDecimal(openingTransactions.Sum(x => x.Debit));
                        }

                        if (totalOpeningCredit > 0 || totalOpeningDebit > 0)
                        {
                            var trialBalanceTransaction = new TrialBalanceTrasactionDto();
                            trialBalanceTransaction.AccountName = account.AccountName;
                            if (totalOpeningDebit >= totalOpeningCredit)
                                trialBalanceTransaction.Debit = totalOpeningDebit - totalOpeningCredit;
                            else
                                trialBalanceTransaction.Credit = totalOpeningCredit - totalOpeningDebit;

                            trialBalanceTransactionList.Add(trialBalanceTransaction);
                        }
                    }

                    trialBalance.AccountGroupName = accountKey.Key;
                    if (trialBalanceTransactionList.Count > 0)
                    {
                        trialBalance.TrialBalanceTrasactions = trialBalanceTransactionList;
                        trialBalance.TotalCredit = trialBalanceTransactionList.Sum(x => x.Credit);
                        trialBalance.TotalDebit = trialBalanceTransactionList.Sum(x => x.Debit);
                    }

                    trialBalanceList.Add(trialBalance);
                }

                total.Add("totalAccountGroupCredit", trialBalanceList.Sum(x => x.TotalCredit));
                total.Add("totalAccountGroupDebit", trialBalanceList.Sum(x => x.TotalDebit));
            }
            return (trialBalanceList, total);
        }
        #endregion

        #region Get Company Wise Juornal Entries
        public async Task<(ResponseDto<AccountTransactionDto>, Dictionary<string, decimal>)> GetCompanyWiseJVTransactionAsync(int companyId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams)
        {
            var accountJVTransactions = await _accountTransactionRepository.GetCompanyAndAccountWiseTransaction(companyId, null, startDate, endDate, search, sortingParams, MGainPayment.Journal.ToString());
            var mappedAccountJVTransactions = _mapper.Map<List<AccountTransactionDto>>(accountJVTransactions).GroupBy(x => x.DocDate.Value.Date);
            var newJVTransactions = new List<AccountTransactionDto>();

            foreach (var jvTransactionKey in mappedAccountJVTransactions)
            {
                var dateWiseTotalDebitCredit = new AccountTransactionDto();
                dateWiseTotalDebitCredit.DocNo = "";
                dateWiseTotalDebitCredit.DocType = "";
                dateWiseTotalDebitCredit.DocParticulars = "Total";
                dateWiseTotalDebitCredit.Credit = jvTransactionKey.Sum(x => x.Credit);
                dateWiseTotalDebitCredit.Debit = jvTransactionKey.Sum(x => x.Debit);

                newJVTransactions.AddRange(jvTransactionKey.ToList());
                newJVTransactions.Add(dateWiseTotalDebitCredit);
            }

            double pagecount = Math.Ceiling(newJVTransactions.Count() / sortingParams.PageSize);

            var responseJvTransaction = new ResponseDto<AccountTransactionDto>()
            {
                Values = SortingExtensions.ApplyPagination(newJVTransactions.AsQueryable(), sortingParams.PageNumber, sortingParams.PageSize).ToList(),
                Pagination = new PaginationDto()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pagecount
                }
            };

            Dictionary<string, decimal> total = new Dictionary<string, decimal>();
            var dateWiseTotalTransactions = newJVTransactions.Where(x => x.DocParticulars == "Total").ToList();
            total.Add("totalCredit", (decimal)dateWiseTotalTransactions.Sum(x => x.Credit));
            total.Add("totalDebit", (decimal)dateWiseTotalTransactions.Sum(x => x.Debit));

            return (responseJvTransaction, total);
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
                Currencyid = mapAccountTransaction.Currencyid,
                Narration = mapAccountTransaction.Narration

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
                    transaction.Narration = mapAccountTransaction.Narration;
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
                    transaction.Narration = mapAccountTransaction.Narration;
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
