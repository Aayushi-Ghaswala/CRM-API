using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Model
{
    public class UserRoleAssignment
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual RoleMaster RoleMaster { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual UserMaster UserMaster { get; set; }
        public UserRoleAssignment()
        {

        }

        public UserRoleAssignment(int roleId, int userId)
        {
            RoleId = roleId;
            UserId = userId;
        }
    }
}
