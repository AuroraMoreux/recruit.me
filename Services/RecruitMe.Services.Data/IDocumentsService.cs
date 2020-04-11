namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Documents;

    public interface IDocumentsService
    {
        IEnumerable<T> GetAllDocumentsForCandidate<T>(string candidateId);

        string GetDocumentNameById(string documentId);

        Task<string> Create(UploadInputModel model, string candidateId);

        bool DocumentNameAlreadyExists(string fileName);

        Task Delete(string documentId);

        Task<byte[]> Download(string documentId);

        bool IsCandidateOwnerOfDocument(string candidateId, string documentId);
    }
}
