using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MKTS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MKTS.Data;

namespace MKTS.Controllers.Data
{
    public class RawDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RawDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RawDataController/Enrollments
        public async Task<IActionResult> Enrollments()
        {
            return View(await _context.Enrollment.ToListAsync());
        }

        public async Task<IActionResult> Retentions()
        {
            return View(await _context.Retention.ToListAsync());
        }
    }
}