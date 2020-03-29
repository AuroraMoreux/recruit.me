namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;

    public interface IDocumentsService
    {
        int GetCount();

        IEnumerable<T> GetAllCandidateDocuments<T>(string userId, int page, int perPage);
    }
}
