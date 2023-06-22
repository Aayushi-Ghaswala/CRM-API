namespace CRM_api.DataAccess.Models
{
    public partial class TblFasttrackSchemeMaster
    {
        public int Id { get; set; }
        public string? Level { get; set; }
        public int? NoOfFasttrackClients { get; set; }
        public int? NoOfNonFasttrackClients { get; set; }
        public int? TotalClient { get; set; }
        public int? Goldpoint { get; set; }
    }
}
