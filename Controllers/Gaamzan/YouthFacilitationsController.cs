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
    public class YouthFacilitationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YouthFacilitationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: YouthFacilitations
        public async Task<IActionResult> Index()
        {
            return View(await _context.YouthFacilitation.ToListAsync());
        }

        // GET: YouthFacilitations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var youthFacilitation = await _context.YouthFacilitation
                .FirstOrDefaultAsync(m => m.YouthFacilitationID == id);
            if (youthFacilitation == null)
            {
                return NotFound();
            }

            return View(youthFacilitation);
        }

        // GET: YouthFacilitations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YouthFacilitations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("YouthFacilitationID,ParticipantName,Gender,District,ClassBatch,ModeFacilitation,SkillEnhanced,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] YouthFacilitation youthFacilitation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(youthFacilitation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(youthFacilitation);
        }

        // GET: YouthFacilitations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var youthFacilitation = await _context.YouthFacilitation.FindAsync(id);
            if (youthFacilitation == null)
            {
                return NotFound();
            }
            return View(youthFacilitation);
        }

        // POST: YouthFacilitations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("YouthFacilitationID,ParticipantName,Gender,District,ClassBatch,ModeFacilitation,SkillEnhanced,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] YouthFacilitation youthFacilitation)
        {
            if (id != youthFacilitation.YouthFacilitationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(youthFacilitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YouthFacilitationExists(youthFacilitation.YouthFacilitationID))
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
            return View(youthFacilitation);
        }

        // GET: YouthFacilitations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var youthFacilitation = await _context.YouthFacilitation
                .FirstOrDefaultAsync(m => m.YouthFacilitationID == id);
            if (youthFacilitation == null)
            {
                return NotFound();
            }

            return View(youthFacilitation);
        }

        // POST: YouthFacilitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var youthFacilitation = await _context.YouthFacilitation.FindAsync(id);
            _context.YouthFacilitation.Remove(youthFacilitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YouthFacilitationExists(int id)
        {
            return _context.YouthFacilitation.Any(e => e.YouthFacilitationID == id);
        }
    }
}
