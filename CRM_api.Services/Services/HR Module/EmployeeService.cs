using AutoMapper;
using CRM_api.DataAccess.IRepositories;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.IServices.HR_Module;

namespace CRM_api.Services.Services.HR_Module
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IUserMasterRepository userMasterRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _userMasterRepository = userMasterRepository;
            _mapper = mapper;
        }

        #region Add Employee
        public async Task<int> AddEmployeeAsync(AddUserMasterDto addUser)
        {
            var employee = _mapper.Map<TblUserMaster>(addUser);
            return await _employeeRepository.AddEmployee(employee);
        }
        #endregion

        #region Get Employee by Id
        public async Task<UserMasterDto> GetEmployeeById(int id)
        {
            var employees = await _employeeRepository.GetEmployeebyId(id);
            var mappedEmployees = _mapper.Map<UserMasterDto>(employees);
            return mappedEmployees;
        }
        #endregion

        #region Get Employees
        public async Task<DisplayUserMasterDto> GetEmployeesAsync(int page)
        {
            var catId = await _userMasterRepository.GetCategoryIdByName(CategoryConstant.employee);
            var employees = await _employeeRepository.GetEmployees(page, catId);

            var mapUsers = _mapper.Map<DisplayUserMasterDto>(employees);
            return mapUsers;
        }
        #endregion

        #region Update Employee
        public async Task<int> UpdateEmployeeAsync(UpdateUserMasterDto updateUser)
        {
            var employee = _mapper.Map<TblUserMaster>(updateUser);
            return await _employeeRepository.UpdateEmployee(employee);
        }
        #endregion
    }
}
