using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblRolePermission
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string? ModuleName { get; set; }
        public bool? AllowAdd { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public bool? AllowView { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual TblRoleMaster TblRoleMaster { get; set; }

        public TblRolePermission()
        {

        }

        public TblRolePermission(int roleId, string moduleName, bool allow_Add, bool allow_Edit, bool allow_Delete, bool allow_View)
        {
            RoleId = roleId;
            ModuleName = moduleName;
            AllowAdd = allow_Add;
            AllowEdit = allow_Edit;
            AllowDelete = allow_Delete;
            AllowView = allow_View;
        }

        public void RolePermissionUpdate(int roleId, string moduleName, bool allow_Add, bool allow_Edit, bool allow_Delete, bool allow_View)
        {
            RoleId = roleId;
            ModuleName = moduleName;
            AllowAdd = allow_Add;
            AllowEdit = allow_Edit;
            AllowDelete = allow_Delete;
            AllowView = allow_View;
        }
    }
}
