namespace RecruitMe.Services
{
    using Microsoft.AspNetCore.StaticFiles;

    public class MimeMappingService : IMimeMappingService
    {
        private readonly FileExtensionContentTypeProvider contentTypeProvider;

        public MimeMappingService(FileExtensionContentTypeProvider contentTypeProvider)
        {
            this.contentTypeProvider = contentTypeProvider;
        }

        public string Map(string fileName)
        {
            if (!this.contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
