namespace CRM_api.Services.Dtos.ResponseDto.Generic_Response
{
    public class ResponseDto<T>
    {
        public List<T> Values { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
