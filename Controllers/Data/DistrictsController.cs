using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MKTS.Data;
using MKTS.Models.Data;

namespace MKTS.Controllers.Data
{
    public class DistrictsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistrictsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Districts
       // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.District;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Districts/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District
                //.Include(d => d.Provinces)
                .FirstOrDefaultAsync(m => m.DistrictID == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // GET: Districts/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["ProvinceName"] = new SelectList(_context.Province, "ProvinceName", "ProvinceName");
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DistrictID,DistrictName,ProvinceName")] District district)
        {
            if (ModelState.IsValid)
            {
                var checkData = _context.District.Where(a => a.DistrictName == district.DistrictName);
                if(checkData.Any())
                {
                    ModelState.AddModelError("DistrictName", "Record Already Exist with the same name");
                    return View(district);
                }
                _context.Add(district);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceName"] = new SelectList(_context.Province, "ProvinceName", "ProvinceName", district.ProvinceName);
            return View(district);
        }

        // GET: Districts/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }
            ViewData["ProvinceName"] = new SelectList(_context.Province, "ProvinceName", "ProvinceName", district.ProvinceName);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("DistrictID,DistrictName,ProvinceName")] District district)
        {
            if (district.DistrictID<=0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var checkData = _context.District.Where(a => a.DistrictName == district.DistrictName);
                if (checkData.Any())
                {
                    ModelState.AddModelError("DistrictName", "Record Already Exist with the same name");
                    ViewData["ProvinceName"] = new SelectList(_context.Province, "ProvinceName", "ProvinceName", district.ProvinceName);
                    return View(district);
                }
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.DistrictID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceName"] = new SelectList(_context.Province, "ProvinceName", "ProvinceName", district.ProvinceName);
            return View(district);
        }

        // GET: Districts/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District
               // .Include(d => d.Provinces)
                .FirstOrDefaultAsync(m => m.DistrictID == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short DistrictID)
        {
            var district = await _context.District.FindAsync(DistrictID);
            _context.District.Remove(district);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(short id)
        {
            return _context.District.Any(e => e.DistrictID == id);
        }
    }
}
