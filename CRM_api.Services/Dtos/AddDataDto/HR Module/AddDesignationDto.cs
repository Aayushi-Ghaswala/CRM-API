using CRM_api.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class AddDesignationDto
    {
        public int? DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
