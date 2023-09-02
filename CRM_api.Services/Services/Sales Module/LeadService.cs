using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Investment_Module;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.Helper.File_Helper;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.Sales_Module;
using CRM_api.Services.IServices.User_Module;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Globalization;

namespace CRM_api.Services.Services.Sales_Module
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IUserCategoryRepository _userCategoryRepository;
        private readonly IUserMasterService _userMasterService;
        private readonly IEmployeeRepository _employeeRepository;

        public LeadService(ILeadRepository leadRepository, IMapper mapper, IConfiguration configuration, IUserMasterRepository userMasterRepository, ICampaignRepository campaignRepository, IStatusRepository statusRepository, IRegionRepository regionRepository, IInvestmentRepository investmentRepository, IUserCategoryRepository userCategoryRepository, IUserMasterService userMasterService, IEmployeeRepository employeeRepository)
        {
            _leadRepository = leadRepository;
            _mapper = mapper;
            _configuration = configuration;
            _userMasterRepository = userMasterRepository;
            _campaignRepository = campaignRepository;
            _statusRepository = statusRepository;
            _regionRepository = regionRepository;
            _investmentRepository = investmentRepository;
            _userCategoryRepository = userCategoryRepository;
            _userMasterService = userMasterService;
            _employeeRepository = employeeRepository;
        }

        #region Get Leads
        public async Task<ResponseDto<LeadDto>> GetLeadsAsync(int? assignTo, string search, SortingParams sortingParams)
        {
            var leads = await _leadRepository.GetLeads(assignTo, search, sortingParams);
            var mapLead = _mapper.Map<ResponseDto<LeadDto>>(leads);
            mapLead.Values.ForEach(
                lead =>
                {
                    var leadData = leads.Values.First(x => x.Id == lead.Id);
                    List<InvestmentTypeDto> investmentTypeDtos = new List<InvestmentTypeDto>();
                    var interestedIn = leadData.InterestedIn.Split(',');
                    interestedIn.ToList().ForEach(x =>
                    {
                        var mapInvestmentType = _mapper.Map<InvestmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                        investmentTypeDtos.Add(mapInvestmentType);
                    });
                    lead.TblInvesmentTypes = investmentTypeDtos;
                });
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
            List<InvestmentTypeDto> investmentTypeDtos = new List<InvestmentTypeDto>();
            var interestedIn = lead.InterestedIn.Split(',');
            interestedIn.ToList().ForEach(x =>
            {
                var mapInvestmentType = _mapper.Map<InvestmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
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

        #region Get Leads For CSV
        public async Task<byte[]> GetLeadsForCSVAsync(int? assignTo, string search, SortingParams sortingParams)
        {
            var leads = await _leadRepository.GetLeadsForCSV(assignTo, search, sortingParams);
            var mapLead = _mapper.Map<List<LeadCSVDto>>(leads);
            mapLead.ForEach(
                lead =>
                {
                    var leadData = leads.First(x => x.MobileNo == lead.MobileNo);
                    var investTypes = string.Empty;
                    var interestedIn = leadData.InterestedIn.Split(',');
                    interestedIn.ToList().ForEach(x =>
                    {
                        var mapInvestmentType = _mapper.Map<InvestmentTypeDto>(_leadRepository.GetInvestmentById(Convert.ToInt32(x)));
                        if (string.IsNullOrEmpty(investTypes))
                            investTypes = mapInvestmentType.InvestmentName;
                        else
                            investTypes += $", {mapInvestmentType.InvestmentName}";
                    });
                    lead.InterestedIn = investTypes;
                    if (lead.DateOfBirth is not null)
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
            var leadList = new List<TblLeadMaster>();
            leadList.Add(lead);
            return await _leadRepository.AddLead(leadList);
        }
        #endregion

        #region Import Lead Data
        public async Task<int> ImportLeadAsync(IFormFile formFile)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = culture;

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\Lead";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //Delete file if already exists with same name
            if (File.Exists(Path.Combine(directory, formFile.FileName)))
            {
                File.Delete(Path.Combine(directory, formFile.FileName));
            }
            var localFilePath = Path.Combine(directory, formFile.FileName);
            File.Copy(filePath, localFilePath);

            var leadList = new List<ImportLeadDto>();

            using (var fs = new StreamReader(localFilePath))
            {
                // to load the records from the file in my List<CsvLine>
                leadList = new CsvReader(fs, culture).GetRecords<ImportLeadDto>().ToList();
            }

            var mappedLeadList = new List<TblLeadMaster>();
            foreach (var lead in leadList)
            {
                var mappedLead = new TblLeadMaster();
                mappedLead.Name = lead.Name;
                mappedLead.Email = lead.Email;
                mappedLead.MobileNo = lead.MobileNo;
                mappedLead.Address = lead.Address;
                mappedLead.DateOfBirth = lead.DateOfBirth;
                mappedLead.Gender = lead.Gender;
                mappedLead.CreatedAt = lead.CreatedAt;
                mappedLead.Description = lead.Description;
                mappedLead.UserId = lead.UserId;

                if (!string.IsNullOrEmpty(lead.AssignUser))
                {
                    var assignTo = await _employeeRepository.GetEmployeeByName(lead.AssignUser);
                    if (assignTo is not null)
                        mappedLead.AssignedTo = assignTo.Id;
                }

                if (!string.IsNullOrEmpty(lead.ReferredUser))
                {
                    var referredBy = await _userMasterRepository.GetUserByName(lead.ReferredUser);
                    if (referredBy is not null)
                        mappedLead.ReferredBy = referredBy.UserId;
                }

                if (!string.IsNullOrEmpty(lead.Campaign))
                {
                    var campaign = await _campaignRepository.GetCampaignByName(lead.Campaign);
                    if (campaign is not null)
                        mappedLead.CampaignId = campaign.Id;
                }

                if (!string.IsNullOrEmpty(lead.Status))
                {
                    var status = await _statusRepository.GetStatusByName(lead.Status);
                    if (status is not null)
                        mappedLead.StatusId = status.Id;
                }

                if (!string.IsNullOrEmpty(lead.Country))
                {
                    var country = await _regionRepository.GetCountryByName(lead.Country);
                    if (country is not null)
                        mappedLead.CountryId = country.CountryId;
                }

                if (!string.IsNullOrEmpty(lead.State))
                {
                    var state = await _regionRepository.GetStateByName(lead.State);
                    if (state is not null)
                        mappedLead.StateId = state.StateId;
                }

                if (!string.IsNullOrEmpty(lead.City))
                {
                    var city = await _regionRepository.GetCityByName(lead.City);
                    if (city is not null)
                        mappedLead.CityId = city.CityId;
                }

                if (!string.IsNullOrEmpty(lead.InterestedIn))
                {
                    var interestedIns = lead.InterestedIn.Split(',');
                    foreach (var interestedIn in interestedIns)
                    {
                        var invId = await _investmentRepository.GetInvTypeByName(interestedIn.Trim());
                        if (invId != 0)
                        {
                            if (string.IsNullOrEmpty(mappedLead.InterestedIn))
                                mappedLead.InterestedIn = invId.ToString();
                            else
                                mappedLead.InterestedIn += ", " + invId.ToString();
                        }
                    }
                }

                mappedLeadList.Add(mappedLead);
            }
            
            return await _leadRepository.AddLead(mappedLeadList);
        }
        #endregion 

        #region Update Lead
        public async Task<int> UpdateLeadAsync(UpdateLeadDto leadDto)
        {
            var lead = await _leadRepository.GetLeadById(leadDto.Id);
            var status = await _statusRepository.GetStatusByName("won");
            var updatedlead = _mapper.Map<TblLeadMaster>(leadDto);
            if (updatedlead.StatusId == status.Id && lead.StatusId != status.Id)
            {
                var userId = await AddUserByLead(updatedlead);
                if (userId > 0)
                    updatedlead.UserId = userId;
                else
                    return 0;
            }
            return await _leadRepository.UpdateLead(updatedlead);
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

            var message = $"Dear User,\n\n" +
                 "I hope this email finds you well. I would like to share the details of a new lead that we have acquired. Please find the information below:\r\n\r\n" +
                 "Name : " + leadDto.Name + "\r\n" +
                 "Phone : " + leadDto.MobileNo + "\r\n" +
                 "Address : " + leadDto.Address + "\r\n" +
                 "Campaign : " + leadDto.CampaignMaster.Name + "\r\n";
            if (leadDto.ReferredUser.UserName is not null)
                message += "Referred By : " + leadDto.ReferredUser.UserName + "\r\n";
            message += "Status : " + leadDto.StatusMaster.Name + "\r\n" +
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

            var message = $"Dear User,\n\n" +
                "I hope this email finds you well. I would like to share the details of a new lead that we have acquired. Please find the information below:\r\n\r\n" +
                "Name : " + leadDto.Name + "\r\n" +
                "Phone : " + leadDto.MobileNo + "\r\n" +
                "Address : " + leadDto.Address + "\r\n" +
                "Campaign : " + leadDto.CampaignMaster.Name + "\r\n";
            if (leadDto.ReferredUser.UserName is not null)
                message += "Referred By : " + leadDto.ReferredUser.UserName + "\r\n";
            message += "Status : " + leadDto.StatusMaster.Name + "\r\n" +
                    "Description : " + leadDto.Description + "\r\n" +
                    "Interested In : " + interestedIn + "\r\n\r\n" +
                    " If you have any questions or need further clarification, feel free to reach out to me.\r\nThank you for your attention to this matter.\r\n\r\nBest regards,\r\nKAGroup";

            var mobile = "9409394771";

            SMSHelper.SendSMS(mobile, message, "1207161796095952513");
            return 1;
        }
        #endregion

        #region Add User By Lead
        private async Task<int> AddUserByLead(TblLeadMaster leadMaster)
        {
            var category = await _userCategoryRepository.GetCategoryByName("Customer");
            var adduserDto = new AddUserMasterDto();
            adduserDto.UserName = leadMaster.Name;
            adduserDto.UserEmail = leadMaster.Email;
            adduserDto.UserMobile = leadMaster.MobileNo;
            adduserDto.UserDoj = DateTime.Now;
            adduserDto.UserDob = leadMaster.DateOfBirth;
            adduserDto.UserWbcActive = true;
            if (category is not null)
                adduserDto.CatId = category.CatId;
            adduserDto.UserCityId = leadMaster.CityId;
            adduserDto.UserStateId = leadMaster.StateId;
            adduserDto.UserCountryId = leadMaster.CountryId;
            adduserDto.UserParentId = leadMaster.ReferredBy;
            adduserDto.UserAddr = leadMaster.Address;

            var userId = await _userMasterService.AddUserAsync(adduserDto, true);

            if (userId > 0)
                return userId;
            else
                return 0;
        }
        #endregion
    }
}
