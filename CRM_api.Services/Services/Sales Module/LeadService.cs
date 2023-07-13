using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.Helper.File_Helper;
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
            mapLead.Values.ForEach(
                lead =>
                {
                    var leadData = leads.Values.First(x => x.Id == lead.Id);
                    List<InvesmentTypeDto> investmentTypeDtos = new List<InvesmentTypeDto>();
                    var interestedIn = leadData.InterestedIn.Split(',');
                    interestedIn.ToList().ForEach(x =>
                    {
                        var mapInvestmentType = _mapper.Map<InvesmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                        investmentTypeDtos.Add(mapInvestmentType);
                    });
                    lead.TblInvesmentTypes = investmentTypeDtos;
                });
            return mapLead;
        }
        #endregion

        #region Get Investment Types
        public async Task<ResponseDto<InvesmentTypeDto>> GetInvestmentTypesAsync(string search, SortingParams sortingParams)
        {
            var investmentType = await _leadRepository.GetInvestmentTypes(search, sortingParams);
            var mapInvestmentType = _mapper.Map<ResponseDto<InvesmentTypeDto>>(investmentType);
            return mapInvestmentType;
        }
        #endregion

        #region Get Lead By Id
        public async Task<LeadDto> GetLeadByIdAsync(int id)
        {
            var lead = await _leadRepository.GetLeadById(id);
            var mappedLead = _mapper.Map<LeadDto>(lead);
            List<InvesmentTypeDto> investmentTypeDtos = new List<InvesmentTypeDto>();
            var interestedIn = lead.InterestedIn.Split(',');
            interestedIn.ToList().ForEach(x =>
            {
                var mapInvestmentType = _mapper.Map<InvesmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                investmentTypeDtos.Add(mapInvestmentType);
            });
            mappedLead.TblInvesmentTypes = investmentTypeDtos;
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

        #region Get Lead By Assignee
        public async Task<ResponseDto<LeadDto>> GetLeadByAssigneeAsync(int assignedTo, string search, SortingParams sortingParams)
        {
            var leads = await _leadRepository.GetLeadByAssignee(assignedTo, search, sortingParams);
            var mapLead = _mapper.Map<ResponseDto<LeadDto>>(leads);
            mapLead.Values.ForEach(
                lead =>
                {
                    var leadData = leads.Values.First(x => x.Id == lead.Id);
                    List<InvesmentTypeDto> investmentTypeDtos = new List<InvesmentTypeDto>();
                    var interestedIn = leadData.InterestedIn.Split(',');
                    interestedIn.ToList().ForEach(x =>
                    {
                        var mapInvestmentType = _mapper.Map<InvesmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                        investmentTypeDtos.Add(mapInvestmentType);
                    });
                    lead.TblInvesmentTypes = investmentTypeDtos;
                });
            return mapLead;
        }
        #endregion

        #region Get Lead By No Assignee
        public async Task<ResponseDto<LeadDto>> GetLeadByNoAssigneeAsync(string search, SortingParams sortingParams)
        {
            var leads = await _leadRepository.GetLeadByNoAssignee(search, sortingParams);
            var mapLead = _mapper.Map<ResponseDto<LeadDto>>(leads);
            mapLead.Values.ForEach(
                lead =>
                {
                    var leadData = leads.Values.First(x => x.Id == lead.Id);
                    List<InvesmentTypeDto> investmentTypeDtos = new List<InvesmentTypeDto>();
                    var interestedIn = leadData.InterestedIn.Split(',');
                    interestedIn.ToList().ForEach(x =>
                    {
                        var mapInvestmentType = _mapper.Map<InvesmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                        investmentTypeDtos.Add(mapInvestmentType);
                    });
                    lead.TblInvesmentTypes = investmentTypeDtos;
                });
            return mapLead;
        }
        #endregion

        #region Get Leads For CSV
        public async Task<byte[]> GetLeadsForCSVAsync(string search, SortingParams sortingParams)
        {
            var leads = await _leadRepository.GetLeadsForCSV(search, sortingParams);
            var mapLead = _mapper.Map<List<LeadCSVDto>>(leads);
            mapLead.ForEach(
                lead =>
                {
                    var leadData = leads.First(x => x.MobileNo == lead.MobileNo);
                    var investTypes = string.Empty;
                    var interestedIn = leadData.InterestedIn.Split(',');
                    interestedIn.ToList().ForEach(x =>
                    {
                        var mapInvestmentType = _mapper.Map<InvesmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                        if (string.IsNullOrEmpty(investTypes))
                            investTypes = mapInvestmentType.InvestmentName;
                        else
                            investTypes += investTypes + $", {mapInvestmentType.InvestmentName}";
                    });
                    lead.InterestedIn = investTypes;
                    lead.DateOfBirth = leadData.DateOfBirth.Value.ToShortDateString(); ;
                    lead.CreatedAt = leadData.CreatedAt.ToShortDateString();
                });

            var mapLeadCSV = new GetCSVHelper<LeadCSVDto>();
            return mapLeadCSV.WriteCSVFile(mapLead); ;
        }
        #endregion

        #region Check MobileNo Exist in Lead
        public int CheckMobileExistAsync(int? id, string mobileNo)
        {
            return _leadRepository.CheckMobileExist(id, mobileNo);
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
        public int SendLeadEmailAsync(LeadDto leadDto)
        {
            if (leadDto.Email == null)
                return 0;

            var interestedIn = string.Empty;
            if (leadDto.TblInvesmentTypes != null)
            {
                leadDto.TblInvesmentTypes.ForEach(x =>
                {
                    if (string.IsNullOrEmpty(interestedIn))
                        interestedIn = x.InvestmentName;
                    else
                        interestedIn += ", " + x.InvestmentName;
                });
            }

            var subject = "CRM - Lead";
            var message = $"Dear " + leadDto.AssignUser.UserName + ",\n\n" +
                "I hope this email finds you well. I would like to share the details of a new lead that we have acquired. Please find the information below:\r\n\r\n" +
                "Name : " + leadDto.Name + "\r\n" +
                "Phone : " + leadDto.MobileNo + "\r\n" +
                "Address : " + leadDto.Address + "\r\n" +
                "Campaign : " + leadDto.CampaignMaster.Name + "\r\n" +
                "Referred By : " + leadDto.ReferredUser.UserName + "\r\n" +
                "Status : " + leadDto.StatusMaster.Name + "\r\n" +
                "Description : " + leadDto.Description + "\r\n" +
                "Interested In : " + interestedIn + "\r\n\r\n" +
                " If you have any questions or need further clarification, feel free to reach out to me.\r\nThank you for your attention to this matter.\r\n\r\nBest regards,\r\nKAGroup";

            var body = new BodyBuilder();
            body.TextBody = message;
            var email = "aayushi@bigscal.com";
            EmailHelper.SendMailAsync(_configuration, email, subject, body);
            return 1;
        }
        #endregion

        #region Send Lead on SMS
        public int SendLeadSMSAsync(LeadDto leadDto)
        {
            if (leadDto.Email == null)
                return 0;
            var interestedIn = string.Empty;
            leadDto.TblInvesmentTypes.ForEach(x =>
            {
                if (string.IsNullOrEmpty(interestedIn))
                    interestedIn = x.InvestmentName;
                else
                    interestedIn += ", " + x.InvestmentName;
            });

            var message = "Dear " + leadDto.AssignUser.UserName + ",\n\n" +
                "I hope this email finds you well. I would like to share the details of a new lead that we have acquired. Please find the information below:\r\n\r\n" +
                "Name : " + leadDto.Name + "\r\n" +
                "Phone : " + leadDto.MobileNo + "\r\n" +
                "Address : " + leadDto.Address + "\r\n" +
                "Campaign : " + leadDto.CampaignMaster.Name + "\r\n" +
                "Referred By : " + leadDto.ReferredUser.UserName + "\r\n" +
                "Status : " + leadDto.StatusMaster.Name + "\r\n" +
                "Description : " + leadDto.Description + "\r\n" +
                "Interested In : " + interestedIn + "\r\n\r\n" +
                " If you have any questions or need further clarification, feel free to reach out to me.\r\nThank you for your attention to this matter.\r\n\r\nBest regards,\r\nKAGroup";

            var mobile = "9409394771";

            SMSHelper.SendSMS(mobile, message, "1207161796095952513");
            return 1;
        }
        #endregion
    }
}
