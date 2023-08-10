using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface IMeetingRepository
    {
        Task<Response<TblMeetingMaster>> GetMeetings(string search, SortingParams sortingParams);
        Task<TblMeetingMaster> GetMeetingById(int id);
        Task<TblMeetingMaster> GetMeetingByPurpose(string purpose);
        Task<Response<TblMeetingMaster>> GetMeetingByLeadId(string search, SortingParams sortingParams, int leadId);
        Task<List<TblMeetingMaster>> GetUserWiseMeetings(int? userId, DateTime date);
        Task<List<TblMeetingMaster>> GetUserWiseMeetingsSchedule(int? userId);
        Task<TblMeetingMaster> AddMeeting(TblMeetingMaster meetingRequest);
        Task<int> AddMeetingAttachments(List<TblMeetingAttachment> attachments);
        Task<int> UpdateMeeting(TblMeetingMaster meeting);
        Task<int> DeactivateMeeting(int id);
    }
}