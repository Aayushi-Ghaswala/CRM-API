namespace CRM_api.Services.Dtos.ResponseDto.User_Module
{
    public class NewUserClientCountDto
    {
        public string Duration { get; set; } = null!;
        public int NewUsersCount { get; set; } = 0;
        public int NewClientCount { get; set; } = 0;

        public NewUserClientCountDto(string duration, int newUsersCount, int newClientCount)
        {
            Duration = duration;
            NewUsersCount = newUsersCount;
            NewClientCount = newClientCount;
        }
    }
}
