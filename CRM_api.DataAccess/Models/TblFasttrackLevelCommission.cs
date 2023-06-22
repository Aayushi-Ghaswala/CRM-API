namespace CRM_api.DataAccess.Models
{
    public class TblFasttrackLevelCommission
    {
        public int Id { get; set; }
        public int Parent_Level { get; set; }
        public decimal Level_Income { get; set; }
        public decimal Basic { get; set; }
        public decimal Silver { get; set; }
        public decimal Gold { get; set; }
        public decimal Platinum { get; set; }
        public decimal Diamond { get; set; }
    }
}
