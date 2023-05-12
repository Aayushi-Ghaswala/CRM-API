using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto
{
    public class DisplayUserMasterDto
    {
        public List<UserMasterDto> Values { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
