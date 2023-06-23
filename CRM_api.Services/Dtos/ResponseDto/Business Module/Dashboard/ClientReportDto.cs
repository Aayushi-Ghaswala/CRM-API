namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard
{
    public class ClientReportDto<T>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public T ClientInvSnapshot { get; set; }
    }
}
