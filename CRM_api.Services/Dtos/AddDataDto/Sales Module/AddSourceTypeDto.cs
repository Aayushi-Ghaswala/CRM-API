namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddSourceTypeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
