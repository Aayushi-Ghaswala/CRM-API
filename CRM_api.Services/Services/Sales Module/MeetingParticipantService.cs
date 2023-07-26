using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Office.Interop.Excel;

using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;

namespace CRM_api.Services.Services.Sales_Module
{
    public class MeetingParticipantService : IMeetingParticipantService
    {
        private readonly IMeetingParticipantRepository _meetingParticipantRepository;
        private readonly IMapper _mapper;

        public MeetingParticipantService(IMeetingParticipantRepository meetingParticipantRepository, IMapper mapper)
        {
            _meetingParticipantRepository = meetingParticipantRepository;
            _mapper = mapper;
        }

        #region Get MeetingParticipants
        public async Task<ResponseDto<MeetingParticipantDto>> GetMeetingParticipantsAsync(string search, SortingParams sortingParams)
        {
            var meetingParticipants = await _meetingParticipantRepository.GetMeetingParticipants(search, sortingParams);
            var mapMeetingParticipant = _mapper.Map<ResponseDto<MeetingParticipantDto>>(meetingParticipants);
            return mapMeetingParticipant;
        }

        #endregion


        #region Get MeetingParticipant By Id
        public async Task<MeetingParticipantDto> GetMeetingParticipantByIdAsync(int id)
        {
            var meetingParticipant = await _meetingParticipantRepository.GetMeetingParticipantById(id);
            var mappedMeetingParticipant = _mapper.Map<MeetingParticipantDto>(meetingParticipant);
            return mappedMeetingParticipant;
        }

        #endregion


        #region Add MeetingParticipant
        public async Task<int> AddMeetingParticipantAsync(AddMeetingParticipantDto meetingParticipantDto)
        {
            var meetingParticipant = _mapper.Map<TblMeetingParticipant>(meetingParticipantDto);
            return await _meetingParticipantRepository.AddMeetingParticipant(meetingParticipant);
        }

        #endregion


        #region Update MeetingParticipant
        public async Task<int> UpdateMeetingParticipantAsync(UpdateMeetingParticipantDto meetingParticipantDto)
        {
            var meetingParticipant = _mapper.Map<TblMeetingParticipant>(meetingParticipantDto);
            return await _meetingParticipantRepository.UpdateMeetingParticipant(meetingParticipant);
        }

        #endregion


        #region Deactivate MeetingParticipant
        public async Task<int> DeactivateMeetingParticipantAsync(int id)
        {
            return await _meetingParticipantRepository.DeactivateMeetingParticipant(id);
        }

        #endregion
    }
}