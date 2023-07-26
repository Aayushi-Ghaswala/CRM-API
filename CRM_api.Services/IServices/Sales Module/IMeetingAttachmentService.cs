using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using Microsoft.AspNetCore.Http;

using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface IMeetingAttachmentService
    {
        Task<ResponseDto<MeetingAttachmentDto>> GetMeetingAttachmentsAsync(string search, SortingParams sortingParams);
        Task<MeetingAttachmentDto> GetMeetingAttachmentByIdAsync(int id);
        Task<int> AddMeetingAttachmentAsync(int meetingId, IFormFile file);
        Task<int> UpdateMeetingAttachmentAsync(UpdateMeetingAttachmentDto meetingAttachmentDto);
        Task<int> DeactivateMeetingAttachmentAsync(string path);
    }
}
