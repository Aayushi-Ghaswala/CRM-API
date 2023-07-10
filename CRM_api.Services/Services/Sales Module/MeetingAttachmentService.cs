using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Services.Sales_Module
{
    public class MeetingAttachmentService : IMeetingAttachmentService
    {
        private readonly IMeetingAttachmentRepository _meetingAttachmentRepository;
        private readonly IMapper _mapper;

        public MeetingAttachmentService(IMeetingAttachmentRepository meetingAttachmentRepository, IMapper mapper)
        {
            _meetingAttachmentRepository = meetingAttachmentRepository;
            _mapper = mapper;
        }

        #region Get MeetingAttachments
        public async Task<ResponseDto<MeetingAttachmentDto>> GetMeetingAttachmentsAsync(string search, SortingParams sortingParams)
        {
            var meetingAttachments = await _meetingAttachmentRepository.GetMeetingAttachments(search,sortingParams);
            var mapMeetingAttachment = _mapper.Map<ResponseDto<MeetingAttachmentDto>>(meetingAttachments);
            return mapMeetingAttachment;
        }
        #endregion

        #region Get MeetingAttachment By Id
        public async Task<MeetingAttachmentDto> GetMeetingAttachmentByIdAsync(int id)
        {
            var meetingAttachment = await _meetingAttachmentRepository.GetMeetingAttachmentById(id);
            var mappedMeetingAttachment = _mapper.Map<MeetingAttachmentDto>(meetingAttachment);
            return mappedMeetingAttachment;
        }
        #endregion

        #region Add MeetingAttachment
        public async Task<int> AddMeetingAttachmentAsync(int meetingId, IFormFile file)
        {
            //var meetingAttachment = _mapper.Map<TblMeetingAttachment>(meetingAttachmentDto);
            return await _meetingAttachmentRepository.AddMeetingAttachment(meetingId, file);
        }
        #endregion

        #region Update MeetingAttachment
        public async Task<int> UpdateMeetingAttachmentAsync(UpdateMeetingAttachmentDto meetingAttachmentDto)
        {
            var meetingAttachment = _mapper.Map<TblMeetingAttachment>(meetingAttachmentDto);
            return await _meetingAttachmentRepository.UpdateMeetingAttachment(meetingAttachment);
        }
        #endregion

        #region Deactivate MeetingAttachment
        public async Task<int> DeactivateMeetingAttachmentAsync(int id)
        {
            return await _meetingAttachmentRepository.DeactivateMeetingAttachment(id);
        }
        #endregion
    }
}
