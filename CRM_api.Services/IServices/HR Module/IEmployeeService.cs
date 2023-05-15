﻿using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IEmployeeService
    {
        Task<int> AddEmployeeAsync(AddUserMasterDto addUser);
        Task<int> UpdateEmployeeAsync(UpdateUserMasterDto updateUser);
        Task<ResponseDto<UserMasterDto>> GetEmployeesAsync(int page);
        Task<UserMasterDto> GetEmployeeById(int id);
    }
}