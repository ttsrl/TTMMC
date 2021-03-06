﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTMMC.Models.DBModels;
using TTMMC.Models.ViewModels;
using TTMMC.Services;
using TTMMC.Utils;

namespace TTMMC.Controllers
{
    public class MaterialController : Controller
    {

        private readonly DBContext _dB;

        public MaterialController(DBContext dB)
        {
            _dB = dB ?? throw new ArgumentNullException(nameof(dB));
        }

        public async Task<IActionResult> Index()
        {
            var ms = await _dB.Materials.OrderByDescending(mx => mx.Id).ToListAsync();
            var m = new IndexMaterialModel
            {
                Materials = ms
            };
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(string name, string description)
        {
            if (!string.IsNullOrEmpty(name))
            {
                //check exist
                var mC = await _dB.Materials.Where(m => m.Name == name.ToFirstCharUpper()).FirstOrDefaultAsync();
                if (!(mC is Material)) //if not exist 
                {
                    var material = new Material
                    {
                        Name = name,
                        Description = description
                    };
                    await _dB.Materials.AddAsync(material);
                    await _dB.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "Error", new { id = 7 });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, string description)
        {
            if (id != 0 && !string.IsNullOrEmpty(name))
            {
                var material = await _dB.Materials.FindAsync(id);
                if (material is Material)
                {
                    var mC = await _dB.Materials.Where(m => m.Name == name.ToFirstCharUpper() && m.Id != id).CountAsync();
                    if (mC == 0)
                    {
                        material.Name = name;
                        material.Description = description;
                        await _dB.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index", "Error", new { id = 8 });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var material = await _dB.Materials.FindAsync(id);
            if (material is Material)
            {
                var mixok = await _dB.MixtureItems.Include(l => l.Material).Where(l => l.Material.Id == id).CountAsync();
                if (mixok == 0)
                {
                    _dB.Materials.Remove(material);
                    await _dB.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "Error", new { id = 21 });
            }
            return RedirectToAction("Index");
        }


    }
}