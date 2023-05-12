using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class DisplayDesignationDto
    {
        public List<DesignationDto> Values { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
