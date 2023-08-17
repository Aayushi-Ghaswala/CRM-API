using Microsoft.AspNetCore.Builder;

namespace CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module
{
    public class UpdateOrderStatusDto
    {
        public int Id { get; set; }
        public string? Statusname { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
