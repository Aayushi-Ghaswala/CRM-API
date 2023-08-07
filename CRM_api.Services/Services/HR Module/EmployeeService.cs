using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.IServices.HR_Module;

namespace CRM_api.Services.Services.HR_Module
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserCategoryRepository _userCategoryRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IUserCategoryRepository userCategoryRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _userCategoryRepository = userCategoryRepository;
        }

        #region Get Employees
        public async Task<ResponseDto<UserMasterDto>> GetEmployeesAsync(string search, SortingParams sortingParams)
        {
            var category = await _userCategoryRepository.GetCategoryByName(CategoryConstant.employee);
            var employees = await _employeeRepository.GetEmployees(category.CatId, search, sortingParams);

            var mapUsers = _mapper.Map<ResponseDto<UserMasterDto>>(employees);

            foreach (var user in mapUsers.Values)
            {
                user.UserName = user.UserName.ToLower();
            }
            return mapUsers;
        }
        #endregion

        #region Get Employee by Id
        public async Task<UserMasterDto> GetEmployeeByIdAsync(int id)
        {
            var employees = await _employeeRepository.GetEmployeebyId(id);
            var mappedEmployees = _mapper.Map<UserMasterDto>(employees);
            return mappedEmployees;
        }
        #endregion

        #region Add Employee
        public async Task<int> AddEmployeeAsync(AddUserMasterDto addUser)
        {
            var employee = _mapper.Map<TblUserMaster>(addUser);
            return await _employeeRepository.AddEmployee(employee);
        }
        #endregion

        #region Update Employee
        public async Task<int> UpdateEmployeeAsync(UpdateUserMasterDto updateUser)
        {
            var employee = _mapper.Map<TblUserMaster>(updateUser);
            return await _employeeRepository.UpdateEmployee(employee);
        }
        #endregion

        #region Deactivate Employee
        public Task<int> DeactivateEmployeeAsync(int id)
        {
            return _employeeRepository.DeactivateEmployee(id);
        }
        #endregion
    }
}
