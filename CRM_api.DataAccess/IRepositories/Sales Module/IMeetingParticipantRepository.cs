using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface IMeetingParticipantRepository
    {
        Task<Response<TblMeetingParticipant>> GetMeetingParticipants(string search, SortingParams sortingParams);
        Task<TblMeetingParticipant> GetMeetingParticipantById(int id);
        Task<int> AddMeetingParticipant(TblMeetingParticipant meetingParticipant);
        Task<int> UpdateMeetingParticipant(TblMeetingParticipant meetingParticipant);
        Task<int> DeactivateMeetingParticipant(int id);
    }
}