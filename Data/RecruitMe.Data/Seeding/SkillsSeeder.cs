namespace RecruitMe.Data.Seeding
{
    using System;
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

            await dbContext.Skills.AddAsync(new Skill { Name = "Active Listening" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Critical Thinking" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Decision-Making" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Interpersonal Communication" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Management" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Leadership" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Organization" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Public Speaking" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Problem Solving" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Teamwork" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Collaboration" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Time Management" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Persuasion" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Creativity" });
            await dbContext.Skills.AddAsync(new Skill { Name = "People Management" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Scheduling" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Customer-first Approach" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Customer Needs Analysis" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Schedule Management" });
            await dbContext.Skills.AddAsync(new Skill { Name = "MS Office" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Salesforce" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Referral Marketing" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Product Knowledge" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Contract Negotiation" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Budgeting" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Business Analysis" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Corporate Communications" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Financial Modelling" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Forecasting" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Cashier Skills" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Patient Assessment" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Patient Care" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Urgent and Emergency Care" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Record-Keeping" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Medicine Administration" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Rehabilitation Therapy" });
            await dbContext.Skills.AddAsync(new Skill { Name = "C#" });
            await dbContext.Skills.AddAsync(new Skill { Name = ".NET" });
            await dbContext.Skills.AddAsync(new Skill { Name = "PHP" });
            await dbContext.Skills.AddAsync(new Skill { Name = "JavaScript" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Python" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Ruby" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Data Structures" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Machine Learning" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Debugging" });
            await dbContext.Skills.AddAsync(new Skill { Name = "UX/UI" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Front-End Development" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Back-End Developent" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Cloud Management" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Agile" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Testing" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Troubleshooting" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Project Launch" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Workflow Development" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Technical Report Writing" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Data Visualization" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Project Lifecycle Management" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Artificial Intelligence" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Mobile Application Development" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Natural Language Processing" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Scientific Computing" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Game Development" });
            await dbContext.Skills.AddAsync(new Skill { Name = "CAD" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Design" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Audio Production" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Video Production" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Animation" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Prototyping" });
            await dbContext.Skills.AddAsync(new Skill { Name = "SEO/SEM" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Social Media Marketing and Paid Social Media Advertising" });
            await dbContext.Skills.AddAsync(new Skill { Name = "CMS Tools" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Graphic Design Skills" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Email Marketing" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Print Design" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Translation" });
            await dbContext.Skills.AddAsync(new Skill { Name = "Journalism" });
        }
    }
}
