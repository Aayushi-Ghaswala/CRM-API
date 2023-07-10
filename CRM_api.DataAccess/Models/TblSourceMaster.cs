namespace CRM_api.DataAccess.Models
{
    public class TblSourceMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
