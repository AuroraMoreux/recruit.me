namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DocumentsService : IDocumentsService
    {
        private readonly IDeletableEntityRepository<Document> documentRepository;
        private readonly ICandidatesService candidatesService;

        public DocumentsService(IDeletableEntityRepository<Document> documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public IEnumerable<T> GetAllCandidateDocuments<T>(string userId, int page, int perPage)
        {
            var documents = this.documentRepository
                .All()
                .Where(d => d.Candidate.ApplicationUserId == userId)
                .OrderBy(d => d.CreatedOn)
                .To<T>()
                .Skip(perPage * (page - 1))
                .Take(perPage);

            return documents;
        }

        public int GetCount()
        {
            return this.documentRepository.All().Count();
        }
    }
}
