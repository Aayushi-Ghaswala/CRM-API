using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.LI_GI_Module
{
    public class InsuranceClientRepository : IInsuranceClientRepository
    {
        private readonly CRMDbContext _context;

        public InsuranceClientRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All InsuranceClient Details
        public async Task<Response<TblInsuranceclient>> GetInsuranceClients(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblInsuranceclients.Where(x => x.IsDeleted != true).Include(x => x.TblInsuranceCompanylist)
                                                         .Include(x => x.TblInsuranceTypeMaster)
                                                         .Include(x => x.TblInvesmentType)
                                                         .Include(x => x.TblSubInvesmentType)
                                                         .Include(x => x.TblUserMaster).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblInsuranceclient>(search).Where(x => x.IsDeleted != true)
                                                         .Include(x => x.TblInsuranceCompanylist)
                                                         .Include(x => x.TblInsuranceTypeMaster)
                                                         .Include(x => x.TblInvesmentType)
                                                         .Include(x => x.TblSubInvesmentType)
                                                         .Include(x => x.TblUserMaster).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var responseInsClients = new Response<TblInsuranceclient>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return responseInsClients;
        }
        #endregion

        #region Get All Insurance Company By Insurance Type
        public async Task<Response<TblInsuranceCompanylist>> GetCompanyListByInsTypeId(int id, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblInsuranceCompanylists.Where(x => x.InsuranceCompanyid == id).AsQueryable();
            
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();
            var responseCompanyList = new Response<TblInsuranceCompanylist>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return responseCompanyList;
        }
        #endregion

        #region Get InsuranceClient By Id
        public async Task<TblInsuranceclient> GetInsuranceClientById(int id)
        {
            var insuranceClient = await _context.TblInsuranceclients.Where(x => x.Id == id && x.IsDeleted != true).Include(x => x.TblInsuranceCompanylist).Include(x => x.TblInsuranceTypeMaster)
                                                .Include(x => x.TblInvesmentType).Include(x => x.TblSubInvesmentType).Include(x => x.TblUserMaster).FirstOrDefaultAsync();
            if (insuranceClient is null) return null;

            return insuranceClient;
        }
        #endregion

        #region Get Insurance Plan Type By Id
        public async Task<TblInsuranceTypeMaster> GetInsuranceplanTypeById(int id)
        {
            var planType = await _context.TblInsuranceTypeMasters.Where(x => x.InsPlantypeId == id).FirstOrDefaultAsync();

            return planType;
        }
        #endregion

        #region Get Invesment Type By Name
        public async Task<TblInvesmentType> GetInvesmentTypeByName(string name)
        {
            var invType = await _context.TblInvesmentTypes.Where(x => x.InvestmentName == name).FirstOrDefaultAsync();

            return invType;
        }
        #endregion

        #region Get Insurance Client Details For Insurance Premium Reminder
        public async Task<IEnumerable<TblInsuranceclient>> GetInsClientsForInsPremiumReminder()
        {
            var insClients = await _context.TblInsuranceclients.Where(x => x.InsPremiumRmdDate <= DateTime.Now && x.IsDeleted != true).ToListAsync();

            return insClients;
        }
        #endregion

        #region Get Insurance Client Details For Insurance Due Reminder Reminder
        public async Task<IEnumerable<TblInsuranceclient>> GetInsClientsForInsDueReminder()
        {
            var insClients = await _context.TblInsuranceclients.Where(x => x.InsDuedate <= DateTime.Now && x.IsDeleted != true).ToListAsync();

            return insClients;
        }
        #endregion

        #region Add InsuranceClient Detail
        public async Task<int> AddInsuranceDetail(TblInsuranceclient tblInsuranceclient)
        {
            if (_context.TblInsuranceclients.Any(x => x.InsPlantypeId == tblInsuranceclient.InsPlantypeId && x.InsUserid == tblInsuranceclient.InsUserid
                                             && x.Companyid == tblInsuranceclient.Companyid && x.InsDuedate < DateTime.Now))
                return 0;

            _context.TblInsuranceclients.Add(tblInsuranceclient);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update InsuranceClient Details
        public async Task<int> UpdateInsuranceClientDetail(TblInsuranceclient tblInsuranceclient)
        {
            var insClient = await _context.TblInsuranceclients.AsNoTracking().Where(x => x.Id == tblInsuranceclient.Id).FirstOrDefaultAsync();
            if (insClient is null) return 0;

            _context.TblInsuranceclients.Update(tblInsuranceclient);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Insurance Client Detail
        public async Task<int> DeactivateInsuranceClientDetail(int id)
        {
            var insClient = await _context.TblInsuranceclients.FindAsync(id);
            if(insClient is null) return 0;

            insClient.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
