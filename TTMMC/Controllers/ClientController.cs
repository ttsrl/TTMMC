﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTMMC.Models.DBModels;
using TTMMC.Models.ViewModels;
using TTMMC.Services;

namespace TTMMC.Controllers
{
    public class ClientController : Controller
    {

        private readonly DBContext _dB;

        public ClientController(DBContext dB)
        {
            _dB = dB ?? throw new ArgumentNullException(nameof(dB));
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _dB.Clients.OrderByDescending(c => c.Id).ToListAsync();
            return View(new IndexClientModel { Clients = clients });
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromServices] Barcode barcode, [FromServices] Utilities _utils, NewClientModel model)
        {
            if (ModelState.IsValid)
            {
                //check exist
                var cC = await _dB.Clients.Where(c => c.VAT == model.VAT.ToUpper() || c.FiscalCode == model.FiscalCode.ToUpper()).FirstOrDefaultAsync();
                if (!(cC is Client)) //if not exist
                {
                    var codes = await _dB.Clients.Select(c => c.Code).ToListAsync();
                    var newc = new Client
                    {
                        Code = barcode.CreateNewEan8(codes),
                        Name = model.Name,
                        FiscalCode = model.FiscalCode,
                        VAT = model.VAT,
                        Phone = model.Phone,
                        Email = model.Email,
                        PEC = model.PEC,
                        State = model.State,
                        Province = model.Province,
                        Town = model.Town,
                        Address = model.AddressStreetMode + " " + model.Address + ", " +  model.AddressNumber
                    };
                    _dB.Clients.Add(newc);
                    barcode.GenerateEan8(newc.Code);
                    await _dB.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Error", new { id = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != 0)
            {
                var client = await _dB.Clients.FindAsync(id);
                if (client is Client)
                {
                    return View(client);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client model)
        {
            var client = await _dB.Clients.FindAsync(model.Id);
            if(client is Client)
            {
                client.Address = model.Address;
                client.Email = model.Email;
                client.FiscalCode = model.FiscalCode;
                client.Name = model.Name;
                client.PEC = model.PEC;
                client.Phone = model.Phone;
                client.Province = model.Province;
                client.State = model.State;
                client.Town = model.Town;
                client.VAT = model.VAT;

                await _dB.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Error", new { id = 2 });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var client = await _dB.Clients.FindAsync(id);
                if (client is Client)
                {
                    var lavok = await _dB.Layouts.Include(l => l.Client).Where(l => l.Client.Id == id).CountAsync();
                    if (lavok == 0)
                    {
                        var moulds = await _dB.Moulds.ToListAsync();
                        foreach (var m in moulds)
                        {
                            if (m.DefaultClient == client)
                            {
                                m.DefaultClient = null;
                            }
                        }
                        _dB.Clients.Remove(client);
                        await _dB.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index", "Error", new { id = 17 });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewModule(int id)
        {
            if (id != 0)
            {
                var client = await _dB.Clients.FindAsync(id);
                if (client is Client)
                {
                    return RedirectToAction("ClientModule", "Pdf", new { id });
                }
            }
            return RedirectToAction("Index");
        }
    }
}