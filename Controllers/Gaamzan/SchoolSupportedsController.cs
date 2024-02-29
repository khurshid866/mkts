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
    public class SchoolSupportedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolSupportedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SchoolSupporteds
        public async Task<IActionResult> Index()
        {
            return View(await _context.SchoolSupported.ToListAsync());
        }

        // GET: SchoolSupporteds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolSupported = await _context.SchoolSupported
                .FirstOrDefaultAsync(m => m.SchoolSupportID == id);
            if (schoolSupported == null)
            {
                return NotFound();
            }

            return View(schoolSupported);
        }

        // GET: SchoolSupporteds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SchoolSupporteds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolSupportID,SchoolName,SchoolType,SchoolLevel,District,Partner,Year,Washroom,SolarPanel,WaterTank,PlasticMat,WaterCooler,BlackBoard,TeacherChair,Registar,Shelter,LearningMaterial,LCDs,Tablets,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] SchoolSupported schoolSupported)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolSupported);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schoolSupported);
        }

        // GET: SchoolSupporteds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolSupported = await _context.SchoolSupported.FindAsync(id);
            if (schoolSupported == null)
            {
                return NotFound();
            }
            return View(schoolSupported);
        }

        // POST: SchoolSupporteds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SchoolSupportID,SchoolName,SchoolType,SchoolLevel,District,Partner,Year,Washroom,SolarPanel,WaterTank,PlasticMat,WaterCooler,BlackBoard,TeacherChair,Registar,Shelter,LearningMaterial,LCDs,Tablets,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] SchoolSupported schoolSupported)
        {
            if (id != schoolSupported.SchoolSupportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolSupported);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolSupportedExists(schoolSupported.SchoolSupportID))
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
            return View(schoolSupported);
        }

        // GET: SchoolSupporteds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolSupported = await _context.SchoolSupported
                .FirstOrDefaultAsync(m => m.SchoolSupportID == id);
            if (schoolSupported == null)
            {
                return NotFound();
            }

            return View(schoolSupported);
        }

        // POST: SchoolSupporteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schoolSupported = await _context.SchoolSupported.FindAsync(id);
            _context.SchoolSupported.Remove(schoolSupported);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolSupportedExists(int id)
        {
            return _context.SchoolSupported.Any(e => e.SchoolSupportID == id);
        }
    }
}
