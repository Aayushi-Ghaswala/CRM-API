namespace CRM_api.DataAccess.Models
{
    public partial class TblRoleMaster
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; }
    }
}
