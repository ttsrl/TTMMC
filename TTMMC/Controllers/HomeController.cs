using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TTMMC.Models.ViewModels;
using TTMMC.Services;

namespace TTMMC.Controllers
{
    public class HomeController : Controller
    {
        //per mantenere in vita il keep alive ad ogni richiesta alla home che viene fatta
        public IActionResult Index([FromServices] KeepAlive ka) 
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
