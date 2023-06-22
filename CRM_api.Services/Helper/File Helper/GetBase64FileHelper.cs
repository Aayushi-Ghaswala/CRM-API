namespace CRM_api.Services.Helper.File_Helper
{
    public static class GetBase64FileHelper
    {
        public static string? GetBase64File(string path)
        {
            byte[] filContent = File.ReadAllBytes(path);
            var base64File = Convert.ToBase64String(filContent);
            return base64File;
        }
    }
}
