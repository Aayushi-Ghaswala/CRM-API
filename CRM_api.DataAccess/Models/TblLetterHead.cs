namespace CRM_api.DataAccess.Models
{
    public partial class TblLetterHead
    {
        public int LetterHeadId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Template { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
