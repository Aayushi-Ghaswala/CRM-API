using AutoMapper;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Loan_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.Loan_Module;

namespace CRM_api.Services.Services.Business_Module.Loan_Module
{
    public class LoanMasterService : ILoanMasterService
    {
        private readonly ILoanMasterRepository _loanMasterRepository;
        private readonly IMapper _mapper;
        public LoanMasterService(ILoanMasterRepository loanMasterRepository, IMapper mapper)
        {
            _loanMasterRepository = loanMasterRepository;
            _mapper = mapper;
        }

        #region Get All Loan Details
        public async Task<ResponseDto<LoanMasterDto>> GetLoanDetailsAsync(int page)
        {
            var loanDetails = await _loanMasterRepository.GetLoanDetails(page);
            var mapLoanDetails = _mapper.Map<ResponseDto<LoanMasterDto>>(loanDetails);

            return mapLoanDetails;
        }
        #endregion

        #region Get All Loan Detail By Id
        public async Task<LoanMasterDto> GetLoanDetailByIdAsync(int id)
        {
            var loanDetails = await _loanMasterRepository.GetLoanDetailById(id);
            var mapLoanDetail = _mapper.Map<LoanMasterDto>(loanDetails);

            return mapLoanDetail;
        }
        #endregion

        #region Add Loan Detail
        public Task<int> AddLoanDetailAsync(AddLoanMasterDto loanMasterDto)
        {
            var addLoanDetail = _mapper.Map<TblLoanMaster>(loanMasterDto);
            addLoanDetail.Date = DateTime.Now;
            return _loanMasterRepository.AddLoanDetail(addLoanDetail);
        }
        #endregion

        #region Update Loan Detail
        public Task<int> UpdateLoanDetailAsync(UpdateLoanMasterDto loanMasterDto)
        {
            var addLoanDetail = _mapper.Map<TblLoanMaster>(loanMasterDto);

            return _loanMasterRepository.UpdateLoanDetail(addLoanDetail);
        }
        #endregion

        #region Deactivate Loan Detail
        public Task<int> DeactivateLoanDetailAsync(int id)
        {
            return _loanMasterRepository.DeactivateLoanDetail(id);
        }
        #endregion
    }
}
