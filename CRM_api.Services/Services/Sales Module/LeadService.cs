using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CRM_api.Services.Services.Sales_Module
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LeadService(ILeadRepository leadRepository, IMapper mapper, IConfiguration configuration)
        {
            _leadRepository = leadRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        #region Get Leads
        public async Task<ResponseDto<LeadDto>> GetLeadsAsync(string search, SortingParams sortingParams)
        {
            var leads = await _leadRepository.GetLeads(search, sortingParams);
            var mapLead = _mapper.Map<ResponseDto<LeadDto>>(leads);
            return mapLead;
        }
        #endregion

        #region Get Investment Types
        public async Task<ResponseDto<InvestmentTypeDto>> GetInvestmentTypesAsync(string search, SortingParams sortingParams)
        {
            var investmentType = await _leadRepository.GetInvestmentTypes(search, sortingParams);
            var mapInvestmentType = _mapper.Map<ResponseDto<InvestmentTypeDto>>(investmentType);
            return mapInvestmentType;
        }
        #endregion

        #region Get Lead By Id
        public async Task<LeadDto> GetLeadByIdAsync(int id)
        {
            var lead = await _leadRepository.GetLeadById(id);
            var mappedLead = _mapper.Map<LeadDto>(lead);
            //SendLeadEmailAsync(mappedLead, mappedLead.Name);
            //SendLeadSMSAsync(mappedLead, mappedLead.Name);
            return mappedLead;
        }
        #endregion

        #region Get Lead By Name
        public async Task<LeadDto> GetLeadByNameAsync(string Name)
        {
            var lead = await _leadRepository.GetLeadByName(Name);
            var mappedLead = _mapper.Map<LeadDto>(lead);
            return mappedLead;
        }
        #endregion

        #region Add Lead
        public async Task<int> AddLeadAsync(AddLeadDto leadDto)
        {
            var lead = _mapper.Map<TblLeadMaster>(leadDto);
            return await _leadRepository.AddLead(lead);
        }
        #endregion

        #region Update Lead
        public async Task<int> UpdateLeadAsync(UpdateLeadDto leadDto)
        {
            var lead = _mapper.Map<TblLeadMaster>(leadDto);
            return await _leadRepository.UpdateLead(lead);
        }
        #endregion

        #region Deactivate Lead
        public async Task<int> DeactivateLeadAsync(int id)
        {
            return await _leadRepository.DeactivateLead(id);
        }
        #endregion

        #region Send Lead on Email
        public int SendLeadEmailAsync(LeadDto leadDto, string userName)
        {
            if (leadDto.Email == null)
                return 0;

            var subject = "CRM - Lead";
            var message = "Dear " + userName + ",\n\n" +
                "I hope this email finds you well. I would like to share the details of a new lead that we have acquired. Please find the information below:\r\n\r\n" +
                "Name : " + leadDto.Name + "\r\n" +
                "Phone : " + leadDto.MobileNo + "\r\n" +
                "Address : " + leadDto.Address + "\r\n" +
                "Campaign : " + leadDto.CampaignMaster.Name + "\r\n" +
                "Referred By : " + leadDto.ReferredUser.UserName + "\r\n" +
                "Status : " + leadDto.StatusMaster.Name + "\r\n" +
                "Description : " + leadDto.Description + "\r\n" +
                "Interested In : " + leadDto.TblInvesmentType.InvestmentName + "\r\n\r\n" +
                " If you have any questions or need further clarification, feel free to reach out to me.\r\nThank you for your attention to this matter.\r\n\r\nBest regards,\r\n" + leadDto.AssignUser.UserName ;

            var body = new BodyBuilder();
            body.TextBody = message;
            var email = "aayushi@bigscal.com";
            EmailHelper.SendMailAsync(_configuration, email, subject, body);
            return 1;
        }
        #endregion

        #region Send Lead on SMS
        public int SendLeadSMSAsync(LeadDto leadDto, string userName)
        {
            if (leadDto.Email == null)
                return 0;

            var message = "Dear " + userName + ",\n\n" +
                "I hope this email finds you well. I would like to share the details of a new lead that we have acquired. Please find the information below:\r\n\r\n" +
                "Name : " + leadDto.Name + "\r\n" +
                "Phone : " + leadDto.MobileNo + "\r\n" +
                "Address : " + leadDto.Address + "\r\n" +
                "Campaign : " + leadDto.CampaignMaster.Name + "\r\n" +
                "Referred By : " + leadDto.ReferredUser.UserName + "\r\n" +
                "Status : " + leadDto.StatusMaster.Name + "\r\n" +
                "Description : " + leadDto.Description + "\r\n" +
                "Interested In : " + leadDto.TblInvesmentType.InvestmentName + "\r\n\r\n" +
                " If you have any questions or need further clarification, feel free to reach out to me.\r\nThank you for your attention to this matter.\r\n\r\nBest regards,\r\n" + leadDto.AssignUser.UserName;

            var mobile = "9409394771";

            SMSHelper.SendSMS(mobile, message, "1207161796095952513");
            return 1;
        }
        #endregion
    }
}
