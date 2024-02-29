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
    public class TargetGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TargetGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TargetGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.TargetGroup.ToListAsync());
        }

        // GET: TargetGroups/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetGroup = await _context.TargetGroup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (targetGroup == null)
            {
                return NotFound();
            }

            return View(targetGroup);
        }

        // GET: TargetGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TargetGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GroupName")] TargetGroup targetGroup)
        {
            if (ModelState.IsValid)
            {
                var checkData = _context.TargetGroup.Where(a => a.GroupName == targetGroup.GroupName);
                if (checkData.Any())
                {
                    ModelState.AddModelError("GroupName", "Record Already Exist with the same name");
                    return View(targetGroup);
                }
                _context.Add(targetGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(targetGroup);
        }

        // GET: TargetGroups/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetGroup = await _context.TargetGroup.FindAsync(id);
            if (targetGroup == null)
            {
                return NotFound();
            }
            return View(targetGroup);
        }

        // POST: TargetGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ID,GroupName")] TargetGroup targetGroup)
        {
          

            if (ModelState.IsValid)
            {
                var checkData = _context.TargetGroup.Where(a => a.GroupName == targetGroup.GroupName);
                if (checkData.Any())
                {
                    ModelState.AddModelError("GroupName", "Record Already Exist with the same name");
                    return View(targetGroup);
                }

                try
                {
                    _context.Update(targetGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetGroupExists(targetGroup.ID))
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
            return View(targetGroup);
        }

        // GET: TargetGroups/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targetGroup = await _context.TargetGroup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (targetGroup == null)
            {
                return NotFound();
            }

            return View(targetGroup);
        }

        // POST: TargetGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short ID)
        {
            var targetGroup = await _context.TargetGroup.FindAsync(ID);
            _context.TargetGroup.Remove(targetGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TargetGroupExists(short id)
        {
            return _context.TargetGroup.Any(e => e.ID == id);
        }
    }
}
