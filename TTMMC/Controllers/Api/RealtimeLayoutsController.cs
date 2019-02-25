using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTMMC.Models;
using TTMMC.Models.DBModels;
using TTMMC.Services;

namespace TTMMC.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/RealtimeLayouts")]
    public class RealtimeLayoutsController : ControllerBase
    {
        private readonly DBContext _dB;
        private readonly MachinesService _machines;

        public RealtimeLayoutsController(DBContext dB, MachinesService machines)
        {
            _dB = dB ?? throw new ArgumentNullException(nameof(dB));
            _machines = machines ?? throw new ArgumentNullException(nameof(machines));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ls = await _dB.Layouts.Include(l => l.LayoutActRecords).Select(l => new {l.Barcode, l.Status, l.LayoutActRecords.Count, l.Machine }).Where(l => l.Status == Models.DBModels.Status.Recording || l.Status == Models.DBModels.Status.Finished).ToListAsync();
            if (ls != null && ls.Count > 0)
            {
                var out_ = new Dictionary<string, object>();
                foreach (var l in ls)
                {
                    var machine = _machines.GetMachineById(l.Machine);
                    var available = (machine is IMachine && machine.Recording == false && machine.GetStatus() == MachineStatus.Online) ? true : false; 
                    out_.Add(l.Barcode, new { status = Enum.GetName(typeof(Status),l.Status), logs = l.Count, machineStatus = available });
                }
                return Ok(out_);
            }
            return NotFound(new { });
        }
    }
}
