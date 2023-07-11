using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class SourceTypeProfile : Profile
    {
        public SourceTypeProfile()
        {
            CreateMap<AddSourceTypeDto, TblSourceTypeMaster>().ReverseMap();
            CreateMap<TblSourceTypeMaster, SourceTypeDto>();
            CreateMap<UpdateSourceTypeDto, TblSourceTypeMaster>();

            CreateMap<TblSourceTypeMaster, SourceTypeDto>().ReverseMap();
            CreateMap<Response<TblSourceTypeMaster>, ResponseDto<SourceTypeDto>>();
        }
    }
}
