using AutoMapper;
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

        #region Add department
        public async Task<int> AddDepartmentAsync(AddDepartmentDto departmentDto)
        {
            var dept = _mapper.Map<TblDepartmentMaster>(departmentDto);
            return await _departmentRepository.AddDepartment(dept);
        }
        #endregion

        #region Get department
        public async Task<ResponseDto<DepartmentDto>> GetDepartmentAsync(int page)
        {
            var depts = await _departmentRepository.GetDepartments(page);
            var mapDepartment = _mapper.Map<ResponseDto<DepartmentDto>>(depts);
            return mapDepartment;
        }
        #endregion

        #region Get department by Id
        public async Task<DepartmentDto> GetDepartmentById(int id)
        {
            var dept = await _departmentRepository.GetDepartmentById(id);
            var mappedDept = _mapper.Map<DepartmentDto>(dept);
            return mappedDept;
        }
        #endregion

        #region Update department
        public async Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto)
        {
            var dept = _mapper.Map<TblDepartmentMaster>(departmentDto);
            return await _departmentRepository.UpdateDepartment(dept);
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
