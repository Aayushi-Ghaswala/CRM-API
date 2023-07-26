using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface IMeetingParticipantService
    {
        Task<ResponseDto<MeetingParticipantDto>> GetMeetingParticipantsAsync(string search, SortingParams sortingParams);
        Task<MeetingParticipantDto> GetMeetingParticipantByIdAsync(int id);
        Task<int> AddMeetingParticipantAsync(AddMeetingParticipantDto meetingParticipantDto);
        Task<int> UpdateMeetingParticipantAsync(UpdateMeetingParticipantDto meetingParticipantDto);
        Task<int> DeactivateMeetingParticipantAsync(int id);
    }
}