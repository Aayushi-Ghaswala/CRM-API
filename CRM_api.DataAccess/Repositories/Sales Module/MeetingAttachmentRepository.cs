using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class MeetingAttachmentRepository : IMeetingAttachmentRepository
    {
        private readonly CRMDbContext _context;

        public MeetingAttachmentRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get MeetingAttachments
        public async Task<Response<TblMeetingAttachment>> GetMeetingAttachments(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblMeetingAttachments.Where(x => x.IsDeleted != true).Include(x => x.TblMeetingMaster).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblMeetingAttachment>(search).Where(x => x.IsDeleted != true).Include(x => x.TblMeetingMaster).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var meetingAttachmentResponse = new Response<TblMeetingAttachment>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return meetingAttachmentResponse;
        }

        #endregion


        #region Get MeetingAttachment by Id
        public async Task<TblMeetingAttachment> GetMeetingAttachmentById(int id)
        {
            var meetingAttachment = await _context.TblMeetingAttachments.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return meetingAttachment;
        }

        #endregion


        #region Add MeetingAttachment
        public async Task<int> AddMeetingAttachment(int meetingId, IFormFile file)
        {
            if (!_context.TblMeetingMasters.Any(x => x.Id == meetingId))
                return 0;

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var directory = Directory.GetCurrentDirectory() + "\\wwwroot\\Meeting-Attachment\\" + meetingId;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //Delete file if already exists with same name
            if (File.Exists(Path.Combine(directory, file.FileName)))
            {
                File.Delete(Path.Combine(directory, file.FileName));
            }
            var localFilePath = Path.Combine(directory, file.FileName);
            File.Copy(filePath, localFilePath);

            var meetingAttachment = new TblMeetingAttachment()
            {
                MeetingId = meetingId,
                Attachment = localFilePath,
                IsDeleted = false
            };

            _context.TblMeetingAttachments.Add(meetingAttachment);
            return await _context.SaveChangesAsync();
        }

        #endregion


        #region Update MeetingAttachment
        public async Task<int> UpdateMeetingAttachment(TblMeetingAttachment meetingAttachment)
        {
            var meetingAttachments = _context.TblMeetingAttachments.AsNoTracking().Where(x => x.Id == meetingAttachment.Id);

            if (meetingAttachments == null) return 0;

            _context.TblMeetingAttachments.Update(meetingAttachment);
            return await _context.SaveChangesAsync();
        }

        #endregion


        #region Deactivate MeetingAttachment
        public async Task<int> DeactivateMeetingAttachment(string path)
        {
            var meetingAttachment = await _context.TblMeetingAttachments.FirstOrDefaultAsync(x => x.Attachment == path);

            if (meetingAttachment == null) return 0;

            _context.TblMeetingAttachments.Remove(meetingAttachment);
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}