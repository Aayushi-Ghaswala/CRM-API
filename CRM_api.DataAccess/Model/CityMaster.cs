namespace CRM_api.DataAccess.Model
{
    public class CityMaster
    {
        public int Id { get; set; }
        public int State_Id { get; set; }
        public string? City_Name { get; set; }
        public virtual StateMaster? StateMaster { get; set; }
    }
}
