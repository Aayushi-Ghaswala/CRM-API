using AutoMapper;
using CRM_api.DataAccess.Model;
using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.MapperProfile
{
    public class RoleMasterProfile : Profile
    {
        public RoleMasterProfile()
        {
            CreateMap<RoleMaster, RoleMasterDto>()
                .ForMember(rm => rm.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(rm => rm.RoleName, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<RolePermission, RolePermissionDto>()
                .ForMember(rp => rp.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(rp => rp.RoleName, opt => opt.MapFrom(src => src.RoleMaster.RoleName))
                .ForMember(rp => rp.ModuleName, opt => opt.MapFrom(src => src.ModuleName))
                .ForMember(rp => rp.Allow_Add, opt => opt.MapFrom(src => src.Allow_Add))
                .ForMember(rp => rp.Allow_Edit, opt => opt.MapFrom(src => src.Allow_Edit))
                .ForMember(rp => rp.Allow_Delete, opt => opt.MapFrom(src => src.Allow_Delete))
                .ForMember(rp => rp.Allow_View, opt => opt.MapFrom(src => src.Allow_View));

            CreateMap<UserRoleAssignment, UserRoleAssignmentDto>()
                .ForMember(rp => rp.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(rp => rp.RoleName, opt => opt.MapFrom(src => src.RoleMaster.RoleName))
                .ForMember(rp => rp.UserName, opt => opt.MapFrom(src => src.UserMaster.User_Name));
        }
    }
}
