using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MKTS.Data;
using MKTS.Models.Data;

namespace MKTS.Controllers.Data
{
    public class YearsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YearsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Years
        public async Task<IActionResult> Index()
        {
            return View(await _context.Year.ToListAsync());
        }

        // GET: Years/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var year = await _context.Year
                .FirstOrDefaultAsync(m => m.ID == id);
            if (year == null)
            {
                return NotFound();
            }

            return View(year);
        }

        // GET: Years/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Years/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,YearName")] Year year)
        {
            if (ModelState.IsValid)
            {
                var checkData = _context.Year.Where(a => a.YearName == year.YearName);
                if (checkData.Any())
                {
                    ModelState.AddModelError("YearName", "Record Already Exist with the same name");
                    return View(year);
                }

                _context.Add(year);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(year);
        }

        // GET: Years/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var year = await _context.Year.FindAsync(id);
            if (year == null)
            {
                return NotFound();
            }
            return View(year);
        }

        // POST: Years/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ID,YearName")] Year year)
        {
            
            if (ModelState.IsValid)
            {

                try
                {
                    var checkData = _context.Year.Where(a => a.YearName == year.YearName);
                    if (checkData.Any())
                    {
                        ModelState.AddModelError("YearName", "Record Already Exist with the same name");
                        return View(year);
                    }
                    _context.Update(year);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearExists(year.ID))
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
            return View(year);
        }

        // GET: Years/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var year = await _context.Year
                .FirstOrDefaultAsync(m => m.ID == id);
            if (year == null)
            {
                return NotFound();
            }

            return View(year);
        }

        // POST: Years/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short ID)
        {
            var year = await _context.Year.FindAsync(ID);
            _context.Year.Remove(year);
          //  await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearExists(short id)
        {
            return _context.Year.Any(e => e.ID == id);
        }
    }
}
