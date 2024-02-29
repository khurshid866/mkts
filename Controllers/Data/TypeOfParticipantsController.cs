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
    public class TypeOfParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeOfParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeOfParticipants
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeOfParticipant.ToListAsync());
        }

        // GET: TypeOfParticipants/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfParticipant = await _context.TypeOfParticipant
                .FirstOrDefaultAsync(m => m.ID == id);
            if (typeOfParticipant == null)
            {
                return NotFound();
            }

            return View(typeOfParticipant);
        }

        // GET: TypeOfParticipants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfParticipants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Participant")] TypeOfParticipant typeOfParticipant)
        {
            if (ModelState.IsValid)
            {
                var checkData = _context.TypeOfParticipant.Where(a => a.Participant == typeOfParticipant.Participant);
                if (checkData.Any())
                {
                    ModelState.AddModelError("Participant", "Record Already Exist with the same name");
                    return View(typeOfParticipant);
                }
                _context.Add(typeOfParticipant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfParticipant);
        }

        // GET: TypeOfParticipants/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfParticipant = await _context.TypeOfParticipant.FindAsync(id);
            if (typeOfParticipant == null)
            {
                return NotFound();
            }
            return View(typeOfParticipant);
        }

        // POST: TypeOfParticipants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("ID,Participant")] TypeOfParticipant typeOfParticipant)
        {
            

            if (ModelState.IsValid)
            {
                var checkData = _context.TypeOfParticipant.Where(a => a.Participant == typeOfParticipant.Participant);
                if (checkData.Any())
                {
                    ModelState.AddModelError("Participant", "Record Already Exist with the same name");
                    return View(typeOfParticipant);
                }
                try
                {
                    _context.Update(typeOfParticipant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfParticipantExists(typeOfParticipant.ID))
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
            return View(typeOfParticipant);
        }

        // GET: TypeOfParticipants/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfParticipant = await _context.TypeOfParticipant
                .FirstOrDefaultAsync(m => m.ID == id);
            if (typeOfParticipant == null)
            {
                return NotFound();
            }

            return View(typeOfParticipant);
        }

        // POST: TypeOfParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short ID)
        {
            var typeOfParticipant = await _context.TypeOfParticipant.FindAsync(ID);
            _context.TypeOfParticipant.Remove(typeOfParticipant);
           // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfParticipantExists(short id)
        {
            return _context.TypeOfParticipant.Any(e => e.ID == id);
        }
    }
}
