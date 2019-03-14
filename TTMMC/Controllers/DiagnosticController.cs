using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTMMC.Models.ViewModels;
using TTMMC.Services;

namespace TTMMC.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly DBContext _dB;

        public DiagnosticController(DBContext dB)
        {
            _dB = dB ?? throw new ArgumentNullException(nameof(dB));
        }

        public async Task<IActionResult> Index()
        {
            var machines = MachinesService.GetStaticMachines().ToList() ?? new List<Models.IMachine>();
            var kars = await _dB.KeepAliveRequests.ToListAsync();
            var servStarted = new Dictionary<string, bool>();
            servStarted.Add("Barcode", Barcode.Started);
            servStarted.Add("Database", DBContext.Started);
            servStarted.Add("KeepAlive", KeepAlive.Started);
            servStarted.Add("LayoutListener", LayoutListener.Started);
            servStarted.Add("Machines", MachinesService.Started);
            servStarted.Add("Utilities", Utilities.Started);

            var model = new DiagnosticModel
            {
                Machines = machines,
                KeepAliveRequests = kars,
                Services = servStarted
            };

            return View(model);
        }
    }
}