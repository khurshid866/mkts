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
    public class TrainingThemesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingThemesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainingThemes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingTheme.ToListAsync());
        }

        // GET: TrainingThemes/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingTheme = await _context.TrainingTheme
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trainingTheme == null)
            {
                return NotFound();
            }

            return View(trainingTheme);
        }

        // GET: TrainingThemes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingThemes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ThemeName")] TrainingTheme trainingTheme)
        {
            if (ModelState.IsValid)
            {
                var checkData = _context.TrainingTheme.Where(a => a.ThemeName == trainingTheme.ThemeName);
                if (checkData.Any())
                {
                    ModelState.AddModelError("ThemeName", "Record Already Exist with the same name");
                    return View(trainingTheme);
                }

                _context.Add(trainingTheme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingTheme);
        }

        // GET: TrainingThemes/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingTheme = await _context.TrainingTheme.FindAsync(id);
            if (trainingTheme == null)
            {
                return NotFound();
            }
            return View(trainingTheme);
        }

        // POST: TrainingThemes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ID,ThemeName")] TrainingTheme trainingTheme)
        {
          

            if (ModelState.IsValid)
            {
                var checkData = _context.TrainingTheme.Where(a => a.ThemeName == trainingTheme.ThemeName);
                if (checkData.Any())
                {
                    ModelState.AddModelError("ThemeName", "Record Already Exist with the same name");
                    return View(trainingTheme);
                }
                try
                {
                    _context.Update(trainingTheme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingThemeExists(trainingTheme.ID))
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
            return View(trainingTheme);
        }

        // GET: TrainingThemes/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingTheme = await _context.TrainingTheme
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trainingTheme == null)
            {
                return NotFound();
            }

            return View(trainingTheme);
        }

        // POST: TrainingThemes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short ID)
        {
            var trainingTheme = await _context.TrainingTheme.FindAsync(ID);
            _context.TrainingTheme.Remove(trainingTheme);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingThemeExists(short id)
        {
            return _context.TrainingTheme.Any(e => e.ID == id);
        }
    }
}
