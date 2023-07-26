using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.AspNetCore.Http;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface IMeetingAttachmentRepository
    {
        Task<Response<TblMeetingAttachment>> GetMeetingAttachments(string search, SortingParams sortingParams);
        Task<TblMeetingAttachment> GetMeetingAttachmentById(int id);
        Task<int> AddMeetingAttachment(int meetingId, IFormFile file);
        Task<int> UpdateMeetingAttachment(TblMeetingAttachment meetingAttachment);
        Task<int> DeactivateMeetingAttachment(string path);
    }
}