using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFolioMaster
    {
        public TblFolioMaster()
        {
            TblFolioDetails = new HashSet<TblFolioDetail>();
        }

        public int Folioid { get; set; }
        public int Userid { get; set; }
        public string Pan { get; set; } = null!;
        public DateTime UploadDate { get; set; }
        public int FolioTypeId { get; set; }

        public virtual TblInvesmentType FolioType { get; set; } = null!;
        public virtual TblUserMaster User { get; set; } = null!;
        public virtual ICollection<TblFolioDetail> TblFolioDetails { get; set; }
    }
}
