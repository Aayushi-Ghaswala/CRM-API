using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFolioTypeMaster
    {
        public int FolioTypeId { get; set; }
        public string FolioType { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
