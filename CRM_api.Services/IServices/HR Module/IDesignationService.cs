using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.ResponseModel;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IDesignationService
    {
        Task<ResponseDto<DesignationDto>> GetDesignation(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<DesignationDto> GetDesignationById(int id);
        Task<IEnumerable<DesignationDto>> GetDesignationByDepartment(int departmentId);
        Task<int> AddDesignation(AddDesignationDto designationMaster);
        Task<int> UpdateDesignation(UpdateDesignationDto designationMaster);
        Task<int> DeactivateDesignation(int id);
    }
}
