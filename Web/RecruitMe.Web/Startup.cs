namespace RecruitMe.Web
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RecruitMe.Common;
    using RecruitMe.Data;
    using RecruitMe.Data.Common;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Data.Seeding;
    using RecruitMe.Services;
    using RecruitMe.Services.Data;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Services.Messaging;
    using RecruitMe.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.Strict;
                    });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(8);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });
            services.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.configuration["SendGridKey"]));
            services.AddTransient<IApplicationUsersService, ApplicationUsersService>();
            services.AddTransient<ICandidatesService, CandidatesService>();
            services.AddTransient<IEmployersService, EmployersService>();
            services.AddTransient<IJobSectorsService, JobSectorsService>();
            services.AddTransient<IDocumentsService, DocumentsService>();
            services.AddTransient<IDocumentCategoriesService, DocumentCategoriesService>();
            services.AddTransient<IFileExtensionsService, FileExtensionsService>();
            services.AddTransient<IFileDownloadService, FileDownloadService>();
            services.AddTransient<IJobLevelsService, JobLevelsService>();
            services.AddTransient<IJobTypesService, JobTypesService>();
            services.AddTransient<ILanguagesService, LanguagesService>();
            services.AddTransient<ISkillsService, SkillsService>();
            services.AddTransient<IJobOffersService, JobOffersService>();
            services.AddTransient<IJobApplicationService, JobApplicationsService>();
            services.AddTransient<IJobApplicationStatusesService, JobApplicationStatusesService>();

            var account = new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret);
            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);
            services.AddSingleton<IMimeMappingService>(new MimeMappingService(new FileExtensionContentTypeProvider()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();

                        endpoints.MapGet("/Identity/Account/Manage/", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Manage/Email", true)));
                        endpoints.MapPost("/Identity/Account/Manage/", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Manage/Email", true)));
                        endpoints.MapGet("/Identity/Account/Manage/Index", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Manage/Email", true)));
                        endpoints.MapPost("/Identity/Account/Manage/Index", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Manage/Email", true)));
                        endpoints.MapGet("/Identity/Account/Manage/TwoFactorAuthentication", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapPost("/Identity/Account/Manage/TwoFactorAuthentication", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapGet("/Identity/Account/Manage/PersonalData", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapPost("/Identity/Account/Manage/PersonalData", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapGet("/Identity/Account/Manage/EnableAuthenticator", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapPost("/Identity/Account/Manage/EnableAuthenticator", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapGet("/Identity/Account/Manage/DownloadPersonalData", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapPost("/Identity/Account/Manage/DownloadPersonalData", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapGet("/Identity/Account/Manage/DeletePersonalData", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapPost("/Identity/Account/Manage/DeletePersonalData", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapGet("/Identity/Account/Manage/ResetAuthenticator", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                        endpoints.MapPost("/Identity/Account/Manage/ResetAuthenticator", context => Task.Factory.StartNew(() => context.Response.Redirect(GlobalConstants.IdentityRedirectPath, true)));
                    });
        }
    }
}
