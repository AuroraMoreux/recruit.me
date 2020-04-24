namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Documents;

    public interface IDocumentsService
    {
        IEnumerable<T> GetAllDocumentsForCandidate<T>(string candidateId);

        string GetDocumentNameById(string documentId);

        Task<string> UploadAsync(UploadInputModel model, string candidateId);

        bool DocumentNameAlreadyExists(string fileName,string candidateId);

        Task<bool> DeleteAsync(string documentId);

        Task<byte[]> DownloadAsync(string documentId);

        bool IsCandidateOwnerOfDocument(string candidateId, string documentId);

        int GetDocumentCountForCandidate(string candidateId);

        T GetDocumentDetails<T>(string documentId);
    }
}
