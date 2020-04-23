namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    public class SkillsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Skills.Any())
            {
                return;
            }

            var skills = new List<string> { "Active Listening", "Critical Thinking", "Decision-Making", "Interpersonal Communication", "Management", "Leadership", "Organization", "Public Speaking", "Problem Solving", "Teamwork", "Collaboration", "Time Management", "Persuasion", "Creativity", "People Management", "Scheduling", "Customer-first Approach", "Customer Needs Analysis", "Schedule Management", "MS Office", "Salesforce", "Referral Marketing", "Product Knowledge", "Contract Negotiation", "Budgeting", "Business Analysis", "Corporate Communications", "Financial Modelling", "Forecasting", "Cashier Skills", "Patient Assessment", "Patient Care", "Urgent and Emergency Care", "Record-Keeping", "Medicine Administration", "Rehabilitation Therapy", "C#", ".NET", "PHP", "JavaScript", "Python", "Ruby", "Data Structures", "Machine Learning", "Debugging", "UX/UI", "Front-End Development", "Back-End Development", "Cloud Management", "Agile", "Testing", "Troubleshooting", "Project Launch", "Workflow Development", "Technical Report Writing", "Data Visualization", "Project Lifecycle Management", "Artificial Intelligence", "Mobile Application Development", "Natural Language Processing", "Scientific Computing", "Game Development", "CAD", "Design", "Audio Production", "Video Production", "Animation", "Prototyping", "SEO/SEM", "Social Media Marketing and Paid Social Media Advertising", "CMS Tools", "Graphic Design Skills", "Email Marketing", "Print Design", "Translation", "Journalism" };

            foreach (var skill in skills)
            {
                await dbContext.Skills.AddAsync(new Skill
                {
                    Name = skill,
                });
            }
        }
    }
}
