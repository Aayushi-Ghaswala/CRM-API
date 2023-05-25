using AutoMapper;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace CRM_api.Services.Services.Business_Module.MutualFunds_Module
{
    public class MutualfundService : IMutualfundService
    {
        private readonly IMutualfundRepositry _mutualfundRepositry;
        private readonly IMapper _mapper;
        private readonly IUserMasterRepository _userMasterRepository;

        public MutualfundService(IMutualfundRepositry mutualfundRepositry, IMapper mapper, IUserMasterRepository userMasterRepository)
        {
            _mutualfundRepositry = mutualfundRepositry;
            _mapper = mapper;
            _userMasterRepository = userMasterRepository;
        }

        #region Import NJ Client File
        public async Task<int> ImportNJClientFile(IFormFile file, bool UpdateIfExist)
        {
            List<AddMutualfundsDto> existUserTransaction = new List<AddMutualfundsDto>();
            List<AddMutualfundsDto> notExistUserTransaction = new List<AddMutualfundsDto>();

            var filePath = System.IO.Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var directoryPath = System.IO.Path.Combine("C:", "juhil", "CRM-Documents");

            if (!(Directory.Exists(directoryPath)))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var localFilePath = System.IO.Path.Combine(directoryPath, file.FileName);

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
            File.Copy(filePath, localFilePath);

            using (var stream = File.Open(localFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataset = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true,
                            ReadHeaderRow = rowReader =>
                            {
                                // Read the header row starting from the fourth row
                                for (var i = 0; i < 3; i++)
                                {
                                    rowReader.Read();
                                }

                            },
                        }
                    });

                    var datatable = dataset.Tables[0];

                    var records = datatable.AsEnumerable().SkipLast(5).ToList().ConvertAll<AddMutualfundsDto>(row =>
                    {
                        var obj = new AddMutualfundsDto();

                        obj.Username = row["Investor"] == DBNull.Value ? null : row["Investor"].ToString();
                        obj.Transactiontype = row["Type"] == DBNull.Value ? null : row["Type"].ToString();
                        obj.Schemename = row["Scheme"] == DBNull.Value ? null : row["Scheme"].ToString();
                        obj.Foliono = row["Folio No/Demat A/C"] == DBNull.Value ? null : row["Folio No/Demat A/C"].ToString();
                        obj.Tradeno = row["Tr. No."] == DBNull.Value ? null : row["Tr. No."].ToString();
                        obj.Date = Convert.ToDateTime(row["Purchase Date"] == DBNull.Value ? null : row["Purchase Date"]);
                        obj.Nav = Convert.ToDouble(row["NAV(₹)"] == DBNull.Value ? null : row["NAV(₹)"]);
                        obj.Noofunit = Convert.ToDecimal(row["Purchase units"] == DBNull.Value ? null : row["Purchase units"]);
                        obj.Invamount = Convert.ToDecimal(row["Inv. Amount(₹)"] == DBNull.Value ? null : row["Inv. Amount(₹)"]);
                        obj.Userpan = row["PAN"] == DBNull.Value ? null : row["PAN"].ToString();

                        var userId = _userMasterRepository.GetUserIdByUserPan(obj.Userpan);
                        var schemeId = _mutualfundRepositry.GetSchemeIdBySchemeName(obj.Schemename);

                        if (userId == 0)
                            obj.Userid = null;
                        else
                            obj.Userid = userId;

                        if (schemeId == 0)
                            obj.SchemeId = null;
                        else
                            obj.SchemeId = schemeId;

                        return obj;
                    });

                    var notExistUser = records.Where(x => x.Userid == null).ToList();
                    notExistUserTransaction.AddRange(notExistUser);

                    var existUser = records.Where(x => x.Userid != null).ToList();
                    existUserTransaction.AddRange(existUser);

                    var mapRecordsForExistUser = _mapper.Map<List<TblMftransaction>>(existUserTransaction);
                    var mapRecordsForNotExistUser = _mapper.Map<List<TblNotexistuserMftransaction>>(notExistUserTransaction);

                    if (UpdateIfExist)
                    {
                        if (mapRecordsForExistUser.Count > 0)
                        {
                            var GetDataForExistUser = await _mutualfundRepositry.GetMFInSpecificDateForExistUser(
                                                        mapRecordsForExistUser.First().Date, mapRecordsForExistUser.First().Date);

                            if (GetDataForExistUser.Count > 0)
                            {
                                foreach (var record in GetDataForExistUser)
                                {
                                    await _mutualfundRepositry.DeleteMFForUserExist(record);
                                }
                            }
                        }

                        if (mapRecordsForNotExistUser.Count > 0)
                        {
                            var GetDataForNotExistUser = await _mutualfundRepositry.GetMFInSpecificDateForNotExistUser(
                                                            mapRecordsForNotExistUser.First().Date, mapRecordsForNotExistUser.Last().Date);

                            if (GetDataForNotExistUser.Count > 0)
                            {
                                foreach (var record in GetDataForNotExistUser)
                                {
                                    await _mutualfundRepositry.DeleteMFForNotUserExist(record);
                                }
                            }
                        }
                    }
                    if (mapRecordsForExistUser.Count > 0)
                    {
                        await _mutualfundRepositry.AddMFDataForExistUser(mapRecordsForExistUser);
                    }
                    await _mutualfundRepositry.AddMFDataForNotExistUser(mapRecordsForNotExistUser);
                }
                return 0;
            }
        }
        #endregion
    }
}
