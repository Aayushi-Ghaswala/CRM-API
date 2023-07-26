using AutoMapper;
using BitMiracle.LibTiff.Classic;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Linq;
using System.Net.Mail;

namespace CRM_api.Services.Services.Sales_Module
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public MeetingService(IMeetingRepository meetingRepository, IMapper mapper, IConfiguration configuration)
        {
            _meetingRepository = meetingRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        #region Get Meetings
        public async Task<ResponseDto<MeetingDto>> GetMeetingsAsync(string search, SortingParams sortingParams)
        {
            var meetings = await _meetingRepository.GetMeetings(search, sortingParams);
            var mapMeeting = _mapper.Map<ResponseDto<MeetingDto>>(meetings);
            return mapMeeting;
        }

        #endregion

        #region Get Meetings By Lead
        public async Task<ResponseDto<MeetingDto>> GetMeetingsByLeadIdAsync(string search, SortingParams sortingParams, int leadId)
        {
            var meetings = await _meetingRepository.GetMeetingByLeadId(search, sortingParams, leadId);
            var mapMeeting = _mapper.Map<ResponseDto<MeetingDto>>(meetings);
            return mapMeeting;
        }

        #endregion


        #region Get Meeting By Id
        public async Task<MeetingDto> GetMeetingByIdAsync(int id)
        {
            var meeting = await _meetingRepository.GetMeetingById(id);
            var mappedMeeting = _mapper.Map<MeetingDto>(meeting);
            return mappedMeeting;
        }

        #endregion

        #region Get Meeting By Purpose
        public async Task<MeetingDto> GetMeetingByPurposeAsync(string purpose)
        {
            var meeting = await _meetingRepository.GetMeetingByPurpose(purpose);
            var mappedMeeting = _mapper.Map<MeetingDto>(meeting);
            return mappedMeeting;
        }

        #endregion

        #region Add Meeting
        public async Task<int> AddMeetingAsync(AddMeetingDto meetingDto, string email)
        {
            if (email.ToLower() == MeetingConstant.doNotEmail.ToLower())
                email = MeetingConstant.doNotEmail;
            else if (email.ToLower() == MeetingConstant.emailWithAttachment.ToLower())
                email = MeetingConstant.emailWithAttachment;
            else if (email.ToLower() == MeetingConstant.emailWithoutAttachment.ToLower())
                email = MeetingConstant.emailWithoutAttachment;

            var meeting = _mapper.Map<TblMeetingMaster>(meetingDto);
            var participants = new List<TblMeetingParticipant>();
            foreach (var participant in meetingDto.MeetingParticipants)
            {
                var addParticipant = new TblMeetingParticipant()
                {
                    ParticipantId = participant.ParticipantId,
                    LeadId = participant.LeadId,
                    IsDeleted = false
                };
                participants.Add(addParticipant);
            }
            meeting.Participants = participants;
            var addedMeeting = await _meetingRepository.AddMeeting(meeting);
            if (addedMeeting is null)
                return 0;
            var attachments = new List<AddAttachmentDto>();
            var addAttachments = new List<TblMeetingAttachment>(); ;
            var directoryPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\Meeting-Attachment\\{addedMeeting.Id}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var file in meetingDto.Files)
            {
                var filePath = Path.Combine(directoryPath, file.fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var fileByte = Convert.FromBase64String(file.base64);
                System.IO.File.WriteAllBytes(filePath, fileByte);
                var attachment = new AddAttachmentDto()
                {
                    fileName = file.fileName,
                    fileByte = fileByte
                };
                attachments.Add(attachment);

                var addFile = new TblMeetingAttachment()
                {
                    Attachment = filePath,
                    MeetingId = addedMeeting.Id,
                    IsDeleted = false
                };
                addAttachments.Add(addFile);
            }
            if (email.ToLower() != MeetingConstant.doNotEmail.ToLower())
                SendMeetingEmailAsync(meetingDto, null, email, attachments);
            return await _meetingRepository.AddMeetingAttachments(addAttachments);
        }

        #endregion

        #region Update Meeting
        public async Task<int> UpdateMeetingAsync(UpdateMeetingDto meetingDto, string email)
        {
            if (email.ToLower() == MeetingConstant.doNotEmail.ToLower())
                email = MeetingConstant.doNotEmail;
            else if (email.ToLower() == MeetingConstant.emailWithAttachment.ToLower())
                email = MeetingConstant.emailWithAttachment;
            else if (email.ToLower() == MeetingConstant.emailWithoutAttachment.ToLower())
                email = MeetingConstant.emailWithoutAttachment;

            var meeting = await _meetingRepository.GetMeetingById(meetingDto.Id);
            var updatedMeeting = _mapper.Map<TblMeetingMaster>(meetingDto);

            foreach (var participant in meetingDto.MeetingParticipants)
            {
                if (!meeting.Participants.Any(x => x.LeadId == participant.LeadId && x.ParticipantId == participant.ParticipantId))
                {
                    var addParticipant = new TblMeetingParticipant()
                    {
                        ParticipantId = participant.ParticipantId,
                        LeadId = participant.LeadId,
                        IsDeleted = false
                    };
                    meeting.Participants.Add(addParticipant);
                }
            }

            List<TblMeetingParticipant> deleteMeetingParticipant = meeting.Participants.Where(m => !meetingDto.MeetingParticipants.Any(x => x.ParticipantId == m.ParticipantId) || !meetingDto.MeetingParticipants.Any(x => x.LeadId == m.LeadId)).ToList();

            foreach (var participant in deleteMeetingParticipant)
            {
                meeting.Participants.Remove(participant);
            }
            updatedMeeting.Participants = meeting.Participants;

            var directoryPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\Meeting-Attachment\\{meeting.Id}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var attachments = new List<AddAttachmentDto>();

            foreach (var file in meetingDto.Files)
            {
                var filePath = Path.Combine(directoryPath, file.fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var fileByte = Convert.FromBase64String(file.base64);
                System.IO.File.WriteAllBytes(filePath, fileByte);
                var attachment = new AddAttachmentDto()
                {
                    fileName = file.fileName,
                    fileByte = fileByte
                };
                attachments.Add(attachment);

                var addFile = new TblMeetingAttachment()
                {
                    Attachment = filePath,
                    MeetingId = meeting.Id,
                    IsDeleted = false
                };
                meeting.Attachments.Add(addFile);
            }
            updatedMeeting.Attachments = meeting.Attachments;
            if (email.ToLower() != MeetingConstant.doNotEmail.ToLower())
                SendMeetingEmailAsync(null, meetingDto, email, attachments);
            return await _meetingRepository.UpdateMeeting(updatedMeeting);
        }

        #endregion

        #region Deactivate Meeting
        public async Task<int> DeactivateMeetingAsync(int id)
        {
            return await _meetingRepository.DeactivateMeeting(id);
        }

        #endregion

        #region Send email of meeting
        public int SendMeetingEmailAsync(AddMeetingDto? meetingDto, UpdateMeetingDto? updateMeetingDto, string email, List<AddAttachmentDto> attachments)
        {
            if (meetingDto != null)
            {
                if (meetingDto.MeetingParticipants.Count <= 0)
                    return 0;
                var message = "";
                foreach (var participant in meetingDto.MeetingParticipants)
                {
                    var subject = meetingDto.Purpose;
                    if (meetingDto.Mode.ToLower() == MeetingConstant.inPerson.ToLower())
                    {
                        message = "Dear " + participant.Name + ",\n\n" +
                                        "I hope this email finds you well. I am writing to request a meeting to discuss " + meetingDto.Purpose + ". I believe that a face-to-face discussion will greatly benefit us in addressing specific issues, concerns, or opportunities.\r\n\r\nI suggest the following details for the meeting:\n\n" +
                                        "Date:" + meetingDto.DateOfMeeting.ToShortDateString() + "\r\nDuration:" + meetingDto.Duration + "\r\nLocation:" + meetingDto.Location + "\r\nAgenda:" + meetingDto.Purpose + "\n\n" +
                                        "If the proposed date and time are not suitable for you, please let me know your availability within the next 2 hours so that we can find a mutually convenient time to meet.Please confirm your availability and provide any additional agenda items or specific points you would like to discuss during the meeting. I believe your input will be valuable in shaping the agenda and ensuring a productive discussion.If there are any documents, reports, or materials that you would like to share in advance of the meeting, please forward them to me at your earliest convenience.\r\n\r\nThank you for your attention to this matter, and I look forward to your positive response. Should you have any questions or need further information, please feel free to reach out to me." + "\n\n" +
                                        "Best regards,\r\n" + meetingDto.MeetingByName;
                    }
                    else if (meetingDto.Mode.ToLower() == MeetingConstant.telephonic.ToLower())
                    {
                        message = "Dear " + participant.Name + ",\n\n" +
                                    "I hope this message finds you well. My name is " + meetingDto.MeetingByName + ", and I am reaching out to request a telephonic call with you. I believe that a conversation would be beneficial for both of us to discuss " + meetingDto.Purpose + ".\r\n\r\nOur call will be at the following Date: " + meetingDto.DateOfMeeting.ToShortDateString() + "and Time: " + meetingDto.DateOfMeeting.ToShortTimeString() + ". However, if any of these options do not work for you, please feel free to suggest an alternative time that is more convenient.The call's estimated duration would be around " + meetingDto.Duration + ". Please let me know if you have any specific topics or questions you would like to address during the call, and I will be more than happy to prepare accordingly.If you prefer, we can also arrange the call through a preferred communication platform or video conferencing tool.\r\n\r\nThank you for considering my request, and I look forward to connecting with you to discuss " + meetingDto.Purpose + ". If there are any changes or updates regarding the call, I will promptly inform you.\n\n" +
                                        "Best regards,\r\n" + meetingDto.MeetingByName;
                    }
                    else if (meetingDto.Mode.ToLower() == MeetingConstant.videoConference.ToLower())
                    {
                        message = "Dear " + participant.Name + ",\n\n" +
                                    "I hope this message finds you well. We are excited to invite you to our upcoming online meeting, which will focus on " + meetingDto.Purpose + ". Your presence and valuable insights will greatly contribute to the success of this discussion.\r\n\r\nMeeting Details:\r\nDate: " + meetingDto.DateOfMeeting.ToShortDateString() + "\r\nTime: " + meetingDto.DateOfMeeting.ToShortTimeString() + "\r\nDuration: " + meetingDto.Duration + "\r\nPlatform: " + meetingDto.Link + "\r\n\r\nIf you have any specific points you would like to add to the agenda or any relevant documents to share, please feel free to let us know before the meeting.As we aim for maximum productivity during the meeting, kindly ensure that you have a stable internet connection and a quiet environment.We look forward to your participation and engagement in this important meeting. Should you have any questions or concerns, please don't hesitate to reach out to us.\r\n\r\nThank you for your time and commitment. We are confident that your contributions will be valuable in shaping the outcomes of this meeting.\n\n" + "Best regards,\r\n" + meetingDto.MeetingByName;
                    }
                    var body = new BodyBuilder();
                    body.TextBody = message;
                    var to = "aayushi@bigscal.com";

                    if (email.ToLower() == MeetingConstant.emailWithAttachment.ToLower())
                    {
                        foreach (var item in attachments)
                        {
                            body.Attachments.Add(item.fileName, item.fileByte);
                        }
                    }

                    EmailHelper.SendMailAsync(_configuration, to, subject, body);
                    return 1;
                }
            }
            else if(updateMeetingDto != null)
            {
                if (updateMeetingDto.MeetingParticipants.Count <= 0)
                    return 0;
                var message = "";
                foreach (var participant in updateMeetingDto.MeetingParticipants)
                {
                    var subject = updateMeetingDto.Purpose;
                    if (updateMeetingDto.Mode.ToLower() == MeetingConstant.inPerson.ToLower())
                    {
                        message = "Dear " + participant.Name + ",\n\n" +
                                        "I hope this email finds you well. I am writing to request a meeting to discuss " + updateMeetingDto.Purpose + ". I believe that a face-to-face discussion will greatly benefit us in addressing specific issues, concerns, or opportunities.\r\n\r\nI suggest the following details for the meeting:\n\n" +
                                        "Date:" + updateMeetingDto.DateOfMeeting.ToShortDateString() + "\r\nDuration:" + updateMeetingDto.Duration + "\r\nLocation:" + updateMeetingDto.Location + "\r\nAgenda:" + updateMeetingDto.Purpose + "\n\n" +
                                        "If the proposed date and time are not suitable for you, please let me know your availability within the next 2 hours so that we can find a mutually convenient time to meet.Please confirm your availability and provide any additional agenda items or specific points you would like to discuss during the meeting. I believe your input will be valuable in shaping the agenda and ensuring a productive discussion.If there are any documents, reports, or materials that you would like to share in advance of the meeting, please forward them to me at your earliest convenience.\r\n\r\nThank you for your attention to this matter, and I look forward to your positive response. Should you have any questions or need further information, please feel free to reach out to me." + "\n\n" +
                                        "Best regards,\r\n" + updateMeetingDto.MeetingByName;
                    }
                    else if (updateMeetingDto.Mode.ToLower() == MeetingConstant.telephonic.ToLower())
                    {
                        message = "Dear " + participant.Name + ",\n\n" +
                                    "I hope this message finds you well. My name is " + updateMeetingDto.MeetingByName + ", and I am reaching out to request a telephonic call with you. I believe that a conversation would be beneficial for both of us to discuss " + updateMeetingDto.Purpose + ".\r\n\r\nOur call will be at the following Date: " + updateMeetingDto.DateOfMeeting.ToShortDateString() + "and Time: " + updateMeetingDto.DateOfMeeting.ToShortTimeString() + ". However, if any of these options do not work for you, please feel free to suggest an alternative time that is more convenient.The call's estimated duration would be around " + updateMeetingDto.Duration + ". Please let me know if you have any specific topics or questions you would like to address during the call, and I will be more than happy to prepare accordingly.If you prefer, we can also arrange the call through a preferred communication platform or video conferencing tool.\r\n\r\nThank you for considering my request, and I look forward to connecting with you to discuss " + updateMeetingDto.Purpose + ". If there are any changes or updates regarding the call, I will promptly inform you.\n\n" +
                                        "Best regards,\r\n" + updateMeetingDto.MeetingByName;
                    }
                    else if (updateMeetingDto.Mode.ToLower() == MeetingConstant.videoConference.ToLower())
                    {
                        message = "Dear " + participant.Name + ",\n\n" +
                                    "I hope this message finds you well. We are excited to invite you to our upcoming online meeting, which will focus on " + updateMeetingDto.Purpose + ". Your presence and valuable insights will greatly contribute to the success of this discussion.\r\n\r\nMeeting Details:\r\nDate: " + updateMeetingDto.DateOfMeeting.ToShortDateString() + "\r\nTime: " + updateMeetingDto.DateOfMeeting.ToShortTimeString() + "\r\nDuration: " + updateMeetingDto.Duration + "\r\nPlatform: " + updateMeetingDto.Link + "\r\n\r\nIf you have any specific points you would like to add to the agenda or any relevant documents to share, please feel free to let us know before the meeting.As we aim for maximum productivity during the meeting, kindly ensure that you have a stable internet connection and a quiet environment.We look forward to your participation and engagement in this important meeting. Should you have any questions or concerns, please don't hesitate to reach out to us.\r\n\r\nThank you for your time and commitment. We are confident that your contributions will be valuable in shaping the outcomes of this meeting.\n\n" + "Best regards,\r\n" + updateMeetingDto.MeetingByName;
                    }
                    var body = new BodyBuilder();
                    body.TextBody = message;
                    var to = "aayushi@bigscal.com";

                    if (email.ToLower() == MeetingConstant.emailWithAttachment.ToLower())
                    {
                        foreach (var item in attachments)
                        {
                            body.Attachments.Add(item.fileName, item.fileByte);
                        }
                    }

                    EmailHelper.SendMailAsync(_configuration, to, subject, body);
                    return 1;
                }
            }
            return 0;
        }

        #endregion
    }
}