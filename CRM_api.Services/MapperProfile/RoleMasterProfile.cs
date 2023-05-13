using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class RoleMasterProfile : Profile
    {
        public RoleMasterProfile()
        {
            CreateMap<AddRoleMasterDto, TblRoleMaster>();
            CreateMap<UpdateRoleMasterDto, TblRoleMaster>();

            CreateMap<AddRolePermissionDto, TblRolePermission>();
            CreateMap<UpdateRolePermissionDto, TblRolePermission>();

            CreateMap<AddUserRoleAssignmentDto, TblRoleAssignment>();
            CreateMap<UpdateRoleAssignmentDto, TblRoleAssignment>();

            CreateMap<TblRoleMaster, RoleMasterDto>()
                .ForMember(rm => rm.Id, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<TblRolePermission, RolePermissionDto>();

            CreateMap<TblRoleAssignment, UserRoleAssignmentDto>();
            CreateMap<Response<TblRoleMaster>, ResponseDto<RoleMasterDto>>();
            CreateMap<Response<TblRolePermission>, ResponseDto<RolePermissionDto>>();
            CreateMap<Response<TblRoleAssignment>, ResponseDto<UserRoleAssignmentDto>>();
        }
    }
}
