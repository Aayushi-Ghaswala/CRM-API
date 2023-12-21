using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Investment_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.LI_GI_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.IServices.Business_Module.LI_GI_Module;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Globalization;
using static CRM_api.Services.Helper.ConstantValue.InvesmentTypeConstant;
using static CRM_api.Services.Helper.ConstantValue.SubInvTypeConstant;

namespace CRM_api.Services.Services.Business_Module.LI_GI_Module
{
    public class InsuranceClientService : IInsuranceClientService
    {
        private readonly IInsuranceClientRepository _insuranceClientRepository;
        private readonly IMapper _mapper;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IInvestmentRepository _investmentRepository;

        public InsuranceClientService(IInsuranceClientRepository insuranceClientRepository, IMapper mapper, IUserMasterRepository userMasterRepository, IInvestmentRepository investmentRepository)
        {
            _insuranceClientRepository = insuranceClientRepository;
            _mapper = mapper;
            _userMasterRepository = userMasterRepository;
            _investmentRepository = investmentRepository;
        }

        #region Get All Insurance Client Details
        public async Task<ResponseDto<InsuranceClientDto>> GetInsuranceClientsAsync(string? filterString, string search, SortingParams sortingParams)
        {
            var insClients = await _insuranceClientRepository.GetInsuranceClients(filterString, search, sortingParams);
            var mapInsClients = _mapper.Map<ResponseDto<InsuranceClientDto>>(insClients);

            return mapInsClients;
        }
        #endregion

        #region Get All Insurance Company By Insurance Type
        public async Task<ResponseDto<InsuranceCompanyListDto>> GetCompanyListByInsTypeIdAsync(int id, SortingParams sortingParams)
        {
            var companyList = await _insuranceClientRepository.GetCompanyListByInsTypeId(id, sortingParams);
            var mapCompanyList = _mapper.Map<ResponseDto<InsuranceCompanyListDto>>(companyList);

            foreach (var company in mapCompanyList.Values)
            {
                company.InsuranceCompanyname = company.InsuranceCompanyname.ToLower();
            }
            return mapCompanyList;
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

        #region Import InsuranceClients File
        public async Task<(string, int)> ImportInsClientsFileAsync(IFormFile formFile)
        {
            var filePath = System.IO.Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\InsuranceClientFile";

            if (!(Directory.Exists(directoryPath)))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var localFilePath = System.IO.Path.Combine(directoryPath, formFile.FileName);

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
                return ("File data already exists.", 0);
            }

            File.Copy(filePath, localFilePath);
            using (var stream = File.Open(localFilePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataset = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true,
                            }
                        });

                        var records = new List<ImportInsClientDto>();
                        var invTypeId = await _insuranceClientRepository.GetInvesmentTypeByName(InvesmentType.Insurance.ToString());
                        var subInvTypes = await _investmentRepository.GetAllSubInvTypes();
                        var planTypes = await _insuranceClientRepository.GetPlanTypes();
                        var planTypesName = planTypes.Select(x => x.InsPlanType.ToLower());

                        foreach (DataTable datatable in dataset.Tables)
                        {
                            if (datatable.Columns.Count > 0)
                            {
                                string firstColumnHeader = datatable.Columns[0].ColumnName;
                                string secondColumnHeader = datatable.Columns[1].ColumnName;
                                if (firstColumnHeader.Equals("Plan Type") || secondColumnHeader.Equals("Plan Type"))
                                {
                                    datatable.AsEnumerable().ToList().ConvertAll<ImportInsClientDto>(row =>
                                    {
                                        if (planTypesName.Contains(row["Plan Type"].ToString().ToLower()))
                                        {
                                            var obj = new ImportInsClientDto();
                                            obj.InsPlantype = row["Plan Type"] == DBNull.Value ? null : row["Plan Type"].ToString();
                                            obj.InsPlan = row["Plan"] == DBNull.Value ? null : row["Plan"].ToString();
                                            obj.InsPolicy = row["Existing Policy No."] == DBNull.Value ? null : row["Existing Policy No."].ToString();
                                            obj.InsPan = row["Pan Card No."] == DBNull.Value ? null : row["Pan Card No."].ToString();
                                            obj.InsMobile = row["Mobile No."] == DBNull.Value ? null : row["Mobile No."].ToString();

                                            DateTime.TryParseExact(row["Premium Due Date"] == DBNull.Value ? null : row["Premium Due Date"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTemp);
                                            obj.InsPremiumRmdDate = dateTemp;

                                            obj.PremiumAmount = Convert.ToDecimal(row["Last Premium Paid Amount / Premium Due Amount(Gross Premium) (Rs) [1]"] == DBNull.Value ? null : row["Last Premium Paid Amount / Premium Due Amount(Gross Premium) (Rs) [1]"]);
                                            obj.InsNewpolicy = row["New Policy Number"] == DBNull.Value ? null : row["New Policy Number"].ToString();
                                            obj.InsEmail = row["Customer E-mail id"] == DBNull.Value ? null : row["Customer E-mail id"].ToString();
                                            obj.InsUsername = row["Policy Holder"] == DBNull.Value ? null : row["Policy Holder"].ToString();
                                            obj.InvType = invTypeId.Id;

                                            var userId = _userMasterRepository.GetUserIdByUserPan(obj.InsPan);
                                            if (userId > 0)
                                                obj.InsUserid = userId;

                                            var planType = planTypes.Where(x => x.InsPlanType.ToLower().Equals(obj.InsPlantype.ToLower())).First();
                                            if (planType is not null)
                                                obj.InsPlantypeId = planType.InsPlantypeId;

                                            if (obj.InsPlantype.ToLower().Equals(InsPlanTypeConstant.ulip.ToLower()) || obj.InsPlantype.ToLower().Equals(InsPlanTypeConstant.traditional.ToLower()) || obj.InsPlantype.ToLower().Equals(InsPlanTypeConstant.termAssurance.ToLower()))
                                            {
                                                var subInvType = subInvTypes.Where(x => x.InvestmentType.ToLower().Equals(SubInvType.Life.ToString().ToLower())).First();
                                                if (subInvType is not null)
                                                    obj.InvSubtype = subInvType.Id;
                                            }
                                            else
                                            {
                                                var subInvType = subInvTypes.Where(x => x.InvestmentType.ToLower().Equals(SubInvType.General.ToString().ToLower())).First();
                                                if (subInvType is not null)
                                                    obj.InvSubtype = subInvType.Id;
                                            }

                                            records.Add(obj);
                                            return obj;
                                        }
                                        else
                                            return null;
                                    });
                                }
                            }
                        }

                        var mappedRecords = _mapper.Map<List<TblInsuranceclient>>(records);
                        await _insuranceClientRepository.ImportInsClientsFile(mappedRecords);
                        return ("File imported successfully.", 1);
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(localFilePath))
                        File.Delete(localFilePath);
                    return (ex.Message, 0);
                }
            }
        }

        #endregion

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

        #region Deactivate Insurance Client Detail
        public async Task<int> DeactivateInsClientAsync(int id)
        {
            return await _insuranceClientRepository.DeactivateInsuranceClientDetail(id);
        }
        #endregion
    }
}
