﻿using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module
{
    public class MFTransactionDto<T>
    {
        public ResponseDto<T>? response { get; set; }
        public decimal? totalPurchaseUnit { get; set; }
        public decimal? totalAmount { get; set; }
    }
}
