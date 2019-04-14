﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TTMMC.Models;
using TTMMC.Models.DBModels;
using TTMMC.Models.ViewModels;
using TTMMC.Services;

namespace TTMMC.Controllers
{
    public class LayoutController : Controller
    {
        private readonly LayoutListener _lListener;
        private readonly DBContext _dB;
        private readonly MachinesService _machines;

        public LayoutController([FromServices] MachinesService machines, DBContext dB, [FromServices] LayoutListener layoutListener)
        {
            _machines = machines;
            _dB = dB ?? throw new ArgumentNullException(nameof(dB));
            _lListener = layoutListener;
        }

        public async Task<IActionResult> Index(int clp)
        {
            var nPerPage = 25;
            var ids = await _dB.Layouts.Select(lc => lc.Id).OrderByDescending(ll => ll).ToListAsync();
            clp = (clp == 0) ? ids.First() : clp;
            var l = await _dB.Layouts
                .Include(ll => ll.Client)
                .Include(ll => ll.Master)
                .Include(ll => ll.Mixture)
                    .ThenInclude(m => m.Items)
                .Include(ll => ll.Mould)
                .Include(ll => ll.LayoutActRecords)
                .Where(ll => ll.Id < clp + 1)
                .OrderByDescending(ll => ll.Id)
                .Take(nPerPage)
                .ToListAsync();
            var c = await _dB.Clients.ToListAsync();

            var decpgs = (decimal)(ids.Count) / (decimal)(nPerPage);
            var nPages = (int)Math.Ceiling(decpgs);

            var model = new IndexLayoutModel
            {
                Layouts = l,
                Machines = _machines.GetMachines().ToList(),
                LayoutsIds = ids,
                NPages = nPages,
                NPerPage = nPerPage,
                ActualId = l.First().Id
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> New(int mould)
        {
            var clients = await _dB.Clients.ToListAsync();
            var masters = await _dB.Masters.ToListAsync();
            var moulds = await _dB.Moulds
                .Include(m => m.DefaultClient)
                .Include(m => m.DefaultMixture)
                .ToListAsync();
            var mixtures = await _dB.Mixtures
                .Include(m => m.Items)
                .ThenInclude(m => m.Material)
                .ToListAsync();
            var packaging = Enum.GetValues(typeof(Package)).Cast<Package>().ToDictionary(t => (int)t, t => t.ToString());
            var defM = moulds.FirstOrDefault(m => m.Id == mould);
            var model = new NewLayoutViewModel
            {
                Machines = _machines.GetMachines().ToList(),
                Clients = clients,
                Moulds = moulds,
                Masters = masters,
                Mixtures = mixtures,
                Packaging = packaging,
                DefaultClient = (defM is Mould) ? defM.DefaultClient : null,
                DefaultMixture = (defM is Mould) ? defM.DefaultMixture : null,
                DefaultMould = (defM is Mould) ? defM : null,
                DefaultMaster = (defM is Mould) ? defM.DefaultMaster : null
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromServices] Barcode barcode, [FromServices] Utilities _utils, NewLayoutModel model)
        {
            if (model.Client > 0 && model.Mould > 0 && model.Mixture > 0 && model.Quantity > 0 && model.Machine > 0)
            {
                var client = await _dB.Clients.FirstOrDefaultAsync(c => c.Id == model.Client);
                var mould = await _dB.Moulds.FirstOrDefaultAsync(c => c.Id == model.Mould);
                var master = await _dB.Masters.FirstOrDefaultAsync(c => c.Id == model.Master);
                var mixture = await _dB.Mixtures.FirstOrDefaultAsync(c => c.Id == model.Mixture);
                var machine = _machines.GetMachineById(model.Machine);
                if (client is Client && mould is Mould && mixture is Mixture && machine is IMachine)
                {
                    var codes = await _dB.Layouts.Select(c => c.Barcode).ToListAsync();
                    var layout = new Layout
                    {
                        Barcode = barcode.CreateNewEan13(codes),
                        Client = client,
                        Mould = mould,
                        Master = master,
                        Mixture = mixture,
                        Quantity = model.Quantity,
                        Machine = model.Machine,
                        Notes = model.Notes,
                        Packaging = model.Packaging,
                        PackagingQuantity = model.PackagingCount,
                        Status = Status.Waiting,
                        Minced = (model.MincedCheck == 1) ? model.Minced : null,
                        Humidification = (model.HumidifiedCheck == 1) ? TimeSpan.FromMinutes(model.Humidified) : TimeSpan.Zero,
                        Start = model.Start
                    };
                    _dB.Layouts.Add(layout);
                    barcode.GenerateEan13(layout.Barcode);
                    await _dB.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index", "Error", new { id = 11 });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(id != 0)
            {
                var layout = await _dB.Layouts
                    .Include(l => l.Client)
                    .Include(l => l.Master)
                    .Include(l => l.Mixture)
                        .ThenInclude(m => m.Items)
                            .ThenInclude(m => m.Material)
                    .Include(l => l.Mould)
                    .FirstOrDefaultAsync(l => l.Id == id);
                var clients = await _dB.Clients.ToListAsync();
                var masters = await _dB.Masters.ToListAsync();
                var moulds = await _dB.Moulds
                    .Include(m => m.DefaultClient)
                    .Include(m => m.DefaultMixture)
                    .ToListAsync();
                var mixtures = await _dB.Mixtures
                    .Include(m => m.Items)
                    .ThenInclude(m => m.Material)
                    .ToListAsync();
                var packaging = Enum.GetValues(typeof(Package)).Cast<Package>().ToDictionary(t => (int)t, t => t.ToString());
                if (layout is Layout && layout.Status == Status.Waiting)
                {
                    var m = new EditLayoutViewModel
                    {
                        Layout = layout,
                        Clients = clients,
                        Machines = _machines.GetMachines().ToList(),
                        Masters = masters,
                        Mixtures = mixtures,
                        Moulds = moulds,
                        Packaging = packaging
                    };
                    return View(m);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewLayoutModel model)
        {
            if (ModelState.IsValid)
            {
                var layout = await _dB.Layouts
                    .Include(l => l.Client)
                    .Include(l => l.Master)
                    .Include(l => l.Mixture)
                    .Include(l => l.Mould)
                    .Where(l => l.Id == id)
                    .FirstOrDefaultAsync();
                if (layout is Layout && model.Client > 0 && model.Mould > 0 && model.Mixture > 0 && model.Quantity > 0 && model.Machine > 0)
                {
                    var client = await _dB.Clients.FirstOrDefaultAsync(c => c.Id == model.Client);
                    var mould = await _dB.Moulds.FirstOrDefaultAsync(c => c.Id == model.Mould);
                    var master = await _dB.Masters.FirstOrDefaultAsync(c => c.Id == model.Master);
                    var mixture = await _dB.Mixtures.FirstOrDefaultAsync(c => c.Id == model.Mixture);
                    var machine = _machines.GetMachineById(model.Machine);
                    if (client is Client && mould is Mould && mixture is Mixture && machine is IMachine)
                    {
                        layout.Client = client;
                        layout.Mould = mould;
                        layout.Master = master;
                        layout.Mixture = mixture;
                        layout.Quantity = model.Quantity;
                        layout.Machine = model.Machine;
                        layout.Notes = model.Notes;
                        layout.Packaging = model.Packaging;
                        layout.PackagingQuantity = model.PackagingCount;
                        layout.Minced = (model.MincedCheck == 1) ? model.Minced : null;
                        layout.Humidification = (model.HumidifiedCheck == 1) ? TimeSpan.FromMinutes(model.Humidified) : TimeSpan.Zero;
                        layout.Start = model.Start;
                        await _dB.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index", "Error", new { id = 12 });
        }

        [HttpGet]
        public async Task<IActionResult> Start(int id)
        {
            if(id != 0)
            {
                var layout = await _dB.Layouts.Where(l => l.Status == Status.Waiting).FirstOrDefaultAsync(l => l.Id == id);
                if(layout is Layout)
                {
                    var m = _machines.GetMachineById(layout.Machine);
                    if (m is IMachine && m.Recording == false && m.GetStatus() == MachineStatus.Online)
                    {
                        if (!_lListener.Contains(layout))
                            _lListener.Add(layout);
                        var ll = _lListener.GetLayoutListenItem(layout);
                        ll.TimerTick = 2;
                        ll.Rounded = true;
                        ll.RoundedPrecision = 2;
                        await ll.Start();
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index", "Error", new { id = 22 });
        }

        [HttpGet]
        public async Task<IActionResult> remove(int id)
        {
            var ff = await _dB.LayoutsRecordsFields.ToListAsync();
            var f = await _dB.LayoutsRecords.ToListAsync();
            var l = await _dB.Layouts.FirstOrDefaultAsync();
            l.Status = Status.Waiting;
            _dB.LayoutsRecordsFields.RemoveRange(ff);
            _dB.LayoutsRecords.RemoveRange(f);
            await _dB.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Stop(int id)
        {
            if (id != 0)
            {
                var layout = await _dB.Layouts.Where(l => l.Status == Status.Recording).FirstOrDefaultAsync(l => l.Id == id);
                if (layout is Layout)
                {
                    await _lListener.Remove(layout);
                    layout.Status = Status.Stopped;
                    await _dB.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Report(int id)
        {
            if(id != 0)
            {
                var layout = await _dB.Layouts.Where(l => l.Status == Status.Finished || l.Status == Status.Stopped).FirstOrDefaultAsync(l => l.Id == id);
                if(layout is Layout)
                {
                    return RedirectToAction("Report", "Pdf", new { id });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if(id != 0)
            {
                var layout = await _dB.Layouts
                    .Include(l => l.LayoutActRecords)
                        .ThenInclude(lr => lr.Fields)
                    .FirstOrDefaultAsync(l => l.Id == id);
                if (layout is Layout)
                {
                    var fields = layout.LayoutActRecords;
                    foreach(var f in fields)
                    {
                        _dB.LayoutsRecordsFields.RemoveRange(f.Fields);
                    }
                    _dB.LayoutsRecords.RemoveRange(layout.LayoutActRecords);
                    _dB.Layouts.Remove(layout);
                    await _dB.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> empty()
        {
            var layout = await _dB.Layouts.Where(b => b.Barcode == "5844240577527").FirstOrDefaultAsync();
            layout.LayoutActRecords = new List<LayoutRecord>();
            layout.LayoutSetRecord = new LayoutRecord();
            await _dB.SaveChangesAsync();

            var logs = await _dB.LayoutsRecordsFields.ToListAsync();
            _dB.LayoutsRecordsFields.RemoveRange(logs);
            var recs = await _dB.LayoutsRecords.ToListAsync();
            _dB.LayoutsRecords.RemoveRange(recs);
            await _dB.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewModule(int id)
        {
            if (id != 0)
            {
                var layout = await _dB.Layouts.FindAsync(id);
                if (layout is Layout)
                {
                    return RedirectToAction("LayoutModule", "Pdf", new { id });
                }
            }
            return RedirectToAction("Index");
        }

    }
}