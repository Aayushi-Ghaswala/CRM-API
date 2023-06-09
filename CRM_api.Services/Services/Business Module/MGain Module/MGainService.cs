using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.IServices.Business_Module.MGain_Module;
using SelectPdf;

namespace CRM_api.Services.Services.Business_Module.MGain_Module
{
    public class MGainService : IMGainService
    {
        private readonly IMGainRepository _mGainRepositery;
        private readonly IMapper _mapper;

        public MGainService(IMGainRepository mGainRepositery, IMapper mapper)
        {
            _mGainRepositery = mGainRepositery;
            _mapper = mapper;
        }

        #region Get All MGain Details
        public async Task<MGainResponseDto<MGainDetailsDto>> GetAllMGainDetailsAsync(int? currancyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams)
        {
            var mGainDetails = await _mGainRepositery.GetMGainDetails(currancyId, type, isClosed, fromDate, toDate, searchingParams, sortingParams);
            var mapMGainDetails = _mapper.Map<MGainResponseDto<MGainDetailsDto>>(mGainDetails);

            foreach (var mGain in mapMGainDetails.response.Values)
            {
                if (mGain.MgainProjectname is not null)
                {
                    var project = await _mGainRepositery.GetProjectByProjectName(mGain.MgainProjectname);
                    var mapProject = _mapper.Map<ProjectMasterDto>(project);
                    mGain.ProjectMaster = mapProject;
                }
            }

            return mapMGainDetails;
        }
        #endregion

        #region Get Payment Details By MGain Id
        public async Task<List<MGainPaymentDto>> GetPaymentByMgainIdAsync(int mGainId)
        {
            var mGainPatyment = await _mGainRepositery.GetPaymentByMGainId(mGainId);
            var mapMGainPayment = _mapper.Map<List<MGainPaymentDto>>(mGainPatyment);
            return mapMGainPayment;
        }
        #endregion

        #region MGain Aggrement HTML
        public async Task<string> MGainAggrementAsync(int mGainId)
        {
            var mGain = await _mGainRepositery.GetMGainDetailById(mGainId);
            var mGainProject = await _mGainRepositery.GetProjectByProjectName(mGain.MgainProjectname);
            var paymentMode = mGain.TblMgainPaymentMethods.First().PaymentMode;
            var currancy = mGain.TblMgainPaymentMethods.First().TblMgainCurrancyMaster.Currancy;

            var filePath = "C:\\juhil\\CRM-Api\\CRM-api\\wwwroot\\MGain Module\\MGainAggrement.html";
            var streamReader = new StreamReader(filePath);
            var aggrement = streamReader.ReadToEnd();

            aggrement = aggrement.Replace("#date", mGain.Date.Value.Day.ToString()).Replace("#month", mGain.Date.Value.ToString("MMMM")).Replace("#year", mGain.Date.Value.Year.ToString())
                                  .Replace("#firstHolderName", mGain.Mgain1stholder).Replace("#address", mGain.Mgain1stholderAddress).Replace("#pinCode", mGain.TblUserMaster.UserPin)
                                  .Replace("#mobile", mGain.Mgain1stholderMobile).Replace("#secondHolderName", mGain.Mgain2ndholdername).Replace("#invAmount", mGain.MgainInvamt.ToString());

            if (mGain.Mgain1stholderAddress == mGain.Mgain2ndholderAddress)
                aggrement = aggrement.Replace("#asabove", "as above");
            aggrement = aggrement.Replace("#asabove", mGain.Mgain2ndholderAddress);

            if (paymentMode == "Cheque")
                aggrement = aggrement.Replace("#PaymentMode", paymentMode).Replace("#PaymentNo", mGain.TblMgainPaymentMethods.Last().ChequeNo)
                                  .Replace("#PaymentDate", mGain.TblMgainPaymentMethods.Last().ChequeDate.Value.ToString("dd-MM-yyyy")).Replace("#bankDetails", mGain.TblMgainPaymentMethods.First().BankName);
            else if (paymentMode == "UPI")
                aggrement = aggrement.Replace("#PaymentMode", paymentMode).Replace("#PaymentNo", mGain.TblMgainPaymentMethods.Last().UpiTransactionNo)
                                  .Replace("#PaymentDate", mGain.TblMgainPaymentMethods.Last().UpiDate.Value.ToString("dd-MM-yyyy")).Replace("#bankDetails", paymentMode);
            else
                aggrement = aggrement.Replace("#PaymentMode", paymentMode).Replace("#PaymentNo", mGain.TblMgainPaymentMethods.Last().ReferenceNo).Replace("#PaymentDate", "")
                                  .Replace("#bankDetails", paymentMode);

            aggrement = aggrement.Replace("#plotNo", mGain.MgainPlotno.ToString()).Replace("#allocateSqFt", mGain.MgainAllocatedsqft.ToString()).Replace("#totalSqFt", mGain.MgainTotalsqft.ToString()).Replace("#projectName", mGain.MgainProjectname)
                                 .Replace("#projectaddress", mGainProject.Address).Replace("#taluka", mGainProject.Taluko).Replace("#city", mGainProject.City).Replace("#state", mGainProject.State)
                                 .Replace("#currancy", currancy).Replace("#first3yearInterest", mGain.TblMgainSchemeMaster.Interst1.ToString())
                                 .Replace("#4to6YearInterest", (mGain.TblMgainSchemeMaster.Interst4 + mGain.TblMgainSchemeMaster.AdditionalInterest4).ToString())
                                 .Replace("#finalInterest", (mGain.TblMgainSchemeMaster.Interst7 + mGain.TblMgainSchemeMaster.AdditionalInterest7).ToString());

            return aggrement;
        }
        #endregion

        #region MGain Payment Receipt
        public async Task<MGainPDFResponseDto> MGainPaymentReceipt(int id)
        {
            var mGain = await _mGainRepositery.GetMGainDetailById(id);
            var mapMGainPaymentReciept = _mapper.Map<MGainPaymentRecieptDto>(mGain);
            mapMGainPaymentReciept.ReleaseDate = mapMGainPaymentReciept.Date.Value.AddYears(10).AddDays(-1);

            var directoryPath = Directory.GetCurrentDirectory() + "\\MGain-Documents\\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var folderPath = Path.Combine(directoryPath) + $"{mGain.Mgain1stholder}\\";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = "MGainReciept" + $"{mGain.Id}" + ".pdf";

            var filePath = folderPath + fileName;

            if (File.Exists(filePath))
            {
                byte[] fileByte = File.ReadAllBytes(filePath);

                var pdfFile = new MGainPDFResponseDto()
                {
                    file = fileByte,
                    FileName = fileName,
                };

                return pdfFile;
            }

            List<decimal?> interestRates = new List<decimal?>();
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst1);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst2);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst3);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst4 + mGain.TblMgainSchemeMaster.AdditionalInterest4);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst5 + mGain.TblMgainSchemeMaster.AdditionalInterest5);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst6 + mGain.TblMgainSchemeMaster.AdditionalInterest6);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst7 + mGain.TblMgainSchemeMaster.AdditionalInterest7);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst8 + mGain.TblMgainSchemeMaster.AdditionalInterest8);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst9 + mGain.TblMgainSchemeMaster.AdditionalInterest9);
            interestRates.Add(mGain.TblMgainSchemeMaster.Interst10 + mGain.TblMgainSchemeMaster.AdditionalInterest10);

            mapMGainPaymentReciept.AverageROI = interestRates.Average();

            var htmlContent = @"<!DOCTYPE html >
  <html>
    <head>
      <style>
        body {
          margin:20px
}
        #main {
        display:flex;
        flex-direction:row;
}
        #main-header1{
          width:40%
}
        #main-header2{
          width:60%
}
        table, td, th {
        border: 1px solid;
        padding:10px;
}

        table {
          width: 100%;
        border-collapse: collapse;
}
      </style>
    </head>";

            htmlContent += $@"
<body style = ""font-size:20px"">
      <div>
        <div id=""main"">
          <div id=""main-header1"">
            <center>
              <img src=""C:\juhil\CRM-Api\CRM-api\wwwroot\MGain Module\KA_Group.png"" width=""250"" height=""250""/>
            </center>
            <p><b> KA FINANCIAL SERVICES LLP </b></p>
            <p>715 - ROYAL TRADE CENTRE,
OPP. STAR BAZAR, ADAJAN,
SURAT - 395009 <p>
            <p><b>Mobile No : </b>7228881196</p>
            <p><b>Email Address : </b>care@kagroup.in</p>
          </div>

          <div id=""main-header2"">
            <table>
              <tr>
                <td style=""width:30%""><b>Date : </b></td>
                <td>{mapMGainPaymentReciept.Date.Value.ToString("dd-MM-yyyy")}</td>
              </tr>
              <tr>
                <td><b>MGain No : </b></td>
                <td>{mapMGainPaymentReciept.Id}</td>
              </tr>
            </table>
            <div style=""margin-left:100px; margin-top:50px;"">
              <p style=""border:1px solid black; padding:5px""> <b>RECIPIENT INFO</b> </p>
              <p><b> {mapMGainPaymentReciept.Mgain1stholder} </b><p>
                <p>{mapMGainPaymentReciept.Mgain1stholderAddress}, </p>
                <div style=""line-height: 1.4;"">
                  <b>Pan No : </b><span>{mapMGainPaymentReciept.Mgain1stholderpan}</span><br>
                    <b>Email Addrss : </b><span>{mapMGainPaymentReciept.Mgain1stholderEmail}</span><br>";

            if (mGain.MgainIsSecondHolder is true)
                htmlContent += @$"<b>Second Holder : </b><span>{mapMGainPaymentReciept.Mgain2ndholdername}</span><br>";
            htmlContent += $@"<b>Second Holder : </b><span><b>NO</b></span><br>";


            htmlContent += @$"<b>Nominee First : </b><span>{mapMGainPaymentReciept.MgainNomineeName}</span><br>
                        </div>
                      </div>
                    </div>
                </div>
                <div style=""margin-top:20px"">
                  <table>
                    <tr>
                      <th>INVESTMENT DATE</th>
                      <th>INVESTMENT AMOUNT</th>
                      <th>MODE</th>
                      <th>CHEQUE / REFERENCE NO</th>
                      <th>RELEASE DATE</th>
                      <th>ROI (AVG)</th>
                      <th>TYPE</th>
                    </tr>";


            foreach (var payment in mGain.TblMgainPaymentMethods)
            {
                if (payment.PaymentMode.ToLower() == MGainPaymentConstant.cheque.ToLower())
                {
                    htmlContent += @$"<tr>
                      <td>{mapMGainPaymentReciept.Date.Value.ToString("dd-MM-yyyy")}</td>
                      <td>{mGain.MgainInvamt}</td>
                      <td>{payment.PaymentMode}</td>
                      <td>{payment.ChequeNo}</td>
                      <td>{mapMGainPaymentReciept.ReleaseDate.ToString("dd-MM-yyyy")}</td>
                      <td>{mapMGainPaymentReciept.AverageROI}</td>
                      <td>{mapMGainPaymentReciept.MgainType}</td>
                    </tr>";
                }
                else if (payment.PaymentMode.ToLower() == MGainPaymentConstant.rtgs.ToLower())
                {
                    htmlContent += @$"<tr>
                      <td>{mapMGainPaymentReciept.Date.Value.ToString("dd-MM-yyyy")}</td>
                      <td>{mGain.MgainInvamt}</td>
                      <td>{payment.PaymentMode}</td>
                      <td>{payment.ReferenceNo}</td>
                      <td>{mapMGainPaymentReciept.ReleaseDate.ToString("dd-MM-yyyy")}</td>
                      <td>{mapMGainPaymentReciept.AverageROI}</td>
                      <td>{mapMGainPaymentReciept.MgainType}</td>
                    </tr>";
                }
                else
                {
                    htmlContent += @$"<tr>
                      <td>{mapMGainPaymentReciept.Date.Value.ToString("dd-MM-yyyy")}</td>
                      <td>{mGain.MgainInvamt}</td>
                      <td>{payment.PaymentMode}</td>
                      <td>{payment.UpiTransactionNo}</td>
                      <td>{mapMGainPaymentReciept.ReleaseDate.ToString("dd-MM-yyyy")}</td>
                      <td>{mapMGainPaymentReciept.AverageROI}</td>
                      <td>{mapMGainPaymentReciept.MgainType}</td>
                    </tr>";
                }
            }

            htmlContent += @$"</table>
                </div>
                <p style=""padding-top:30px; padding-bottom:30px; text-align:center;""><b>We Acknowledge Hereby The Receipt Of Rs. {mapMGainPaymentReciept.MgainInvamt} On {mapMGainPaymentReciept.Date.Value.ToString("dd-MM-yyyy")} For M-Gain By Way Of {mapMGainPaymentReciept.TblMgainPaymentMethods.First().PaymentMode}.</b></p>

                <div>
                  <p><b>Note :- </b></p>
                  <ul>
                    <li>Interset Rate
                    	<ul>
                        	<li>Upto 3 Years - {mapMGainPaymentReciept.TblMgainSchemeMaster.Interst1}%</li>
                            <li>4 To 6 Years - {interestRates.Skip(3).Take(3).Average()}%</li>
                            <li>7 To 10 Years - {interestRates.Skip(6).Take(4).Average()}%</li>
                        </ul>
                    </li>
                    <li>anytime Withdrawal after 3 years</li>
                    <li>Senior Citizen get extra 1% on Interest Rate</li>
                  </ul>
                </div>
                <div style=""text-align:right;"">
                	<p><b>For, KA FINANCIAL SERVICES LLP</b></p>
                </div>
                <div style=""margin-top:60px"">
                <p style=""text-align:right; margin-right:60px;""><span style=""border-top: 2px solid black"">Authorised Signature</span></p>
                </div>
              </div>
              </body>
            </html>";

            PdfDocument document = new PdfDocument();

            // Create a new HTML to PDF converter
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MarginTop = 50;
            converter.Options.MarginLeft = 30;
            converter.Options.MarginRight = 30;

            // Convert the HTML string to PDF
            PdfDocument result = converter.ConvertHtmlString(htmlContent);

            // Save the PDF document to a memory stream
            MemoryStream stream = new MemoryStream();
            result.Save(stream);
            stream.Position = 0;

            FileStream stream1 = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
            stream.CopyTo(stream1);

            var pdfResponse = new MGainPDFResponseDto()
            {
                file = stream.ToArray(),
                FileName = fileName
            };

            return pdfResponse;
        }
        #endregion

        #region Get All Projects
        public async Task<ResponseDto<ProjectMasterDto>> GetAllProjectAsync(string? searchingParams, SortingParams sortingParams)
        {
            var projects = await _mGainRepositery.GetAllProject(searchingParams, sortingParams);
            var mapProjects = _mapper.Map<ResponseDto<ProjectMasterDto>>(projects);
            return mapProjects;
        }
        #endregion

        #region Get Plots By ProjectId
        public async Task<ResponseDto<PlotMasterDto>> GetPlotsByProjectIdAsync(int projectId, decimal invAmount, string? searchingParams, SortingParams sortingParams)
        {
            var plots = await _mGainRepositery.GetPlotsByProjectId(projectId, invAmount, searchingParams, sortingParams);
            var mapPlots = _mapper.Map<ResponseDto<PlotMasterDto>>(plots);
            return mapPlots;
        }
        #endregion

        #region Get All Currency
        public async Task<List<MGainCurrancyDto>> GetAllCurrenciesAsync()
        {
            var currancy = await _mGainRepositery.GetAllCurrencies();
            var mapCurrency = _mapper.Map<List<MGainCurrancyDto>>(currancy);
            return mapCurrency;
        }
        #endregion

        #region Add MGain Details
        public async Task<TblMgaindetail> AddMGainDetailAsync(AddMGainDetailsDto addMGainDetails)
        {
            var mGainDetails = _mapper.Map<TblMgaindetail>(addMGainDetails);

            var directoryPath = Directory.GetCurrentDirectory() + "\\MGain-Documents\\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var folderPath = directoryPath + $"{mGainDetails.Mgain1stholder}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (addMGainDetails.Mgain1stholderSignature is not null)
            {
                var firstHolderSignature = addMGainDetails.Mgain1stholderSignature.FileName;
                var firstHolderSignaturePath = Path.Combine(folderPath, firstHolderSignature);

                if (File.Exists(firstHolderSignaturePath))
                {
                    File.Delete(firstHolderSignaturePath);
                }

                using (var fs = new FileStream(firstHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.Mgain1stholderSignature.CopyTo(fs);
                }
                mGainDetails.Mgain1stholderSignature = firstHolderSignaturePath;
            }
            else mGainDetails.Mgain1stholderSignature = null;


            if (addMGainDetails.Mgain2ndholderSignature is not null)
            {
                var secondHolderSignature = addMGainDetails.Mgain2ndholderSignature.FileName;
                var secondHolderSignaturePath = Path.Combine(folderPath, secondHolderSignature);

                if (File.Exists(secondHolderSignaturePath))
                {
                    File.Delete(secondHolderSignaturePath);
                }

                using (var fs = new FileStream(secondHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.Mgain2ndholderSignature.CopyTo(fs);
                }
                mGainDetails.Mgain2ndholderSignature = secondHolderSignaturePath;
            }
            else mGainDetails.Mgain2ndholderSignature = null;

            if (addMGainDetails.MgainNomineePan is not null)
            {
                var nomineePan = addMGainDetails.MgainNomineePan.FileName;
                var nomineePanPath = Path.Combine(folderPath, nomineePan);

                if (File.Exists(nomineePanPath))
                {
                    File.Delete(nomineePanPath);
                }

                using (var fs = new FileStream(nomineePanPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.MgainNomineePan.CopyTo(fs);
                }

                mGainDetails.MgainNomineePan = nomineePanPath;
            }
            else mGainDetails.MgainNomineePan = null;

            if (addMGainDetails.MgainNomineeAadhar is not null)
            {
                var nomineeAadhar = addMGainDetails.MgainNomineeAadhar.FileName;
                var nomineeAadharPath = Path.Combine(folderPath, nomineeAadhar);

                if (File.Exists(nomineeAadharPath))
                {
                    File.Delete(nomineeAadharPath);
                }

                using (var fs = new FileStream(nomineeAadharPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.MgainNomineeAadhar.CopyTo(fs);
                }
                mGainDetails.MgainNomineeAadhar = nomineeAadharPath;
            }
            else mGainDetails.MgainNomineeAadhar = null;

            if (addMGainDetails.MgainNomineeBirthCertificate is not null)
            {
                var birthCertificate = addMGainDetails.MgainNomineeBirthCertificate.FileName;
                var birthCertificatePath = Path.Combine(folderPath, birthCertificate);

                if (File.Exists(birthCertificatePath))
                {
                    File.Delete(birthCertificatePath);
                }

                using (var fs = new FileStream(birthCertificatePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.MgainNomineeBirthCertificate.CopyTo(fs);
                }
                mGainDetails.MgainNomineeBirthCertificate = birthCertificatePath;
            }
            else mGainDetails.MgainNomineeBirthCertificate = null;

            var mGain = await _mGainRepositery.AddMGainDetails(mGainDetails);
            return mGain;
        }
        #endregion

        #region Add Payment Details
        public async Task<int> AddPaymentDetailsAsync(List<AddMGainPaymentDto> paymentDtos)
        {
            var mGainPayments = _mapper.Map<List<TblMgainPaymentMethod>>(paymentDtos);
            return await _mGainRepositery.AddPaymentDetails(mGainPayments);
        }
        #endregion

        #region MGain Aggrement PDF
        public async Task<MGainPDFResponseDto> GenerateMGainAggrementAsync(int id, string htmlContent)
        {
            var MGain = await _mGainRepositery.GetMGainDetailById(id);

            var directoryPath = Directory.GetCurrentDirectory() + "\\MGain-Documents\\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var folderPath = Path.Combine(directoryPath) + $"{MGain.Mgain1stholder}\\";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{MGain.Mgain1stholder}" + " aggrement " + $"{MGain.Date.Value.ToString("dd-MM-yyyy")}" + ".pdf";

            var filePath = folderPath + fileName;

            if (File.Exists(filePath))
            {
                byte[] fileByte = File.ReadAllBytes(filePath);

                var pdfFile = new MGainPDFResponseDto()
                {
                    file = fileByte,
                    FileName = fileName,
                };

                return pdfFile;
            }

            PdfDocument document = new PdfDocument();

            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MarginTop = 100;
            converter.Options.MarginBottom = 129;
            converter.Options.MarginLeft = 60;
            converter.Options.MarginRight = 60;

            PdfDocument result = converter.ConvertHtmlString(htmlContent);

            MemoryStream stream = new MemoryStream();
            result.Save(stream);

            stream.Position = 0;

            FileStream stream1 = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
            stream.CopyTo(stream1);

            var pdfResponse = new MGainPDFResponseDto()
            {
                file = stream.ToArray(),
                FileName = fileName
            };

            return pdfResponse;
        }
        #endregion

        #region Update MGain Details
        public async Task<int> UpdateMGainDetailsAsync(UpdateMGainDetailsDto updateMGainDetails)
        {
            var updateMGain = _mapper.Map<TblMgaindetail>(updateMGainDetails);
            var mgain = await _mGainRepositery.GetMGainDetailById(updateMGain.Id);
            var plot = await _mGainRepositery.GetPlotById(updateMGainDetails.PlotId);
            var mGainPlot = await _mGainRepositery.GetPlotByProjectAndPlotNo(mgain.MgainTotalsqft, mgain.MgainPlotno);

            var directoryPath = Directory.GetCurrentDirectory() + "\\MGain-Documents\\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var folderPath = directoryPath + $"{updateMGainDetails.Mgain1stholder}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (updateMGainDetails.Mgain1stholderSignature is not null)
            {
                if (File.Exists(mgain.Mgain1stholderSignature))
                {
                    File.Delete(mgain.Mgain1stholderSignature);
                }

                var firstHolderSignature = updateMGainDetails.Mgain1stholderSignature.FileName;
                var firstHolderSignaturePath = Path.Combine(folderPath, firstHolderSignature);
                using (var fs = new FileStream(firstHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.Mgain1stholderSignature.CopyTo(fs);
                }
                updateMGain.Mgain1stholderSignature = firstHolderSignaturePath;
            }
            else updateMGain.Mgain1stholderSignature = null;


            if (updateMGainDetails.Mgain2ndholderSignature is not null)
            {
                if (File.Exists(mgain.Mgain2ndholderSignature))
                {
                    File.Delete(mgain.Mgain2ndholderSignature);
                }

                var secondHolderSignature = updateMGainDetails.Mgain2ndholderSignature.FileName;
                var secondHolderSignaturePath = Path.Combine(folderPath, secondHolderSignature);
                using (var fs = new FileStream(secondHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.Mgain2ndholderSignature.CopyTo(fs);
                }
                updateMGain.Mgain2ndholderSignature = secondHolderSignaturePath;
            }
            else updateMGain.Mgain2ndholderSignature = null;

            if (updateMGainDetails.MgainNomineePan is not null)
            {
                if (File.Exists(mgain.MgainNomineePan))
                {
                    File.Delete(mgain.MgainNomineePan);
                }

                var nomineePan = updateMGainDetails.MgainNomineePan.FileName;
                var nomineePanPath = Path.Combine(folderPath, nomineePan);
                using (var fs = new FileStream(nomineePanPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainNomineePan.CopyTo(fs);
                }

                updateMGain.MgainNomineePan = nomineePanPath;
            }
            else updateMGain.MgainNomineePan = null;

            if (updateMGainDetails.MgainNomineeAadhar is not null)
            {
                if (File.Exists(mgain.MgainNomineeAadhar))
                {
                    File.Delete(mgain.MgainNomineeAadhar);
                }

                var nomineeAadhar = updateMGainDetails.MgainNomineeAadhar.FileName;
                var nomineeAadharPath = Path.Combine(folderPath, nomineeAadhar);
                using (var fs = new FileStream(nomineeAadharPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainNomineeAadhar.CopyTo(fs);
                }
                updateMGain.MgainNomineeAadhar = nomineeAadharPath;
            }
            else updateMGain.MgainNomineeAadhar = null;

            if (updateMGainDetails.MgainNomineeBirthCertificate is not null)
            {
                if (File.Exists(mgain.MgainNomineeBirthCertificate))
                {
                    File.Delete(mgain.MgainNomineeBirthCertificate);
                }

                var birthCertificate = updateMGainDetails.MgainNomineeBirthCertificate.FileName;
                var birthCertificatePath = Path.Combine(folderPath, birthCertificate);
                using (var fs = new FileStream(birthCertificatePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainNomineeBirthCertificate.CopyTo(fs);
                }
                updateMGain.MgainNomineeBirthCertificate = birthCertificatePath;
            }
            else updateMGain.MgainNomineeBirthCertificate = null;

            if (updateMGainDetails.MgainCancelledCheque is not null)
            {
                if (File.Exists(mgain.MgainCancelledCheque))
                {
                    File.Delete(mgain.MgainCancelledCheque);
                }

                var cancelledCheque = updateMGainDetails.MgainCancelledCheque.FileName;
                var cancelledChequePath = Path.Combine(folderPath, cancelledCheque);
                using (var fs = new FileStream(cancelledChequePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainCancelledCheque.CopyTo(fs);
                }
                updateMGain.MgainCancelledCheque = cancelledChequePath;
            }
            else updateMGain.MgainCancelledCheque = null;


            if (updateMGainDetails.MgainProjectname == mgain.MgainProjectname && plot.PlotNo == mgain.MgainPlotno)
            {
                if (updateMGain.MgainInvamt != mgain.MgainInvamt)
                {
                    plot.Available_SqFt = Math.Round((decimal)(plot.Available_SqFt + mgain.MgainAllocatedsqft), 4);
                    plot.Available_PlotValue = Math.Round((decimal)(plot.Available_PlotValue + mgain.MgainAllocatedsqftamt), 4);

                    updateMGain.MgainPlotno = plot.PlotNo;
                    updateMGain.MgainAllocatedsqft = Math.Round((decimal)((updateMGain.MgainInvamt * plot.SqFt) / (plot.SqFt * plot.Rate)), 4);
                    updateMGain.MgainAllocatedsqftamt = Math.Round((decimal)(updateMGain.MgainAllocatedsqft * plot.Rate), 4);
                    updateMGain.MgainTotalsqft = Math.Round((decimal)plot.SqFt, 4);
                    updateMGain.MgainTotalplotamt = Math.Round((decimal)(plot.SqFt * plot.Rate), 3);

                    plot.Available_SqFt = Math.Round((decimal)(plot.Available_SqFt - updateMGain.MgainAllocatedsqft), 4);
                    plot.Available_PlotValue = Math.Round((decimal)(plot.Available_PlotValue - updateMGain.MgainAllocatedsqftamt), 4);
                }
                else if (updateMGain.MgainRedemamt > 0)
                {
                    if (updateMGain.MgainRedemamt < updateMGain.MgainInvamt)
                    {
                        plot.Available_SqFt = Math.Round((decimal)(plot.Available_SqFt + mgain.MgainAllocatedsqft), 4);
                        plot.Available_PlotValue = Math.Round((decimal)(plot.Available_PlotValue + mgain.MgainAllocatedsqftamt), 4);

                        var availableAmount = updateMGain.MgainInvamt - updateMGain.MgainRedemamt;

                        updateMGain.MgainPlotno = plot.PlotNo;
                        updateMGain.MgainAllocatedsqft = Math.Round((decimal)((availableAmount * plot.SqFt) / (plot.SqFt * plot.Rate)), 4);
                        updateMGain.MgainAllocatedsqftamt = Math.Round((decimal)(updateMGain.MgainAllocatedsqft * plot.Rate), 4);
                        updateMGain.MgainTotalsqft = Math.Round((decimal)plot.SqFt, 4);
                        updateMGain.MgainTotalplotamt = Math.Round((decimal)(plot.SqFt * plot.Rate), 3);

                        plot.Available_SqFt = Math.Round((decimal)(plot.Available_SqFt - updateMGain.MgainAllocatedsqft), 4);
                        plot.Available_PlotValue = Math.Round((decimal)(plot.Available_PlotValue - updateMGain.MgainAllocatedsqftamt), 4);
                    }
                }
            }
            else
            {
                if (mgain.MgainPlotno is not null && mgain.MgainProjectname is not null)
                {
                    mGainPlot.Available_SqFt = Math.Round((decimal)(mGainPlot.Available_SqFt + mgain.MgainAllocatedsqft), 4);
                    mGainPlot.Available_PlotValue = Math.Round((decimal)(mGainPlot.Available_PlotValue + mgain.MgainAllocatedsqftamt), 4);

                    updateMGain.MgainPlotno = plot.PlotNo;
                    updateMGain.MgainAllocatedsqft = Math.Round((decimal)((updateMGain.MgainInvamt * plot.SqFt) / (plot.SqFt * plot.Rate)), 4);
                    updateMGain.MgainAllocatedsqftamt = Math.Round((decimal)(updateMGain.MgainAllocatedsqft * plot.Rate), 4);
                    updateMGain.MgainTotalsqft = Math.Round((decimal)plot.SqFt, 4);
                    updateMGain.MgainTotalplotamt = Math.Round((decimal)(plot.SqFt * plot.Rate), 3);

                    await _mGainRepositery.UpdatePlotDetails(mGainPlot);
                }
                else
                {
                    updateMGain.MgainPlotno = plot.PlotNo;
                    updateMGain.MgainAllocatedsqft = Math.Round((decimal)((updateMGain.MgainInvamt * plot.SqFt) / (plot.SqFt * plot.Rate)), 4);
                    updateMGain.MgainAllocatedsqftamt = Math.Round((decimal)(updateMGain.MgainAllocatedsqft * plot.Rate), 4);
                    updateMGain.MgainTotalsqft = Math.Round((decimal)plot.SqFt, 4);
                    updateMGain.MgainTotalplotamt = Math.Round((decimal)(plot.SqFt * plot.Rate), 3);
                }

                plot.Available_SqFt = Math.Round((decimal)(plot.Available_SqFt - updateMGain.MgainAllocatedsqft), 4);
                plot.Available_PlotValue = Math.Round((decimal)(plot.Available_PlotValue - updateMGain.MgainAllocatedsqftamt), 4);
            }

            if (updateMGain.MgainIsclosed is true)
            {
                updateMGain.MgainPlotno = mgain.MgainPlotno;
                updateMGain.MgainAllocatedsqft = mgain.MgainAllocatedsqft;
                updateMGain.MgainAllocatedsqftamt = mgain.MgainAllocatedsqftamt;
                updateMGain.MgainTotalsqft = mgain.MgainTotalsqft;
                updateMGain.MgainTotalplotamt = mgain.MgainTotalplotamt;

                plot.Available_SqFt = Math.Round((decimal)(plot.Available_SqFt + mgain.MgainAllocatedsqft), 4);
                plot.Available_PlotValue = Math.Round((decimal)(plot.Available_PlotValue + mgain.MgainAllocatedsqftamt), 4);
            }

            await _mGainRepositery.UpdatePlotDetails(plot);
            return await _mGainRepositery.UpdateMGainDetails(updateMGain);
        }
        #endregion  

        #region Update MGain Payment Details
        public async Task<int> UpdateMGainPaymentAsync(List<UpdateMGainPaymentDto> updateMGainPayment)
        {
            foreach (var mGainDetails in updateMGainPayment)
            {
                var mapMGainPayment = _mapper.Map<TblMgainPaymentMethod>(mGainDetails);
                await _mGainRepositery.UpdateMGainPayment(mapMGainPayment);
            }
            return 1;
        }
        #endregion

        #region Delete MGain Payment Details
        public async Task<int> DeleteMGainPaymentAsync(int id)
        {
            var mGainPayment = await _mGainRepositery.GetPaymentById(id);
            return await _mGainRepositery.DeleteMGainPayment(mGainPayment);
        }
        #endregion
    }
}
