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
    public class GaamzanEnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GaamzanEnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GaamzanEnrollments
        public async Task<IActionResult> Index()
        {
            return View(await _context.GaamzanEnrollment.ToListAsync());
        }

        // GET: GaamzanEnrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gaamzanEnrollment = await _context.GaamzanEnrollment
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (gaamzanEnrollment == null)
            {
                return NotFound();
            }

            return View(gaamzanEnrollment);
        }

        // GET: GaamzanEnrollments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GaamzanEnrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,StudentName,Gender,Age,Class,AdmnMonth,AdmnDay,AdmnYear,Disability,FatherName,SchoolName,SchoolLevel,District,Tehsil,UnionCouncil,Village,TeacherName,TeacherContact,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] GaamzanEnrollment gaamzanEnrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gaamzanEnrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gaamzanEnrollment);
        }

        // GET: GaamzanEnrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gaamzanEnrollment = await _context.GaamzanEnrollment.FindAsync(id);
            if (gaamzanEnrollment == null)
            {
                return NotFound();
            }
            return View(gaamzanEnrollment);
        }

        // POST: GaamzanEnrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentID,StudentName,Gender,Age,Class,AdmnMonth,AdmnDay,AdmnYear,Disability,FatherName,SchoolName,SchoolLevel,District,Tehsil,UnionCouncil,Village,TeacherName,TeacherContact,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] GaamzanEnrollment gaamzanEnrollment)
        {
            if (id != gaamzanEnrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gaamzanEnrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GaamzanEnrollmentExists(gaamzanEnrollment.EnrollmentID))
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
            return View(gaamzanEnrollment);
        }

        // GET: GaamzanEnrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gaamzanEnrollment = await _context.GaamzanEnrollment
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (gaamzanEnrollment == null)
            {
                return NotFound();
            }

            return View(gaamzanEnrollment);
        }

        // POST: GaamzanEnrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gaamzanEnrollment = await _context.GaamzanEnrollment.FindAsync(id);
            _context.GaamzanEnrollment.Remove(gaamzanEnrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GaamzanEnrollmentExists(int id)
        {
            return _context.GaamzanEnrollment.Any(e => e.EnrollmentID == id);
        }
    }
}
