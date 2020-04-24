namespace RecruitMe.Services
{
    using System.Threading.Tasks;

    public interface IFileDownloadService
    {
        Task<byte[]> DownloadFileAsync(string url);
    }
}
