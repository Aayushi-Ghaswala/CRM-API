namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddParticipantDto
    {
        public int? ParticipantId { get; set; }
        public int? LeadId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
    }
}