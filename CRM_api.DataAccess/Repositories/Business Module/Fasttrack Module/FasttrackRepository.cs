using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.Fasttrack_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CRM_api.DataAccess.Repositories.Business_Module.Fasttrack_Module
{
    public class FasttrackRepository : IFasttrackRepository
    {
        private readonly CRMDbContext _context;
        private readonly IConfiguration _configuration;
        public List<FasttrackResponseModel> fasttrackResponseList = new List<FasttrackResponseModel>();
        public List<FasttrackResponseModel> tempTableDataList = new List<FasttrackResponseModel>();

        public FasttrackRepository(CRMDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Get Sub Inv types
        public async Task<Response<FasttrackInvTypeResponse>> GetFtSubInvTypes(int? invTypeId, string? search, SortingParams sortingParams)
        {
            try
            {
                double pageCount = 0;
                IQueryable<FasttrackInvTypeResponse> filterData = null;

                if (invTypeId != null)
                {
                    filterData = _context.TblFasttrackLedgers.Where(l => l.TypeId != null && l.SubTypeId != null && l.TypeId == invTypeId).Include(f => f.TblSubInvesmentType).Select(f => new FasttrackInvTypeResponse { Id = f.SubTypeId, InvName = f.TblSubInvesmentType.InvestmentType }).Distinct().AsQueryable();
                }
                else
                {
                    filterData = _context.TblFasttrackLedgers.Where(l => l.SubTypeId != null).Include(f => f.TblSubInvesmentType).Select(f => new FasttrackInvTypeResponse { Id = f.SubTypeId, InvName = f.TblSubInvesmentType.InvestmentType }).Distinct().AsQueryable();
                }

                if (search != null)
                {
                    if (invTypeId != null)
                    {
                        filterData = _context.Search<TblFasttrackLedger>(search).Where(l => l.TypeId != null && l.SubTypeId != null && l.TypeId == invTypeId).Include(f => f.TblSubInvesmentType).Select(f => new FasttrackInvTypeResponse { Id = f.SubTypeId, InvName = f.TblSubInvesmentType.InvestmentType }).Distinct().AsQueryable();
                    }
                    else
                    {
                        filterData = _context.Search<TblFasttrackLedger>(search).Where(l => l.SubTypeId != null).Include(f => f.TblSubInvesmentType).Select(f => new FasttrackInvTypeResponse { Id = f.SubTypeId, InvName = f.TblSubInvesmentType.InvestmentType }).Distinct().AsQueryable();
                    }
                }

                pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

                // Apply sorting
                var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

                // Apply pagination
                var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

                var subTypeData = new Response<FasttrackInvTypeResponse>()
                {
                    Values = paginatedData,
                    Pagination = new Pagination()
                    {
                        CurrentPage = sortingParams.PageNumber,
                        Count = (int)pageCount
                    }
                };
                return subTypeData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Inv types
        public async Task<Response<FasttrackInvTypeResponse>> GetFtInvTypes(int? userId, string? search, SortingParams sortingParams)
        {
            try
            {
                double pageCount = 0;
                IQueryable<FasttrackInvTypeResponse> filterData = null;

                if (search != null)
                {
                    filterData = _context.Search<TblFasttrackLedger>(search).Where(l => (userId == null || (l.Userid != null && l.Userid == userId)) && l.TblInvesmentType != null).Include(f => f.TblUserMaster).Include(f => f.TblInvesmentType).Select(f => new FasttrackInvTypeResponse { Id = f.TypeId, InvName = f.TblInvesmentType.InvestmentName }).Distinct().AsQueryable();
                }
                else
                {
                    filterData = _context.TblFasttrackLedgers.Where(l => (userId == null || (l.Userid != null && l.Userid == userId)) && l.TblInvesmentType != null).Include(f => f.TblUserMaster).Include(f => f.TblInvesmentType).Select(f => new FasttrackInvTypeResponse { Id = f.TypeId, InvName = f.TblInvesmentType.InvestmentName }).Distinct().AsQueryable();
                }

                pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

                // Apply sorting
                var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

                // Apply pagination
                var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

                var typeData = new Response<FasttrackInvTypeResponse>()
                {
                    Values = paginatedData,
                    Pagination = new Pagination()
                    {
                        CurrentPage = sortingParams.PageNumber,
                        Count = (int)pageCount
                    }
                };
                return typeData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get fasttrack username
        public async Task<Response<UserNameResponse>> GetFtUsername(int? typeId, int? subTypeId, string? search, SortingParams sortingParams)
        {
            try
            {
                double pageCount = 0;
                IQueryable<UserNameResponse> filterData = null;

                if (search != null)
                {
                    filterData = _context.Search<TblFasttrackLedger>(search).Where(f => (typeId == null || (f.TypeId != null && f.TypeId == typeId)) && (subTypeId == null || (f.SubTypeId != null && f.SubTypeId == subTypeId))).Include(f => f.TblUserMaster).Select(f => new UserNameResponse { UserId = f.Userid, UserName = f.TblUserMaster.UserName }).Distinct().AsQueryable();
                }
                else
                {
                    filterData = _context.TblFasttrackLedgers.Where(f => (typeId == null || (f.TypeId != null && f.TypeId == typeId)) && (subTypeId == null || (f.SubTypeId != null && f.SubTypeId == subTypeId)) && f.Userid != null).Include(f => f.TblUserMaster).Select(f => new UserNameResponse { UserId = f.Userid, UserName = f.TblUserMaster.UserName }).Distinct().AsQueryable();
                }

                pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

                // Apply sorting
                var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

                // Apply pagination
                var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

                var userData = new Response<UserNameResponse>()
                {
                    Values = paginatedData,
                    Pagination = new Pagination()
                    {
                        CurrentPage = sortingParams.PageNumber,
                        Count = (int)pageCount
                    }
                };
                return userData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get fasttrack ledger report
        public async Task<LedgerResponse<TblFasttrackLedger>> GetFasttrackLedgerReport(int? userId, int? typeId, int? subTypeId, string? search, SortingParams sortingParams)
        {
            try
            {
                double pageCount = 0;
                List<TblFasttrackLedger> data = new List<TblFasttrackLedger>();
                IQueryable<TblFasttrackLedger> filterData = data.AsQueryable();

                decimal? totalCredit = 0;
                decimal? totalDebit = 0;

                filterData = _context.TblFasttrackLedgers
                            .Where(f =>
                                (userId == null || f.Userid == userId) &&
                                (typeId == null || (f.TypeId != null && f.TypeId == typeId)) &&
                                (subTypeId == null || (f.SubTypeId != null && f.SubTypeId == subTypeId))
                            )
                            .Include(f => f.TblUserMaster)
                            .Include(f => f.TblInvesmentType)
                            .Include(f => f.TblSubInvesmentType)
                            .AsQueryable();

                totalCredit = filterData.Where(g => g.Credit != 0).Sum(g => g.Credit);
                totalDebit = filterData.Where(g => g.Debit != 0).Sum(g => g.Debit);

                if (search != null)
                {
                    filterData = _context.Search<TblFasttrackLedger>(search)
                                    .Where(f =>
                                        (userId == null || f.Userid == userId) &&
                                        (typeId == null || (f.TypeId != null && f.TypeId == typeId)) &&
                                        (subTypeId == null || (f.SubTypeId != null && f.SubTypeId == subTypeId))
                                    )
                                    .Include(f => f.TblUserMaster)
                                    .Include(f => f.TblInvesmentType)
                                    .Include(f => f.TblSubInvesmentType)
                                    .AsQueryable();
                }

                pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

                // Apply sorting
                var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

                // Apply pagination
                var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

                var fasttrackData = new Response<TblFasttrackLedger>()
                {
                    Values = paginatedData,
                    Pagination = new Pagination()
                    {
                        CurrentPage = sortingParams.PageNumber,
                        Count = (int)pageCount
                    }
                };
                var fasttrackResponse = new LedgerResponse<TblFasttrackLedger>()
                {
                    response = fasttrackData,
                    TotalCredit = totalCredit,
                    TotalDebit = totalDebit,
                };
                return fasttrackResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region check and add entry 
        public async Task CheckAddBusinessCommissionEntry(FasttrackResponseModel fasttrackResponseModel)
        {
            if (fasttrackResponseList.Any(f => f.UserId == fasttrackResponseModel.UserId))
            {
                var fasttrack = fasttrackResponseList.First(f => f.UserId == fasttrackResponseModel.UserId);
                fasttrack.Commission += fasttrackResponseModel.Commission;
            }
            else
                fasttrackResponseList.Add(fasttrackResponseModel);
        }
        #endregion

        #region Parent bussiness commission allocation
        public async Task parentBussinessCommissionAllocation(int userId, decimal commission, string userLevel, int user_parent_level)
        {
            if (user_parent_level < 6 && commission is not 0 or > (decimal)0.000)
            {
                var fasttrackUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == userId);

                var fasttrackModel = new FasttrackResponseModel(userId, fasttrackUser, userLevel, commission, 0, "Fasttrack Bussiness Commission For Parent");
                await CheckAddBusinessCommissionEntry(fasttrackModel);

                if (fasttrackUser.UserParentid != null)
                {
                    var parentCommission = Math.Round(commission * 25 / 100, 3);
                    await parentBussinessCommissionAllocation((int)fasttrackUser.UserParentid, parentCommission, userLevel, user_parent_level + 1);
                }
            }
        }
        #endregion

        #region Get fasttrack commission view
        public async Task<Response<FasttrackResponseModel>> GetFasttrackCommissionView(DateTime date, string? search, SortingParams sortingParams)
        {
            try
            {
                #region TblFasttrackSchemeMasters
                var fasttrackUsersList = await _context.TblUserMasters.Where(u => u.UserFasttrack == true).ToListAsync();
                fasttrackUsersList.Reverse();
                foreach (var fasttrackUser in fasttrackUsersList)
                {
                    var totalWbcCount = _context.TblUserMasters.Where(u => u.UserParentid == fasttrackUser.UserId && u.UserWbcActive == true && u.UserFasttrack == false && u.UserDoj == date).Count();
                    var totalFasttrackCount = _context.TblUserMasters.Where(u => u.UserParentid == fasttrackUser.UserId && u.UserFasttrack == true && u.FastTrackActivationDate == date).Count();

                    if (totalFasttrackCount > 0)
                    {
                        var count = totalFasttrackCount;
                        while (count != 0)
                        {
                            await parentCommissionAllocation(fasttrackUser.UserId, 1);
                            count--;
                        }
                    }
                    if (totalWbcCount > 0)
                    {
                        var count = totalWbcCount;
                        while (count != 0)
                        {
                            var fasttrackUserLevel = await GetUserFasttrackCategory(fasttrackUser.UserId);
                            var fasttrackSchemeMaster = await _context.TblFasttrackSchemeMasters.FirstOrDefaultAsync(f => f.Level == fasttrackUserLevel);
                            var userGP = fasttrackSchemeMaster.Goldpoint;
                            await parentGpAllocation(fasttrackUser.UserId, 1, (int)userGP);
                            count--;
                        }
                    }
                }
                #endregion

                var fasttrackBenefits = await GetFasttrackBenefits();

                //Check for business inv
                var stockBenefit = fasttrackBenefits.FirstOrDefault(f => f.Product.ToLower().Contains(BusinessConstants.Stocks));
                if (stockBenefit != null)
                {
                    #region Stocks
                    var stockTransList = await _context.TblStockData.Include(s => s.TblUserMaster).Where(s => s.TblUserMaster.UserFasttrack == true && s.StDate == date).ToListAsync();
                    var stockUsers = stockTransList.GroupBy(s => s.StClientname);

                    var stockSharingBrokerage = await _context.TblStockSharingBrokerage.FirstOrDefaultAsync(s => s.BrokerageName.ToLower().Contains(BusinessConstants.KAGroup));

                    foreach (var userTrans in stockUsers)
                    {
                        var bussUserId = (int)userTrans.First().Userid;
                        var bussinessUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == bussUserId || u.UserName.ToLower().Contains(userTrans.First().StClientname.ToLower()));
                        var parentUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == bussinessUser.UserParentid);
                        var userLevel = await GetUserFasttrackCategory(bussUserId);
                        var usersPercentageForComm = await FindUserLevelCommission(userLevel, stockBenefit);

                        decimal firstParentComm = 0m;
                        foreach (var stockTrans in userTrans)
                        {
                            var totalCommForEachTrans = stockTrans.StBrokerage * stockTrans.StQty;
                            //75%
                            var totalKaGroupBrokrage = Math.Round((decimal)totalCommForEachTrans * stockSharingBrokerage.BrokeragePercentage / 100, 3);
                            firstParentComm = Math.Round(totalKaGroupBrokrage * usersPercentageForComm / 100, 3);

                            if (stockBenefit.IsParentAllocation)
                                await parentBussinessCommissionAllocation(parentUser.UserId, firstParentComm, userLevel, 1);
                            else
                            {
                                var fasttrackModel = new FasttrackResponseModel(parentUser.UserId, parentUser, "", firstParentComm, 0, "Fasttrack Bussiness Commission For Parent");
                                await CheckAddBusinessCommissionEntry(fasttrackModel);
                            }
                        }
                    }
                    #endregion
                }

                var mgainBenefit = fasttrackBenefits.FirstOrDefault(f => f.Product.ToLower().Contains(BusinessConstants.Mgain));
                if (mgainBenefit != null)
                {
                    #region Mgain
                    var mgainClientList = await _context.TblMgaindetails.Include(m => m.TblUserMaster).Where(m => m.Date == date && m.TblUserMaster.UserFasttrack == true).ToListAsync();
                    var mgainList = mgainClientList.GroupBy(m => m.MgainUserid);

                    foreach (var user in mgainList)
                    {
                        var totalInvMgainAmount = user.Sum(m => m.MgainInvamt);
                        var parentUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == user.FirstOrDefault().TblUserMaster.UserParentid);
                        if (parentUser == null)
                            continue;
                        var userLevel = await GetUserFasttrackCategory((int)user.FirstOrDefault().MgainUserid);
                        decimal usersPercentageForComm = await FindUserLevelCommission(userLevel, mgainBenefit);

                        var userCommission = Math.Round((decimal)totalInvMgainAmount * usersPercentageForComm / 100, 3);
                        if (mgainBenefit.IsParentAllocation)
                            await parentBussinessCommissionAllocation(parentUser.UserId, userCommission, userLevel, 1);
                        else
                        {
                            var fasttrackModel = new FasttrackResponseModel(parentUser.UserId, parentUser, userLevel, userCommission, 0, "Fasttrack Bussiness Commission For Parent");
                            await CheckAddBusinessCommissionEntry(fasttrackModel);
                        }
                    }
                    #endregion
                }

                var loanBenefit = fasttrackBenefits.FirstOrDefault(f => f.Product.ToLower().Contains(BusinessConstants.Loan));
                if (loanBenefit != null)
                {
                    #region Loan
                    var loanAccTransList = await _context.TblAccountTransactions.Where(l => l.DocType.ToLower().Equals(BusinessConstants.Journal) && l.DocParticulars.ToLower().Equals(BusinessConstants.Loan)).ToListAsync();

                    var loanTransList = loanAccTransList.DistinctBy(x => x.DocNo).ToList();

                    //var userIds = loanTransList.Select(l => l.DocUserid).ToList();

                    foreach (var loan in loanTransList)
                    {
                        var user = await _context.TblUserMasters.Where(u => u.UserId == loan.DocUserid).FirstOrDefaultAsync();
                        if (user != null && user.UserParentid != null)
                        {
                            var parentUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == user.UserParentid);
                            var userLevel = await GetUserFasttrackCategory((int)loan.DocUserid);
                            decimal usersPercentageForComm = await FindUserLevelCommission(userLevel, loanBenefit);

                            var loanAmount = loan.Credit is null or 0 ? (decimal)loan.Debit : (decimal)loan.Credit;
                            var userCommission = Math.Round(loanAmount * usersPercentageForComm / 100, 3);
                            if (loanBenefit.IsParentAllocation)
                                await parentBussinessCommissionAllocation((int)user.UserParentid, userCommission, userLevel, 1);
                            else
                            {
                                var fasttrackModel = new FasttrackResponseModel(parentUser.UserId, parentUser, userLevel, userCommission, 0, "Fasttrack Bussiness Commission For Parent");
                                await CheckAddBusinessCommissionEntry(fasttrackModel);
                            }
                        }
                    }
                    #endregion
                }

                if (fasttrackResponseList.Count > 0)
                {
                    await AddDataIntoTempTable(fasttrackResponseList);
                }
                var filterData = fasttrackResponseList.AsQueryable();
                var pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

                if (search != null)
                    filterData = _context.SearchFasttrack<FasttrackResponseModel>(search, fasttrackResponseList.AsQueryable());

                // Apply sorting
                var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

                // Apply pagination
                var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

                var fasttrackResponse = new Response<FasttrackResponseModel>()
                {
                    Values = paginatedData,
                    Pagination = new Pagination()
                    {
                        CurrentPage = sortingParams.PageNumber,
                        Count = (int)pageCount
                    }
                };
                return fasttrackResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region User level wise commission
        public async Task<decimal> FindUserLevelCommission(string userLevel, TblFasttrackBenefits benefits)
        {
            decimal usersPercentageForComm = 0m;

            if (userLevel == BusinessConstants.UserLevel.Basic.ToString())
                usersPercentageForComm = benefits.Basic;
            else if (userLevel == BusinessConstants.UserLevel.Silver.ToString())
                usersPercentageForComm = benefits.Silver;
            else if (userLevel == BusinessConstants.UserLevel.Gold.ToString())
                usersPercentageForComm = benefits.Gold;
            else if (userLevel == BusinessConstants.UserLevel.Platinum.ToString())
                usersPercentageForComm = benefits.Platinum;
            else if (userLevel == BusinessConstants.UserLevel.Diamond.ToString())
                usersPercentageForComm = benefits.Diamond;
            return usersPercentageForComm;
        }
        #endregion

        #region Add data into temp Table
        private async Task<int> AddDataIntoTempTable(List<FasttrackResponseModel> list)
        {
            var typeRes = await CheckIfExistsInDb();
            if (typeRes == -1)
            {
                var parameter = new SqlParameter
                {
                    ParameterName = "@EntitiesTable",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "user_ft_table_type",
                    Value = CreateDataTable(list) // Helper method to convert the list of entities to a DataTable
                };

                return await _context.Database.ExecuteSqlRawAsync("EXEC [kaadmin].[InsertFasttrackEntities] @EntitiesTable", parameter);
            }
            else
                return 0;
        }
        #endregion

        #region Create data table for SP
        private DataTable CreateDataTable(List<FasttrackResponseModel> models)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserLevel", typeof(string));
            dataTable.Columns.Add("Commission", typeof(decimal));
            dataTable.Columns.Add("GoldPoint", typeof(int));

            foreach (FasttrackResponseModel entity in models)
            {
                dataTable.Rows.Add(entity.UserId, entity.UserLevel, entity.Commission, entity.GoldPoint);
            }

            return dataTable;
        }
        #endregion

        #region Check If Type Exists In Db
        public async Task<int> CheckIfExistsInDb()
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var query = "IF OBJECT_ID('InsertFasttrackEntities', 'P') IS NOT NULL DROP PROCEDURE InsertFasttrackEntities";
            using var command = new SqlCommand(query, conn);

            try
            {
                await conn.OpenAsync();
                var spResult = await command.ExecuteNonQueryAsync();

                query = "IF OBJECT_ID('GetUserFasttrackData', 'P') IS NOT NULL DROP PROCEDURE GetUserFasttrackData";
                command.CommandText = query;
                var getUserSPResult = await command.ExecuteNonQueryAsync();

                query = "IF OBJECT_ID('user_ft_temp_table', 'U') IS NOT NULL DROP TABLE user_ft_temp_table";
                command.CommandText = query;
                var tblResult = await command.ExecuteNonQueryAsync();

                query = "IF TYPE_ID(N'user_ft_table_type') IS NOT NULL DROP TYPE user_ft_table_type";
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

        #region Create temp table
        public async Task<int> CreateTempTable()
        {
            var createTableSql = "CREATE TABLE user_ft_temp_table (UserId INT, UserLevel NVARCHAR(50), Commission decimal(10,3), GoldPoint INT)";
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
            var createType = "CREATE TYPE [kaadmin].[user_ft_table_type] AS TABLE(UserId INT, UserLevel NVARCHAR(50), Commission decimal(10,3), GoldPoint INT)";
            return await _context.Database.ExecuteSqlRawAsync(createType);
        }
        #endregion

        #region Create Stored procedure to add data
        public async Task<int> CreateSPForInsertion()
        {
            var createSP = "CREATE OR ALTER PROCEDURE [InsertFasttrackEntities] @EntitiesTable user_ft_table_type READONLY AS BEGIN INSERT INTO user_ft_temp_table (UserId, UserLevel, Commission, GoldPoint) SELECT UserId, UserLevel, Commission, GoldPoint FROM @EntitiesTable END";

            return await _context.Database.ExecuteSqlRawAsync(createSP);
        }
        #endregion

        #region Create SP to get data from temp table
        public async Task<int> CreateSPForDataRetrival()
        {
            var tempDataSP = "CREATE OR ALTER PROCEDURE [GetUserFTData] AS BEGIN SELECT UserId, UserLevel, Commission, GoldPoint FROM user_ft_temp_table END";
            return await _context.Database.ExecuteSqlRawAsync(tempDataSP);
        }
        #endregion

        #region Parent Gp allocation
        public async Task parentGpAllocation(int userId, int user_parent_level, int goldPoint)
        {
            if (user_parent_level < 6)
            {
                var wbcUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == userId);
                var fasttrackModel = new FasttrackResponseModel(userId, wbcUser, "", 0, goldPoint, "Fasttrack level gold point");
                await CheckAddGPEntry(fasttrackModel);

                if (wbcUser.UserParentid != null)
                {
                    var gp = Math.Round((decimal)goldPoint * 25 / 100);
                    await parentGpAllocation((int)wbcUser.UserParentid, user_parent_level + 1, (int)gp);
                }
            }
        }
        #endregion

        #region Parent commission allocation
        public async Task parentCommissionAllocation(int userId, int user_parent_level)
        {
            if (user_parent_level >= 6)
                return;

            var fasttrackUser = await _context.TblUserMasters.FirstOrDefaultAsync(u => u.UserId == userId);
            var fasttrackUserLevel = await GetUserFasttrackCategory(userId);
            var fasttrackLevelCommission = await _context.TblFasttrackLevelCommissions.FirstOrDefaultAsync(f => f.Parent_Level == user_parent_level);
            decimal userCommission = 0;
            if (fasttrackUserLevel == "Basic")
                userCommission = fasttrackLevelCommission.Level_Income + fasttrackLevelCommission.Basic;
            else if (fasttrackUserLevel == "Silver")
                userCommission = fasttrackLevelCommission.Level_Income + fasttrackLevelCommission.Silver;
            else if (fasttrackUserLevel == "Gold")
                userCommission = fasttrackLevelCommission.Level_Income + fasttrackLevelCommission.Gold;
            else if (fasttrackUserLevel == "Platinum")
                userCommission = fasttrackLevelCommission.Level_Income + fasttrackLevelCommission.Platinum;
            else if (fasttrackUserLevel == "Diamond")
                userCommission = fasttrackLevelCommission.Level_Income + fasttrackLevelCommission.Diamond;

            var fasttrackModel = new FasttrackResponseModel(userId, fasttrackUser, fasttrackUserLevel, userCommission, 0, "Fasttrack level commission");
            CheckAddCommissionEntry(fasttrackModel);

            if (userCommission != 0)
            {
                if (fasttrackUser.UserParentid != null)
                    await parentCommissionAllocation((int)fasttrackUser.UserParentid, user_parent_level + 1);
            }
        }
        #endregion

        #region check and add goldpoint entry 
        public async Task CheckAddGPEntry(FasttrackResponseModel fasttrackResponseModel)
        {
            if (fasttrackResponseList.Any(f => f.UserId == fasttrackResponseModel.UserId))
            {
                var fasttrack = fasttrackResponseList.First(f => f.UserId == fasttrackResponseModel.UserId);
                fasttrack.GoldPoint += fasttrackResponseModel.GoldPoint;
            }
            else
                fasttrackResponseList.Add(fasttrackResponseModel);
        }
        #endregion

        #region check and add commission entry 
        public async Task CheckAddCommissionEntry(FasttrackResponseModel fasttrackResponseModel)
        {
            if (fasttrackResponseList.Any(f => f.UserId == fasttrackResponseModel.UserId && f.UserLevel == fasttrackResponseModel.UserLevel))
            {
                var fasttrack = fasttrackResponseList.First(f => f.UserId == fasttrackResponseModel.UserId && f.UserLevel == fasttrackResponseModel.UserLevel);
                fasttrack.Commission += fasttrackResponseModel.Commission;
            }
            else
                fasttrackResponseList.Add(fasttrackResponseModel);
        }
        #endregion

        #region Check commission / goldpoint entries exists
        public async Task<int> CheckCommissionReleaseStatus(DateTime date)
        {
            var existingFasttrackLedgerUsers = await _context.TblFasttrackLedgers.Where(f => f.Timestamp == date).ToListAsync();
            return existingFasttrackLedgerUsers.Count();
        }

        public async Task<int> CheckGoldPointReleaseStatus(DateTime date)
        {
            var existingGoldPointUsers = await _context.TblGoldPoints.Where(g => g.Timestamp == date && g.Type.ToLower().Contains("fasttrack")).ToListAsync();
            return existingGoldPointUsers.Count();
        }
        #endregion

        #region Release commission
        public async Task<(int, string, List<FasttrackResponseModel>?)> ReleaseCommission(DateTime date, bool isTruncate = false)
        {
            var goldpointRes = await CheckGoldPointReleaseStatus(date);
            var commissionRes = await CheckCommissionReleaseStatus(date);
            if ((goldpointRes > 0 || commissionRes > 0) && !isTruncate)
                return (0, "Commission has already released for the day. Do you still want to truncate and Re-release commission?", null);

            if ((goldpointRes > 0 || commissionRes > 0) && isTruncate)
            {
                if (goldpointRes > 0)
                {
                    var gpEntries = await _context.TblGoldPoints.Where(g => g.Timestamp == date && g.Type.ToLower().Contains("fasttrack")).ToListAsync();
                    _context.TblGoldPoints.RemoveRange(gpEntries);
                    await _context.SaveChangesAsync();
                }
                if (commissionRes > 0)
                {
                    var comEntries = await _context.TblFasttrackLedgers.Where(f => f.Timestamp == date).ToListAsync();
                    _context.TblFasttrackLedgers.RemoveRange(comEntries);
                    await _context.SaveChangesAsync();
                }
            }

            var spRes = await CreateSPForDataRetrival();
            if (spRes == 0)
                return (-1, "Procedure not found..", null);

            await GetTempTableData();

            List<TblGoldPoint> goldPointModelList = new List<TblGoldPoint>();
            List<TblFasttrackLedger> fasttrackLedgerList = new List<TblFasttrackLedger>();

            var goldPointUserList = tempTableDataList.Where(g => g.GoldPoint != 0).ToList();
            var commissionUserList = tempTableDataList.Where(c => c.Commission > 0.000m).ToList();

            if (goldPointUserList.Count <= 0 || commissionUserList.Count <= 0)
                return (-1, "No records to release commisison.", null);

            goldPointModelList.AddRange(goldPointUserList.Select(u => createGoldPointObject(u, date)));
            fasttrackLedgerList.AddRange(commissionUserList.Select(f => createFasttrackLedgerObject(f, date)));

            if (goldPointModelList.Count > 0 || fasttrackLedgerList.Count > 0)
            {
                var tasks = new List<Task>();

                if (goldPointModelList.Count > 0)
                {
                    tasks.Add(_context.TblGoldPoints.AddRangeAsync(goldPointModelList));
                }
                if (fasttrackLedgerList.Count > 0)
                {
                    tasks.Add(_context.TblFasttrackLedgers.AddRangeAsync(fasttrackLedgerList));
                }

                if (tasks.Any())
                {
                    await Task.WhenAll(tasks);
                    await _context.SaveChangesAsync();
                    await DropTable();
                }

                return (tasks.Count, "Fasttrack commission released successfully.", commissionUserList);
            }
            return (-1, "Unable to release fasttrack commission", null);
        }
        #endregion

        #region Create GoldPoint Object
        public TblGoldPoint createGoldPointObject(FasttrackResponseModel model, DateTime date)
        {
            var goldpoint = new TblGoldPoint();
            goldpoint.Timestamp = date;
            goldpoint.Credit = model.GoldPoint;
            goldpoint.Debit = 0;
            goldpoint.Userid = model.UserId;
            goldpoint.Type = "Fasttrack Level Goldpoint";
            goldpoint.PointCategory = 1;
            return goldpoint;
        }
        #endregion

        #region Create fasttrack ledger object
        public TblFasttrackLedger createFasttrackLedgerObject(FasttrackResponseModel model, DateTime date)
        {
            var fasttrackLedger = new TblFasttrackLedger();
            fasttrackLedger.Timestamp = date;
            fasttrackLedger.Credit = model.Commission;
            fasttrackLedger.Debit = 0.0m;
            fasttrackLedger.Userid = model.UserId;
            fasttrackLedger.Narration = "Fasttrack Level Commission";
            return fasttrackLedger;
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

                    using (var command = new SqlCommand("GetUserFTData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int userId = reader.GetInt32("UserId");
                                string userLevel = reader.GetString("UserLevel");
                                decimal commission = reader.GetDecimal("Commission");
                                int goldPoint = reader.GetInt32("GoldPoint");

                                // Process the data as needed
                                tempTableDataList.Add(new FasttrackResponseModel(userId, userLevel, commission, goldPoint));
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
            var createTableSql = "DROP TABLE user_ft_temp_table";
            await _context.Database.ExecuteSqlRawAsync(createTableSql);
        }
        #endregion

        #region Get fasttrack category of user
        public async Task<string> GetUserFasttrackCategory(int userId)
        {
            var userCategory = "";
            var allUsers = await _context.TblUserMasters.Where(u => u.UserIsactive == true).ToListAsync();
            var fasttrack_scheme_master = await _context.TblFasttrackSchemeMasters.ToListAsync();
            fasttrack_scheme_master.Reverse();
            var basicScheme = fasttrack_scheme_master.Last();

            var currUser = allUsers.Where(u => u.UserId == userId).FirstOrDefault();

            var totalContacts = allUsers.Where(u => u.UserParentid == currUser.UserId).Count();

            //getting curr new joinee user's parent's wbc and fasttrack clients
            var totalWbcCount = allUsers.Where(u => u.UserParentid == currUser.UserId && u.UserWbcActive == true && u.UserFasttrack == false).Count();
            var totalFasttrackCount = allUsers.Where(u => u.UserParentid == currUser.UserId && u.UserFasttrack == true).Count();

            if (totalWbcCount > basicScheme.NoOfNonFasttrackClients && totalFasttrackCount > basicScheme.NoOfFasttrackClients)
            {
                userCategory = fasttrack_scheme_master.Where(x => x.NoOfFasttrackClients <= totalFasttrackCount && x.NoOfNonFasttrackClients <= totalWbcCount).First().Level;
            }
            else
            {
                userCategory = fasttrack_scheme_master.Last().Level;
            }
            return userCategory;
        }
        #endregion

        #region Get Fasttrack Benefits
        public async Task<List<TblFasttrackBenefits>> GetFasttrackBenefits()
        {
            return await _context.TblFasttrackBenefits.ToListAsync();
        }
        #endregion

        #region Get Fasttrack Scheme
        public async Task<List<TblFasttrackSchemeMaster>> GetFasttrackSchemes()
        {
            return await _context.TblFasttrackSchemeMasters.ToListAsync();
        }
        #endregion

        #region Get Fasttrack Level Commission
        public async Task<List<TblFasttrackLevelCommission>> GetFasttrackLevelCommission()
        {
            return await _context.TblFasttrackLevelCommissions.ToListAsync();
        }
        #endregion

        #region Add Fasttrack benefits
        public async Task<int> AddFasttrackBenefit(TblFasttrackBenefits tblFasttrackBenefits)
        {
            if (await _context.TblFasttrackBenefits.AnyAsync(x => x.Product.ToLower() == tblFasttrackBenefits.Product.ToLower()))
                return 0;

            await _context.TblFasttrackBenefits.AddAsync(tblFasttrackBenefits);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Fasttrack benefits
        public async Task<int> UpdateFasttrackBenefit(TblFasttrackBenefits tblFasttrackBenefits)
        {
            _context.TblFasttrackBenefits.Update(tblFasttrackBenefits);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update fasttrack levels commission
        public async Task<int> UpdateFasttrackLevelsCommission(TblFasttrackLevelCommission tblFasttrackLevelCommission)
        {
            _context.TblFasttrackLevelCommissions.Update(tblFasttrackLevelCommission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update fasttrack scheme
        public async Task<int> UpdateFasttrackScheme(TblFasttrackSchemeMaster tblFasttrackSchemeMaster)
        {
            _context.TblFasttrackSchemeMasters.Update(tblFasttrackSchemeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}