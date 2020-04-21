namespace RecruitMe.Web.ViewModels.Candidates
{
    using System.Linq;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class ProfileViewModel : IMapFrom<Candidate>, IHaveCustomMappings
    {
        public bool IsProfileCreated { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string Education { get; set; }

        public string Languages { get; set; }

        public string Skills { get; set; }

        public string AboutMe { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Candidate, ProfileViewModel>()
                .ForMember(ivm => ivm.Name, options =>
                   {
                       options.MapFrom(c => c.FirstName + " " + c.LastName);
                   })
                .ForMember(ivm => ivm.Languages, options =>
                   {
                       options.MapFrom(c => string.Join(", ", c.Languages.Select(l => l.Language.Name).ToList()));
                   })
                .ForMember(ivm => ivm.Skills, options =>
                {
                    options.MapFrom(c => string.Join(", ", c.Skills.Select(l => l.Skill.Name).ToList()));
                });
        }
    }
}
