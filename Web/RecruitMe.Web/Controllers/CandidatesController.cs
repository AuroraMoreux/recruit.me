namespace RecruitMe.Web.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;

    public class CandidatesController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.CandidateRoleName);

            return this.View();
        }
    }
}
