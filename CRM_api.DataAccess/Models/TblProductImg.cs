using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblProductImg
    {
        public int Id { get; set; }
        public string Img { get; set; } = null!;
        public int ProductId { get; set; }
        public DateTime CeratedDate { get; set; }
        public int? Modifyby { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Isdeleted { get; set; }

        public virtual TblWbcMallProduct Product { get; set; } = null!;
    }
}
