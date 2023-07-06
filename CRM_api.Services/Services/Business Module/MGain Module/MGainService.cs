using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.Business_Module.MGain_Module;
using SelectPdf;
using static CRM_api.Services.Helper.ConstantValue.GenderConstant;
using static CRM_api.Services.Helper.ConstantValue.MaritalStatusConstant;
using static CRM_api.Services.Helper.ConstantValue.MGainAccountPaymentConstant;

namespace CRM_api.Services.Services.Business_Module.MGain_Module
{
    public class MGainService : IMGainService
    {
        private readonly IMGainRepository _mGainRepository;
        private readonly IMGainSchemeRepository _mGainSchemeRepository;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IMapper _mapper;

        public MGainService(IMGainRepository mGainRepository, IMapper mapper, IMGainSchemeRepository mGainSchemeRepository, IUserMasterRepository userMasterRepository)
        {
            _mGainRepository = mGainRepository;
            _mapper = mapper;
            _mGainSchemeRepository = mGainSchemeRepository;
            _userMasterRepository = userMasterRepository;
        }

        #region Get All MGain Details
        public async Task<MGainResponseDto<MGainDetailsDto>> GetAllMGainDetailsAsync(int? currencyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams)
        {
            var mGainDetails = await _mGainRepository.GetMGainDetails(currencyId, type, isClosed, fromDate, toDate, searchingParams, sortingParams);
            var mapMGainDetails = _mapper.Map<MGainResponseDto<MGainDetailsDto>>(mGainDetails);

            foreach (var mGain in mapMGainDetails.response.Values)
            {
                if (mGain.MgainProjectname is not null && mGain.MgainPlotno is not null)
                {
                    //Find project
                    var project = await _mGainRepository.GetProjectByProjectName(mGain.MgainProjectname);
                    var mapProject = _mapper.Map<ProjectMasterDto>(project);
                    mGain.ProjectMaster = mapProject;

                    //Find plot
                    var plot = await _mGainRepository.GetPlotByProjectAndPlotNo(mGain.MgainProjectname, mGain.MgainPlotno);
                    var mapPlot = _mapper.Map<PlotMasterDto>(plot);
                    mGain.PlotMaster = mapPlot;
                }

                if (mGain.Mgain2ndprojectname is not null && mGain.Mgain2ndplotno is not null)
                {
                    //Find project
                    var project = await _mGainRepository.GetProjectByProjectName(mGain.Mgain2ndprojectname);
                    var mapProject = _mapper.Map<ProjectMasterDto>(project);
                    mGain.SecondProjectMaster = mapProject;

                    //Find plot
                    var plot = await _mGainRepository.GetPlotByProjectAndPlotNo(mGain.Mgain2ndprojectname, mGain.Mgain2ndplotno);
                    var mapPlot = _mapper.Map<PlotMasterDto>(plot);
                    mGain.SecondPlotMaster = mapPlot;
                }

                mGain.Tenure = 10;
            }

            return mapMGainDetails;
        }
        #endregion

        #region Get Payment Details By MGain Id
        public async Task<List<MGainPaymentDto>> GetPaymentByMgainIdAsync(int mGainId)
        {
            var mGainPatyment = await _mGainRepository.GetPaymentByMGainId(mGainId);
            var mapMGainPayment = _mapper.Map<List<MGainPaymentDto>>(mGainPatyment);
            return mapMGainPayment;
        }
        #endregion

        #region MGain Aggrement HTML
        public async Task<string> MGainAggrementAsync(int mGainId)
        {
            var mGain = await _mGainRepository.GetMGainDetailById(mGainId);
            var mGainProject = await _mGainRepository.GetProjectByProjectName(mGain.MgainProjectname);
            var paymentMode = mGain.TblMgainPaymentMethods.First().PaymentMode;
            var currancy = mGain.TblMgainPaymentMethods.First().TblMgainCurrancyMaster.Currancy;

            var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\MGain Module\\MGainAggrement.html";
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
            try
            {
                var mGain = await _mGainRepository.GetMGainDetailById(id);
                var mapMGainPaymentReciept = _mapper.Map<MGainPaymentRecieptDto>(mGain);
                mapMGainPaymentReciept.ReleaseDate = mapMGainPaymentReciept.Date.Value.AddYears(10).AddDays(-1);

                var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\MGain-Documents\\";

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
                    File.Delete(filePath);
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
              <img src='{Directory.GetCurrentDirectory()}\wwwroot\MGain Module\KA_Group.png' width=""250"" height=""250""/>
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
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Get MGain Cumulative Interest Computation
        public async Task<MGainCumulativeReportDto> GetMgGainCumulativeInterestReportAsync(int fromYear, int toYear, int? schemeId, string? search, SortingParams sortingParams)
        {
            var mgainDetails = await _mGainRepository.GetMGainCumulativeDetails(fromYear, toYear, schemeId, search, sortingParams, MGainTypeConstant.cumulative);
            var cumulativeInterests = new List<MgainCumulativeInterestDto>();
            var cumulativeReport = new MGainCumulativeReportDto();

            if (mgainDetails is not null)
            {
                foreach (var mgainDetail in mgainDetails)
                {
                    var cumulativeInterest = new MgainCumulativeInterestDto();
                    cumulativeInterest.Date = mgainDetail.Date.Value;
                    cumulativeInterest.Id = mgainDetail.Id;
                    cumulativeInterest.Mgain1stholder = mgainDetail.Mgain1stholder;
                    cumulativeInterest.MgainSchemename = mgainDetail.TblMgainSchemeMaster.Schemename;
                    cumulativeInterest.InvestmentAmount = (decimal)mgainDetail.MgainInvamt;
                    cumulativeInterest.FinalAmount = (decimal)mgainDetail.MgainInvamt;

                    var mgainDate = mgainDetail.Date.Value;
                    var yearDifference = (toYear - mgainDate.Year) - 1;

                    if (yearDifference >= 0)
                    {
                        List<decimal?> interestRates = new List<decimal?>()
                        {
                            mgainDetail.TblMgainSchemeMaster.Interst1,
                            mgainDetail.TblMgainSchemeMaster.Interst2,
                            mgainDetail.TblMgainSchemeMaster.Interst3,
                            mgainDetail.TblMgainSchemeMaster.Interst4 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest4,
                            mgainDetail.TblMgainSchemeMaster.Interst5 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest5,
                            mgainDetail.TblMgainSchemeMaster.Interst6 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest6,
                            mgainDetail.TblMgainSchemeMaster.Interst7 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest7,
                            mgainDetail.TblMgainSchemeMaster.Interst8 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest8,
                            mgainDetail.TblMgainSchemeMaster.Interst9 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest9,
                            mgainDetail.TblMgainSchemeMaster.Interst10 + mgainDetail.TblMgainSchemeMaster.AdditionalInterest10
                        };

                        for (var i = 0; i <= yearDifference; i++)
                        {
                            if (i <= 9)
                            {
                                var interestRate = interestRates[i];

                                if (i is 0)
                                {
                                    var monthDifference = (12 - mgainDate.AddYears(1).Month) + 3;
                                    decimal? firstYearTotalInterest = Math.Round((decimal)(cumulativeInterest.FinalAmount * (interestRate / 100)));

                                    for (var j = 0; j <= monthDifference; j++)
                                    {
                                        if (j == 0)
                                        {
                                            var daysInMonth = DateTime.DaysInMonth(mgainDetail.Date.Value.Year, mgainDetail.Date.Value.Month);
                                            var days = daysInMonth - (mgainDate.Day - 1);
                                            cumulativeInterest.InterestForPeriod += Math.Round((decimal)(cumulativeInterest.FinalAmount * days * (interestRate / 100)) / 365);
                                        }
                                        else cumulativeInterest.InterestForPeriod += Math.Round((decimal)firstYearTotalInterest / 12);
                                    }
                                    cumulativeInterest.FinalAmount = cumulativeInterest.InvestmentAmount + cumulativeInterest.InterestForPeriod;
                                }
                                else if (i == 9)
                                {
                                    if ((mgainDetail.Date.Value.Month) < 4)
                                    {
                                        cumulativeInterest.InterestForPeriod += Math.Round((decimal)(cumulativeInterest.FinalAmount * (9 + mgainDetail.Date.Value.Month) * (interestRate / 100)) / 12);
                                        cumulativeInterest.FinalAmount = cumulativeInterest.InvestmentAmount + cumulativeInterest.InterestForPeriod;
                                    }
                                    else
                                    {
                                        cumulativeInterest.InterestForPeriod += Math.Round((decimal)(cumulativeInterest.FinalAmount * (mgainDetail.Date.Value.Month - 4) * (interestRate / 100)) / 12);
                                        cumulativeInterest.FinalAmount = cumulativeInterest.InvestmentAmount + cumulativeInterest.InterestForPeriod;
                                    }
                                }
                                else
                                {
                                    cumulativeInterest.InterestForPeriod += Math.Round((decimal)(cumulativeInterest.FinalAmount * (interestRate / 100)));
                                    cumulativeInterest.FinalAmount = cumulativeInterest.InvestmentAmount + cumulativeInterest.InterestForPeriod;
                                }
                            }
                        }

                        cumulativeInterests.Add(cumulativeInterest);
                    }
                }

                cumulativeReport.CumulativeInterests = cumulativeInterests;
                cumulativeReport.TotalInterestForPeriod = Math.Round((decimal)cumulativeInterests.Sum(x => x.InterestForPeriod));
                cumulativeReport.TotalFinalAmount = Math.Round((decimal)cumulativeInterests.Sum(x => x.FinalAmount));
            }

            return cumulativeReport;
        }
        #endregion

        #region Get MGain 10 Years Interest Details
        public async Task<MGainTenYearReportDto> GetMGain10YearsInterestDetailsAsync(string userName, int schemeId, DateTime invDate, decimal mGainAmount, string mGainType)
        {
            var mgainScheme = await _mGainSchemeRepository.GetMGainSchemeById(schemeId);
            var mgainAmount = mGainAmount;
            var daysInMonth = DateTime.DaysInMonth(invDate.Year, invDate.Month);
            var days = daysInMonth - (invDate.Day - 1);

            var date = invDate;
            var startDate = invDate.AddYears(1);
            List<string> months = new List<string>();
            while (date != startDate)
            {
                months.Add(date.ToString("MMMM"));
                date = date.AddMonths(1);
                if (date.Month == startDate.Month && date.Year == startDate.Year)
                    break;

            }

            List<decimal?> interestRates = new List<decimal?>();
            interestRates.Add(mgainScheme.Interst1);
            interestRates.Add(mgainScheme.Interst2);
            interestRates.Add(mgainScheme.Interst3);
            interestRates.Add(mgainScheme.Interst4 + mgainScheme.AdditionalInterest4);
            interestRates.Add(mgainScheme.Interst5 + mgainScheme.AdditionalInterest5);
            interestRates.Add(mgainScheme.Interst6 + mgainScheme.AdditionalInterest6);
            interestRates.Add(mgainScheme.Interst7 + mgainScheme.AdditionalInterest7);
            interestRates.Add(mgainScheme.Interst8 + mgainScheme.AdditionalInterest8);
            interestRates.Add(mgainScheme.Interst9 + mgainScheme.AdditionalInterest9);
            interestRates.Add(mgainScheme.Interst10 + mgainScheme.AdditionalInterest10);

            var yearlyInterests = new List<MonthDetailDto>();
            bool flag = true;
            if (mGainType is MGainTypeConstant.nonCumulative)
            {
                interestRates.ForEach(interestRate =>
                {
                    if (flag)
                    {
                        var firstYear = CalculateFirstYearInterestAsync(mgainAmount, interestRate, days);
                        yearlyInterests.Add(firstYear);
                        flag = false;
                    }
                    else
                    {
                        var yearlyInterest = CalculateYearlyInterestAsync(mgainAmount, interestRate);
                        yearlyInterests.Add(yearlyInterest);
                    }
                });
            }
            else
            {
                interestRates.ForEach(interestRate =>
                {
                    if (flag)
                    {
                        var firstYear = CalculateCumulativeFirstYearInterestAsync(mgainAmount, interestRate, days);
                        yearlyInterests.Add(firstYear);
                        mgainAmount += firstYear.TotalInterest;
                        flag = false;
                    }
                    else
                    {
                        var yearlyInterest = CalculateYearlyInterestAsync(mgainAmount, interestRate);
                        yearlyInterests.Add(yearlyInterest);
                        mgainAmount += yearlyInterest.TotalInterest;
                    }
                });
            }

            var interestCalculation = new MGainTenYearReportDto();
            interestCalculation.UserName = userName;
            interestCalculation.Months = months;
            interestCalculation.YearlyInterests = yearlyInterests;
            interestCalculation.InvDate = invDate;
            interestCalculation.MGainAmount = mGainAmount;
            interestCalculation.InterestRates = interestRates;
            interestCalculation.MGainType = mGainType;
            interestCalculation.MGainScheme = mgainScheme.Schemename;
            interestCalculation.TenYearTotalInterest = yearlyInterests.Sum(x => x.TotalInterest);
            interestCalculation.TotalReturn = interestCalculation.MGainAmount + interestCalculation.TenYearTotalInterest;
            interestCalculation.TotalReturnRecievedOn = invDate.AddYears(10).Date;

            return interestCalculation;
        }
        #endregion

        #region Get MGain Interest Certificate
        public async Task<InterestCertificateDto> GetMGainIntertestCertificateAsync(int userId, int year)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(userId);
            var mgainAccDetails = await _mGainRepository.GetMGainAccTransactionByUserId(userId, year);
            if (mgainAccDetails.Count > 0)
            {
                var mgain = await _mGainRepository.GetMGainDetailById((int)mgainAccDetails.Select(x => x.Mgainid).First());

                var interestCertificate = new InterestCertificateDto();
                if (mgain.Mgain1stholderGender == Gender.Male.ToString())
                    interestCertificate.UserName = "Mr. " + user.UserName;
                else if (mgain.Mgain1stholderGender == Gender.Female.ToString() && mgain.Mgain1stholderMaritalstatus == MaritalStatus.Married.ToString())
                    interestCertificate.UserName = "Mrs. " + user.UserName;
                else if (mgain.Mgain1stholderGender == Gender.Female.ToString() && mgain.Mgain1stholderMaritalstatus == MaritalStatus.Unmarried.ToString())
                    interestCertificate.UserName = "Ms. " + user.UserName;
                else
                    interestCertificate.UserName = user.UserName;
                interestCertificate.UserId = userId;
                var months = new List<string>()
            {
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "Nevember",
                "December",
                "January",
                "February",
                "March"
            };

                var interestReports = new List<InterestReportDto>();
                foreach (var month in months)
                {
                    var prevMonth = "";
                    if (month == "April")
                        prevMonth = "March";
                    else
                    {
                        var index = months.FindIndex(x => x == month);
                        prevMonth = months[index - 1];
                    }
                    var details = mgainAccDetails.Where(x => x.DocType == MGainPayment.Payment.ToString() && x.DocDate.Value.ToString("MMMM") == month && x.Debit != 0).ToList();
                    if (details.Count > 0)
                    {
                        foreach (var detail in details)
                        {
                            var interestReport = new InterestReportDto();
                            interestReport.DepositeCode = detail.Id;
                            interestReport.Date = detail.DocDate;
                            interestReport.InterestPaid = detail.Debit;
                            interestReport.InterestAccrued = 0;
                            var tax = mgainAccDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                        && x.DocParticulars == "TDS " + (year - 1) + "-" + year.ToString().Substring(year.ToString().Length - 2) && x.Debit != 0).FirstOrDefault();
                            interestReport.TaxDeducted = 0;
                            if (tax is not null)
                                interestReport.TaxDeducted = tax.Debit;
                            interestReport.ShcemeName = mgain.TblMgainSchemeMaster.Schemename;
                            interestReport.ACDescription = mgain.MgainType;
                            interestReport.Currency = "₹";
                            interestReport.OverheadTaxDeducted = "N/A";
                            interestReports.Add(interestReport);
                        }
                        if (month == "March")
                        {
                            foreach (var detail in details)
                            {
                                var interestMarchReport = new InterestReportDto();
                                interestMarchReport.DepositeCode = detail.Id;
                                interestMarchReport.InterestAccrued = detail.Debit;
                                interestMarchReport.InterestPaid = 0;
                                var tax = mgainAccDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                        && x.DocParticulars == "TDS " + (year - 1) + "-" + year.ToString().Substring(year.ToString().Length - 2) && x.Debit != 0).FirstOrDefault();
                                interestMarchReport.TaxDeducted = 0;
                                if (tax is not null)
                                    interestMarchReport.TaxDeducted = tax.Debit;
                                interestMarchReport.ShcemeName = mgain.TblMgainSchemeMaster.Schemename;
                                interestMarchReport.ACDescription = mgain.MgainType;
                                interestMarchReport.Currency = "₹";
                                interestMarchReport.OverheadTaxDeducted = "N/A";
                                interestReports.Add(interestMarchReport);
                            }
                        }
                    }
                }
                interestCertificate.InterestReports = interestReports;
                return interestCertificate;
            }
            return null;
        }
        #endregion

        #region Get MGain Interest Ledger
        public async Task<MGainLedgerDto> GetMGainInterestLedgerAsync(int userId, int year)
        {
            var mgainDetails = await _mGainRepository.GetMGainAccTransactionByUserId(userId, year);
            var user = await _userMasterRepository.GetUserMasterbyId(userId);
            var mgainLedger = new MGainLedgerDto();
            mgainLedger.UserId = userId;
            mgainLedger.UserName = user.UserName;
            var months = new List<string>()
            {
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "Nevember",
                "December",
                "January",
                "February",
                "March"
            };
            var interestLedgers = new List<InterestLedgerDto>();
            foreach (var month in months)
            {
                var details = mgainDetails.Where(x => x.DocType == MGainPayment.Payment.ToString() && x.DocDate.Value.ToString("MMMM") == month && x.Credit != 0).ToList();

                if (details.Count > 0)
                {
                    var prevMonth = "";
                    if (month == "April")
                        prevMonth = "March";
                    else
                    {
                        var index = months.FindIndex(x => x == month);
                        prevMonth = months[index - 1];
                    }
                    var flag = true;
                    foreach (var detail in details)
                    {
                        if (flag)
                        {
                            var interestLedger = new InterestLedgerDto();
                            interestLedger.Perticular = "M-GAIN INTEREST_" + detail.Mgainid;
                            interestLedger.Date = detail.DocDate.Value.ToString("dd-MM-yyyy");
                            interestLedger.Debit = detail.Debit;
                            interestLedger.Credit = detail.Credit;
                            interestLedgers.Add(interestLedger);

                            if (detail.TblMgaindetail.MgainIsTdsDeduction == true)
                            {
                                var taxDetail = mgainDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                    && x.DocParticulars == "TDS " + (year - 1) + "-" + year.ToString().Substring(year.ToString().Length - 2) && x.Debit != 0).FirstOrDefault();
                                if (taxDetail is not null)
                                {
                                    var taxLedger = new InterestLedgerDto();
                                    taxLedger.Perticular = taxDetail.DocParticulars + "_" + detail.Mgainid;
                                    taxLedger.Debit = taxDetail.Debit;
                                    taxLedger.Credit = taxDetail.Credit;
                                    interestLedgers.Add(taxLedger);
                                }
                            }

                            flag = false;
                        }
                        else
                        {
                            var interestLedger = new InterestLedgerDto();
                            interestLedger.Perticular = "M-GAIN INTEREST_" + detail.Mgainid;
                            interestLedger.Debit = detail.Debit;
                            interestLedger.Credit = detail.Credit;
                            interestLedgers.Add(interestLedger);

                            if (detail.TblMgaindetail.MgainIsTdsDeduction == true)
                            {
                                var taxDetail = mgainDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                    && x.DocParticulars == "TDS " + (year - 1) + "-" + year.ToString().Substring(year.ToString().Length - 2) && x.Debit != 0).FirstOrDefault();
                                var taxLedger = new InterestLedgerDto();
                                taxLedger.Perticular = taxDetail.DocParticulars + "_" + detail.Mgainid;
                                taxLedger.Debit = detail.Debit;
                                taxLedger.Credit = detail.Credit;
                                interestLedgers.Add(taxLedger);
                            }
                        }
                    }

                    var interestMonthLedger = new InterestLedgerDto();
                    interestMonthLedger.Perticular = string.Concat(prevMonth, " ", "Month Interest", "").ToUpper();
                    interestLedgers.Add(interestMonthLedger);
                }
            }

            mgainLedger.Total = interestLedgers.Sum(x => x.Credit);
            mgainLedger.InterestsLedger = interestLedgers;

            return mgainLedger;
        }
        #endregion

        #region MGain Monthly Non-Cumulative Interest Computation & Release
        public async Task<MGainNCmonthlyTotalDto> GetNonCumulativeMonthlyReportAsync(int month, int year, int? schemeId, decimal? tds, bool? isJournal, DateTime? jvEntryDate, string? jvNarration, bool? isPayment, DateTime? crEntryDate, string? crNarration, string? searchingParams, SortingParams sortingParams, bool? isSendSMS)
        {
            DateTime date = Convert.ToDateTime("01" + "-" + month + "-" + year);
            DateTime currentDate = date.AddDays(-1);

            var mGainDetails = await _mGainRepository.GetAllMGainDetailsMonthly(schemeId, searchingParams, sortingParams, MGainTypeConstant.nonCumulative, currentDate);

            List<MGainNonCumulativeMonthlyReportDto> MGainNonCumulativeMonthlyReports = new List<MGainNonCumulativeMonthlyReportDto>();
            List<TblAccountTransaction> allAccountTransactions = new List<TblAccountTransaction>();

            string? tdsYear = null;

            var currYear = date.Year;

            if (date.Month >= 4)
            {
                tdsYear = "TDS " + currYear + "-" + (currYear + 1).ToString().Substring((currYear + 1).ToString().Length - 2);
            }
            else
            {
                tdsYear = "TDS " + (currYear - 1).ToString() + "-" + currYear.ToString().Substring(currYear.ToString().Length - 2);
            }

            var account = await _mGainRepository.GetAccountByUserId(0, tdsYear);

            if(account is null)
            {
                TblAccountMaster addAccount = new TblAccountMaster();
                addAccount.UserId = 0;
                addAccount.AccountName = tdsYear;
                addAccount.OpeningBalance = 0;
                await _mGainRepository.AddUserAccount(addAccount);  
            }

            foreach (var MGainDetail in mGainDetails)
            {
                if (MGainDetail.Date.Value.AddYears(10) > currentDate)
                {
                    var MGainNonCumulativeMonthlyReport = new MGainNonCumulativeMonthlyReportDto();
                    List<TblAccountTransaction> accountTransactions = new List<TblAccountTransaction>();

                    List<decimal?> interestRates = new List<decimal?>();
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst1);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst2);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst3);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst4 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest4);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst5 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest5);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst6 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest6);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst7 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest7);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst8 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest8);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst9 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest9);
                    interestRates.Add(MGainDetail.TblMgainSchemeMaster.Interst10 + MGainDetail.TblMgainSchemeMaster.AdditionalInterest10);

                    var yearDifference = currentDate.Year - MGainDetail.Date.Value.Year;

                    if (currentDate.Month < MGainDetail.Date.Value.Month)
                    {
                        yearDifference -= 1;
                    }

                    MGainNonCumulativeMonthlyReport.Interst1 = MGainDetail.TblMgainSchemeMaster.Interst1;
                    MGainNonCumulativeMonthlyReport.Interst4 = MGainDetail.TblMgainSchemeMaster.Interst4;
                    MGainNonCumulativeMonthlyReport.Interst8 = MGainDetail.TblMgainSchemeMaster.Interst8;
                    MGainNonCumulativeMonthlyReport.Date = MGainDetail.Date.Value.ToString("dd-MM-yyyy");
                    MGainNonCumulativeMonthlyReport.IntAccNo = 12345679890;
                    MGainNonCumulativeMonthlyReport.IntBankName = "State Bank Of India";
                    MGainNonCumulativeMonthlyReport.MgainInvamt = MGainDetail.MgainInvamt;
                    MGainNonCumulativeMonthlyReport.Mgain1stholder = MGainDetail.Mgain1stholder;

                    if (MGainDetail.MgainRedemdate is not null)
                        MGainNonCumulativeMonthlyReport.MgainRedemdate = MGainDetail.MgainRedemdate.Value.ToString("dd-MM-yyyy");
                    else MGainNonCumulativeMonthlyReport.MgainRedemdate = null;

                    MGainNonCumulativeMonthlyReport.MgainType = MGainDetail.MgainType;
                    MGainNonCumulativeMonthlyReport.YearlyInterest = MGainDetail.TblMgainSchemeMaster.YearlyInterest;
                    MGainNonCumulativeMonthlyReport.MonthlyInterest = MGainDetail.TblMgainSchemeMaster.MonthlyInterest;

                    if (yearDifference == 0)
                    {
                        if (currentDate.Month == MGainDetail.Date.Value.Month)
                        {
                            var daysInMonth = DateTime.DaysInMonth(MGainDetail.Date.Value.Year, MGainDetail.Date.Value.Month);
                            var days = daysInMonth - (MGainDetail.Date.Value.Day - 1);

                            MGainNonCumulativeMonthlyReport.InterstRate = interestRates.First();

                            MGainNonCumulativeMonthlyReport.InterestAmount = Math.Round((decimal)((MGainDetail.MgainInvamt * days * (MGainNonCumulativeMonthlyReport.InterstRate / 100)) / 365), 0);

                            if (MGainDetail.MgainIsTdsDeduction is true)
                                MGainNonCumulativeMonthlyReport.TDS = Math.Round((decimal)((MGainNonCumulativeMonthlyReport.InterestAmount * tds) / 100));
                            else MGainNonCumulativeMonthlyReport.TDS = 0;

                            MGainNonCumulativeMonthlyReport.PayAmount = MGainNonCumulativeMonthlyReport.InterestAmount - MGainNonCumulativeMonthlyReport.TDS;

                            MGainNonCumulativeMonthlyReports.Add(MGainNonCumulativeMonthlyReport);

                            if (isJournal is true)
                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, currentDate, MGainDetail.MgainIsTdsDeduction, isJournal, jvEntryDate, jvNarration, false, null, null, tdsYear);
                            else if (isPayment is true)
                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, currentDate, MGainDetail.MgainIsTdsDeduction, false, null, null, isPayment, crEntryDate, crNarration, tdsYear);

                            if (isSendSMS is true)
                            {
                                string message = $"Dear Investors,Greetings from KA Group!MGain interest of Rs. {MGainNonCumulativeMonthlyReport.InterestAmount} - for the month of {month}-{year} has been credited in your Respective Bank.Thank You.";
                                string mobile = "9173230023";
                                SMSHelper.SendSMS(mobile, message, "");
                            }

                            allAccountTransactions.AddRange(accountTransactions);
                        }
                        else if (currentDate.Month > MGainDetail.Date.Value.AddMonths(3).Month)
                        {
                            MGainNonCumulativeMonthlyReport.InterstRate = interestRates.First();
                            MGainNonCumulativeMonthlyReport.InterestAmount = Math.Round((decimal)(((MGainDetail.MgainInvamt * MGainNonCumulativeMonthlyReport.InterstRate) / 100) / 12), 0);

                            if (MGainDetail.MgainIsTdsDeduction is true)
                                MGainNonCumulativeMonthlyReport.TDS = Math.Round((decimal)((MGainNonCumulativeMonthlyReport.InterestAmount * tds) / 100), 0);
                            else MGainNonCumulativeMonthlyReport.TDS = 0;

                            MGainNonCumulativeMonthlyReport.PayAmount = MGainNonCumulativeMonthlyReport.InterestAmount - MGainNonCumulativeMonthlyReport.TDS;

                            MGainNonCumulativeMonthlyReports.Add(MGainNonCumulativeMonthlyReport);

                            if (isJournal is true)
                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, currentDate, MGainDetail.MgainIsTdsDeduction, isJournal, jvEntryDate, jvNarration, false, null, null, tdsYear);
                            else if (isPayment is true)
                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, currentDate, MGainDetail.MgainIsTdsDeduction, false, null, null, isPayment, crEntryDate, crNarration, tdsYear);

                            if (isSendSMS is true)
                            {
                                string message = $"Dear Investors,Greetings from KA Group!MGain interest of Rs{MGainNonCumulativeMonthlyReport.InterestAmount} - for the month of {month}-{year} has been credited in your Respective Bank.Thank You.";
                                string mobile = "9173230023";
                                SMSHelper.SendSMS(mobile, message, "");
                            }

                            allAccountTransactions.AddRange(accountTransactions);
                        }
                    }
                    else
                    {
                        MGainNonCumulativeMonthlyReport.InterstRate = interestRates.Skip(yearDifference).First();
                        MGainNonCumulativeMonthlyReport.InterestAmount = Math.Round((decimal)(((MGainDetail.MgainInvamt * MGainNonCumulativeMonthlyReport.InterstRate) / 100) / 12), 0);

                        if (MGainDetail.MgainIsTdsDeduction is true)
                            MGainNonCumulativeMonthlyReport.TDS = Math.Round((decimal)((MGainNonCumulativeMonthlyReport.InterestAmount * tds) / 100), 0);
                        else MGainNonCumulativeMonthlyReport.TDS = 0;

                        MGainNonCumulativeMonthlyReport.PayAmount = MGainNonCumulativeMonthlyReport.InterestAmount - MGainNonCumulativeMonthlyReport.TDS;

                        MGainNonCumulativeMonthlyReports.Add(MGainNonCumulativeMonthlyReport);

                        if (isJournal is true)
                            accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, currentDate, MGainDetail.MgainIsTdsDeduction, isJournal, jvEntryDate, jvNarration, false, null, null, tdsYear);
                        else if (isPayment is true)
                            accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, currentDate, MGainDetail.MgainIsTdsDeduction, false, null, null, isPayment, crEntryDate, crNarration, tdsYear);

                        if (isSendSMS is true)
                        {
                            string message = $"Dear Investors,Greetings from KA Group!MGain interest of Rs{MGainNonCumulativeMonthlyReport.InterestAmount} - for the month of {month}-{year} has been credited in your Respective Bank.Thank You.";
                            string mobile = "9173230023";
                            SMSHelper.SendSMS(mobile, message, "");
                        }

                        allAccountTransactions.AddRange(accountTransactions);
                    }
                }
            }

            var totalInterstAmount = MGainNonCumulativeMonthlyReports.Sum(x => x.InterestAmount);
            var totalTDSAmount = MGainNonCumulativeMonthlyReports.Sum(x => x.TDS);
            var totalPayAmount = MGainNonCumulativeMonthlyReports.Sum(x => x.PayAmount);

            var mGainNCMonthlytotal = new MGainNCmonthlyTotalDto()
            {
                MGainNonCumulativeMonthlyReports = MGainNonCumulativeMonthlyReports,
                TotalInterestAmount = totalInterstAmount,
                TotalTDSAmount = totalTDSAmount,
                TotalPayAmount = totalPayAmount
            };

            if (allAccountTransactions.Count > 0)
            {
                allAccountTransactions.Reverse();
                await _mGainRepository.AddMGainInterest(allAccountTransactions, currentDate);
            }

            return mGainNCMonthlytotal;
        }
        #endregion

        #region Get Valuation Report By User
        public async Task<List<MGainValuationDto>> GetValuationReportByUserIdAsync(int UserId)
        {
            var mGainDetails = await _mGainRepository.GetMGainDetailsByUserId(UserId);
            List<MGainValuationDto> mGainValuations = new List<MGainValuationDto>();

            foreach (var mGainDetail in mGainDetails)
            {
                var mGainValuation = new MGainValuationDto();

                List<decimal?> interestRates = new List<decimal?>();
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst1);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst2);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst3);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst4 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest4);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst5 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest5);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst6 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest6);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst7 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest7);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst8 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest8);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst9 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest9);
                interestRates.Add(mGainDetail.TblMgainSchemeMaster.Interst10 + mGainDetail.TblMgainSchemeMaster.AdditionalInterest10);

                mGainValuation.MGainId = mGainDetail.Id;
                mGainValuation.Date = mGainDetail.Date;
                mGainValuation.MgainInvamt = mGainDetail.MgainInvamt;
                mGainValuation.TblMgainPaymentMethods = _mapper.Map<List<MGainPaymentDto>>(mGainDetail.TblMgainPaymentMethods);
                mGainValuation.MgainType = mGainDetail.MgainType;
                mGainValuation.Schemename = mGainDetail.TblMgainSchemeMaster.Schemename;
                mGainValuation.MgainBankName = mGainDetail.MgainBankName;
                mGainValuation.Tenure = 12 * (DateTime.Now.Year - mGainDetail.Date.Value.Year) + DateTime.Now.Month - mGainDetail.Date.Value.Month;

                var accountTransactions = await _mGainRepository.GetAccountTransactionByMgainId(mGainDetail.Id, 0, 0);
                mGainValuation.InterestPayout = accountTransactions.Where(x => x.DocType.ToLower() == MGainAccountPaymentConstant.MGainPayment.Payment.ToString().ToLower()).Sum(x => x.Debit);
                var year = DateTime.Now.Year - mGainDetail.Date.Value.Year;
                mGainValuation.InterestRate = interestRates.Take(year).Average();
                mGainValuation.AmountUnlockDate = mGainValuation.Date.Value.AddYears(3).AddDays(-1);
                if (mGainValuation.AmountUnlockDate > DateTime.Now)
                {
                    mGainValuation.RemainingLockinPeriod = Math.Round((mGainValuation.AmountUnlockDate.Value - DateTime.Now).TotalDays, 0);
                }

                mGainValuations.Add(mGainValuation);
            }

            return mGainValuations;
        }
        #endregion

        #region Get MGain Month wise Total Interest Paid
        public async Task<MGainTotalInterestPaidDto<MGainUserInterestPaidDto>> GetMonthWiseInterestPaidAsync(int month, int year, string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            var accountTransactions = await _mGainRepository.GetAccountTransactionByMgainId(0, month, year);
            var userwiseAccountTransaction = accountTransactions.GroupBy(x => x.DocUserid).ToList();
            List<MGainUserInterestPaidDto> userwiseinterstPaid = new List<MGainUserInterestPaidDto>();

            foreach (var transaction in userwiseAccountTransaction)
            {
                MGainUserInterestPaidDto mGainTotalInterestPaid = new MGainUserInterestPaidDto();

                var mGain = await _mGainRepository.GetMGainDetailsByUserId((int)transaction.Key);
                mGainTotalInterestPaid.UserName = mGain.Select(x => x.Mgain1stholder).FirstOrDefault();
                mGainTotalInterestPaid.DocDate = transaction.Select(x => x.DocDate).FirstOrDefault();   
                mGainTotalInterestPaid.totalInterestPaid = transaction.Where(x => x.DocType.ToLower() == MGainAccountPaymentConstant.MGainPayment.Payment.ToString().ToLower()).Sum(x => x.Credit);

                userwiseinterstPaid.Add(mGainTotalInterestPaid);
            }

            if (searchingParams != null)
            {
                userwiseinterstPaid = userwiseinterstPaid.Where(x => x.UserName.ToLower().Contains(searchingParams.ToLower()) || x.totalInterestPaid.ToString().Contains(searchingParams)).ToList();
            }

            var InterestPaid = userwiseinterstPaid.Sum(x => x.totalInterestPaid);

            IQueryable<MGainUserInterestPaidDto> mGainTotalInterests = userwiseinterstPaid.AsQueryable();

            pageCount = Math.Ceiling(userwiseinterstPaid.Count / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(mGainTotalInterests, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination 
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var userInterestData = new ResponseDto<MGainUserInterestPaidDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var totalInterestData = new MGainTotalInterestPaidDto<MGainUserInterestPaidDto>()
            {
                response = userInterestData,
                TotalInterestPaid = InterestPaid
            };

            return totalInterestData;
        }
        #endregion

        #region Get All Projects
        public async Task<ResponseDto<ProjectMasterDto>> GetAllProjectAsync(string? searchingParams, SortingParams sortingParams)
        {
            var projects = await _mGainRepository.GetAllProject(searchingParams, sortingParams);
            var mapProjects = _mapper.Map<ResponseDto<ProjectMasterDto>>(projects);

            foreach (var project in projects.Values)
            {
                project.Name = project.Name.ToLower();
            }
            return mapProjects;
        }
        #endregion

        #region Get Plots By ProjectId
        public async Task<ResponseDto<PlotMasterDto>> GetPlotsByProjectIdAsync(int projectId, int? plotId,  string? searchingParams, SortingParams sortingParams)
        {
            var plots = await _mGainRepository.GetPlotsByProjectId(projectId, plotId, searchingParams, sortingParams);
            var mapPlots = _mapper.Map<ResponseDto<PlotMasterDto>>(plots);
            return mapPlots;
        }
        #endregion

        #region Get All Currency
        public async Task<List<MGainCurrancyDto>> GetAllCurrenciesAsync()
        {
            var currancy = await _mGainRepository.GetAllCurrencies();
            var mapCurrency = _mapper.Map<List<MGainCurrancyDto>>(currancy);
            return mapCurrency;
        }
        #endregion

        #region Add MGain Details
        public async Task<TblMgaindetail> AddMGainDetailAsync(AddMGainDetailsDto addMGainDetails)
        {
            var mGainDetails = _mapper.Map<TblMgaindetail>(addMGainDetails);
            mGainDetails.MgainSchemename = _mGainSchemeRepository.GetMGainSchemeById((int)addMGainDetails.MgainSchemeid).Result.Schemename;
            var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\MGain-Documents\\";

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

            var mGain = await _mGainRepository.AddMGainDetails(mGainDetails);

            TblAccountMaster tblAccountMaster = new TblAccountMaster();
            tblAccountMaster.UserId = mGain.MgainUserid;
            tblAccountMaster.AccountName = mGain.Mgain1stholder;
            tblAccountMaster.OpeningBalance = 0;

            await _mGainRepository.AddUserAccount(tblAccountMaster);
            return mGain;
        }
        #endregion

        #region Add Payment Details
        public async Task<int> AddPaymentDetailsAsync(List<AddMGainPaymentDto> paymentDtos)
        {
            var mGainPayments = _mapper.Map<List<TblMgainPaymentMethod>>(paymentDtos);
            return await _mGainRepository.AddPaymentDetails(mGainPayments);
        }
        #endregion

        #region MGain Aggrement PDF
        public async Task<MGainPDFResponseDto> GenerateMGainAggrementAsync(int id, string htmlContent)
        {
            var MGain = await _mGainRepository.GetMGainDetailById(id);

            var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\MGain-Documents\\";

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
            var mgain = await _mGainRepository.GetMGainDetailById(updateMGain.Id);

            var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\MGain-Documents\\";    

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var folderPath = directoryPath + $"{updateMGainDetails.Mgain1stholder}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (updateMGainDetails.Mgain1stholderSignatureFile is not null)
            {
                if (File.Exists(mgain.Mgain1stholderSignature))
                {
                    File.Delete(mgain.Mgain1stholderSignature);
                }

                var firstHolderSignature = updateMGainDetails.Mgain1stholderSignatureFile.FileName;
                var firstHolderSignaturePath = Path.Combine(folderPath, firstHolderSignature);
                using (var fs = new FileStream(firstHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.Mgain1stholderSignatureFile.CopyTo(fs);
                }
                updateMGain.Mgain1stholderSignature = firstHolderSignaturePath;
            }
            else updateMGain.Mgain1stholderSignature = updateMGainDetails.Mgain1stholderSignature;


            if (updateMGainDetails.Mgain2ndholderSignatureFile is not null)
            {
                if (File.Exists(mgain.Mgain2ndholderSignature))
                {
                    File.Delete(mgain.Mgain2ndholderSignature);
                }

                var secondHolderSignature = updateMGainDetails.Mgain2ndholderSignatureFile.FileName;
                var secondHolderSignaturePath = Path.Combine(folderPath, secondHolderSignature);
                using (var fs = new FileStream(secondHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.Mgain2ndholderSignatureFile.CopyTo(fs);
                }
                updateMGain.Mgain2ndholderSignature = secondHolderSignaturePath;
            }
            else updateMGain.Mgain2ndholderSignature = updateMGainDetails.Mgain2ndholderSignature;

            if (updateMGainDetails.MgainNomineePanFile is not null)
            {
                if (File.Exists(mgain.MgainNomineePan))
                {
                    File.Delete(mgain.MgainNomineePan);
                }

                var nomineePan = updateMGainDetails.MgainNomineePanFile.FileName;
                var nomineePanPath = Path.Combine(folderPath, nomineePan);
                using (var fs = new FileStream(nomineePanPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainNomineePanFile.CopyTo(fs);
                }

                updateMGain.MgainNomineePan = nomineePanPath;
            }
            else updateMGain.MgainNomineePan = updateMGainDetails.MgainNomineePan;

            if (updateMGainDetails.MgainNomineeAadharFile is not null)
            {
                if (File.Exists(mgain.MgainNomineeAadhar))
                {
                    File.Delete(mgain.MgainNomineeAadhar);
                }

                var nomineeAadhar = updateMGainDetails.MgainNomineeAadharFile.FileName;
                var nomineeAadharPath = Path.Combine(folderPath, nomineeAadhar);
                using (var fs = new FileStream(nomineeAadharPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainNomineeAadharFile.CopyTo(fs);
                }
                updateMGain.MgainNomineeAadhar = nomineeAadharPath;
            }
            else updateMGain.MgainNomineeAadhar = updateMGainDetails.MgainNomineeAadhar;

            if (updateMGainDetails.MgainNomineeBirthCertificateFile is not null)
            {
                if (File.Exists(mgain.MgainNomineeBirthCertificate))
                {
                    File.Delete(mgain.MgainNomineeBirthCertificate);
                }

                var birthCertificate = updateMGainDetails.MgainNomineeBirthCertificateFile.FileName;
                var birthCertificatePath = Path.Combine(folderPath, birthCertificate);
                using (var fs = new FileStream(birthCertificatePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainNomineeBirthCertificateFile.CopyTo(fs);
                }
                updateMGain.MgainNomineeBirthCertificate = birthCertificatePath;
            }
            else updateMGain.MgainNomineeBirthCertificate = updateMGainDetails.MgainNomineeBirthCertificate;

            if (updateMGainDetails.MgainCancelledChequeFile is not null)
            {
                if (File.Exists(mgain.MgainCancelledCheque))
                {
                    File.Delete(mgain.MgainCancelledCheque);
                }

                var cancelledCheque = updateMGainDetails.MgainCancelledChequeFile.FileName;
                var cancelledChequePath = Path.Combine(folderPath, cancelledCheque);
                using (var fs = new FileStream(cancelledChequePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    updateMGainDetails.MgainCancelledChequeFile.CopyTo(fs);
                }
                updateMGain.MgainCancelledCheque = cancelledChequePath;
            }
            else updateMGain.MgainCancelledCheque = updateMGainDetails.MgainCancelledCheque;

            mgain.MgainPlotno = mgain.MgainPlotno.Trim();
            mgain.Mgain2ndplotno = mgain.Mgain2ndplotno.Trim();
            var mGain1stPlot = new TblPlotMaster();
            var mGain2ndPlot = new TblPlotMaster();
            var firstPlot = new TblPlotMaster();
            var secondPlot = new TblPlotMaster();

            if (mgain.MgainProjectname is not null && mgain.MgainPlotno is not null)
                mGain1stPlot = await _mGainRepository.GetPlotByProjectAndPlotNo(mgain.MgainProjectname, mgain.MgainPlotno);

            if (mgain.Mgain2ndprojectname is not null && mgain.Mgain2ndplotno is not null)
                mGain2ndPlot = await _mGainRepository.GetPlotByProjectAndPlotNo(mgain.Mgain2ndprojectname, mgain.Mgain2ndplotno);

            if(updateMGainDetails.MGain1stPlotId != 0)
                firstPlot = await _mGainRepository.GetPlotById(updateMGainDetails.MGain1stPlotId);

            if (updateMGainDetails.MGain2ndPlotId != 0)
                secondPlot = await _mGainRepository.GetPlotById(updateMGainDetails.MGain2ndPlotId);

            if (updateMGainDetails.MGain1stPlotId != 0 && updateMGainDetails.MGain2ndPlotId != 0)
            {
                if ((updateMGainDetails.MgainProjectname == mgain.MgainProjectname && firstPlot.PlotNo == mgain.MgainPlotno) &&
                            (updateMGainDetails.Mgain2ndprojectname == mgain.Mgain2ndprojectname && secondPlot.PlotNo == mgain.Mgain2ndplotno))
                {
                    if (updateMGain.MgainInvamt != mgain.MgainInvamt)
                    {
                        if (updateMGain.MgainInvamt > mgain.MgainAllocatedsqftamt)
                        {
                            var amount = updateMGain.MgainInvamt - mgain.MgainAllocatedsqftamt;
                            var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, true, false, false, false, false, true, true, false, false, false);

                            await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                            updateMGain = assignPlot.Item1;
                        }
                    }
                    else
                        updateMGain = AssignPlot(updateMGain, mgain, 0, null, null, null, null, false, false, false, false, false, false, false, true, true, false, false).Item1;

                    if (updateMGainDetails.MgainRedemamt > 0)
                    {
                        if (updateMGain.MgainRedemamt < updateMGain.MgainInvamt)
                        {
                            if (updateMGainDetails.MgainRedemamt > mgain.Mgain2ndallocatedsqftamt)
                            {
                                var amount = updateMGain.MgainInvamt - updateMGainDetails.MgainRedemamt;
                                var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, true, false, false, false, false, true);

                                await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                                updateMGain = assignPlot.Item1;
                            }
                            else
                            {
                                var amount = updateMGain.MgainInvamt - mgain.MgainAllocatedsqftamt - updateMGainDetails.MgainRedemamt;
                                var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, true, false, false, false, false, true, true, false, false, false);

                                await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                                updateMGain = assignPlot.Item1;
                            }
                        }
                    }
                }
                else if ((updateMGainDetails.MgainProjectname == mgain.MgainProjectname && firstPlot.PlotNo == mgain.MgainPlotno) &&
                            (updateMGainDetails.Mgain2ndprojectname != mgain.Mgain2ndprojectname || secondPlot.PlotNo != mgain.Mgain2ndplotno))
                {
                    if (mgain.Mgain2ndplotno is not null)
                    {
                        var amount = updateMGain.MgainInvamt - mgain.MgainAllocatedsqftamt;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, false, true, false, false, true, true, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                    else
                    {
                        var amount = updateMGain.MgainInvamt - (firstPlot.Available_PlotValue + mgain.MgainAllocatedsqftamt);
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, false, true, false, true, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                }
                else if ((updateMGainDetails.MgainProjectname != mgain.MgainProjectname || firstPlot.PlotNo != mgain.MgainPlotno) &&
                            (updateMGainDetails.Mgain2ndprojectname == mgain.Mgain2ndprojectname && secondPlot.PlotNo == mgain.Mgain2ndplotno))
                {
                    var amount = updateMGain.MgainInvamt - firstPlot.Available_PlotValue;
                    var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, true, true, false, true, false, false, false, false);

                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                    updateMGain = assignPlot.Item1;
                }
                else
                {
                    if (mgain.MgainPlotno is not null && mgain.MgainProjectname is not null && mgain.Mgain2ndplotno is not null && mgain.Mgain2ndprojectname is not null)
                    {
                        var amount = updateMGain.MgainInvamt - firstPlot.Available_PlotValue;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, true, true, false, true, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                    else if (mgain.MgainPlotno is not null && mgain.MgainProjectname is not null && mgain.Mgain2ndplotno is null && mgain.Mgain2ndprojectname is null)
                    {
                        var amount = updateMGain.MgainInvamt - firstPlot.Available_PlotValue;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, false, true, false, true, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                    else
                    {
                        var amount = updateMGain.MgainInvamt - firstPlot.Available_PlotValue;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, false, false, true, false, true, true, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                }

                if (updateMGain.MgainIsclosed is true)
                {
                    updateMGain.MgainRedemdate = DateTime.Now.Date;
                    updateMGain.MgainRedemamt = updateMGain.MgainInvamt;
                    var assignPlot = AssignPlot(updateMGain, mgain, 0, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, false, false, true, true, false, false);

                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                    updateMGain = assignPlot.Item1;
                }
            }
            else if (updateMGainDetails.MGain1stPlotId != 0 && updateMGainDetails.MGain2ndPlotId == 0)
            {
                if (mgain.MgainProjectname is not null && mgain.MgainPlotno is not null && mgain.Mgain2ndprojectname is not null && mgain.Mgain2ndplotno is not null)
                {
                    if (updateMGainDetails.MgainProjectname == mgain.MgainProjectname && firstPlot.PlotNo == mgain.MgainPlotno)
                    {
                        if (updateMGain.MgainInvamt != mgain.MgainInvamt)
                        {
                            if (updateMGain.MgainInvamt <= mgain.MgainAllocatedsqftamt)
                            {
                                var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, true, false, false, false, false, true);//

                                await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                                updateMGain = assignPlot.Item1;
                            }
                        }
                        else
                            updateMGain = AssignPlot(updateMGain, mgain, 0, null, null, null, null, false, false, false, false, false, false, false, true, false, false, false).Item1;
                    }
                    else if (updateMGainDetails.MgainProjectname != mgain.MgainProjectname || firstPlot.PlotNo != mgain.MgainPlotno)
                    {
                        if (updateMGain.MgainInvamt != mgain.MgainInvamt)
                        {
                            var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, true, false, false, false, false, true);//

                            await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                            updateMGain = assignPlot.Item1;
                        }
                    }
                    else
                        updateMGain = AssignPlot(updateMGain, mgain, 0, null, null, null, null, false, false, false, false, false, false, false, true, false, false, false).Item1;
                }
                else
                {
                    if (mgain.MgainPlotno is not null && mgain.MgainProjectname is not null)
                    {
                        var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, null, false, false, true, false, false, true, false, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                    else
                    {
                        var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, null, null, false, false, false, false, false, true, false, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                }

                if (updateMGain.MgainIsclosed is true)
                {
                    updateMGain.MgainRedemdate = DateTime.Now.Date;
                    updateMGain.MgainRedemamt = updateMGain.MgainInvamt;
                    var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, null, false, false, true, false, false, false, false, false, false, false, false);

                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                    updateMGain = assignPlot.Item1;
                }
            }
            else
            {
                if (mgain.MgainProjectname is not null && mgain.MgainPlotno is not null)
                {
                    var assignPlot = AssignPlot(updateMGain, mgain, 0, null, null, mGain1stPlot, null, false, false, false, false, false, false, false, false, false, true, false);
                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                }

                if (mgain.Mgain2ndprojectname is not null && mgain.Mgain2ndplotno is not null)
                {
                    var assignPlot = AssignPlot(updateMGain, mgain, 0, null, null, null, mGain2ndPlot, false, false, false, false, false, false, false, false, false, false, true);
                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                }
            }

            return await _mGainRepository.UpdateMGainDetails(updateMGain);
        }
        #endregion  

        #region Update MGain Payment Details
        public async Task<int> UpdateMGainPaymentAsync(List<UpdateMGainPaymentDto> updateMGainPayment)
        {
            foreach (var mGainDetails in updateMGainPayment)
            {
                var mapMGainPayment = _mapper.Map<TblMgainPaymentMethod>(mGainDetails);
                await _mGainRepository.UpdateMGainPayment(mapMGainPayment);
            }
            return 1;
        }
        #endregion

        #region Delete MGain Payment Details
        public async Task<int> DeleteMGainPaymentAsync(int id)
        {
            var mGainPayment = await _mGainRepository.GetPaymentById(id);
            return await _mGainRepository.DeleteMGainPayment(mGainPayment);
        }
        #endregion

        #region Method For Account Entry
        public List<TblAccountTransaction> AccountEntry(MGainNonCumulativeMonthlyReportDto MGainNonCumulativeMonthlyReport, int? mGainId, int? mGainUserId, DateTime? date, bool? isTdsDeduction, bool? isJournal, DateTime? jvEntryDate, string? jvNarration, bool? isPayment, DateTime? crEntryDate, string? crNarration, string? tdsYear)
        {
            List<TblAccountTransaction> accountTransactions = new List<TblAccountTransaction>();
            if (isJournal is true)
            {
                TblAccountTransaction creditAccountTransaction = new TblAccountTransaction();
                TblAccountTransaction debitAccountTransaction = new TblAccountTransaction();

                //Journal Credit Entry
                creditAccountTransaction.DocDate = jvEntryDate;
                creditAccountTransaction.DocParticulars = jvNarration;
                creditAccountTransaction.DocType = MGainAccountPaymentConstant.MGainPayment.Journal.ToString();
                creditAccountTransaction.DocNo = "JV" + mGainId;
                creditAccountTransaction.Debit = 0;
                creditAccountTransaction.Credit = MGainNonCumulativeMonthlyReport.InterestAmount;
                creditAccountTransaction.DocUserid = mGainUserId;
                creditAccountTransaction.Accountid = _mGainRepository.GetAccountByUserId(mGainUserId, null).Result.AccountId;
                creditAccountTransaction.Mgainid = mGainId;

                //Journal Debit Entry
                debitAccountTransaction.DocDate = jvEntryDate;
                debitAccountTransaction.DocParticulars = jvNarration;
                debitAccountTransaction.DocType = MGainAccountPaymentConstant.MGainPayment.Journal.ToString();
                debitAccountTransaction.DocNo = "JV" + mGainId;
                debitAccountTransaction.Debit = MGainNonCumulativeMonthlyReport.InterestAmount;
                debitAccountTransaction.Credit = 0;
                debitAccountTransaction.DocUserid = mGainUserId;
                debitAccountTransaction.Accountid = _mGainRepository.GetAccountByUserId(0, jvNarration).Result.AccountId;
                debitAccountTransaction.Mgainid = mGainId;

                accountTransactions.Add(debitAccountTransaction);
                accountTransactions.Add(creditAccountTransaction);

                if (isTdsDeduction is true)
                {
                    TblAccountTransaction creditTDCAccountTransaction = new TblAccountTransaction();
                    TblAccountTransaction debitTDCAccountTransaction = new TblAccountTransaction();

                    //Journal TDS Credit Entry
                    creditTDCAccountTransaction.DocDate = jvEntryDate;
                    creditTDCAccountTransaction.DocParticulars = tdsYear;
                    creditTDCAccountTransaction.DocType = MGainAccountPaymentConstant.MGainPayment.Journal.ToString();
                    creditTDCAccountTransaction.DocNo = "JV" + mGainId;
                    creditTDCAccountTransaction.Debit = 0;
                    creditTDCAccountTransaction.Credit = MGainNonCumulativeMonthlyReport.TDS;
                    creditTDCAccountTransaction.DocUserid = mGainUserId;
                    creditTDCAccountTransaction.Accountid = _mGainRepository.GetAccountByUserId(0, tdsYear).Result.AccountId;
                    creditTDCAccountTransaction.Mgainid = mGainId;

                    //Journal TDs Debit Entry
                    debitTDCAccountTransaction.DocDate = jvEntryDate;
                    debitTDCAccountTransaction.DocParticulars = tdsYear;
                    debitTDCAccountTransaction.DocType = MGainAccountPaymentConstant.MGainPayment.Journal.ToString();
                    debitTDCAccountTransaction.DocNo = "JV" + mGainId;
                    debitTDCAccountTransaction.Debit = MGainNonCumulativeMonthlyReport.TDS;
                    debitTDCAccountTransaction.Credit = 0;
                    debitTDCAccountTransaction.DocUserid = mGainUserId;
                    debitTDCAccountTransaction.Accountid = _mGainRepository.GetAccountByUserId(mGainUserId, null).Result.AccountId;
                    debitTDCAccountTransaction.Mgainid = mGainId;

                    accountTransactions.Add(debitTDCAccountTransaction);
                    accountTransactions.Add(creditTDCAccountTransaction);
                }
            }

            if (isPayment is true)
            {
                TblAccountTransaction creditAccountTransaction = new TblAccountTransaction();
                TblAccountTransaction debitAccountTransaction = new TblAccountTransaction();

                //Payment Credit Entry
                creditAccountTransaction.DocDate = crEntryDate;
                creditAccountTransaction.DocParticulars = crNarration;
                creditAccountTransaction.DocType = MGainAccountPaymentConstant.MGainPayment.Payment.ToString();
                creditAccountTransaction.DocNo = "CP" + mGainId;
                creditAccountTransaction.Debit = 0;
                creditAccountTransaction.Credit = MGainNonCumulativeMonthlyReport.InterestAmount;
                creditAccountTransaction.DocUserid = mGainUserId;
                creditAccountTransaction.Accountid = _mGainRepository.GetAccountByUserId(0, crNarration).Result.AccountId;
                creditAccountTransaction.Mgainid = mGainId;

                //Payment Debit Entry
                debitAccountTransaction.DocDate = crEntryDate;
                debitAccountTransaction.DocParticulars = crNarration;
                debitAccountTransaction.DocType = MGainAccountPaymentConstant.MGainPayment.Payment.ToString();
                debitAccountTransaction.DocNo = "CP" + mGainId;
                debitAccountTransaction.Debit = MGainNonCumulativeMonthlyReport.InterestAmount;
                debitAccountTransaction.Credit = 0;
                debitAccountTransaction.DocUserid = mGainUserId;
                debitAccountTransaction.Accountid = _mGainRepository.GetAccountByUserId(mGainUserId, null).Result.AccountId;
                debitAccountTransaction.Mgainid = mGainId;

                accountTransactions.Add(debitAccountTransaction);
                accountTransactions.Add(creditAccountTransaction);
            }
            return accountTransactions;
        }
        #endregion

        #region Calculate Non-Cumulative First Year Interest
        private static MonthDetailDto CalculateFirstYearInterestAsync(decimal mgainAmount, decimal? interestRate, int days)
        {
            var firstYear = new MonthDetailDto();
            decimal? mgainFirstYearTotalInterest = Math.Round((decimal)(mgainAmount * (interestRate / 100)));
            firstYear.FirstMonthInterest = Math.Round((decimal)((mgainAmount * days * (interestRate / 100)) / 365), 0);
            firstYear.FifthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.SixthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.SeventhMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.EighthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.NinethMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.TenthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.EleventhMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.TwelfthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.TotalInterest = firstYear.FirstMonthInterest + firstYear.FifthMonthInterest + firstYear.SixthMonthInterest + firstYear.SeventhMonthInterest +
                                      firstYear.EighthMonthInterest + firstYear.NinethMonthInterest + firstYear.TenthMonthInterest + firstYear.EleventhMonthInterest +
                                      firstYear.TwelfthMonthInterest;
            firstYear.Days = days;

            return firstYear;
        }
        #endregion

        #region Calculate Non-Cumulative Yearly Interest
        private static MonthDetailDto CalculateYearlyInterestAsync(decimal mgainAmount, decimal? interestRate)
        {
            var yearlyInterest = new MonthDetailDto();
            yearlyInterest.TotalInterest = Math.Round((decimal)(mgainAmount * (interestRate / 100)));
            yearlyInterest.FirstMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.SecondMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.ThirdMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.FourthMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.FifthMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.SixthMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.SeventhMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.EighthMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.NinethMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.TenthMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.EleventhMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);
            yearlyInterest.TwelfthMonthInterest = Math.Round((decimal)yearlyInterest.TotalInterest / 12);

            return yearlyInterest;
        }
        #endregion

        #region Calculate Cumulative First Year Interest
        private static MonthDetailDto CalculateCumulativeFirstYearInterestAsync(decimal mgainAmount, decimal? interestRate, int days)
        {
            var firstYear = new MonthDetailDto();
            decimal? mgainFirstYearTotalInterest = Math.Round((decimal)(mgainAmount * (interestRate / 100)));
            firstYear.FirstMonthInterest = Math.Round((decimal)((mgainAmount * days * (interestRate / 100)) / 365), 0);
            firstYear.SecondMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.ThirdMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.FourthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.FifthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.SixthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.SeventhMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.EighthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.NinethMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.TenthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.EleventhMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.TwelfthMonthInterest = Math.Round((decimal)mgainFirstYearTotalInterest / 12);
            firstYear.TotalInterest = firstYear.FirstMonthInterest + firstYear.FifthMonthInterest + firstYear.SixthMonthInterest + firstYear.SeventhMonthInterest +
                                      firstYear.EighthMonthInterest + firstYear.NinethMonthInterest + firstYear.TenthMonthInterest + firstYear.EleventhMonthInterest +
                                      firstYear.TwelfthMonthInterest + firstYear.SecondMonthInterest + firstYear.ThirdMonthInterest + firstYear.FourthMonthInterest;
            firstYear.Days = days;

            return firstYear;
        }
        #endregion

        #region Assign Plots
        public (TblMgaindetail, List<TblPlotMaster>) AssignPlot(TblMgaindetail updateMGain, TblMgaindetail mgain, decimal? invAmount, TblPlotMaster firstPlot, TblPlotMaster secondPlot, TblPlotMaster mGain1stPlot,
                      TblPlotMaster mGain2ndPlot, bool? isFirst, bool? isSecond, bool? isMGainFirst, bool? isMGainSecond, bool? isFirstAssignFull, bool? isAssignFirst, bool? isAssignSecond, bool? isAssignSameFirst
                                , bool? isAssignSameSecond, bool? isNotFirst, bool? isNotSecond)
        {
            List<TblPlotMaster> updatePlots = new List<TblPlotMaster>();

            if (isFirst is true)
            {
                firstPlot.Available_SqFt = Math.Round((decimal)(firstPlot.Available_SqFt + mgain.MgainAllocatedsqft), 4);
                firstPlot.Available_PlotValue = Math.Round((decimal)(firstPlot.Available_PlotValue + mgain.MgainAllocatedsqftamt), 4);

                updatePlots.Add(firstPlot);
            }

            if (isSecond is true)
            {
                secondPlot.Available_SqFt = Math.Round((decimal)(secondPlot.Available_SqFt + mgain.Mgain2ndallocatedsqft), 4);
                secondPlot.Available_PlotValue = Math.Round((decimal)(secondPlot.Available_PlotValue + mgain.Mgain2ndallocatedsqftamt), 4);

                updatePlots.Add(secondPlot);
            }

            if (isMGainFirst is true)
            {
                mGain1stPlot.Available_SqFt = Math.Round((decimal)(mGain1stPlot.Available_SqFt + mgain.MgainAllocatedsqft), 4);
                mGain1stPlot.Available_PlotValue = Math.Round((decimal)(mGain1stPlot.Available_PlotValue + mgain.MgainAllocatedsqftamt), 4);

                updatePlots.Add(mGain1stPlot);
            }

            if (isMGainSecond is true)
            {
                mGain2ndPlot.Available_SqFt = Math.Round((decimal)(mGain2ndPlot.Available_SqFt + mgain.Mgain2ndallocatedsqft), 4);
                mGain2ndPlot.Available_PlotValue = Math.Round((decimal)(mGain2ndPlot.Available_PlotValue + mgain.Mgain2ndallocatedsqftamt), 4);

                updatePlots.Add(mGain2ndPlot);
            }

            if (isFirstAssignFull is true)
            {
                updateMGain.MgainPlotno = firstPlot.PlotNo;
                updateMGain.MgainAllocatedsqft = firstPlot.Available_SqFt;
                updateMGain.MgainAllocatedsqftamt = firstPlot.Available_PlotValue;
                updateMGain.MgainTotalsqft = firstPlot.SqFt;
                updateMGain.MgainTotalplotamt = firstPlot.PlotValue;

                firstPlot.Available_SqFt = Math.Round((decimal)(firstPlot.Available_SqFt - updateMGain.MgainAllocatedsqft), 4);
                firstPlot.Available_PlotValue = Math.Round((decimal)(firstPlot.Available_PlotValue - updateMGain.MgainAllocatedsqftamt), 4);

                updatePlots.Add(firstPlot);
            }

            if (isAssignFirst is true)
            {
                updateMGain.MgainPlotno = firstPlot.PlotNo;
                updateMGain.MgainAllocatedsqft = Math.Round((decimal)((invAmount * firstPlot.SqFt) / (firstPlot.SqFt * firstPlot.Rate)), 4);
                updateMGain.MgainAllocatedsqftamt = Math.Round((decimal)(updateMGain.MgainAllocatedsqft * firstPlot.Rate), 4);
                updateMGain.MgainTotalsqft = Math.Round((decimal)firstPlot.SqFt, 4);
                updateMGain.MgainTotalplotamt = Math.Round((decimal)(firstPlot.SqFt * firstPlot.Rate), 3);

                firstPlot.Available_SqFt = Math.Round((decimal)(firstPlot.Available_SqFt - updateMGain.MgainAllocatedsqft), 4);
                firstPlot.Available_PlotValue = Math.Round((decimal)(firstPlot.Available_PlotValue - updateMGain.MgainAllocatedsqftamt), 4);

                updatePlots.Add(firstPlot);
            }

            if (isAssignSecond is true)
            {
                updateMGain.Mgain2ndplotno = secondPlot.PlotNo;
                updateMGain.Mgain2ndallocatedsqft = Math.Round((decimal)((invAmount * secondPlot.SqFt) / (secondPlot.SqFt * secondPlot.Rate)), 4);
                updateMGain.Mgain2ndallocatedsqftamt = Math.Round((decimal)(updateMGain.Mgain2ndallocatedsqft * secondPlot.Rate), 4);
                updateMGain.Mgain2ndtotalsqft = Math.Round((decimal)secondPlot.SqFt, 4);

                secondPlot.Available_SqFt = Math.Round((decimal)(secondPlot.Available_SqFt - updateMGain.Mgain2ndallocatedsqft), 4);
                secondPlot.Available_PlotValue = Math.Round((decimal)(secondPlot.Available_PlotValue - updateMGain.Mgain2ndallocatedsqftamt), 4);

                updatePlots.Add(secondPlot);
            }

            if (isAssignSameFirst is true)
            {
                updateMGain.MgainPlotno = mgain.MgainPlotno;
                updateMGain.MgainAllocatedsqft = mgain.MgainAllocatedsqft;
                updateMGain.MgainAllocatedsqftamt = mgain.MgainAllocatedsqftamt;
                updateMGain.MgainTotalsqft = mgain.MgainTotalsqft;
                updateMGain.MgainTotalplotamt = mgain.MgainTotalplotamt;
            }

            if (isAssignSameSecond is true)
            {
                updateMGain.Mgain2ndplotno = mgain.Mgain2ndplotno;
                updateMGain.Mgain2ndallocatedsqft = mgain.Mgain2ndallocatedsqft;
                updateMGain.Mgain2ndallocatedsqftamt = mgain.Mgain2ndallocatedsqftamt;
                updateMGain.Mgain2ndtotalsqft = mgain.Mgain2ndtotalsqft;
            }

            if(isNotFirst is true)
            {
                updateMGain.MgainPlotno = null;
                updateMGain.MgainAllocatedsqft = null;
                updateMGain.MgainAllocatedsqftamt = null;
                updateMGain.MgainTotalsqft = null;
                updateMGain.MgainTotalplotamt = null;
            }

            if(isNotSecond is true)
            {
                updateMGain.Mgain2ndplotno = null;
                updateMGain.Mgain2ndallocatedsqft = null;
                updateMGain.Mgain2ndallocatedsqftamt = null;
                updateMGain.Mgain2ndtotalsqft = null;
            }

            return (updateMGain, updatePlots);
        }
        #endregion
    }
}
