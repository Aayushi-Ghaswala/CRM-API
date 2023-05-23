using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.LI_GI_Module;
using static CRM_api.Services.Helper.ConstantValue.InvesmentTypeConstant;

namespace CRM_api.Services.Services.Business_Module.LI_GI_Module
{
    public class InsuranceClientService : IInsuranceClientService
    {
        private readonly IInsuranceClientRepository _insuranceClientRepository;
        private readonly IMapper _mapper;
        private readonly IUserMasterRepository _userMasterRepository;

        public InsuranceClientService(IInsuranceClientRepository insuranceClientRepository, IMapper mapper, IUserMasterRepository userMasterRepository)
        {
            _insuranceClientRepository = insuranceClientRepository;
            _mapper = mapper;
            _userMasterRepository = userMasterRepository;
        }

        #region Add InsuranceClient Detail
        public async Task<int> AddInsuranceClientAsync(AddInsuranceClientDto insuranceClientDto)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(insuranceClientDto.InsUserid);
            var planType = await _insuranceClientRepository.GetInsuranceplanTypeById(insuranceClientDto.InsPlantypeId);
            var invType = await _insuranceClientRepository.GetInvesmentTypeByName(InvesmentType.Insurance.ToString());
            var insuranceClient = _mapper.Map<TblInsuranceclient>(insuranceClientDto);

            insuranceClient.InsUsername = user.UserName;
            insuranceClient.InsPan = user.UserPan;
            insuranceClient.InsEmail = user.UserEmail;
            insuranceClient.InsMobile = user.UserMobile;
            insuranceClient.InsPlantype = planType.InsPlanType;
            insuranceClient.InvType = invType.Id;

            return await _insuranceClientRepository.AddInsuranceDetail(insuranceClient);

        }
        #endregion

        #region Update InsuranceClient Detail
        public async Task<int> UpdateInsuranceClientAsync(UpdateInsuranceClientDto insuranceClientDto)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(insuranceClientDto.InsUserid);
            var planType = await _insuranceClientRepository.GetInsuranceplanTypeById(insuranceClientDto.InsPlantypeId);
            var insuranceClient = _mapper.Map<TblInsuranceclient>(insuranceClientDto);

            insuranceClient.InsUsername = user.UserName;
            insuranceClient.InsPan = user.UserPan;
            insuranceClient.InsEmail = user.UserEmail;
            insuranceClient.InsMobile = user.UserMobile;
            insuranceClient.InsPlantype = planType.InsPlanType;

            return await _insuranceClientRepository.UpdateInsuranceClientDetail(insuranceClient);

        }
        #endregion

        #region Get All Insurance Company By Insurance Type
        public async Task<ResponseDto<InsuranceCompanyListDto>> GetCompanyListByInsTypeIdAsync(int id, SortingParams sortingParams)
        {
            var companyList = await _insuranceClientRepository.GetCompanyListByInsTypeId(id, sortingParams);
            var mapCompanyList = _mapper.Map<ResponseDto<InsuranceCompanyListDto>>(companyList);

            return mapCompanyList;
        }
        #endregion

        #region Get All Insurance Client Details
        public async Task<ResponseDto<InsuranceClientDto>> GetInsuranceClientsAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            var insClients = await _insuranceClientRepository.GetInsuranceClients(searchingParams, sortingParams);
            var mapInsClients = _mapper.Map<ResponseDto<InsuranceClientDto>>(insClients);

            return mapInsClients;
        }
        #endregion

        #region Get Insurance Client Detail By Id
        public async Task<InsuranceClientDto> GetInsuranceClientByIdAsync(int id)
        {
            var insClient = await _insuranceClientRepository.GetInsuranceClientById(id);
            var mapInsClient = _mapper.Map<InsuranceClientDto>(insClient);

            return mapInsClient;
        }
        #endregion

        #region Deactivate Insurance Client Detail
        public async Task<int> DeactivateInsClientAsync(int id)
        {
            return await _insuranceClientRepository.DeactivateInsuranceClientDetail(id);
        }
        #endregion
    }
}
