using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.User_Module
{
    public class UserAssignRoleResponseDto
    {
        public List<UserRoleAssignmentDto> Values { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
