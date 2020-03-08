using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RecruitMe.Web.Areas.Identity.IdentityHostingStartup))]

namespace RecruitMe.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
