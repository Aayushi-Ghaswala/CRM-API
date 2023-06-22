using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;

namespace CRM_api.Services.MapperProfile
{
    public class FasttrackProfile : Profile
    {
        public FasttrackProfile()
        {
            CreateMap<UpdateFasttrackSchemeDto, TblFasttrackSchemeMaster>();
            CreateMap<UpdateFasttrackLevelCommissionDto, TblFasttrackLevelCommission>();
        }
    }
}
