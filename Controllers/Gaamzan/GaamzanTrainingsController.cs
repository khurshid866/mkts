using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MKTS.Data;
using MKTS.Models.Gaamzan;

namespace MKTS.Controllers.Gaamzan
{
    public class GaamzanTrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GaamzanTrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GaamzanTrainings
        public async Task<IActionResult> Index()
        {
            return View(await _context.GaamzanTraining.ToListAsync());
        }

        // GET: GaamzanTrainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gaamzanTraining = await _context.GaamzanTraining
                .FirstOrDefaultAsync(m => m.TrainingID == id);
            if (gaamzanTraining == null)
            {
                return NotFound();
            }

            return View(gaamzanTraining);
        }

        // GET: GaamzanTrainings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GaamzanTrainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingID,ParticipantName,ParticipantType,Gender,TrainingTheme,TrainingMode,TrainingHours,District,Partner,SchoolName,Email,Contact,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] GaamzanTraining gaamzanTraining)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gaamzanTraining);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gaamzanTraining);
        }

        // GET: GaamzanTrainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gaamzanTraining = await _context.GaamzanTraining.FindAsync(id);
            if (gaamzanTraining == null)
            {
                return NotFound();
            }
            return View(gaamzanTraining);
        }

        // POST: GaamzanTrainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingID,ParticipantName,ParticipantType,Gender,TrainingTheme,TrainingMode,TrainingHours,District,Partner,SchoolName,Email,Contact,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] GaamzanTraining gaamzanTraining)
        {
            if (id != gaamzanTraining.TrainingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gaamzanTraining);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GaamzanTrainingExists(gaamzanTraining.TrainingID))
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
            return View(gaamzanTraining);
        }

        // GET: GaamzanTrainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gaamzanTraining = await _context.GaamzanTraining
                .FirstOrDefaultAsync(m => m.TrainingID == id);
            if (gaamzanTraining == null)
            {
                return NotFound();
            }

            return View(gaamzanTraining);
        }

        // POST: GaamzanTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gaamzanTraining = await _context.GaamzanTraining.FindAsync(id);
            _context.GaamzanTraining.Remove(gaamzanTraining);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GaamzanTrainingExists(int id)
        {
            return _context.GaamzanTraining.Any(e => e.TrainingID == id);
        }
    }
}
