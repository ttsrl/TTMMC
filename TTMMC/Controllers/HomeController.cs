using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TTMMC.Models.ViewModels;

namespace TTMMC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            HttpContext.Session.SetString("Session", "Active");
            return View();
        }

        public IActionResult Error(int id = 500)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Code = id });
        }

    }
}
