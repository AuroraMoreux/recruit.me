namespace RecruitMe.Services
{
    using System.Threading.Tasks;

    public interface IFileDownloadService
    {
        Task<byte[]> DownloadFile(string url);
    }
}
