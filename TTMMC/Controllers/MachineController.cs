using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TTMMC.Models;
using TTMMC.Models.ViewModels;
using TTMMC.Services;

namespace TTMMC.Controllers
{
    public class MachineController : Controller
    {
        private readonly MachinesService _machines;

        public MachineController([FromServices] MachinesService machines)
        {
            _machines = machines;
        }

        public IActionResult Index()
        {
            var machines = _machines.GetMachines().ToList();
            //reorder 
            var onlineM = new List<IMachine>();
            var offlineM = new List<IMachine>();
            foreach (var m in machines)
            {
                if(m.Status == MachineStatus.Online)
                    onlineM.Add(m);
                else
                    offlineM.Add(m);
            }
            onlineM = onlineM.OrderBy(m => m.Id).ToList();
            offlineM = offlineM.OrderBy(m => m.Id).ToList();
            machines.Clear();
            machines.AddRange(onlineM);
            machines.AddRange(offlineM);
            var model = new HomeModel
            {
                Machines = machines
            };
            return View(model);
        }

        public IActionResult Details(int id)
        {
            if(id != 0)
            {
                var mdb = _machines.GetMachineById(id);
                if (mdb is IMachine)
                {
                    var m = new MachineDetailsModel
                    {
                        Machine = mdb
                    };
                    return View(m);
                }

            }
            return RedirectToAction("Index");
        }

    }
}