using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.Account_Module;
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
        private readonly IAccountTransactionservice _accountTransactionservice;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public MGainService(IMGainRepository mGainRepository, IMapper mapper, IMGainSchemeRepository mGainSchemeRepository, IUserMasterRepository userMasterRepository, IAccountTransactionservice accountTransactionservice, IAccountRepository accountRepository)
        {
            _mGainRepository = mGainRepository;
            _mapper = mapper;
            _mGainSchemeRepository = mGainSchemeRepository;
            _userMasterRepository = userMasterRepository;
            _accountTransactionservice = accountTransactionservice;
            _accountRepository = accountRepository;
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
            TblProjectMaster mGain2ndProject = null;
            if (mGain.Mgain2ndprojectname is not null)
                mGain2ndProject = await _mGainRepository.GetProjectByProjectName(mGain.Mgain2ndprojectname);
            var paymentMode = mGain.TblMgainPaymentMethods.First().PaymentMode;
            var currancy = mGain.TblMgainPaymentMethods.First().TblMgainCurrancyMaster.Currancy;

            var htmlContent = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title></title>
</head>
<body>
    <div style=""text-align: justify; font-family: Serif; font-size: 20px"">
        <center>
            <h3>AGREEMENT FOR M GAIN</h3>";

            htmlContent += $@"<p>THIS AGREEMENT is made and executed on, this {mGain.Date.Value.Day}th day of {mGain.Date.Value.ToString("MMMM")}, {mGain.Date.Value.Year}</p>
            <h3>BETWEEN</h3>
        </center>
        <p>
            <b>KA FINANCIAL SERVICES LIMITED LIABILITY ,</b> registered under the LLP, ACT 2008 vide LLPIN:-AAI- 2331, having its registered office situated at 715, 7TH FLOOR, ROYAL TRADE CENTRE, OPP. STAR BAZAR, HAJIRA ROAD, SURAT, GUJARAT- 395009 INDIA, represented by MR. AMIT HEMANTBHdatAI MEHTA Designated Partner of the LLP hereinafter referred to as the “BORROWER” or the FIRST PARTY to the agreement.
        </p>
        <h3><center>AND</center></h3>";

            if (mGain.Mgain2ndholdername is not null)
            {
                if (mGain.Mgain1stholderAddress == mGain.Mgain2ndholderAddress)
                {
                    htmlContent += $@"<p><b>{mGain.Mgain1stholder}, RESIDING AT: {mGain.Mgain1stholderAddress}-{mGain.TblUserMaster.UserPin} (Mo.no.{mGain.Mgain1stholderMobile}) and {mGain.Mgain2ndholdername}, Residing at: as above,</b> here in after referred to as the “LENDER” which expression, unless repugnant to the context shall mean and includes its legal representatives, assignee, nominee and administrator, the SECOND PARTY to the agreement.</p>
        <p><b>WHEREAS,</b> The BORROWER approached the LENDER to borrow the sum of <b>Rs.{mGain.MgainInvamt}/-</b> as secured against Immovable Non- Agricultural Floating Asset i.e. Land.</p>
        <p><b>AND WHEREAS</b> the LENDER agreed to lend the amount of <b>Rs.{mGain.MgainInvamt}/-</b> to the BORROWER on the acceptance of the Immovable Non- Agricultural Floating Asset i.e. Land.</p>
        <p>It being the express intention of both the parties that this AGREEMENT will be governed according to the terms and condition laid below as per the Agreement.</p>
        <p><b>NOW THIS DEED WITNESSTH AS FOLLOWS:-</b></p><br />
        <div style=""margin-left:20px; "">
            <ol>
                <p>";
                }
                else
                {
                    htmlContent += $@"<p><b>{mGain.Mgain1stholder}, RESIDING AT: {mGain.Mgain1stholderAddress}-{mGain.TblUserMaster.UserPin} (Mo.no.{mGain.Mgain1stholderMobile}) and {mGain.Mgain2ndholdername}, Residing at: {mGain.Mgain2ndholderAddress},</b> here in after referred to as the “LENDER” which expression, unless repugnant to the context shall mean and includes its legal representatives, assignee, nominee and administrator, the SECOND PARTY to the agreement.</p>
        <p><b>WHEREAS,</b> The BORROWER approached the LENDER to borrow the sum of <b>Rs.{mGain.MgainInvamt}/-</b> as secured against Immovable Non- Agricultural Floating Asset i.e. Land.</p>
        <p><b>AND WHEREAS</b> the LENDER agreed to lend the amount of <b>Rs.{mGain.MgainInvamt}/-</b> to the BORROWER on the acceptance of the Immovable Non- Agricultural Floating Asset i.e. Land.</p>
        <p>It being the express intention of both the parties that this AGREEMENT will be governed according to the terms and condition laid below as per the Agreement.</p>
        <p><b>NOW THIS DEED WITNESSTH AS FOLLOWS:-</b></p><br />
        <div style=""margin-left:20px; "">
            <ol>
                <p>";
                }
            }
            else
            {
                htmlContent += $@"<p><b>{mGain.Mgain1stholder}, RESIDING AT: {mGain.Mgain1stholderAddress}-{mGain.TblUserMaster.UserPin} (Mo.no.{mGain.Mgain1stholderMobile}) here in after referred to as the “LENDER” which expression, unless repugnant to the context shall mean and includes its legal representatives, assignee, nominee and administrator, the SECOND PARTY to the agreement.</p>
        <p><b>WHEREAS,</b> The BORROWER approached the LENDER to borrow the sum of <b>Rs.{mGain.MgainInvamt}/-</b> as secured against Immovable Non- Agricultural Floating Asset i.e. Land.</p>
        <p><b>AND WHEREAS</b> the LENDER agreed to lend the amount of <b>Rs.{mGain.MgainInvamt}/-</b> to the BORROWER on the acceptance of the Immovable Non- Agricultural Floating Asset i.e. Land.</p>
        <p>It being the express intention of both the parties that this AGREEMENT will be governed according to the terms and condition laid below as per the Agreement.</p>
        <p><b>NOW THIS DEED WITNESSTH AS FOLLOWS:-</b></p><br />
        <div style=""margin-left:20px; "">
            <ol>
                <p>";
            }

            if (paymentMode.ToLower().Equals("Cheque".ToLower()))
            {
                htmlContent += $@"
                <li><b>AMOUNT OF ADVANCE:</b></li>
                The LENDER agrees to advance amount to the BORROWER of sum of
                <b>Rs.{mGain.MgainInvamt}/-</b>.</p>

                <p>
                <li><b>MODE OF PAYMENT:</b></li>The Amount Recieved from LENDER has been collected in the form of({paymentMode}) bearing
                <b>Rs.{mGain.MgainInvamt}/-</b> by {mGain.TblMgainPaymentMethods.First().BankName} {paymentMode} No. {mGain.TblMgainPaymentMethods.Last().ChequeNo}, Dt. {mGain.TblMgainPaymentMethods.Last().ChequeDate}.</p>

                <p>
                <li><b>SECURITIES:</b></li>
                The BORROWER is providing Immovable Non-Agricultural Floating Asset i.e. Land as security having its ample value as on the date {mGain.Date.Value.Day} th day of {mGain.Date.Value.ToString("MMMM")}, {mGain.Date.Value.Year} in respect of this agreement given by the LENDER. Allotment of Land as and when required as per clause 6 of the agreement shall be done accordingly to the value mentioned in this clause.
                </p>";
            }
            else if (paymentMode.ToLower().Equals("UPI".ToLower()))
            {
                htmlContent += $@"
                <li><b>AMOUNT OF ADVANCE:</b></li>
                The LENDER agrees to advance amount to the BORROWER of sum of
                <b>Rs.{mGain.MgainInvamt}/-</b>.</p>

                <p>
                <li><b>MODE OF PAYMENT:</b></li>The Amount Recieved from LENDER has been collected in the form of({paymentMode}) bearing
                <b>Rs.{mGain.MgainInvamt}/-</b> by {mGain.TblMgainPaymentMethods.First().BankName} {paymentMode} No. {mGain.TblMgainPaymentMethods.Last().UpiTransactionNo}, Dt. {mGain.TblMgainPaymentMethods.Last().UpiDate}.</p>

                <p>
                <li><b>SECURITIES:</b></li>
                The BORROWER is providing Immovable Non-Agricultural Floating Asset i.e. Land as security having its ample value as on the date {mGain.Date.Value.Day} th day of {mGain.Date.Value.ToString("MMMM")}, {mGain.Date.Value.Year} in respect of this agreement given by the LENDER. Allotment of Land as and when required as per clause 6 of the agreement shall be done accordingly to the value mentioned in this clause.
                </p>";
            }
            else if (paymentMode.ToLower().Equals("RTGS".ToLower()))
            {
                htmlContent += $@"
                <li><b>AMOUNT OF ADVANCE:</b></li>
                The LENDER agrees to advance amount to the BORROWER of sum of
                <b>Rs.{mGain.MgainInvamt}/-</b>.</p>

                <p>
                <li><b>MODE OF PAYMENT:</b></li>The Amount Recieved from LENDER has been collected in the form of({paymentMode}) bearing
                <b>Rs.{mGain.MgainInvamt}/-</b> by {mGain.TblMgainPaymentMethods.First().BankName} {paymentMode} No. {mGain.TblMgainPaymentMethods.Last().ReferenceNo}.</p>

                <p>
                <li><b>SECURITIES:</b></li>
                The BORROWER is providing Immovable Non-Agricultural Floating Asset i.e. Land as security having its ample value as on the date {mGain.Date.Value.Day} th day of {mGain.Date.Value.ToString("MMMM")}, {mGain.Date.Value.Year} in respect of this agreement given by the LENDER. Allotment of Land as and when required as per clause 6 of the agreement shall be done accordingly to the value mentioned in this clause.
                </p>";
            }

            if (mGain.Mgain2ndplotno is not null)
            {
                htmlContent += $@"<p>
                    <b>Description of Land against Advances:</b><br>
                    All that piece and parcel of Plot No: {mGain.MgainPlotno} having {mGain.MgainAllocatedsqft} Square Feets out of {mGain.MgainTotalsqft} Square Feets on the project known as “{mGain.MgainProjectname}” bearing {mGainProject.Address}, Taluka: {mGainProject.Taluko}, District: {mGainProject.City}, State: {mGainProject.State}, Country: India and All that piece and parcel of Plot No: {mGain.Mgain2ndplotno} having {mGain.Mgain2ndallocatedsqft} Square Feets out of {mGain.Mgain2ndtotalsqft} Square Feets on the project known as “{mGain.Mgain2ndprojectname}” bearing {mGain2ndProject.Address}, Taluka: {mGain2ndProject.Taluko}, District: {mGain2ndProject.City}, State: {mGain2ndProject.State}, Country: India along with undivided proportionate share in land with all rights.
                </p>";
            }
            else
            {
                htmlContent += $@"<p>
                    <b>Description of Land against Advances:</b><br>
                    All that piece and parcel of Plot No: {mGain.MgainPlotno} having {mGain.MgainAllocatedsqft} Square Feets out of {mGain.MgainTotalsqft} Square Feets on the project known as “{mGain.MgainProjectname}” bearing {mGainProject.Address}, Taluka: {mGainProject.Taluko}, District: {mGainProject.City}, State: {mGainProject.State}, Country: India along with undivided proportionate share in land with all rights.
                </p>";
            }

            if (mGain.Mgain2ndholdername is not null)
            {
                htmlContent += @$"<p>
                <li><b>CURRENCY:</b></li>
                The Advances has been received in form of Indian currency i.e. in {currancy}.
                </p>

                <p>
                <li><b>TENURE OF THE PROPERTY:</b></li>
                The Tenure of the holding of land shall be according to the term till repayment of the advances to the LENDER by the BORROWER; further the LENDER is restricted to recall the aforesaid amount, once kept before the expiry of initial period of 3 years. If in any case the LENDER wants to recall the amount of Advance after the expiry of the 3rd year, he shall intimate such intension by giving prior notice of at least 30 days.
                </p>

                <p>
                <li><b>INTEREST:</b></li>
                The borrower shall pay to the lender interest on the principal amount of the advance at the fixed rate of interest 1% per month in the initial 9 month.
                <br>
                Further, the borrower shall pay to the lender interest on the principal amount of the advance for the succeeding period i.e. till 3 years at the fixed rate of interest {mGain.TblMgainSchemeMaster.Interst1}% per annum, from 4th year to 6th year at the fixed rate of interest {(mGain.TblMgainSchemeMaster.Interst4 + mGain.TblMgainSchemeMaster.AdditionalInterest4)}% per annum and from 7th year till 10th year at the fixed rate of interest {(mGain.TblMgainSchemeMaster.Interst7 + mGain.TblMgainSchemeMaster.AdditionalInterest7)}% per annum.
                <br>
                If borrower fails to pay interest amount in any month then such interest amount shall be carried forward to subsequent month till the end of 12 months from the date of non-payment of interest amount;
                <br>
                Thereafter borrower shall transfer asset marked in the name of Lender at the time of execution of this agreement in the proportion of total amount due.
                </p>

                <p>
                <li><b>COVENANTS/UNDERTAKINGS TO THE PARTIES:</b></li>
                <br>
                <b>BORROWER:</b>
                <br>
                1. Shall promptly notify any event or circumstances, this might operate as a cause of delay in the completion of this agreement.
                <br>
                2. Has provided accurate and true information in respect of Immovable Non- Agricultural Floating Asset i.e. Land and also the title of has been legally verified and clear.
                <br>
                3. Shall be responsible only for the interest if any against the Advance lent by the BORROWER and should not be responsible to pay except the same.
                <br>
                4. Shall be responsible for all the legal formalities also bearing execution expenses in relation to this Agreement.
                <br>
                <br>

                <b>LENDER:</b>
                <br>
                1. Shall provide accurate and true information.
                <br>
                2. Has landed the accepted amount to the BORROWER.
                <br>
                3. Shall not make any changes or development on the Immovable Non- Agricultural Floating Asset i.e. Land of the Company
                <br>

                <p>
                <li><b>TERMS OF REPAYMENTS:</b></li>
                In case of Repayment of the Advance Amount at any time after the completion of lock in period of three (3rd) years but before termination, the Lender has to inform minimum 30 days advance than the scheduled payment cycles of the BORROWER (i.e. the scheduled payment cycles are January 1st to 10th, April 1st to 10th, July 1st to 10th and October 1st to 10th) so that the Borrower can disburse the money to Lender during either of the payment cycles mentioned above (i.e. if the Lender needs payment in 1-10 Jan 2024 window then he has to inform on or before 30th November, 2023.
                <br>
                In this case, the interest will be calculated till the last day of the immediate previous month of upcoming payment cycle (i.e. in case of above example i.e. 31st December, 2023).
                <p>

                <p>
                <li><b>TERMS OF PREPAYMENTS:</b></li>
                If the Borrower wants to pay advance amount before the expiry of the tenure, then the BORROWER shall pay to the LENDER, the Principal amount along with initial 3 months pending interest of the 1st year.

                <p>
                <li><b>BENEFIT OF EQUITY SHARES:</b></li>
                In the event of issuance of IPO whenever issued by the BORROWER i.e. Company, the LENDER can prefer to get captivating benefit on issued IPO price of such Equity Shares against their Advances. The amount of Equity Shares shall be provided to the LENDER according to the Principal amount of Advances.
                </p>

                <p>
                <li><b>EVENTS OF DEFAULT:</b></li>
                In the event of any default by the BORROWER, the LENDER of money have right of Foreclosure of under the Specific Relief Act, 1963
                </p>

                <p>
                <li><b>FORCE MAJEURE:</b></li>
                For the purpose of this Agreement, Force Majeure shall mean governmental laws, orders or regulations, act of God, and other similar contingencies.
                <br>
                None of the parties will remain liable to the other for Force Majeure of any loss incurred due to such reasons.
                </p>

                <p>
                <li><b>ARBITRATION CLAUSE:</b></li>
                That in the event of any dispute between the parties in relation to the agreement, the same shall be dealt by Arbitrator under the provisions of the Indian Arbitration and Conciliation Act, 1996, shall apply in that behalf.
                </p>
            </ol>
        </div>
        <p>IN WITNESS WHEREOF THE PARTIES HAVE EXECUTED THIS AGREEMENT AS ON THE ABOVE REFFERED DATEIN THE PRESENCE OF THE FOLLOWING WITNESSES:-</p>
        <br>
        <p><b>SIGN AND DELIVERED</b></p>
        <p><b>For and on behalf of</b></p>
        <p style=""line-height:1.5""><b>KA FINANCIAL SERVICES LIMITED LIABILITY PARTNERSHIP</b></p>
        <div style=""display:flex;margin-bottom:20px"">
            <div style=""flex:1;display:flex;align-items:flex-end"">
                <b style=""border-top:2px solid black"">AMIT HEMANTBHAI MEHTA (BORROWER)</b>
            </div>
            <div>
                <canvas id=""canvas""
                        height=""170""
                        width=""132""
                        style=""border: 3px solid #385D8A"">
                </canvas>
            </div>
            <div>
                <svg height=""140"" width=""300"">
                    <ellipse cx=""200""
                             cy=""85""
                             rx=""60""
                             ry=""30""
                             style=""stroke:gray;fill: transparent"" />
                </svg>
            </div>
        </div>
        <div style=""display:flex;margin-bottom:20px"">
            <div style=""flex:1;display:flex;align-items:flex-end"">
                <b style=""border-top: 1px solid black"">{mGain.Mgain1stholder} (LENDER)</b>
            </div>
            <div>
                <canvas id=""canvas""
                        height=""170""
                        width=""132""
                        style=""border: 3px solid #385D8A"">
                </canvas>
            </div>
            <div>
                <svg height=""140"" width=""300"">
                    <ellipse cx=""200""
                             cy=""85""
                             rx=""60""
                             ry=""30""
                             style=""stroke:gray;fill: transparent"" />
                </svg>
            </div>
        </div>
        <div style=""display:flex;margin-bottom:20px"">
            <div style=""flex:1;display:flex;align-items:flex-end"">
                <b style=""border-top: 1px solid black"">{mGain.Mgain2ndholdername} (Joint Holder)</b>
            </div>
            <div>
                <canvas id=""canvas""
                        height=""170""
                        width=""132""
                        style=""border: 3px solid #385D8A"">
                </canvas>
            </div>
            <div>
                <svg height=""140"" width=""300"">
                    <ellipse cx=""200""
                             cy=""85""
                             rx=""60""
                             ry=""30""
                             style=""stroke:gray;fill: transparent"" />
                </svg>
            </div>
        </div>
    </div>

</body>
</html>";
            }
            else
            {
                htmlContent += @$"<p>
                <li><b>CURRENCY:</b></li>
                The Advances has been received in form of Indian currency i.e. in {currancy}.
                </p>

                <p>
                <li><b>TENURE OF THE PROPERTY:</b></li>
                The Tenure of the holding of land shall be according to the term till repayment of the advances to the LENDER by the BORROWER; further the LENDER is restricted to recall the aforesaid amount, once kept before the expiry of initial period of 3 years. If in any case the LENDER wants to recall the amount of Advance after the expiry of the 3rd year, he shall intimate such intension by giving prior notice of at least 30 days.
                </p>

                <p>
                <li><b>INTEREST:</b></li>
                The borrower shall pay to the lender interest on the principal amount of the advance at the fixed rate of interest 1% per month in the initial 9 month.
                <br>
                Further, the borrower shall pay to the lender interest on the principal amount of the advance for the succeeding period i.e. till 3 years at the fixed rate of interest {mGain.TblMgainSchemeMaster.Interst1}% per annum, from 4th year to 6th year at the fixed rate of interest {(mGain.TblMgainSchemeMaster.Interst4 + mGain.TblMgainSchemeMaster.AdditionalInterest4)}% per annum and from 7th year till 10th year at the fixed rate of interest {(mGain.TblMgainSchemeMaster.Interst7 + mGain.TblMgainSchemeMaster.AdditionalInterest7)}% per annum.
                <br>
                If borrower fails to pay interest amount in any month then such interest amount shall be carried forward to subsequent month till the end of 12 months from the date of non-payment of interest amount;
                <br>
                Thereafter borrower shall transfer asset marked in the name of Lender at the time of execution of this agreement in the proportion of total amount due.
                </p>

                <p>
                <li><b>COVENANTS/UNDERTAKINGS TO THE PARTIES:</b></li>
                <br>
                <b>BORROWER:</b>
                <br>
                1. Shall promptly notify any event or circumstances, this might operate as a cause of delay in the completion of this agreement.
                <br>
                2. Has provided accurate and true information in respect of Immovable Non- Agricultural Floating Asset i.e. Land and also the title of has been legally verified and clear.
                <br>
                3. Shall be responsible only for the interest if any against the Advance lent by the BORROWER and should not be responsible to pay except the same.
                <br>
                4. Shall be responsible for all the legal formalities also bearing execution expenses in relation to this Agreement.
                <br>
                <br>

                <b>LENDER:</b>
                <br>
                1. Shall provide accurate and true information.
                <br>
                2. Has landed the accepted amount to the BORROWER.
                <br>
                3. Shall not make any changes or development on the Immovable Non- Agricultural Floating Asset i.e. Land of the Company
                <br>

                <p>
                <li><b>TERMS OF REPAYMENTS:</b></li>
                In case of Repayment of the Advance Amount at any time after the completion of lock in period of three (3rd) years but before termination, the Lender has to inform minimum 30 days advance than the scheduled payment cycles of the BORROWER (i.e. the scheduled payment cycles are January 1st to 10th, April 1st to 10th, July 1st to 10th and October 1st to 10th) so that the Borrower can disburse the money to Lender during either of the payment cycles mentioned above (i.e. if the Lender needs payment in 1-10 Jan 2024 window then he has to inform on or before 30th November, 2023.
                <br>
                In this case, the interest will be calculated till the last day of the immediate previous month of upcoming payment cycle (i.e. in case of above example i.e. 31st December, 2023).
                <p>

                <p>
                <li><b>TERMS OF PREPAYMENTS:</b></li>
                If the Borrower wants to pay advance amount before the expiry of the tenure, then the BORROWER shall pay to the LENDER, the Principal amount along with initial 3 months pending interest of the 1st year.

                <p>
                <li><b>BENEFIT OF EQUITY SHARES:</b></li>
                In the event of issuance of IPO whenever issued by the BORROWER i.e. Company, the LENDER can prefer to get captivating benefit on issued IPO price of such Equity Shares against their Advances. The amount of Equity Shares shall be provided to the LENDER according to the Principal amount of Advances.
                </p>

                <p>
                <li><b>EVENTS OF DEFAULT:</b></li>
                In the event of any default by the BORROWER, the LENDER of money have right of Foreclosure of under the Specific Relief Act, 1963
                </p>

                <p>
                <li><b>FORCE MAJEURE:</b></li>
                For the purpose of this Agreement, Force Majeure shall mean governmental laws, orders or regulations, act of God, and other similar contingencies.
                <br>
                None of the parties will remain liable to the other for Force Majeure of any loss incurred due to such reasons.
                </p>

                <p>
                <li><b>ARBITRATION CLAUSE:</b></li>
                That in the event of any dispute between the parties in relation to the agreement, the same shall be dealt by Arbitrator under the provisions of the Indian Arbitration and Conciliation Act, 1996, shall apply in that behalf.
                </p>
            </ol>
        </div>
        <p>IN WITNESS WHEREOF THE PARTIES HAVE EXECUTED THIS AGREEMENT AS ON THE ABOVE REFFERED DATEIN THE PRESENCE OF THE FOLLOWING WITNESSES:-</p>
        <br>
        <p><b>SIGN AND DELIVERED</b></p>
        <p><b>For and on behalf of</b></p>
        <p style=""line-height:1.5""><b>KA FINANCIAL SERVICES LIMITED LIABILITY PARTNERSHIP</b></p>
        <div style=""display:flex;margin-bottom:20px"">
            <div style=""flex:1;display:flex;align-items:flex-end"">
                <b style=""border-top:2px solid black"">AMIT HEMANTBHAI MEHTA (BORROWER)</b>
            </div>
            <div>
                <canvas id=""canvas""
                        height=""170""
                        width=""132""
                        style=""border: 3px solid #385D8A"">
                </canvas>
            </div>
            <div>
                <svg height=""140"" width=""300"">
                    <ellipse cx=""200""
                             cy=""85""
                             rx=""60""
                             ry=""30""
                             style=""stroke:gray;fill: transparent"" />
                </svg>
            </div>
        </div>
        <div style=""display:flex;margin-bottom:20px"">
            <div style=""flex:1;display:flex;align-items:flex-end"">
                <b style=""border-top: 1px solid black"">{mGain.Mgain1stholder} (LENDER)</b>
            </div>
            <div>
                <canvas id=""canvas""
                        height=""170""
                        width=""132""
                        style=""border: 3px solid #385D8A"">
                </canvas>
            </div>
            <div>
                <svg height=""140"" width=""300"">
                    <ellipse cx=""200""
                             cy=""85""
                             rx=""60""
                             ry=""30""
                             style=""stroke:gray;fill: transparent"" />
                </svg>
            </div>
        </div>
    </div>
</body>
</html>";
            }
            htmlContent = htmlContent.Replace("\r\n", " ");
            return htmlContent;
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
        public async Task<InterestCertificateDto> GetMGainIntertestCertificateAsync(int userId, int financialYearId)
        {
            var financialYear = await _accountRepository.GetFinancialYearById(financialYearId);
            if (financialYear is not null)
            {
                var user = await _userMasterRepository.GetUserMasterbyId(userId);
                var mgainAccDetails = await _mGainRepository.GetMGainAccTransactionByUserId(userId, financialYear.Startdate, financialYear.Enddate);
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
                    interestCertificate.Date = financialYear.Enddate.Value.ToString("dd MMM, yyyy");
                    var months = new List<string>()
            {
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
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
                                interestReport.MgainId = detail.Mgainid;
                                interestReport.DepositeCode = detail.Id;
                                interestReport.Date = detail.DocDate;
                                interestReport.InterestPaid = detail.Debit;
                                interestReport.InterestAccrued = 0;
                                var tax = mgainAccDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                            && x.DocParticulars == ("TDS " + financialYear.Year) && x.Credit != 0).FirstOrDefault();
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
                                    interestMarchReport.MgainId = detail.Mgainid;
                                    interestMarchReport.DepositeCode = detail.Id;
                                    interestMarchReport.InterestAccrued = detail.Debit;
                                    interestMarchReport.InterestPaid = 0;
                                    var tax = mgainAccDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                            && x.DocParticulars == ("TDS " + financialYear.Year) && x.Credit != 0).FirstOrDefault();
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
            }
            return null;
        }
        #endregion

        #region Get MGain Interest Ledger
        public async Task<MGainLedgerDto> GetMGainInterestLedgerAsync(int userId, int financialYearId)
        {
            var financialYear = await _accountRepository.GetFinancialYearById(financialYearId);
            if (financialYear is not null)
            {
                var mgainDetails = await _mGainRepository.GetMGainAccTransactionByUserId(userId, financialYear.Startdate, financialYear.Enddate);
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
                "November",
                "December",
                "January",
                "February",
                "March"
            };
                var interestLedgers = new List<InterestLedgerDto>();
                foreach (var month in months)
                {
                    var details = mgainDetails.Where(x => x.DocType == MGainPayment.Payment.ToString() && x.DocDate.Value.ToString("MMMM") == month && x.Debit != 0).ToList();

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

                        foreach (var detail in details)
                        {
                            var interestLedger = new InterestLedgerDto();
                            interestLedger.Perticular = "M-GAIN INTEREST_" + detail.Mgainid;
                            interestLedger.Narration = string.Concat(prevMonth, " ", "Month Interest", "").ToUpper();
                            interestLedger.Date = detail.DocDate.Value.ToString("dd-MM-yyyy");
                            interestLedger.Debit = detail.Debit;
                            interestLedger.Credit = detail.Credit;
                            interestLedgers.Add(interestLedger);

                            if (detail.TblMgaindetail.MgainIsTdsDeduction == true)
                            {
                                var taxDetail = mgainDetails.Where(x => x.DocType == MGainPayment.Journal.ToString() && x.DocDate.Value.ToString("MMMM") == prevMonth && x.Mgainid == detail.Mgainid
                                                    && x.DocParticulars == ("TDS " + financialYear.Year) && x.Credit != 0).FirstOrDefault();
                                if (taxDetail is not null)
                                {
                                    var taxLedger = new InterestLedgerDto();
                                    taxLedger.Perticular = taxDetail.DocParticulars + "_" + detail.Mgainid;
                                    taxLedger.Narration = string.Concat(prevMonth, " ", "Month Tax Deduct", "").ToUpper();
                                    taxLedger.Debit = taxDetail.Debit;
                                    taxLedger.Credit = taxDetail.Credit;
                                    interestLedgers.Add(taxLedger);
                                }
                            }
                        }
                    }
                }

                mgainLedger.Total = interestLedgers.Sum(x => x.Debit);
                mgainLedger.InterestsLedger = interestLedgers;

                return mgainLedger;
            }
            return null;
        }
        #endregion

        #region MGain Monthly Non-Cumulative Interest Computation & Release
        public async Task<(MGainNCmonthlyTotalDto, string)> GetNonCumulativeMonthlyReportAsync(int month, int year, int? schemeId, decimal? tds, bool? isPayment, DateTime? crEntryDate, string? crNarration, string? searchingParams, SortingParams sortingParams, bool? isSendSMS, bool isJournal = false, string jvNarration = null, DateTime? jvEntryDate = null)
        {
            DateTime date = Convert.ToDateTime("01" + "-" + month + "-" + year);
            DateTime currentDate = date.AddDays(-1);

            var mGainDetails = await _mGainRepository.GetAllMGainDetailsMonthly(schemeId, searchingParams, sortingParams, MGainTypeConstant.nonCumulative, currentDate);

            List<MGainNonCumulativeMonthlyReportDto> MGainNonCumulativeMonthlyReports = new List<MGainNonCumulativeMonthlyReportDto>();
            List<TblAccountTransaction> allAccountTransactions = new List<TblAccountTransaction>();

            string? tdsYear = null;
            string? docNo = null;
            string? tdsDocNo = null;
            int? companyId = 0;

            var currYear = date.Year;

            if (date.Month >= 4)
            {
                tdsYear = "TDS " + currYear + "-" + (currYear + 1).ToString().Substring((currYear + 1).ToString().Length - 2);
            }
            else
            {
                tdsYear = "TDS " + (currYear - 1).ToString() + "-" + currYear.ToString().Substring(currYear.ToString().Length - 2);
            }

            var account = _mGainRepository.GetAccountByUserId(0, tdsYear);

            if (account is null)
            {
                TblAccountMaster addAccount = new TblAccountMaster();
                addAccount.UserId = 0;
                addAccount.AccountName = tdsYear;
                addAccount.OpeningBalance = 0;
                await _mGainRepository.AddUserAccount(addAccount);
            }

            var company = _accountRepository.GetCompanyByName("KA Financial Services LLP");
            if (company is not null)
                companyId = company.Id;

            foreach (var MGainDetail in mGainDetails)
            {
                if (MGainDetail.Date.Value.AddYears(10) > currentDate)
                {
                    var MGainNonCumulativeMonthlyReport = new MGainNonCumulativeMonthlyReportDto();
                    List<TblAccountTransaction> accountTransactions = new List<TblAccountTransaction>();
                    string? transactionType = null;

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

                    if (MGainDetail.TblMgainPaymentMethods.Count() > 0)
                    {
                        transactionType = MGainDetail.TblMgainPaymentMethods.FirstOrDefault().PaymentMode;
                    }

                    MGainNonCumulativeMonthlyReport.Id = MGainDetail.Id;
                    MGainNonCumulativeMonthlyReport.MgainUserid = MGainDetail.MgainUserid;
                    MGainNonCumulativeMonthlyReport.Interst1 = MGainDetail.TblMgainSchemeMaster.Interst1;
                    MGainNonCumulativeMonthlyReport.Interst4 = MGainDetail.TblMgainSchemeMaster.Interst4;
                    MGainNonCumulativeMonthlyReport.Interst8 = MGainDetail.TblMgainSchemeMaster.Interst8;
                    MGainNonCumulativeMonthlyReport.Date = MGainDetail.Date.Value.ToString("dd-MM-yyyy");
                    if (MGainDetail.MgainIsAnotherBank is true)
                    {
                        MGainNonCumulativeMonthlyReport.IntAccNo = MGainDetail.MgainAccountNumber;
                        MGainNonCumulativeMonthlyReport.IntBankName = MGainDetail.MgainBankName;
                    }
                    MGainNonCumulativeMonthlyReport.MgainInvamt = MGainDetail.MgainInvamt;
                    MGainNonCumulativeMonthlyReport.Mgain1stholder = MGainDetail.Mgain1stholder;

                    if (MGainDetail.MgainRedemdate is not null)
                        MGainNonCumulativeMonthlyReport.MgainRedemdate = MGainDetail.MgainRedemdate.Value.ToString("dd-MM-yyyy");
                    else MGainNonCumulativeMonthlyReport.MgainRedemdate = null;

                    MGainNonCumulativeMonthlyReport.MgainType = MGainDetail.MgainType;
                    MGainNonCumulativeMonthlyReport.YearlyInterest = MGainDetail.TblMgainSchemeMaster.YearlyInterest;
                    MGainNonCumulativeMonthlyReport.MonthlyInterest = MGainDetail.TblMgainSchemeMaster.MonthlyInterest;

                    if (MGainDetail.TblMgainPaymentMethods.Count() > 0)
                        MGainNonCumulativeMonthlyReport.CurrancyId = MGainDetail.TblMgainPaymentMethods.FirstOrDefault().CurrancyId;

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
                            {
                                docNo = await _accountTransactionservice.GetTransactionDocNoAsync("Journal", docNo);

                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, MGainDetail.MgainIsTdsDeduction, isJournal, jvEntryDate, jvNarration, false, null, null, tdsYear, docNo, tdsDocNo, MGainNonCumulativeMonthlyReport.CurrancyId, companyId, transactionType);
                            }
                            if (isPayment is true)
                            {
                                docNo = await _accountTransactionservice.GetTransactionDocNoAsync("Payment", docNo);
                                if (MGainDetail.MgainIsTdsDeduction is true)
                                {
                                    tdsDocNo = await _accountTransactionservice.GetTransactionDocNoAsync("Journal", tdsDocNo);

                                    accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, MGainDetail.MgainIsTdsDeduction, false, null, null, isPayment, crEntryDate, crNarration, tdsYear, docNo, tdsDocNo, MGainNonCumulativeMonthlyReport.CurrancyId, companyId, transactionType);
                                }
                            }

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
                            {
                                docNo = await _accountTransactionservice.GetTransactionDocNoAsync("Journal", docNo);
                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, MGainDetail.MgainIsTdsDeduction, isJournal, jvEntryDate, jvNarration, false, null, null, tdsYear, docNo, tdsDocNo, MGainNonCumulativeMonthlyReport.CurrancyId, companyId, transactionType);
                            }
                            if (isPayment is true)
                            {
                                docNo = await _accountTransactionservice.GetTransactionDocNoAsync("Payment", docNo);

                                if (MGainDetail.MgainIsTdsDeduction is true)
                                {
                                    tdsDocNo = await _accountTransactionservice.GetTransactionDocNoAsync("Journal", tdsDocNo);

                                    accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, MGainDetail.MgainIsTdsDeduction, false, null, null, isPayment, crEntryDate, crNarration, tdsYear, docNo, tdsDocNo, MGainNonCumulativeMonthlyReport.CurrancyId, companyId, transactionType);
                                }
                            }

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
                        {
                            docNo = await _accountTransactionservice.GetTransactionDocNoAsync("Journal", docNo);
                            accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, MGainDetail.MgainIsTdsDeduction, isJournal, jvEntryDate, jvNarration, false, null, null, tdsYear, docNo, tdsDocNo, MGainNonCumulativeMonthlyReport.CurrancyId, companyId, transactionType);
                        }
                        if (isPayment is true)
                        {
                            docNo = await _accountTransactionservice.GetTransactionDocNoAsync("Payment", docNo);
                            if (MGainDetail.MgainIsTdsDeduction is true)
                            {
                                tdsDocNo = await _accountTransactionservice.GetTransactionDocNoAsync("Journal", tdsDocNo);

                                accountTransactions = AccountEntry(MGainNonCumulativeMonthlyReport, MGainDetail.Id, MGainDetail.MgainUserid, MGainDetail.MgainIsTdsDeduction, false, null, null, isPayment, crEntryDate, crNarration, tdsYear, docNo, tdsDocNo, MGainNonCumulativeMonthlyReport.CurrancyId, companyId, transactionType);
                            }
                        }

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
                var entry = _mGainRepository.AddMGainInterest(allAccountTransactions, currentDate);

                if (entry != 0)
                    return (mGainNCMonthlytotal, $"{entry} Entry added successfully.");
                else
                    return (null, "Account transaction already exists.");
            }

            return (mGainNCMonthlytotal, null);
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

                if (mGainDetail.MgainType.ToLower().Equals("non-cumulative"))
                {
                    var accountTransactions = await _mGainRepository.GetAccountTransactionByMgainId(mGainDetail.Id, 0, 0);
                    mGainValuation.InterestPayout = (decimal)accountTransactions.Where(x => x.DocType.ToLower() == MGainAccountPaymentConstant.MGainPayment.Payment.ToString().ToLower()).Sum(x => x.Debit);
                }
                else
                {
                    decimal? finalAmount = mGainDetail.MgainInvamt;
                    decimal? interestForPeriod = 0;
                    var yearDifference = (DateTime.Now.Year - mGainDetail.Date.Value.Year) - 1;

                    if (yearDifference >= 0)
                    {
                        for (var i = 0; i <= yearDifference; i++)
                        {
                            if (i <= 9)
                            {
                                var interestRate = interestRates[i];

                                if (i is 0)
                                {
                                    var monthDifference = (12 - mGainDetail.Date.Value.AddYears(1).Month) + 3;
                                    decimal? firstYearTotalInterest = Math.Round((decimal)(mGainDetail.MgainInvamt * (interestRate / 100)));

                                    for (var j = 0; j <= monthDifference; j++)
                                    {
                                        if (j == 0)
                                        {
                                            var daysInMonth = DateTime.DaysInMonth(mGainDetail.Date.Value.Year, mGainDetail.Date.Value.Month);
                                            var days = daysInMonth - (mGainDetail.Date.Value.Day - 1);
                                            interestForPeriod += Math.Round((decimal)(mGainDetail.MgainInvamt * days * (interestRate / 100)) / 365);
                                        }
                                        else interestForPeriod += Math.Round((decimal)firstYearTotalInterest / 12);
                                    }
                                    finalAmount = finalAmount + interestForPeriod;
                                }
                                else if (i == 9)
                                {
                                    if ((mGainDetail.Date.Value.Month) < 4)
                                    {
                                        interestForPeriod += Math.Round((decimal)(finalAmount * (9 + mGainDetail.Date.Value.Month) * (interestRate / 100)) / 12);
                                        finalAmount = mGainDetail.MgainInvamt + interestForPeriod;
                                    }
                                    else
                                    {
                                        interestForPeriod += Math.Round((decimal)(finalAmount * (mGainDetail.Date.Value.Month - 4) * (interestRate / 100)) / 12);
                                        finalAmount = mGainDetail.MgainInvamt + interestForPeriod;
                                    }
                                }
                                else
                                {
                                    interestForPeriod += Math.Round((decimal)(finalAmount * (interestRate / 100)));
                                    finalAmount = mGainDetail.MgainInvamt + interestForPeriod;
                                }
                            }
                        }
                    }

                    mGainValuation.InterestAccrued = (decimal)interestForPeriod;
                }
                var year = DateTime.Now.Year - mGainDetail.Date.Value.Year;
                mGainValuation.InterestRate = interestRates.Take(year + 1).Average();
                mGainValuation.AmountUnlockDate = mGainValuation.Date.Value.AddYears(3).AddDays(-1);
                if (mGainValuation.AmountUnlockDate > DateTime.Now)
                {
                    mGainValuation.RemainingLockinPeriod = (((mGainValuation.AmountUnlockDate.Value.Year - DateTime.Now.Year) * 12) + mGainValuation.AmountUnlockDate.Value.Month - DateTime.Now.Month);
                }

                mGainValuations.Add(mGainValuation);
            }

            return mGainValuations;
        }
        #endregion

        #region Valuation Report PDF
        public async Task<MGainPDFResponseDto> ValuationReportPDF(List<MGainValuationDto> mGainValuations)
        {
            try
            {
                var mGain = await _mGainRepository.GetMGainDetailById(mGainValuations.FirstOrDefault().MGainId);

                string htmlContent = $@"<!DOCTYPE html>
<html>
<head>
  <style>
    
        table, td, th {{
        border: 1px solid;
        text-align: center;
}}
table {{
          width: 100%;
        border-collapse: collapse;
}}
      </style>
<title>Page Title</title>
</head>
<body>
<div style=""display:flex; flex-direction:row; justify-content:space-between;"">
<div>{mGain.Mgain1stholder}</div>
<div>Valuation Report</div>
</div>
<hr>

<div style=""margin-top:100px"">
                  <table>
                    <tr>
                      <th style=""width:100px"">Date</th>
                      <th>INVESTMENT</th>
                      <th>Payment Mode</th>
                      <th>Additional Details</th>
                      <th>Type</th>
                      <th>Tenure</th>
                      <th>Scheme</th>
                      <th>Interest(p. a.)</th>
                      <th>Interest Accrued</th>
                      <th>Interest Payout</th>
                      <th>Lockin Period</th>
                      <th>Amount Unlock Date</th>
                    </tr>";

                foreach (var report in mGainValuations)
                {
                    htmlContent += @$"<tr>
                      <td>{report.Date.Value.ToString("dd-MM-yyyy")}</td>
                      <td>{report.MgainInvamt}</td>";
                    if (report.TblMgainPaymentMethods.Count() > 0)
                    {
                        htmlContent += "<td>";
                        foreach (var payment in report.TblMgainPaymentMethods)
                        {
                            htmlContent += $@"
                                    <div> - {payment.PaymentMode}</div>";
                        };
                        htmlContent += "</td>";

                        htmlContent += "<td>";
                        foreach (var payment in report.TblMgainPaymentMethods)
                        {
                            if (payment.PaymentMode.ToLower().Equals("CHEQUE".ToLower()))
                            {
                                if (!string.IsNullOrEmpty(payment.ChequeNo))
                                {
                                    htmlContent += $@"
                                    <div> - {payment.ChequeNo}</div>";
                                }
                                else
                                {
                                    htmlContent += $@"
                                    <div>-</div>";
                                }
                            }
                            else if (payment.PaymentMode.ToLower().Equals("RTGS".ToLower()))
                            {
                                if (!string.IsNullOrEmpty(payment.ChequeNo))
                                {
                                    htmlContent += $@"
                                    <div> - {payment.ReferenceNo}</div>";
                                }
                                else
                                {
                                    htmlContent += $@"
                                    <div>-</div>";
                                }
                            }
                            else if (payment.PaymentMode.ToLower().Equals("UPI".ToLower()))
                            {
                                if (!string.IsNullOrEmpty(payment.ChequeNo))
                                {
                                    htmlContent += $@"
                                    <div>{payment.UpiTransactionNo}</div>";
                                }
                                else
                                {
                                    htmlContent += $@"
                                    <div>-</div>";
                                }
                            }
                        };
                        htmlContent += "</td>";
                    }
                    else
                    {
                        htmlContent += $@"
                        <td>
                            <div>-</div>
                        </td>
                        <td>
                            <div>-</div>
                        </td>";
                    }
                    htmlContent += $@"
                    <td>{report.MgainType}</td>
                    <td>{report.Tenure}</td>
                    <td>{report.Schemename}</td>
                    <td>{report.InterestRate}</td>
                    <td>{report.InterestAccrued}</td>
                    <td>{report.InterestPayout}</td>
                    <td>{report.RemainingLockinPeriod}</td>
                    <td>{report.AmountUnlockDate.Value.ToString("dd-MM-yyyy")}</td>
                </tr>";
                }

                htmlContent += $@"
</table>
</body>
</html>";

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

                var fileName = $"{mGain.Mgain1stholder}-mGainValuation";

                var filePath = folderPath + fileName;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                PdfDocument document = new PdfDocument();

                // Create a new HTML to PDF converter
                HtmlToPdf converter = new HtmlToPdf();

                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                converter.Options.MarginTop = 50;
                converter.Options.MarginLeft = 30;
                converter.Options.MarginRight = 30;

                // Convert the HTML string to PDF
                PdfDocument result = converter.ConvertHtmlString(htmlContent);

                // Save the PDF document to a memory stream
                MemoryStream stream = new MemoryStream();
                result.Save(stream);
                stream.Position = 0;

                var response = new MGainPDFResponseDto()
                {
                    file = stream.ToArray(),
                    FileName = fileName
                };

                return response;
            }
            catch (Exception)
            {
                return null;
            }
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

        #region Get Plots By ProjectId
        public async Task<List<PlotMasterDto>> GetPlotsByProjectIdAsync(int projectId, int? plotId)
        {
            var plots = await _mGainRepository.GetPlotsByProjectId(projectId, plotId);
            var mapPlots = _mapper.Map<List<PlotMasterDto>>(plots);
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

            if (addMGainDetails.Mgain1stholderSignatureFile is not null)
            {
                var firstHolderSignature = addMGainDetails.Mgain1stholderSignatureFile.FileName;
                var firstHolderSignaturePath = Path.Combine(folderPath, firstHolderSignature);

                if (File.Exists(firstHolderSignaturePath))
                {
                    File.Delete(firstHolderSignaturePath);
                }

                using (var fs = new FileStream(firstHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.Mgain1stholderSignatureFile.CopyTo(fs);
                }
                mGainDetails.Mgain1stholderSignature = firstHolderSignaturePath;
            }
            else mGainDetails.Mgain1stholderSignature = null;


            if (addMGainDetails.Mgain2ndholderSignatureFile is not null)
            {
                var secondHolderSignature = addMGainDetails.Mgain2ndholderSignatureFile.FileName;
                var secondHolderSignaturePath = Path.Combine(folderPath, secondHolderSignature);

                if (File.Exists(secondHolderSignaturePath))
                {
                    File.Delete(secondHolderSignaturePath);
                }

                using (var fs = new FileStream(secondHolderSignaturePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.Mgain2ndholderSignatureFile.CopyTo(fs);
                }
                mGainDetails.Mgain2ndholderSignature = secondHolderSignaturePath;
            }
            else mGainDetails.Mgain2ndholderSignature = null;

            if (addMGainDetails.MgainNomineePanFile is not null)
            {
                var nomineePan = addMGainDetails.MgainNomineePanFile.FileName;
                var nomineePanPath = Path.Combine(folderPath, nomineePan);

                if (File.Exists(nomineePanPath))
                {
                    File.Delete(nomineePanPath);
                }

                using (var fs = new FileStream(nomineePanPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.MgainNomineePanFile.CopyTo(fs);
                }

                mGainDetails.MgainNomineePan = nomineePanPath;
            }
            else mGainDetails.MgainNomineePan = null;

            if (addMGainDetails.MgainNomineeAadharFile is not null)
            {
                var nomineeAadhar = addMGainDetails.MgainNomineeAadharFile.FileName;
                var nomineeAadharPath = Path.Combine(folderPath, nomineeAadhar);

                if (File.Exists(nomineeAadharPath))
                {
                    File.Delete(nomineeAadharPath);
                }

                using (var fs = new FileStream(nomineeAadharPath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.MgainNomineeAadharFile.CopyTo(fs);
                }
                mGainDetails.MgainNomineeAadhar = nomineeAadharPath;
            }
            else mGainDetails.MgainNomineeAadhar = null;

            if (addMGainDetails.MgainNomineeBirthCertificateFile is not null)
            {
                var birthCertificate = addMGainDetails.MgainNomineeBirthCertificateFile.FileName;
                var birthCertificatePath = Path.Combine(folderPath, birthCertificate);

                if (File.Exists(birthCertificatePath))
                {
                    File.Delete(birthCertificatePath);
                }

                using (var fs = new FileStream(birthCertificatePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    addMGainDetails.MgainNomineeBirthCertificateFile.CopyTo(fs);
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

            if (mgain.MgainPlotno is not null)
                mgain.MgainPlotno = mgain.MgainPlotno.Trim();

            if (mgain.Mgain2ndplotno is not null)
                mgain.Mgain2ndplotno = mgain.Mgain2ndplotno.Trim();

            var mGain1stPlot = new TblPlotMaster();
            var mGain2ndPlot = new TblPlotMaster();
            var firstPlot = new TblPlotMaster();
            var secondPlot = new TblPlotMaster();

            if (mgain.MgainProjectname is not null && mgain.MgainPlotno is not null)
                mGain1stPlot = await _mGainRepository.GetPlotByProjectAndPlotNo(mgain.MgainProjectname, mgain.MgainPlotno);

            if (mgain.Mgain2ndprojectname is not null && mgain.Mgain2ndplotno is not null)
                mGain2ndPlot = await _mGainRepository.GetPlotByProjectAndPlotNo(mgain.Mgain2ndprojectname, mgain.Mgain2ndplotno);

            if (updateMGainDetails.MGain1stPlotId != 0)
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
                            var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, false, true, false, false, true, true, false, false, false);

                            await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                            updateMGain = assignPlot.Item1;
                        }
                    }
                    else
                        updateMGain = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, null, null, null, null, false, false, false, false, false, false, false, true, true, false, false).Item1;

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
                    if ((mgain.MgainPlotno is not null && mgain.MgainProjectname is not null) && (mgain.Mgain2ndplotno is not null && mgain.Mgain2ndprojectname is not null))
                    {
                        var amount = updateMGain.MgainInvamt - firstPlot.Available_PlotValue;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, true, true, false, true, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
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
                    else if (mgain.MgainPlotno is null && mgain.MgainProjectname is null && mgain.Mgain2ndplotno is null && mgain.Mgain2ndprojectname is null)
                    {
                        var amount = updateMGain.MgainInvamt - firstPlot.Available_PlotValue;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, false, false, true, false, true, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
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
                                var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, true, false, false, false, false, true);

                                await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                                updateMGain = assignPlot.Item1;
                            }
                        }
                        else
                            updateMGain = AssignPlot(updateMGain, mgain, 0, null, null, null, null, false, false, false, false, false, false, false, true, false, false, false).Item1;
                    }
                    else if (updateMGainDetails.MgainProjectname != mgain.MgainProjectname || firstPlot.PlotNo != mgain.MgainPlotno)
                    {
                        var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, true, false, false, false, false, true);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
                    else
                        updateMGain = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, null, null, null, null, false, false, false, false, false, false, false, true, false, false, false).Item1;
                }
                else
                {
                    if (mgain.MgainPlotno is not null && mgain.MgainProjectname is not null)
                    {
                        if (mgain.MgainProjectname != updateMGainDetails.MgainProjectname || mgain.MgainPlotno != firstPlot.PlotNo)
                        {
                            var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, null, false, false, true, false, false, true, false, false, false, false, false);

                            await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                            updateMGain = assignPlot.Item1;
                        }
                        else if (mgain.MgainProjectname == updateMGainDetails.MgainProjectname && mgain.MgainPlotno == firstPlot.PlotNo)
                        {
                            if (updateMGain.MgainInvamt == mgain.MgainInvamt)
                            {
                                updateMGain = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, null, false, false, false, false, false, false, false, true, false, false, false).Item1;
                            }
                            else
                            {
                                var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, null, false, false, true, false, false, true, false, false, false, false, false);

                                await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                                updateMGain = assignPlot.Item1;
                            }
                        }
                    }
                    else
                    {
                        var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, null, null, false, false, false, false, false, true, false, false, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;
                    }
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

            if (updateMGain.MgainRedemamt > 0 && updateMGain.MgainRedemamt <= updateMGain.MgainInvamt)
            {
                if (updateMGain.Mgain2ndprojectname is not null && updateMGain.Mgain2ndplotno is not null)
                {
                    if (updateMGain.MgainRedemamt > updateMGain.Mgain2ndallocatedsqftamt)
                    {
                        var amount = updateMGain.MgainInvamt - updateMGain.MgainRedemamt;
                        updateMGain.MgainInvamt = amount;

                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, true, true, false, false, false, true, false, false, false, false, true);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;

                        if (mgain.MgainRedemamt is not null)
                            updateMGain.MgainRedemamt += updateMGain.MgainRedemamt;
                    }
                    else
                    {
                        //if redem amount is is more than 0 and less than second allocated amount than assign first same and reassign second plot of remaining amount
                        var amount = updateMGain.MgainInvamt - mgain.MgainAllocatedsqftamt - updateMGainDetails.MgainRedemamt;
                        updateMGain.MgainInvamt = updateMGain.MgainInvamt - updateMGain.MgainRedemamt;
                        var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, true, false, false, false, false, true, true, false, false, false);

                        await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                        updateMGain = assignPlot.Item1;

                        if (mgain.MgainRedemamt is not null)
                            updateMGain.MgainRedemamt += updateMGain.MgainRedemamt;
                    }
                }
                else
                {
                    var amount = updateMGain.MgainInvamt - updateMGain.MgainRedemamt;
                    updateMGain.MgainInvamt = amount;
                    var assignPlot = AssignPlot(updateMGain, mgain, amount, firstPlot, null, mGain1stPlot, null, true, false, false, false, false, true, false, false, false, false, false);

                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                    updateMGain = assignPlot.Item1;

                    if (mgain.MgainRedemamt is not null)
                        updateMGain.MgainRedemamt += updateMGain.MgainRedemamt;
                }
            }

            if (updateMGain.MgainIsclosed is true)
            {
                if (updateMGain.Mgain2ndprojectname is not null && updateMGain.Mgain2ndplotno is not null)
                {
                    updateMGain.MgainRedemdate = DateTime.Now.Date;
                    updateMGain.MgainRedemamt = updateMGain.MgainInvamt;
                    var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, secondPlot, mGain1stPlot, mGain2ndPlot, false, false, true, true, false, false, false, true, true, false, false);

                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                    updateMGain = assignPlot.Item1;
                }
                else
                {
                    updateMGain.MgainRedemdate = DateTime.Now.Date;
                    updateMGain.MgainRedemamt = updateMGain.MgainInvamt;
                    var assignPlot = AssignPlot(updateMGain, mgain, updateMGain.MgainInvamt, firstPlot, null, mGain1stPlot, null, false, false, true, false, false, false, false, true, false, false, false);

                    await _mGainRepository.UpdatePlotDetails(assignPlot.Item2);
                    updateMGain = assignPlot.Item1;
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
        public List<TblAccountTransaction> AccountEntry(MGainNonCumulativeMonthlyReportDto MGainNonCumulativeMonthlyReport, int? mGainId, int? mGainUserId, bool? isTdsDeduction, bool? isJournal, DateTime? jvEntryDate, string? jvNarration, bool? isPayment, DateTime? crEntryDate, string? crNarration, string? tdsYear, string? docNo, string? tdsDocNo, int? currancyId, int? companyId, string? transactionType)
        {
            List<TblAccountTransaction> accountTransactions = new List<TblAccountTransaction>();
            if (isJournal is true)
            {
                var creditAccountTransaction = new TblAccountTransaction(jvEntryDate, jvNarration, MGainAccountPaymentConstant.MGainPayment.Journal.ToString(), docNo, 0, MGainNonCumulativeMonthlyReport.InterestAmount, mGainUserId, _mGainRepository.GetAccountByUserId(mGainUserId, null).AccountId, mGainId, companyId, transactionType, currancyId);

                var debitAccountTransaction = new TblAccountTransaction(jvEntryDate, jvNarration, MGainAccountPaymentConstant.MGainPayment.Journal.ToString(), docNo, MGainNonCumulativeMonthlyReport.InterestAmount, 0, mGainUserId, _mGainRepository.GetAccountByUserId(mGainUserId, null).AccountId, mGainId, companyId, transactionType, currancyId);

                accountTransactions.Add(creditAccountTransaction);
                accountTransactions.Add(debitAccountTransaction);
            }

            if (isPayment is true)
            {
                var creditAccountTransaction = new TblAccountTransaction(crEntryDate, crNarration, MGainAccountPaymentConstant.MGainPayment.Payment.ToString(), docNo, 0, MGainNonCumulativeMonthlyReport.InterestAmount, mGainUserId, _mGainRepository.GetAccountByUserId(0, crNarration).AccountId, mGainId, companyId, transactionType, currancyId);

                var debitAccountTransaction = new TblAccountTransaction(crEntryDate, crNarration, MGainAccountPaymentConstant.MGainPayment.Payment.ToString(), docNo, MGainNonCumulativeMonthlyReport.InterestAmount, 0, mGainUserId, _mGainRepository.GetAccountByUserId(mGainUserId, null).AccountId, mGainId, companyId, transactionType, currancyId);

                if (isTdsDeduction is true)
                {
                    var creditTDCAccountTransaction = new TblAccountTransaction(crEntryDate, crNarration, MGainAccountPaymentConstant.MGainPayment.Journal.ToString(), tdsDocNo, 0, MGainNonCumulativeMonthlyReport.InterestAmount, mGainUserId, _mGainRepository.GetAccountByUserId(0, tdsYear).AccountId, mGainId, companyId, transactionType, currancyId);

                    var debitTDCAccountTransaction = new TblAccountTransaction(crEntryDate, crNarration, MGainAccountPaymentConstant.MGainPayment.Journal.ToString(), tdsDocNo, MGainNonCumulativeMonthlyReport.InterestAmount, 0, mGainUserId, _mGainRepository.GetAccountByUserId(mGainUserId, null).AccountId, mGainId, companyId, transactionType, currancyId);

                    accountTransactions.Add(creditTDCAccountTransaction);
                    accountTransactions.Add(debitTDCAccountTransaction);
                }

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
        public (TblMgaindetail, List<TblPlotMaster>) AssignPlot(TblMgaindetail updateMGain, TblMgaindetail mgain, decimal? invAmount, TblPlotMaster firstPlot, TblPlotMaster secondPlot, TblPlotMaster mGain1stPlot, TblPlotMaster mGain2ndPlot, bool? isFirst, bool? isSecond, bool? isMGainFirst, bool? isMGainSecond, bool? isFirstAssignFull, bool? isAssignFirst, bool? isAssignSecond, bool? isAssignSameFirst, bool? isAssignSameSecond, bool? isNotFirst, bool? isNotSecond)
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

            if (isNotFirst is true)
            {
                updateMGain.MgainProjectname = null;
                updateMGain.MgainPlotno = null;
                updateMGain.MgainAllocatedsqft = null;
                updateMGain.MgainAllocatedsqftamt = null;
                updateMGain.MgainTotalsqft = null;
                updateMGain.MgainTotalplotamt = null;
            }

            if (isNotSecond is true)
            {
                updateMGain.Mgain2ndprojectname = null;
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
