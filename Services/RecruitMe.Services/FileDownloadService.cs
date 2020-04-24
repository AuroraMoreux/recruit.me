namespace RecruitMe.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class FileDownloadService : IFileDownloadService
    {
        public async Task<byte[]> DownloadFileAsync(string url)
        {
            using var client = new HttpClient();
            using var result = await client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}
