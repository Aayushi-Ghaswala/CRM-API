﻿using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IEmployeeService
    {
        Task<ResponseDto<UserMasterDto>> GetEmployeesAsync(string search, SortingParams sortingParams);
        Task<UserMasterDto> GetEmployeeByIdAsync(int id);
        Task<int> AddEmployeeAsync(AddUserMasterDto addUser);
        Task<int> UpdateEmployeeAsync(UpdateUserMasterDto updateUser);
        Task<int> DeactivateEmployeeAsync(int id);
    }
}
