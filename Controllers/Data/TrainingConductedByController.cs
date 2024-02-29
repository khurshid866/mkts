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
    public class TrainingConductedByController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingConductedByController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainingConductedBy
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingConductedBy.ToListAsync());
        }

        // GET: TrainingConductedBy/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingConductedBy = await _context.TrainingConductedBy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trainingConductedBy == null)
            {
                return NotFound();
            }

            return View(trainingConductedBy);
        }

        // GET: TrainingConductedBy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingConductedBy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ConductedBy")] TrainingConductedBy trainingConductedBy)
        {
            if (ModelState.IsValid)
            {
                var checkData = _context.TrainingConductedBy.Where(a => a.ConductedBy == trainingConductedBy.ConductedBy);
                if (checkData.Any())
                {
                    ModelState.AddModelError("ConductedBy", "Record Already Exist with the same name");
                    return View(trainingConductedBy);
                }
                _context.Add(trainingConductedBy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingConductedBy);
        }

        // GET: TrainingConductedBy/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingConductedBy = await _context.TrainingConductedBy.FindAsync(id);
            if (trainingConductedBy == null)
            {
                return NotFound();
            }
            return View(trainingConductedBy);
        }

        // POST: TrainingConductedBy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ID,ConductedBy")] TrainingConductedBy trainingConductedBy)
        {
           
            if (ModelState.IsValid)
            {
                var checkData = _context.TrainingConductedBy.Where(a => a.ConductedBy == trainingConductedBy.ConductedBy);
                if (checkData.Any())
                {
                    ModelState.AddModelError("ConductedBy", "Record Already Exist with the same name");
                    return View(trainingConductedBy);
                }
                try
                {
                    _context.Update(trainingConductedBy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingConductedByExists(trainingConductedBy.ID))
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
            return View(trainingConductedBy);
        }

        // GET: TrainingConductedBy/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingConductedBy = await _context.TrainingConductedBy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trainingConductedBy == null)
            {
                return NotFound();
            }

            return View(trainingConductedBy);
        }

        // POST: TrainingConductedBy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short ID)
        {
            var trainingConductedBy = await _context.TrainingConductedBy.FindAsync(ID);
            _context.TrainingConductedBy.Remove(trainingConductedBy);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingConductedByExists(short id)
        {
            return _context.TrainingConductedBy.Any(e => e.ID == id);
        }
    }
}
