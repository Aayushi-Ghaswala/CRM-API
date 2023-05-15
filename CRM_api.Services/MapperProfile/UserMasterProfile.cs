using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class UserMasterProfile : Profile
    {
        public UserMasterProfile()
        {
            CreateMap<TblUserCategoryMaster, UserCategoryDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.CatId));

            CreateMap<TblUserMaster, UserMasterDto>();
            CreateMap<Pagination, PaginationDto>();
            CreateMap<Response<TblUserMaster>, ResponseDto<UserMasterDto>>();

            CreateMap<Response<TblUserCategoryMaster>, ResponseDto<UserCategoryDto>>();

            CreateMap<UpdateUserMasterDto, TblUserMaster>();
            CreateMap<AddUserMasterDto, TblUserMaster>()
                .AfterMap((dto, user) =>
                {
                    user.UserFcmid = Guid.NewGuid().ToString();
                });
        }
    }
}
