namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class JobOffersViewModel : IMapFrom<JobOffer>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Position { get; set; }

        public string City { get; set; }

        public DateTime CreatedOn { get; set; }

        public string EmployerName { get; set; }

        public string JobSectorName { get; set; }

        public string JobLevelName { get; set; }

        public string JobLevelId { get; set; }

        public string JobDetails
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (this.Salary.HasValue)
                {
                    sb.Append($"Salary: {this.Salary.Value:f2}€; ");
                }

                if (this.JobTypes.Count() > 0)
                {
                    sb.Append(string.Join("; ", this.JobTypes));
                }

                return sb.ToString();
            }
        }

        public decimal? Salary { get; set; }

        public IEnumerable<string> JobTypes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobOffer, JobOffersViewModel>()
                .ForMember(jovm => jovm.JobTypes, options =>
                   {
                       options.MapFrom(jo => jo.JobTypes.Select(jt => jt.JobType.Name).ToList());
                   })
                .ForMember(jovm => jovm.CreatedOn, options =>
                   {
                       options.MapFrom(jo => jo.CreatedOn);
                   });
        }
    }
}
