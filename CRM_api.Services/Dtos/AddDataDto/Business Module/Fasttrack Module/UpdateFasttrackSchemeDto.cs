namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module
{
    public class UpdateFasttrackSchemeDto
    {
        public int Id { get; set; }
        public string? Level { get; set; }
        public int? NoOfFasttrackClients { get; set; }
        public int? NoOfNonFasttrackClients { get; set; }
        public int? TotalClient { get; set; }
        public int? Goldpoint { get; set; }
    }
}
