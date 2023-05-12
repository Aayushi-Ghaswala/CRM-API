using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.MapperProfile
{
    public class RoleMasterProfile : Profile
    {
        public RoleMasterProfile()
        {
            //CreateMap<TblRoleMaster, RoleMasterDto>()
            //    .ForMember(rm => rm.Id, opt => opt.MapFrom(src => src.RoleId));

            //CreateMap<TblRolePermission, RolePermissionDto>()
            //    .ForMember(rp => rp.RoleName, opt => opt.MapFrom(src => src.TblRoleMaster.RoleName));

            //CreateMap<TblRoleAssignment, UserRoleAssignmentDto>()
            //    .ForMember(rp => rp.RoleName, opt => opt.MapFrom(src => src.TblRoleMaster.RoleName))
            //    .ForMember(rp => rp.UserName, opt => opt.MapFrom(src => src.TblUserMaster.UserName));
            CreateMap<TblRoleMaster, RoleMasterDto>()
                .ForMember(rm => rm.Id, opt => opt.MapFrom(src => src.RoleId));

            CreateMap<TblRolePermission, RolePermissionDto>();

            CreateMap<TblRoleAssignment, UserRoleAssignmentDto>();
            CreateMap<Response<TblRoleMaster>, ResponseDto<RoleMasterDto>>();
            CreateMap<RolePermissionResponse, RolePermissionResponseDto>();
            CreateMap<UserAssignRoleResponse, UserAssignRoleResponseDto>();
        }
    }
}
