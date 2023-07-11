using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Dynamic.Core;

namespace CRM_api.DataAccess.Repositories.Business_Module.WBC_Module
{
    public class WBCRepository : IWBCRepository
    {
        private readonly CRMDbContext _context;
        public readonly IUserMasterRepository _userMasterRepository;
        public readonly IInsuranceClientRepository _insuranceClientRepository;
        public readonly ILoanMasterRepository _loanMasterRepository;
        public readonly IStocksRepository _stocksRepository;
        public readonly IMutualfundRepository _mutualfundRepository;
        public readonly IMGainRepository _mGainRepository;
        public readonly IConfiguration _configuration;
        public List<WbcGPResponseModel> wbcGPResponseModels = new List<WbcGPResponseModel>();
        public List<WbcGPResponseModel> tempTableDataList = new List<WbcGPResponseModel>();
        public List<WbcGPResponseModel> wbcGPFilteredList = new List<WbcGPResponseModel>();

        public WBCRepository(CRMDbContext context, IUserMasterRepository userMasterRepository, IInsuranceClientRepository insuranceClientRepository, ILoanMasterRepository loanMasterRepository, IStocksRepository stocksRepository, IMGainRepository mGainRepository, IMutualfundRepository mutualfundRepository, IConfiguration configuration)
        {
            _context = context;
            _userMasterRepository = userMasterRepository;
            _insuranceClientRepository = insuranceClientRepository;
            _loanMasterRepository = loanMasterRepository;
            _stocksRepository = stocksRepository;
            _mGainRepository = mGainRepository;
            _mutualfundRepository = mutualfundRepository;
            _configuration = configuration;
        }

        #region Create temp table
        public async Task<int> CreateTempTable()
        {
            var createTableSql = "CREATE TABLE user_gp_temp_table (UserId INT, WbcSchemeId INT, WbcTypeName NVARCHAR(50), SubInvestmentType NVARCHAR(50), SubSubInvestmentType NVARCHAR(50), ReferralGP INT, GoldPoint INT, OnTheSpotGP INT, IsRedeemable BIT)";
            try
            {
                return await _context.Database.ExecuteSqlRawAsync(createTableSql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Create Table Type
        public async Task<int> CreateTableType()
        {
            var createType = "CREATE TYPE [kaadmin].[user_gp_table_type] AS TABLE(UserId INT, WbcSchemeId INT, WbcTypeName NVARCHAR(50), SubInvestmentType NVARCHAR(50), SubSubInvestmentType NVARCHAR(50), ReferralGP INT, GoldPoint INT, OnTheSpotGP INT, IsRedeemable BIT)";
            return await _context.Database.ExecuteSqlRawAsync(createType);
        }
        #endregion

        #region Create Stored procedure to add data
        public async Task<int> CreateSPForInsertion()
        {
            var createSP = "CREATE OR ALTER PROCEDURE [InsertEntities] @EntitiesTable user_gp_table_type READONLY AS BEGIN INSERT INTO user_gp_temp_table (UserId, WbcSchemeId, WbcTypeName, SubInvestmentType, SubSubInvestmentType, ReferralGP, GoldPoint, OnTheSpotGP, IsRedeemable) SELECT UserId, WbcSchemeId, WbcTypeName, SubInvestmentType, SubSubInvestmentType, ReferralGP, GoldPoint, OnTheSpotGP, IsRedeemable FROM @EntitiesTable END";

            return await _context.Database.ExecuteSqlRawAsync(createSP);
        }
        #endregion

        #region Create SP to get data from temp table
        public async Task<int> CreateSPForDataRetrival()
        {
            var tempDataSP = "CREATE OR ALTER PROCEDURE [GetUserGPData] AS BEGIN SELECT UserId, WbcSchemeId, WbcTypeName, SubInvestmentType, SubSubInvestmentType, ReferralGP, GoldPoint, OnTheSpotGP, IsRedeemable FROM user_gp_temp_table END";
            return await _context.Database.ExecuteSqlRawAsync(tempDataSP);
        }
        #endregion

        #region Check If Type Exists In Db
        public async Task<int> CheckIfExistsInDb()
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var query = "IF OBJECT_ID('InsertEntities', 'P') IS NOT NULL DROP PROCEDURE InsertEntities";
            using var command = new SqlCommand(query, conn);

            try
            {
                await conn.OpenAsync();
                var spResult = await command.ExecuteNonQueryAsync();

                query = "IF OBJECT_ID('GetUserGPData', 'P') IS NOT NULL DROP PROCEDURE GetUserGPData";
                command.CommandText = query;
                var getUserSPResult = await command.ExecuteNonQueryAsync();

                query = "IF OBJECT_ID('user_gp_temp_table', 'U') IS NOT NULL DROP TABLE user_gp_temp_table";
                command.CommandText = query;
                var tblResult = await command.ExecuteNonQueryAsync();

                query = "IF TYPE_ID(N'user_gp_table_type') IS NOT NULL DROP TYPE user_gp_table_type";
                command.CommandText = query;
                var tblTypeResult = await command.ExecuteNonQueryAsync();

                if (spResult == -1 && getUserSPResult == -1 && tblResult == -1 && tblTypeResult == -1)
                {
                    tblResult = await CreateTempTable();
                    tblTypeResult = await CreateTableType();
                    spResult = await CreateSPForInsertion();
                }

                if (spResult == -1 && tblResult == -1 && tblTypeResult == -1)
                    return spResult;
                
                return 0;
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    await conn.CloseAsync();
                }
                return 0;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    await conn.CloseAsync();
                }
            }
        }
        #endregion

        #region Add data into temp Table
        private async Task<int> AddDataIntoTempTable(List<WbcGPResponseModel> list)
        {
            var typeRes = await CheckIfExistsInDb();
            if (typeRes == -1)
            {
                var parameter = new SqlParameter
                {
                    ParameterName = "@EntitiesTable",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "user_gp_table_type",
                    Value = CreateDataTable(list) // Helper method to convert the list of entities to a DataTable
                };

                return await _context.Database.ExecuteSqlRawAsync("EXEC [kaadmin].[InsertEntities] @EntitiesTable", parameter);
            }
            else
                return 0;
        }
        #endregion

        private DataTable CreateDataTable(List<WbcGPResponseModel> models)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("WbcSchemeId", typeof(int));
            dataTable.Columns.Add("WbcTypeName", typeof(string));
            dataTable.Columns.Add("SubInvestmentType", typeof(string));
            dataTable.Columns.Add("SubSubInvestmentType", typeof(string));
            dataTable.Columns.Add("ReferralGP", typeof(int));
            dataTable.Columns.Add("GoldPoint", typeof(int));
            dataTable.Columns.Add("OnTheSpotGP", typeof(int));
            dataTable.Columns.Add("IsRedeemable", typeof(bool));

            foreach (WbcGPResponseModel entity in models)
            {
                dataTable.Rows.Add(entity.UserId, entity.WbcSchemeId, entity.WbcTypeName, entity.SubInvestmentType, entity.SubSubInvestmentType, entity.ReferralGP, entity.GoldPoint, entity.OnTheSpotGP, entity.IsRedeemable);
            }

            return dataTable;
        }

        #region Get WBC Shceme By WbcTypeId
        public async Task<TblWbcSchemeMaster> GetWBCSchemeByWBCTypeId(int id, DateTime date)
        {
            var wbcScheme = await _context.TblWbcSchemeMasters.Where(x => x.WbcTypeId == id && x.FromDate <= date && x.ToDate >= date).Include(x => x.TblWbcTypeMaster).Include(w => w.TblSubInvesmentType).Include(w => w.TblSubsubInvType).FirstOrDefaultAsync();

            return wbcScheme;
        }
        #endregion

        #region Get All User Referal Details
        public async Task<List<TblGoldReferral>> GetUserReferalDetails(DateTime date)
        {
            var userReferals = await _context.TblGoldReferrals.Where(x => x.RefDate.Value.Month == date.Month && x.RefDate.Value.Year == date.Year).Include(x => x.RefBy).ToListAsync();

            return userReferals;
        }
        #endregion

        #region Get Count For User Referel GP
        public async Task CalculateUserReferedGPAsync(DateTime date)
        {
            var userReferals = await GetUserReferalDetails(date);
            var userReferalsUserWise = userReferals.GroupBy(x => x.RefById);
            var wbcScheme = await GetWBCSchemeByWBCTypeId(2, date);
            foreach (var userReferalUserWise in userReferalsUserWise)
            {
                var redemableGP = 0;
                redemableGP = userReferalUserWise.Where(x => Convert.ToDateTime(x.RefDate).Month == Convert.ToDateTime(x.RefBy.UserDoj).Month && Convert.ToDateTime(x.RefDate).Year == Convert.ToDateTime(x.RefBy.UserDoj).Year).Count();
                var nonRedemableGP = userReferalUserWise.Where(x => Convert.ToDateTime(x.RefDate).Month != Convert.ToDateTime(x.RefBy.UserDoj).Month && Convert.ToDateTime(x.RefDate).Year != Convert.ToDateTime(x.RefBy.UserDoj).Year).ToList();
                int userId = (int)userReferalUserWise.Select(x => x.RefById).First();
                var user = await _userMasterRepository.GetUserMasterbyId(userId);

                //Month Calculation Formula
                var difference = 12 * (date.Year - Convert.ToDateTime(user.UserDoj).Year) + DateTime.Now.Month - Convert.ToDateTime(user.UserDoj).Month;
                var totalRemainingContacts = 0;
                for (var i = 1; i <= difference; i++)
                {
                    var nextDate = Convert.ToDateTime(user.UserDoj).AddMonths(i);
                    var totalMonthContact = userReferalUserWise.Where(x => Convert.ToDateTime(x.RefDate).Month == nextDate.Month && Convert.ToDateTime(x.RefDate).Year == nextDate.Year).Count();
                    if (totalMonthContact == 0)
                        totalRemainingContacts += (bool)user.UserFasttrack ? 30 : 10;
                    else
                        totalRemainingContacts = 0;
                }

                var wbcGP = redemableGP * wbcScheme.GoldPoint;
                var wbcGPNonRedemable = nonRedemableGP.Count * wbcScheme.GoldPoint;
                if (totalRemainingContacts <= 120)
                {
                    var currMonthNonRedemableGP = nonRedemableGP.Where(x => Convert.ToDateTime(x.RefDate).Month == date.Month && Convert.ToDateTime(x.RefDate).Year == date.Year).Count();
                    if (currMonthNonRedemableGP > 0)
                    {

                        if (await _insuranceClientRepository.GetInsClientByUserId(userId, date) > 0)
                        {
                            wbcGP += currMonthNonRedemableGP * wbcScheme.GoldPoint;
                            wbcGPNonRedemable -= currMonthNonRedemableGP * wbcScheme.GoldPoint;
                        }
                        else if (await _loanMasterRepository.GetLoanDetailByUserId(userId, date) > 0)
                        {
                            wbcGP += currMonthNonRedemableGP * wbcScheme.GoldPoint;
                            wbcGPNonRedemable -= currMonthNonRedemableGP * wbcScheme.GoldPoint;
                        }
                        else if (await _stocksRepository.GetStockMonthlyByUserName(user.UserName, date) > 0)
                        {
                            wbcGP += currMonthNonRedemableGP * wbcScheme.GoldPoint;
                            wbcGPNonRedemable -= currMonthNonRedemableGP * wbcScheme.GoldPoint;
                        }
                        else if (_mutualfundRepository.GetMFInSpecificDateForExistUser(date, null, userId).Result.Count() > 0)
                        {
                            wbcGP += currMonthNonRedemableGP * wbcScheme.GoldPoint;
                            wbcGPNonRedemable -= currMonthNonRedemableGP * wbcScheme.GoldPoint;
                        }
                        else if (await _mGainRepository.GetMonthlyMGainDetailByUserId(userId, date) > 0)
                        {
                            wbcGP += currMonthNonRedemableGP * wbcScheme.GoldPoint;
                            wbcGPNonRedemable -= currMonthNonRedemableGP * wbcScheme.GoldPoint;
                        }
                    }
                    var subSubInvType = "";
                    var subInvType = "";
                    if (wbcScheme.TblSubsubInvType != null || wbcScheme.TblSubsubInvType != null)
                    {
                        subSubInvType = String.IsNullOrEmpty(wbcScheme.TblSubsubInvType.SubInvType) ? "" : wbcScheme.TblSubsubInvType.SubInvType;
                        subInvType = String.IsNullOrEmpty(wbcScheme.TblSubInvesmentType.InvestmentType) ? "" : wbcScheme.TblSubInvesmentType.InvestmentType;
                    }
                    if (wbcGP > 0) 
                    {
                        await parentGpAllocation(userId, wbcScheme.Id, wbcScheme.TblWbcTypeMaster.WbcType, subInvType, subSubInvType, wbcGP, 0, 0);
                    }
                    if (wbcGPNonRedemable > 0)
                    {   
                        wbcGPResponseModels.Add(new WbcGPResponseModel(userId, user.UserName, wbcScheme.Id, wbcScheme.TblWbcTypeMaster.WbcType, subInvType, subSubInvType, wbcGPNonRedemable, 0, 0, false));
                    }
                }
            }
        }
        #endregion

        #region Get wbc GP of month
        public async Task<Response<WbcGPResponseModel>> GetGP(string? search, DateTime date, SortingParams sortingParams)
        {
            try
            {
                var dataResult = 0;
                //tbl_wbc_scheme_master
                var wbcSchemeMaster = await _context.TblWbcSchemeMasters.Include(w => w.TblWbcTypeMaster).Include(w => w.TblSubInvesmentType).Include(w => w.TblSubsubInvType).Where(w => w.ToDate >= date).ToListAsync();

                await CalculateUserReferedGPAsync(date);
                var wbcTypes = await _context.TblWbcTypeMasters.ToListAsync();

                #region Insurance
                //Insurance
                var insClientsList = await _context.TblInsuranceclients.Include(i => i.TblUserMaster).Include(i => i.TblSubInvesmentType).Include(i => i.TblInsuranceTypeMaster).Where(i => i.InsStartdate == date).ToListAsync();

                var wbcSchemesInv = wbcSchemeMaster.Where(w => w.TblSubInvesmentType != null).ToList().Where(w => w.TblSubInvesmentType.InvestmentType.ToLower().Contains(BusinessesConstants.LifeIns) || w.TblSubInvesmentType.InvestmentType.ToLower().Contains(BusinessesConstants.GeneralIns)).ToList();

                foreach (var client in insClientsList)
                {
                    var subSubInvTypeInv = "";
                    //For WBC New Introduction / Conversion (to parent) - 1
                    var clients = _context.TblInsuranceclients.Where(i => i.InsUserid == client.InsUserid).GroupBy(i => i.InvSubtype).Select(g => new { Inv_Subtype = g.Key, TotCount = g.Count() }).ToList();
                    foreach (var ele in clients)
                    {
                        if (ele.TotCount == 1)
                        {
                            //curr user's first parent
                            var userWbcScheme = wbcSchemesInv.FirstOrDefault(w => w.ParticularsId == client.InvSubtype && w.WbcTypeId == 1);
                            if (userWbcScheme != null)
                            {
                                if (userWbcScheme.TblSubsubInvType != null)
                                {
                                    subSubInvTypeInv = String.IsNullOrEmpty(userWbcScheme.TblSubsubInvType.SubInvType) ? "" : userWbcScheme.TblSubsubInvType.SubInvType;
                                }
                                if (client.TblUserMaster.UserParentid != null)
                                    await parentGpAllocation(client.TblUserMaster.UserParentid, userWbcScheme.Id, userWbcScheme.TblWbcTypeMaster.WbcType, userWbcScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeInv, 0, userWbcScheme.GoldPoint, 0);
                            }
                        }
                    }

                    //For WBC My Own Transaction (self) - 3
                    var userWbcOwnScheme = wbcSchemesInv.FirstOrDefault(w => w.ParticularsId == client.InvSubtype && w.WbcTypeId == 3 && w.ParticularsSubTypeId == null);
                    if (userWbcOwnScheme != null)
                    {
                        if (userWbcOwnScheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeInv = String.IsNullOrEmpty(userWbcOwnScheme.TblSubsubInvType.SubInvType) ? "" : userWbcOwnScheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.TblUserMaster.UserId, userWbcOwnScheme.Id, userWbcOwnScheme.TblWbcTypeMaster.WbcType, userWbcOwnScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeInv, 0, userWbcOwnScheme.GoldPoint, 0);
                    }

                    var userWbcOwnBenefit = wbcSchemesInv.Where(x => x.TblSubsubInvType != null && x.Business != null).FirstOrDefault(w => w.ParticularsId == client.InvSubtype && w.WbcTypeId == 3 && w.TblSubsubInvType.SubInvType.ToLower().Contains(client.InsPlantype.ToLower()));
                    if (userWbcOwnBenefit.Business != null)
                    {
                        if (client.InsAmount >= Convert.ToInt32(userWbcOwnBenefit.Business))
                        {
                            var allowable_on_the_spot_gp = 0m;
                            var everyBenefit = Math.Round((decimal)client.InsAmount / Convert.ToDecimal(userWbcOwnBenefit.Business));
                            if (everyBenefit > 0)
                            {
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefit.On_the_spot_GP) * Convert.ToDecimal(everyBenefit);
                            }
                            else
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefit.On_the_spot_GP);
                            if (userWbcOwnBenefit.TblSubsubInvType != null)
                            {
                                subSubInvTypeInv = String.IsNullOrEmpty(userWbcOwnBenefit.TblSubsubInvType.SubInvType) ? "" : userWbcOwnBenefit.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserId, userWbcOwnBenefit.Id, userWbcOwnBenefit.TblWbcTypeMaster.WbcType, userWbcOwnBenefit.TblSubInvesmentType.InvestmentType, subSubInvTypeInv, 0, 0, (int)allowable_on_the_spot_gp);
                        }
                    }

                    //Portfolio Review - 4
                    var portfolioReviewRequest = await _context.TblPortfolioReviewRequests.Where(p => Convert.ToInt32(p.RequestUserid) == client.InsUserid && p.RequestType == client.InvType && p.RequestDate == date).ToListAsync();
                    var scheme = wbcSchemesInv.FirstOrDefault(w => w.WbcTypeId == 4 && w.TblSubInvesmentType.Id == client.InvSubtype);
                    if (portfolioReviewRequest.Count == 1)
                    {
                        if (scheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeInv = String.IsNullOrEmpty(scheme.TblSubsubInvType.SubInvType) ? "" : scheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.TblUserMaster.UserId, scheme.Id, scheme.TblWbcTypeMaster.WbcType, scheme.TblSubInvesmentType.InvestmentType, subSubInvTypeInv, 0, scheme.GoldPoint, 0);
                    }
                }
                #endregion

                #region Mutual Fund
                //MF
                var mfClientsList = await _context.TblMftransactions.Include(i => i.TblUserMaster).Where(m => m.Date == date).ToListAsync();
                var wbcSchemeMF = wbcSchemeMaster.Where(w => w.TblSubInvesmentType != null).ToList().Where(w => w.TblSubInvesmentType.InvestmentType.ToLower().Contains(BusinessesConstants.MF)).ToList();

                foreach (var client in mfClientsList)
                {
                    if (client.Transactiontype.ToLower() == "pip")
                        client.Transactiontype = "Lumpsum";

                    else if (client.Transactiontype.ToLower() == "pip(sip)")
                        client.Transactiontype = "SIP";

                    var subSubInvTypeMF = "";
                    //For WBC New Introduction / Conversion (to parent) - 1
                    var mfTrans = await _context.TblMftransactions.Where(m => m.Userid == client.Userid && m.Transactiontype.Contains("SIP")).ToListAsync();
                    if (mfTrans.Count == 1)
                    {
                        var wbcScheme = wbcSchemeMF.FirstOrDefault(w => w.WbcTypeId == 1);
                        if (wbcScheme != null)
                        {
                            if (wbcScheme.TblSubsubInvType != null)
                            {
                                subSubInvTypeMF = String.IsNullOrEmpty(wbcScheme.TblSubsubInvType.SubInvType) ? "" : wbcScheme.TblSubsubInvType.SubInvType;
                            }
                            if (client.TblUserMaster.UserParentid != null)
                                await parentGpAllocation(client.TblUserMaster.UserParentid, wbcScheme.Id, wbcScheme.TblWbcTypeMaster.WbcType, wbcScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeMF, 0, wbcScheme.GoldPoint, 0);
                        }
                    }

                    //For WBC My Own Transaction (self) - 3

                    var mfUserWbcOwnScheme = wbcSchemeMF.Where(x => x.TblSubsubInvType != null).FirstOrDefault(w => w.WbcTypeId == 3 && w.TblSubsubInvType.SubInvType.ToLower().Contains(client.Transactiontype.ToLower()) && w.Business == null);
                    if (mfUserWbcOwnScheme != null)
                    {
                        if (mfUserWbcOwnScheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeMF = String.IsNullOrEmpty(mfUserWbcOwnScheme.TblSubsubInvType.SubInvType) ? "" : mfUserWbcOwnScheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.Userid, mfUserWbcOwnScheme.Id, mfUserWbcOwnScheme.TblWbcTypeMaster.WbcType, mfUserWbcOwnScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeMF, 0, mfUserWbcOwnScheme.GoldPoint, 0);
                    }

                    var userWbcOwnBenefitMF = wbcSchemeMF.Where(x => x.TblSubsubInvType != null && x.Business != null).FirstOrDefault(w => w.WbcTypeId == 3 && w.TblSubsubInvType.SubInvType.ToLower().Contains(client.Transactiontype.ToLower()) && w.Business != null);


                    var mfClients = mfClientsList.Where(i => i.Userid == client.Userid && i.Transactiontype.ToLower().Contains(client.Transactiontype.ToLower())).ToList();

                    var distinctMfTrans = mfClients.DistinctBy(m => m.Foliono).ToList();
                    var totalMfInvAmt = distinctMfTrans.Sum(m => m.Invamount);

                    if (userWbcOwnBenefitMF != null && totalMfInvAmt > 0)
                    {
                        if (totalMfInvAmt >= Convert.ToInt32(userWbcOwnBenefitMF.Business))
                        {
                            var allowable_on_the_spot_gp = 0m;
                            var everyBenefit = Math.Round((decimal)totalMfInvAmt / Convert.ToDecimal(userWbcOwnBenefitMF.Business));
                            if (everyBenefit > 0)
                            {
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitMF.On_the_spot_GP) * Convert.ToDecimal(everyBenefit);
                            }
                            else
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitMF.On_the_spot_GP);
                            if (userWbcOwnBenefitMF.TblSubsubInvType != null)
                            {
                                subSubInvTypeMF = String.IsNullOrEmpty(userWbcOwnBenefitMF.TblSubsubInvType.SubInvType) ? "" : userWbcOwnBenefitMF.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.Userid, userWbcOwnBenefitMF.Id, userWbcOwnBenefitMF.TblWbcTypeMaster.WbcType, userWbcOwnBenefitMF.TblSubInvesmentType.InvestmentType, subSubInvTypeMF, 0, 0, (int)allowable_on_the_spot_gp);
                        }
                    }

                    //Portfolio Review - 4
                    var portfolioReviewRequest = await _context.TblPortfolioReviewRequests.Where(p => Convert.ToInt32(p.RequestUserid) == client.Userid && p.RequestType == 1 && p.RequestDate == date).ToListAsync();
                    var scheme = wbcSchemeMF.FirstOrDefault(w => w.WbcTypeId == 4 && w.TblSubsubInvType.SubInvType.Contains(client.Transactiontype));
                    if (portfolioReviewRequest.Count == 1)
                    {
                        if (scheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeMF = String.IsNullOrEmpty(scheme.TblSubsubInvType.SubInvType) ? "" : scheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.Userid, scheme.Id, scheme.TblWbcTypeMaster.WbcType, scheme.TblSubInvesmentType.InvestmentType, subSubInvTypeMF, 0, scheme.GoldPoint, 0);
                    }
                }
                #endregion

                #region Mgain
                //Mgain
                var mgainClientsList = await _context.TblMgaindetails.Include(m => m.TblUserMaster).Where(m => m.Date == date && m.MgainIsactive == true).ToListAsync();
                var wbcSchemeMgain = wbcSchemeMaster.Where(w => w.TblSubInvesmentType != null).ToList().Where(w => w.TblSubInvesmentType.InvestmentType.ToLower().Contains(BusinessesConstants.Mgain)).ToList();

                foreach (var client in mgainClientsList)
                {
                    var subSubInvTypeMgain = "";
                    //For WBC New Introduction / Conversion (to parent) - 1
                    var mgainList = await _context.TblMgaindetails.Where(m => m.MgainUserid == client.MgainUserid).ToListAsync();
                    if (mgainList.Count == 1)
                    {
                        var wbcScheme = wbcSchemeMgain.FirstOrDefault(w => w.WbcTypeId == 1);
                        if (wbcScheme != null)
                        {
                            if (wbcScheme.TblSubsubInvType != null)
                            {
                                subSubInvTypeMgain = String.IsNullOrEmpty(wbcScheme.TblSubsubInvType.SubInvType) ? "" : wbcScheme.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserParentid, wbcScheme.Id, wbcScheme.TblWbcTypeMaster.WbcType, wbcScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeMgain, 0, wbcScheme.GoldPoint, 0);
                        }
                    }

                    //For WBC My Own Transaction (self) - 3
                    var mgainUserWbcOwnScheme = wbcSchemeMgain.FirstOrDefault(w => w.WbcTypeId == 3 && w.Business == null);
                    if (mgainUserWbcOwnScheme != null)
                    {
                        if (mgainUserWbcOwnScheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeMgain = String.IsNullOrEmpty(mgainUserWbcOwnScheme.TblSubsubInvType.SubInvType) ? "" : mgainUserWbcOwnScheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.MgainUserid, mgainUserWbcOwnScheme.Id, mgainUserWbcOwnScheme.TblWbcTypeMaster.WbcType, mgainUserWbcOwnScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeMgain, 0, mgainUserWbcOwnScheme.GoldPoint, 0);
                    }

                    var userWbcOwnBenefitMgain = wbcSchemeMgain.FirstOrDefault(w => w.WbcTypeId == 3 && w.Business != null);
                    var totalInvAmtMgain = mgainClientsList.Sum(i => i.MgainInvamt);

                    if (userWbcOwnBenefitMgain != null && totalInvAmtMgain > 0)
                    {
                        if (totalInvAmtMgain >= Convert.ToInt32(userWbcOwnBenefitMgain.Business))
                        {
                            var allowable_on_the_spot_gp = 0m;
                            var everyBenefit = Math.Round((decimal)totalInvAmtMgain / Convert.ToDecimal(userWbcOwnBenefitMgain.Business));
                            if (everyBenefit > 0)
                            {
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitMgain.On_the_spot_GP) * Convert.ToDecimal(everyBenefit);
                            }
                            else
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitMgain.On_the_spot_GP);
                            if (userWbcOwnBenefitMgain.TblSubsubInvType != null)
                            {
                                subSubInvTypeMgain = String.IsNullOrEmpty(userWbcOwnBenefitMgain.TblSubsubInvType.SubInvType) ? "" : userWbcOwnBenefitMgain.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserId, userWbcOwnBenefitMgain.Id, userWbcOwnBenefitMgain.TblWbcTypeMaster.WbcType, userWbcOwnBenefitMgain.TblSubInvesmentType.InvestmentType, subSubInvTypeMgain, 0, 0, (int)allowable_on_the_spot_gp);
                        }
                    }

                    //Portfolio Review - 4
                    var mgainPortfolioReview = await _context.TblPortfolioReviewRequests.Where(p => Convert.ToInt32(p.RequestUserid) == client.MgainUserid && p.RequestType == 1 && p.RequestDate == date).ToListAsync();
                    var scheme = wbcSchemeMgain.FirstOrDefault(w => w.WbcTypeId == 4);
                    if (mgainPortfolioReview.Count == 1)
                    {
                        if (scheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeMgain = String.IsNullOrEmpty(scheme.TblSubsubInvType.SubInvType) ? "" : scheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.MgainUserid, scheme.Id, scheme.TblWbcTypeMaster.WbcType, scheme.TblSubInvesmentType.InvestmentType, subSubInvTypeMgain, 0, scheme.GoldPoint, 0);
                    }
                }
                #endregion

                #region Loan Inv
                //Loan
                var loanClientsList = await _context.TblLoanMasters.Include(l => l.TblUserMaster).Where(l => l.StartDate == date).ToListAsync();
                var wbcSchemeLoan = wbcSchemeMaster.Where(w => w.TblSubInvesmentType != null).ToList().Where(w => w.TblSubInvesmentType.InvestmentType.ToLower().Contains(BusinessesConstants.Loan)).ToList();

                foreach (var client in loanClientsList)
                {
                    var subSubInvTypeLoan = "";
                    //For WBC New Introduction / Conversion (to parent) - 1
                    var loanList = await _context.TblLoanMasters.Where(m => m.UserId == client.UserId).ToListAsync();
                    if (loanList.Count == 1)
                    {
                        var wbcScheme = wbcSchemeLoan.FirstOrDefault(w => w.WbcTypeId == 1);
                        if (wbcScheme != null)
                        {
                            if (wbcScheme.TblSubsubInvType != null)
                            {
                                subSubInvTypeLoan = String.IsNullOrEmpty(wbcScheme.TblSubsubInvType.SubInvType) ? "" : wbcScheme.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserParentid, wbcScheme.Id, wbcScheme.TblWbcTypeMaster.WbcType, wbcScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeLoan, 0, wbcScheme.GoldPoint, 0);
                        }
                    }

                    //For WBC My Own Transaction (self) - 3
                    var loanUserWbcOwnScheme = wbcSchemeLoan.FirstOrDefault(w => w.WbcTypeId == 3 && w.Business == null);
                    if (loanUserWbcOwnScheme != null)
                    {
                        if (loanUserWbcOwnScheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeLoan = String.IsNullOrEmpty(loanUserWbcOwnScheme.TblSubsubInvType.SubInvType) ? "" : loanUserWbcOwnScheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.UserId, loanUserWbcOwnScheme.Id, loanUserWbcOwnScheme.TblWbcTypeMaster.WbcType, loanUserWbcOwnScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeLoan, 0, loanUserWbcOwnScheme.GoldPoint, 0);
                    }

                    var userWbcOwnBenefitLoan = wbcSchemeLoan.FirstOrDefault(w => w.WbcTypeId == 3 && w.Business != null);
                    var totalInvAmtLoan = loanClientsList.Sum(i => i.LoanAmount);

                    if (userWbcOwnBenefitLoan != null && totalInvAmtLoan > 0)
                    {
                        if (totalInvAmtLoan >= Convert.ToInt32(userWbcOwnBenefitLoan.Business))
                        {
                            var allowable_on_the_spot_gp = 0m;
                            var everyBenefit = Math.Round((decimal)totalInvAmtLoan / Convert.ToDecimal(userWbcOwnBenefitLoan.Business));
                            if (everyBenefit > 0)
                            {
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitLoan.On_the_spot_GP) * Convert.ToDecimal(everyBenefit);
                            }
                            else
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitLoan.On_the_spot_GP);
                            if (userWbcOwnBenefitLoan.TblSubsubInvType != null)
                            {
                                subSubInvTypeLoan = String.IsNullOrEmpty(userWbcOwnBenefitLoan.TblSubsubInvType.SubInvType) ? "" : userWbcOwnBenefitLoan.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserId, userWbcOwnBenefitLoan.Id, userWbcOwnBenefitLoan.TblWbcTypeMaster.WbcType, userWbcOwnBenefitLoan.TblSubInvesmentType.InvestmentType, subSubInvTypeLoan, 0, 0, (int)allowable_on_the_spot_gp);
                        }
                    }

                    //Portfolio Review - 4
                    var loanPortfolioReview = await _context.TblPortfolioReviewRequests.Where(p => Convert.ToInt32(p.RequestUserid) == client.UserId && p.RequestType == 4 && p.RequestDate == date).ToListAsync();
                    var scheme = wbcSchemeLoan.FirstOrDefault(w => w.WbcTypeId == 4);
                    if (loanPortfolioReview.Count == 1)
                    {
                        if (scheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeLoan = String.IsNullOrEmpty(scheme.TblSubsubInvType.SubInvType) ? "" : scheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.UserId, scheme.Id, scheme.TblWbcTypeMaster.WbcType, scheme.TblSubInvesmentType.InvestmentType, subSubInvTypeLoan, 0, scheme.GoldPoint, 0);
                    }
                }
                #endregion

                #region Stocks
                //Stocks
                var stockClientsList = await _context.TblStockData.Include(s => s.TblUserMaster).Where(s => s.StDate == date).ToListAsync();
                var stockClientsLists = stockClientsList.GroupBy(s => s.StClientname);
                var wbcSchemeStock = wbcSchemeMaster.Where(w => w.TblSubInvesmentType != null).ToList().Where(w => w.TblSubInvesmentType.InvestmentType.ToLower().Contains(BusinessesConstants.Stocks)).ToList();

                foreach (var clients in stockClientsLists)
                {
                    var client = clients.FirstOrDefault();
                    var subSubInvTypeStocks = "";
                    //For WBC New Introduction / Conversion (to parent) - 1
                    var stockList = await _context.TblStockData.Where(s => s.Userid == client.Userid || s.StClientname.ToLower().Equals(client.StClientname.ToLower())).ToListAsync();
                    if (stockList.Count == 1)
                    {
                        var wbcScheme = wbcSchemeStock.FirstOrDefault(w => w.WbcTypeId == 1);
                        if (wbcScheme != null)
                        {
                            if (wbcScheme.TblSubsubInvType != null)
                            {
                                subSubInvTypeStocks = String.IsNullOrEmpty(wbcScheme.TblSubsubInvType.SubInvType) ? "" : wbcScheme.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserParentid, wbcScheme.Id, wbcScheme.TblWbcTypeMaster.WbcType, wbcScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeStocks, 0, wbcScheme.GoldPoint, 0);
                        }
                    }

                    //For WBC My Own Transaction (self) - 3
                    var stockUserWbcOwnScheme = wbcSchemeStock.FirstOrDefault(w => w.WbcTypeId == 3 && w.Business == null);
                    if (stockUserWbcOwnScheme != null)
                    {
                        if (stockUserWbcOwnScheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeStocks = String.IsNullOrEmpty(stockUserWbcOwnScheme.TblSubsubInvType.SubInvType) ? "" : stockUserWbcOwnScheme.TblSubsubInvType.SubInvType;
                        }
                        if (client.Userid != null)
                            await parentGpAllocation(client.Userid, stockUserWbcOwnScheme.Id, stockUserWbcOwnScheme.TblWbcTypeMaster.WbcType, stockUserWbcOwnScheme.TblSubInvesmentType.InvestmentType, subSubInvTypeStocks, 0, stockUserWbcOwnScheme.GoldPoint, 0);
                    }

                    var userWbcOwnBenefitStock = wbcSchemeStock.FirstOrDefault(w => w.WbcTypeId == 3 && w.Business != null);

                    var allTrans = stockClientsList.Where(s => s.StClientname.ToLower().Equals(client.StClientname.ToLower()));
                    var totalInvAmtStock = allTrans.Sum(s => s.StNetcostvalue);

                    if (userWbcOwnBenefitStock != null && totalInvAmtStock > 0)
                    {
                        if (totalInvAmtStock >= Convert.ToInt32(userWbcOwnBenefitStock.Business))
                        {
                            var allowable_on_the_spot_gp = 0m;
                            var everyBenefit = Math.Round((decimal)totalInvAmtStock / Convert.ToDecimal(userWbcOwnBenefitStock.Business));
                            if (everyBenefit > 0)
                            {
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitStock.On_the_spot_GP) * Convert.ToDecimal(everyBenefit);
                            }
                            else
                                allowable_on_the_spot_gp = Convert.ToDecimal(userWbcOwnBenefitStock.On_the_spot_GP);
                            if (userWbcOwnBenefitStock.TblSubsubInvType != null)
                            {
                                subSubInvTypeStocks = String.IsNullOrEmpty(userWbcOwnBenefitStock.TblSubsubInvType.SubInvType) ? "" : userWbcOwnBenefitStock.TblSubsubInvType.SubInvType;
                            }
                            await parentGpAllocation(client.TblUserMaster.UserId, userWbcOwnBenefitStock.Id, userWbcOwnBenefitStock.TblWbcTypeMaster.WbcType, userWbcOwnBenefitStock.TblSubInvesmentType.InvestmentType, subSubInvTypeStocks, 0, 0, (int)allowable_on_the_spot_gp);
                        }
                    }

                    //Portfolio Review - 4
                    var stockPortfolioReview = await _context.TblPortfolioReviewRequests.Where(p => Convert.ToInt32(p.RequestUserid) == client.Userid && p.RequestType == 1 && p.RequestDate == date).ToListAsync();
                    var scheme = wbcSchemeStock.FirstOrDefault(w => w.WbcTypeId == 4);
                    if (stockPortfolioReview.Count == 1)
                    {
                        if (scheme.TblSubsubInvType != null)
                        {
                            subSubInvTypeStocks = String.IsNullOrEmpty(scheme.TblSubsubInvType.SubInvType) ? "" : scheme.TblSubsubInvType.SubInvType;
                        }
                        await parentGpAllocation(client.Userid, scheme.Id, scheme.TblWbcTypeMaster.WbcType, scheme.TblSubInvesmentType.InvestmentType, subSubInvTypeStocks, 0, scheme.GoldPoint, 0);
                    }
                }
                #endregion

                if (wbcGPResponseModels.Count > 0)
                    dataResult = await AddDataIntoTempTable(wbcGPResponseModels);

                var filterData = wbcGPResponseModels.AsQueryable();
                var pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

                if (search != null)
                    filterData = _context.SearchWBC<WbcGPResponseModel>(search, wbcGPResponseModels.AsQueryable());

                // Apply sorting
                var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

                // Apply pagination
                var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

                var wbcResponse = new Response<WbcGPResponseModel>()
                {
                    Values = paginatedData,
                    Pagination = new Pagination()
                    {
                        CurrentPage = sortingParams.PageNumber,
                        Count = (int)pageCount
                    }
                };
                return wbcResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Data from Temp Table
        public async Task GetTempTableData()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("GetUserGPData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int userId = reader.GetInt32("UserId");
                                int wbcSchemeId = reader.GetInt32("WbcSchemeId");
                                string wbcSchemeName = reader.GetString("WbcTypeName");
                                string subInvestmentType = reader.GetString("SubInvestmentType");
                                string subSubInvestmentType = reader.GetString("SubSubInvestmentType");
                                int referralGP = reader.GetInt32("ReferralGP");
                                int goldPoint = reader.GetInt32("GoldPoint");
                                int onTheSpotGP = reader.GetInt32("OnTheSpotGP");
                                bool isRedeemable = reader.GetBoolean("IsRedeemable");

                                // Process the data as needed
                                tempTableDataList.Add(new WbcGPResponseModel(userId, wbcSchemeId, wbcSchemeName, subInvestmentType, subSubInvestmentType, referralGP, goldPoint, onTheSpotGP, isRedeemable));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Drop table
        public async Task DropTable()
        {
            var createTableSql = "DROP TABLE user_gp_temp_table";
            await _context.Database.ExecuteSqlRawAsync(createTableSql);
        }
        #endregion

        #region Release GP
        public async Task<int> ReleaseGP(DateTime date)
        {
            var spRes = await CreateSPForDataRetrival();
            if (spRes == 0)
                return 0;

            await GetTempTableData();
            var goldPointModelList = new List<TblGoldPoint>();
            var onTheSpotList = new List<TblUserOnTheSpotGP>();
            if (tempTableDataList.Count > 0)
            {
                foreach (var data in tempTableDataList)
                {
                    var goldPoint = new TblGoldPoint();
                    var onTheSpot = new TblUserOnTheSpotGP();
                    var user = await _context.TblUserMasters.Where(u => u.UserId == data.UserId).FirstOrDefaultAsync();
                    var totalContacts = ((12 * (date.Year - Convert.ToDateTime(user.UserDoj).Year) + date.Month - Convert.ToDateTime(user.UserDoj).Month) + 1) * 10;
                    var currMonthRefferedUser = _context.TblGoldReferrals.Where(g => g.RefById == user.UserId && g.RefDate.Value.Month == date.Month && g.RefDate.Value.Year == date.Year).Count();
                    var refferedUser = _context.TblGoldReferrals.Where(g => g.RefById == user.UserId).Count();

                    var availableContacts = (refferedUser - totalContacts) * 100;

                    if (data.ReferralGP > 0 && data.WbcTypeName.Equals("WBC Referral"))
                    {
                        if (availableContacts > 0)
                        {
                            if (currMonthRefferedUser > 10)
                            {
                                var onTheSpotDebit = (currMonthRefferedUser - 10) * 100;
                                goldPoint.Timestamp = date;
                                goldPoint.Credit = data.ReferralGP;
                                goldPoint.Userid = user.UserId;
                                goldPoint.Type = data.WbcTypeName;
                                goldPoint.PointCategory = data.IsRedeemable ? 1 : 0;
                                goldPointModelList.Add(goldPoint);

                                onTheSpot.WbcSchemeId = data.WbcSchemeId;
                                onTheSpot.UserId = user.UserId;
                                onTheSpot.Credit = data.OnTheSpotGP; //0
                                onTheSpot.Debit = onTheSpotDebit;
                                onTheSpot.Date = date;
                                onTheSpot.WbcTypeName = data.WbcTypeName;
                                onTheSpotList.Add(onTheSpot);
                            }
                        }
                        else
                        {
                            goldPoint.Timestamp = date;
                            goldPoint.Credit = data.ReferralGP;
                            goldPoint.Userid = user.UserId;
                            goldPoint.Type = data.WbcTypeName;
                            goldPoint.PointCategory = data.IsRedeemable ? 1 : 0;
                            goldPointModelList.Add(goldPoint);
                        }
                    }
                    else if (data.WbcTypeName.Equals("Team earning"))
                    {
                        if (data.GoldPoint > 0 || data.ReferralGP > 0)
                        {
                            goldPoint.Timestamp = date;
                            goldPoint.Credit = data.GoldPoint + data.ReferralGP;
                            goldPoint.Userid = user.UserId;
                            goldPoint.Type = data.WbcTypeName;
                            goldPoint.PointCategory = data.IsRedeemable ? 1 : 0;
                            goldPointModelList.Add(goldPoint);
                        }
                        if (data.OnTheSpotGP > 0)
                        {
                            onTheSpot.WbcSchemeId = data.WbcSchemeId;
                            onTheSpot.UserId = user.UserId;
                            onTheSpot.Credit = data.OnTheSpotGP;
                            onTheSpot.Debit = 0;
                            onTheSpot.Date = date;
                            onTheSpot.WbcTypeName = data.WbcTypeName;
                            onTheSpotList.Add(onTheSpot);
                        }
                    }
                    else if (data.GoldPoint > 0 || data.OnTheSpotGP > 0)
                    {
                        if (data.GoldPoint > 0)
                        {
                            goldPoint.Timestamp = date;
                            goldPoint.Credit = data.GoldPoint;
                            goldPoint.Userid = user.UserId;
                            goldPoint.Type = data.WbcTypeName;
                            goldPoint.PointCategory = data.IsRedeemable ? 1 : 0;
                            goldPointModelList.Add(goldPoint);
                        }
                        if (data.OnTheSpotGP > 0)
                        {
                            onTheSpot.WbcSchemeId = data.WbcSchemeId;
                            onTheSpot.UserId = user.UserId;
                            onTheSpot.Credit = data.OnTheSpotGP;
                            onTheSpot.Debit = 0;
                            onTheSpot.Date = date;
                            onTheSpot.WbcTypeName = data.WbcTypeName;
                            onTheSpotList.Add(onTheSpot);
                        }
                    }
                }

                if (goldPointModelList.Count > 0 || onTheSpotList.Count > 0)
                {
                    var res = 0;
                    var onTheSpot = 0;
                    if (goldPointModelList.Count > 0)
                    {
                        await _context.TblGoldPoints.AddRangeAsync(goldPointModelList);
                        res = await _context.SaveChangesAsync();
                    }
                    if (onTheSpotList.Count > 0)
                    {
                        await _context.TblUserOnTheSpotGP.AddRangeAsync(onTheSpotList);
                        onTheSpot = await _context.SaveChangesAsync();
                    }

                    if (res > 0 || onTheSpot > 0)
                    {
                        await DropTable();
                        return res;
                    }
                    else 
                        return 0;
                }
            }
            return 0;
        }
        #endregion

        #region Check if already exists
        public void CheckAddSchemeEntry(WbcGPResponseModel model)
        {
            if (wbcGPResponseModels.Any(w => w.UserId == model.UserId && w.WbcTypeName.ToLower().Equals(model.WbcTypeName.ToLower()) && w.IsRedeemable == model.IsRedeemable))
            {
                var wbcGp = wbcGPResponseModels.Where(x => x.UserId == model.UserId && x.WbcTypeName.ToLower().Equals(model.WbcTypeName.ToLower())).First();
                if (!String.IsNullOrEmpty(model.SubInvestmentType))
                {
                    if (String.IsNullOrEmpty(wbcGp.SubInvestmentType))
                        wbcGp.SubInvestmentType = model.SubInvestmentType + ", ";
                    else
                        if (!wbcGp.SubInvestmentType.ToLower().Equals(model.SubInvestmentType.ToLower()))
                            wbcGp.SubInvestmentType += ", " + model.SubInvestmentType;
                }
                if (!String.IsNullOrEmpty(model.SubSubInvestmentType))
                {
                    if (String.IsNullOrEmpty(wbcGp.SubSubInvestmentType))
                        wbcGp.SubSubInvestmentType = model.SubSubInvestmentType + ", ";
                    else
                        if (!wbcGp.SubSubInvestmentType.ToLower().Equals(model.SubSubInvestmentType.ToLower()))
                        wbcGp.SubSubInvestmentType += ", " + model.SubSubInvestmentType;
                }
                wbcGp.SubInvestmentType = wbcGp.SubInvestmentType.EndsWith(", ") ? wbcGp.SubInvestmentType.Replace(", ", "") : wbcGp.SubInvestmentType;
                wbcGp.SubSubInvestmentType = wbcGp.SubSubInvestmentType.EndsWith(", ") ? wbcGp.SubSubInvestmentType.Replace(", ", "") : wbcGp.SubSubInvestmentType;
                wbcGp.GoldPoint += model.GoldPoint;
                wbcGp.ReferralGP += model.ReferralGP;
                wbcGp.OnTheSpotGP += model.OnTheSpotGP;
            }
            else
            {
                wbcGPResponseModels.Add(model);
            }
        }
        #endregion

        #region function for parent user gp allocation
        public async Task parentGpAllocation(int? userid, int wbcSchemeId, string wbcTypeName, string subInvestmentType, string subSubInvestmentType, int? referralGP, int? goldPoint, int on_the_spot)
        {
            var user = userid;
            var GpForOtherParent = (decimal)goldPoint > 0 ? (decimal)goldPoint : (decimal)referralGP > 0 ? (decimal)referralGP : on_the_spot;
            if (GpForOtherParent >= 10)
            {
                var parentUser = await _context.TblUserMasters.FirstAsync(u => u.UserId == userid);
                var wbcParent = new WbcGPResponseModel();
                if ((decimal)goldPoint > 0)
                    wbcParent = new WbcGPResponseModel(parentUser.UserId, parentUser.UserName, wbcSchemeId, wbcTypeName, subInvestmentType, subSubInvestmentType, 0, (int)GpForOtherParent, 0, true);
                else if ((decimal)on_the_spot > 0)
                    wbcParent = new WbcGPResponseModel(parentUser.UserId, parentUser.UserName, wbcSchemeId, wbcTypeName, subInvestmentType, subSubInvestmentType, 0, 0, (int)GpForOtherParent, true);
                else
                    wbcParent = new WbcGPResponseModel(parentUser.UserId, parentUser.UserName, wbcSchemeId, wbcTypeName, subInvestmentType, subSubInvestmentType, (int)GpForOtherParent, 0, 0, true);

                CheckAddSchemeEntry(wbcParent);

                if (parentUser.UserParentid != null)
                {
                    user = (int)parentUser.UserParentid;
                    GpForOtherParent = Math.Round(GpForOtherParent * 25 / 100, 0);
                    if ((decimal)goldPoint > 0)
                        await parentGpAllocation(user, 0, "Team earning", "", "", 0, (int)GpForOtherParent, 0);
                    else if ((decimal)on_the_spot > 0)
                        await parentGpAllocation(user, 0, "Team earning", "", "", 0, 0, (int)GpForOtherParent);
                    else
                        await parentGpAllocation(user, 0, "Team earning", "", "", (int)GpForOtherParent, 0, 0);
                }
            }
            else
                return;
        }
        #endregion

        #region Get all Wbc scheme types
        public async Task<Response<TblWbcTypeMaster>> GetAllWbcSchemeTypes(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblWbcTypeMasters.AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblWbcTypeMaster>(searchingParams).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var wbcSchemeTypesResponse = new Response<TblWbcTypeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return wbcSchemeTypesResponse;
        }
        #endregion

        #region Get all subInvestment types
        public async Task<Response<TblSubInvesmentType>> GetAllSubInvestmentTypes(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblSubInvesmentTypes.AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblSubInvesmentType>(searchingParams).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subInvestmentTypesResponse = new Response<TblSubInvesmentType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return subInvestmentTypesResponse;
        }
        #endregion

        #region Get all subSubInvestment types
        public async Task<Response<TblSubsubInvType>> GetAllSubSubInvestmentTypes(string? searchingParams, SortingParams sortingParams, int? subInvestmentTypeId)
        {
            double pageCount = 0;
            List<TblSubsubInvType> data = new List<TblSubsubInvType>();
            IQueryable<TblSubsubInvType> filterData = data.AsQueryable();

            if (subInvestmentTypeId != null) 
                filterData = _context.TblSubsubInvTypes.Include(s => s.TblSubInvesmentType).Where(s => s.SubInvTypeId == subInvestmentTypeId).AsQueryable();
            else
                filterData = _context.TblSubsubInvTypes.Include(s => s.TblSubInvesmentType).AsQueryable();

            if (searchingParams != null)
            {
                if (subInvestmentTypeId != null) 
                    filterData = _context.Search<TblSubsubInvType>(searchingParams).Include(s => s.TblSubInvesmentType).Where(s => s.SubInvTypeId == subInvestmentTypeId).AsQueryable();
                else
                    filterData = _context.Search<TblSubsubInvType>(searchingParams).Include(s => s.TblSubInvesmentType).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subSubInvestmentTypesResponse = new Response<TblSubsubInvType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return subSubInvestmentTypesResponse;
        }
        #endregion

        #region Get All WBC Schemes
        public async Task<Response<TblWbcSchemeMaster>> GetAllWbcSchemes(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblWbcSchemeMasters.Include(w => w.TblWbcTypeMaster).Include(s => s.TblSubInvesmentType).Include(w => w.TblSubsubInvType).AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblWbcSchemeMaster>(searchingParams).Include(w => w.TblWbcTypeMaster).Include(s => s.TblSubInvesmentType).Include(w => w.TblSubsubInvType).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var wbcSchemeResponse = new Response<TblWbcSchemeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return wbcSchemeResponse;
        }
        #endregion

        #region Add WBC Scheme
        public async Task<int> AddWbcScheme(TblWbcSchemeMaster wbcSchemeMaster)
        {
            _context.TblWbcSchemeMasters.Add(wbcSchemeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update WBC Scheme
        public Task<int> UpdateWbcScheme(TblWbcSchemeMaster wbcSchemeMaster)
        {
            _context.TblWbcSchemeMasters.Update(wbcSchemeMaster);
            return _context.SaveChangesAsync();
        }
        #endregion

        #region Delete WBC Scheme
        public async Task<int> DeleteWbcScheme(int id)
        {
            var scheme = await _context.TblWbcSchemeMasters.FindAsync(id);
            if (scheme == null) return 0;

            _context.TblWbcSchemeMasters.Remove(scheme);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
