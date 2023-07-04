using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.MapperProfile
{
    public class UserLeaveProfile : Profile
    {
        public UserLeaveProfile()
        {
            CreateMap<AddUserLeaveDto, TblUserLeave>().ReverseMap();
            CreateMap<TblUserLeave, UserLeaveDto>();
            CreateMap<UpdateUserLeaveDto, TblUserLeave>();

            CreateMap<TblUserLeave, UserLeaveDto>().ReverseMap();
            CreateMap<Response<TblUserLeave>, ResponseDto<UserLeaveDto>>();
        }
    }
}
