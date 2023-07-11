using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<AddSourceDto, TblSourceMaster>().ReverseMap();
            CreateMap<TblSourceMaster, SourceDto>();
            CreateMap<UpdateSourceDto, TblSourceMaster>();

            CreateMap<TblSourceMaster, SourceDto>().ReverseMap();
            CreateMap<Response<TblSourceMaster>, ResponseDto<SourceDto>>();
        }
    }
}
