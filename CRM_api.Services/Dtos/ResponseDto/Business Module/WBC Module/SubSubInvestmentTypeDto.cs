using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module
{
    public class SubSubInvestmentTypeDto
    {
        public int Id { get; set; }
        public string? SubInvType { get; set; }
        public int? SubInvTypeId { get; set; }
        [ForeignKey(nameof(SubInvTypeId))]
        public virtual SubInvestmentTypeDto TblSubInvesmentType { get; set; }
    }
}
