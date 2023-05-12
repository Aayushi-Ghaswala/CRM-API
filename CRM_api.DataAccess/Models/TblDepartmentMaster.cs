namespace CRM_api.DataAccess.Models
{
    public partial class TblDepartmentMaster
    {
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
