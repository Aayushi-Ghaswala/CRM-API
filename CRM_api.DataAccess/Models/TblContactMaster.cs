using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblContactMaster
    {
        public int Contactid { get; set; }
        public string? MobileNo { get; set; }
        public int? RefUserid { get; set; }
        public DateTime? RefContactdate { get; set; }
        public string? RefContactname { get; set; }
        public string? RefContactno { get; set; }
    }
}
