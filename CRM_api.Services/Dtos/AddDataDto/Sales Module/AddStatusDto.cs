﻿namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddStatusDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
