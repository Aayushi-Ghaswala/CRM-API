﻿using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module
{
    public interface IInsuranceClientRepository
    {
        Task<int> GetInsClientByUserId(int userId, DateTime date);
        Task<Response<TblInsuranceclient>> GetInsuranceClients(string? filterString, string search, SortingParams sortingParams);
        Task<List<TblInsuranceTypeMaster>> GetPlanTypes();
        Task<TblInsuranceclient> GetInsuranceClientById(int id);
        Task<TblInsuranceTypeMaster> GetInsuranceplanTypeById(int id);
        IEnumerable<TblInsuranceclient> GetInsClientsForInsPremiumReminder();
        IEnumerable<TblInsuranceclient> GetInsClientsForInsDueReminder();
        Task<TblInvesmentType> GetInvesmentTypeByName(string name);
        Task<Response<TblInsuranceCompanylist>> GetCompanyListByInsTypeId(int id, SortingParams sortingParams);
        Task<int> AddInsuranceDetail(TblInsuranceclient tblInsuranceclient);
        Task<int> ImportInsClientsFile(List<TblInsuranceclient> tblInsuranceclients);
        Task<int> UpdateInsuranceClientDetail(TblInsuranceclient tblInsuranceclient, bool flag = false);
        Task<int> DeactivateInsuranceClientDetail(int id);
        Task<int> GetSubInsTypeIdByName(string name);
    }
}
