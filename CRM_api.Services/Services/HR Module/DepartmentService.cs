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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        #region Get Department
        public async Task<ResponseDto<DepartmentDto>> GetDepartmentAsync(string search, SortingParams sortingParams)
        {
            var depts = await _departmentRepository.GetDepartments(search, sortingParams);
            var mapDepartment = _mapper.Map<ResponseDto<DepartmentDto>>(depts);
            return mapDepartment;
        }
        #endregion

        #region Get Department By Id
        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentById(id);
            var mappedDepartment = _mapper.Map<DepartmentDto>(department);
            return mappedDepartment;
        }
        #endregion

        #region Add department
        public async Task<int> AddDepartmentAsync(AddDepartmentDto departmentDto)
        {
            var department = _mapper.Map<TblDepartmentMaster>(departmentDto);
            return await _departmentRepository.AddDepartment(department);
        }
        #endregion

        #region Update department
        public async Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<TblDepartmentMaster>(departmentDto);
            return await _departmentRepository.UpdateDepartment(department);
        }
        #endregion

        #region Deactivate department
        public async Task<int> DeactivateDepartmentAsync(int id)
        {
            return await _departmentRepository.DeactivateDepartment(id);
        }
        #endregion
    }
}
