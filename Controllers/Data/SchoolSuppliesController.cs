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
    public class SchoolSuppliesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolSuppliesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SchoolSupplies
        public async Task<IActionResult> Index()
        {
            return View(await _context.schoolSupply.ToListAsync());
        }

        // GET: SchoolSupplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolSupply = await _context.schoolSupply
                .FirstOrDefaultAsync(m => m.SSID == id);
            if (schoolSupply == null)
            {
                return NotFound();
            }

            return View(schoolSupply);
        }

        // GET: SchoolSupplies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SchoolSupplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SSID,SchoolName,SchoolType,SchoolLevel,Gender,Province,District,Partner,Year,WashroomRepair,ConstructionWall,ConstructionWashroom,InstallationHandpump,RefurbishmentSchool,InstallationSolarPanel,RepairSchoolGate,InstallationFiltrationPlant,WhiteWash,ElectricRepairments,Flooring,PlasticMat,WaterCooler,BlackBoard,TeacherChair,AttendanceRegister,WaterTank,Desk,CupboardRack,ElectricCooler")] SchoolSupply schoolSupply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolSupply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schoolSupply);
        }

        // GET: SchoolSupplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolSupply = await _context.schoolSupply.FindAsync(id);
            if (schoolSupply == null)
            {
                return NotFound();
            }
            return View(schoolSupply);
        }

        // POST: SchoolSupplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SSID,SchoolName,SchoolType,SchoolLevel,Gender,Province,District,Partner,Year,WashroomRepair,ConstructionWall,ConstructionWashroom,InstallationHandpump,RefurbishmentSchool,InstallationSolarPanel,RepairSchoolGate,InstallationFiltrationPlant,WhiteWash,ElectricRepairments,Flooring,PlasticMat,WaterCooler,BlackBoard,TeacherChair,AttendanceRegister,WaterTank,Desk,CupboardRack,ElectricCooler")] SchoolSupply schoolSupply)
        {
            if (id != schoolSupply.SSID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolSupply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolSupplyExists(schoolSupply.SSID))
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
            return View(schoolSupply);
        }

        // GET: SchoolSupplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolSupply = await _context.schoolSupply
                .FirstOrDefaultAsync(m => m.SSID == id);
            if (schoolSupply == null)
            {
                return NotFound();
            }

            return View(schoolSupply);
        }

        // POST: SchoolSupplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int SSID)
        {
            var schoolSupply = await _context.schoolSupply.FindAsync(SSID);
            _context.schoolSupply.Remove(schoolSupply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolSupplyExists(int id)
        {
            return _context.schoolSupply.Any(e => e.SSID == id);
        }
    }
}
