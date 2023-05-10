namespace CRM_api.DataAccess.Model
{
    public class RoleMaster
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }

        public RoleMaster()
        {

        }

        public RoleMaster(string? roleName)
        {
            RoleName = roleName;
        }
    }
}
