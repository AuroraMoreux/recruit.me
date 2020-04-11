namespace RecruitMe.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class FileDownloadService : IFileDownloadService
    {
        public async Task<byte[]> DownloadFile(string url)
        {
            using HttpClient client = new HttpClient();
            using HttpResponseMessage result = await client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}
