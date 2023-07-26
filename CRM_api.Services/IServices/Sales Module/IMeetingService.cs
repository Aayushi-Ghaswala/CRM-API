using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface IMeetingService
    {
        Task<ResponseDto<MeetingDto>> GetMeetingsAsync(string search, SortingParams sortingParams);
        Task<MeetingDto> GetMeetingByIdAsync(int id);
        Task<MeetingDto> GetMeetingByPurposeAsync(string purpose);
        Task<ResponseDto<MeetingDto>> GetMeetingsByLeadIdAsync(string search, SortingParams sortingParams, int leadId);
        Task<int> AddMeetingAsync(AddMeetingDto meetingDto, string email);
        Task<int> UpdateMeetingAsync(UpdateMeetingDto meetingDto, string email);
        Task<int> DeactivateMeetingAsync(int id);
        int SendMeetingEmailAsync(AddMeetingDto meetingDto, UpdateMeetingDto updateMeetingDto, string email, List<AddAttachmentDto> attachments);
    }
}