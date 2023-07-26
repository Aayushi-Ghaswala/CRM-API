using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddAttachmentDto
    {
        public string base64 { get; set; }
        public string fileName { get; set; }
        public byte[]? fileByte { get; set; }
    }
}
