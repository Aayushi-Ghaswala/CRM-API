using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Model
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ModuleName { get; set; }
        public bool Allow_Add { get; set; }
        public bool Allow_Edit { get; set; }
        public bool Allow_Delete { get; set; }
        public bool Allow_View { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual RoleMaster RoleMaster { get; set; }

        public RolePermission()
        {

        }

        public RolePermission(int roleId, string moduleName, bool allow_Add, bool allow_Edit, bool allow_Delete, bool allow_View)
        {
            RoleId = roleId;
            ModuleName = moduleName;
            Allow_Add = allow_Add;
            Allow_Edit = allow_Edit;
            Allow_Delete = allow_Delete;
            Allow_View = allow_View;
        }

        public void RolePermissionUpdate(int roleId, string moduleName, bool allow_Add, bool allow_Edit, bool allow_Delete, bool allow_View)
        {
            RoleId = roleId;
            ModuleName = moduleName;
            Allow_Add = allow_Add;
            Allow_Edit = allow_Edit;
            Allow_Delete = allow_Delete;
            Allow_View = allow_View;
        }
    }
}
