using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbrahimPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public abstract class AdminBaseController : Controller
    {
    }
}

