using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;

namespace CRM_api.Services.Services.HR_Module
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        #region Get Employees
        public async Task<ResponseDto<EmployeeMasterDto>> GetEmployeesAsync(string search, SortingParams sortingParams)
        {
            var employees = await _employeeRepository.GetEmployees(search, sortingParams);

            var mapEmployees = _mapper.Map<ResponseDto<EmployeeMasterDto>>(employees);

            foreach (var user in mapEmployees.Values)
            {
                user.Name = user.Name.ToLower();
            }
            return mapEmployees;
        }
        #endregion

        #region Add Employee
        public async Task<(int, string)> AddEmployeeAsync(AddEmployeeDto addEmployee)
        {
            var mapEmployee = _mapper.Map<TblEmployeeMaster>(addEmployee);
            var employee = await _employeeRepository.AddEmployee(mapEmployee);

            if (employee == null)
            {
                return (0, "Unable to add Employee.");
            }
            
            if (addEmployee.AddEmployeeQualification.Count() > 0)
            {
                var mapEmployeeQualification = _mapper.Map<List<TblEmployeeQualification>>(addEmployee.AddEmployeeQualification);

                foreach (var employeeQualification in mapEmployeeQualification)
                {
                    employeeQualification.EmpId = employee.Id;
                    await _employeeRepository.AddEmployeeQualification(employeeQualification);
                }
            }

            if (addEmployee.AddEmployeeExperiences is not null && addEmployee.AddEmployeeExperiences.Count() > 0)
            {
                var mapEmployeeExperience = _mapper.Map<List<TblEmployeeExperience>>(addEmployee.AddEmployeeExperiences);

                foreach (var employeeExperience in mapEmployeeExperience)
                {
                    employeeExperience.EmpId = employee.Id;
                    await _employeeRepository.AddEmployeeExperience(employeeExperience);
                }
            }
            return (1, "Employee added successfully.");
        }
        #endregion

        #region Update Employee
        public async Task<int> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployee)
        {
            var mapEmployee = _mapper.Map<TblEmployeeMaster>(updateEmployee);
            var employee = await _employeeRepository.UpdateEmployee(mapEmployee);

            if(employee == 0)
            {
                return 0;
            }

            if (updateEmployee.updateEmployeeQualification is not null)
            {
                var mapEmployeeQualification = _mapper.Map<List<TblEmployeeQualification>>(updateEmployee.updateEmployeeQualification);

                foreach(var employeeQualification in mapEmployeeQualification)
                {
                    if (employeeQualification.Id != 0)
                        await _employeeRepository.UpdateEmployeeQualification(employeeQualification);
                    else
                    {
                        employeeQualification.EmpId = updateEmployee.Id;
                        await _employeeRepository.AddEmployeeQualification(employeeQualification);
                    }
                }
            }

            if (updateEmployee.updateEmployeeExperience is not null)
            {
                var mapEmployeeExperience= _mapper.Map<List<TblEmployeeExperience>>(updateEmployee.updateEmployeeExperience);

                foreach (var employeeExperience in mapEmployeeExperience)
                {
                    if (employeeExperience.Id != 0)
                        await _employeeRepository.UpdateEmployeeExperience(employeeExperience);
                    else
                    {
                        employeeExperience.EmpId = updateEmployee.Id;
                        await _employeeRepository.AddEmployeeExperience(employeeExperience);
                    }
                }
            }
            return 1;
        }
        #endregion

        #region Deactivate Employee
        public Task<int> DeactivateEmployeeAsync(int id)
        {
            return _employeeRepository.DeactivateEmployee(id);
        }
        #endregion

        #region Delete Employee Qualification
        public Task<int> DeleteEmployeeQualificationAsync(int id)
        {
            return _employeeRepository.DeleteEmployeeQualification(id);
        }
        #endregion

        #region Delete Employee Experience
        public Task<int> DeleteEmployeeExperienceAsync(int id)
        {
            return _employeeRepository.DeleteEmployeeExperience(id);
        }
        #endregion
    }
}
