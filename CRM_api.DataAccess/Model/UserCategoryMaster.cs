using System.ComponentModel.DataAnnotations;

namespace CRM_api.DataAccess.Model
{
    public class UserCategoryMaster
    {
        [Key]
        public int Cat_Id { get; set; }
        public string? Cat_Name { get; set; }
        public Nullable<bool> Cat_IsActive { get; set; }
    }
}
