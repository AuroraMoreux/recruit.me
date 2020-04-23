namespace RecruitMe.Services.Data.Tests.Common
{
    using System.Reflection;

    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Candidates;

    public class AutoMapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
               typeof(CreateCandidateProfileInputModel).GetTypeInfo().Assembly,
               typeof(Candidate).GetTypeInfo().Assembly);
        }
    }
}
