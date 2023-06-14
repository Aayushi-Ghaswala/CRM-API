namespace CRM_api.DataAccess.Models
{
    public partial class TblModuleMaster
    {
        public int Id { get; set; }
        public string? ModuleName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
