namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class SourceTypeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
