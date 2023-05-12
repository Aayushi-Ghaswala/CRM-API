using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.User_Module
{
    public class RolePermissionResponseDto
    {
        public List<RolePermissionDto> Values { get;set; }
        public PaginationDto Pagination { get; set; }
    }
}
