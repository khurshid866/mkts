using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MKTS.Data;

namespace MKTS.Controllers
{
    public class RetentionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetentionController(ApplicationDbContext context)
        {
            //_hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public class RetentionData
        {
            //Province
            public List<string> Province { get; set; }
            public List<int> ProvinceGirlsDropout { get; set; }
            public List<int> ProvinceGirlsRetain { get; set; }
            public List<int> ProvinceBoysDropout { get; set; }
            public List<int> ProvinceBoysRetain { get; set; }

       

            //Partner
            public List<string> Partners { get; set; }
            public List<int> PartnerGirlsDropout { get; set; }
             public List<int> PartnerGirlsRetain { get; set; }
            public List<int> partnerBoysDropout { get; set; }
            public List<int> partnerBoysRetain { get; set; }

          
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<string> ProvinceList = await _context.Province.Select(a => a.ProvinceName).ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["Province"] = new SelectList(ProvinceList, "ProvinceName");

            //Year
            List<string> Year = await _context.Retention.Select(a => a.Year.ToString()).Distinct().ToListAsync();
            Year.Insert(0, "Over All");
            ViewData["Year"] = new SelectList(Year, "Year");

            
            return View();
        }

        public async Task<IActionResult> Filter(string ProvinceID,  int? YearID)
        {
            RetentionData retentionData = new RetentionData();

            YearID = YearID == null ? 0 : YearID;
            // ProvinceID = ProvinceID == null ? "All" : ProvinceID;
            //DistrictID = DistrictID == null ? "Over All" : DistrictID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            var RetentionRaw = _context.Retention.Where(a => a.Year > 0).OrderBy(a => a.Province);

            if (YearID > 0)
                RetentionRaw = RetentionRaw.Where(a => a.Year == YearID).OrderBy(a => a.Province);
           
            if (ProvinceID != "Over All")
            {
                RetentionRaw = RetentionRaw.Where(a => a.Province == ProvinceID).OrderBy(a => a.Province);
            }
            //////////**** Province wise Retention
          
            var provinceQry = RetentionRaw.GroupBy(a => a.Province).Select(g => new
            {
                Province = g.Max(a=>a.Province),
                DropoutBoys = (int) g.Average(a => a.DropoutBoys),
                RetainBoys = (int)(100- g.Average(a => a.DropoutBoys)),
                DropoutGirls =(int) g.Average(a => a.DropoutGirls),
                RetainGirls = (int)(100 - g.Average(a => a.DropoutGirls)),
            });
            retentionData.ProvinceBoysDropout = await provinceQry.OrderBy(a=>a.Province).Select(a => a.DropoutBoys).ToListAsync();
            retentionData.ProvinceBoysRetain = await provinceQry.OrderBy(a => a.Province).Select(a => a.RetainBoys).ToListAsync();
           
            retentionData.ProvinceGirlsDropout = await provinceQry.OrderBy(a => a.Province).Select(a => a.DropoutGirls).ToListAsync();
            retentionData.ProvinceGirlsRetain = await provinceQry.OrderBy(a => a.Province).Select(a => a.RetainGirls).ToListAsync();
            retentionData.Province = await provinceQry.OrderBy(a => a.Province).Select(a => a.Province).ToListAsync();
            
           
            //////**** Partner Chart
            ///
            var PartnerQry = RetentionRaw.GroupBy(a => a.Partner).Select(g => new
            {
                Partner = g.Max(a => a.Partner),
                DropoutBoys = (int)g.Average(a => a.DropoutBoys),
                RetainBoys =(int) (100 - g.Average(a => a.DropoutBoys)),
                DropoutGirls = (int)g.Average(a => a.DropoutGirls),
                RetainGirls = (int)(100 - g.Average(a => a.DropoutGirls)),
            });
            retentionData.partnerBoysDropout = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.DropoutBoys).ToListAsync();
            retentionData.partnerBoysRetain = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.RetainBoys).ToListAsync();

            retentionData.PartnerGirlsDropout = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.DropoutGirls).ToListAsync();
            retentionData.PartnerGirlsRetain = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.RetainGirls).ToListAsync();
            retentionData.Partners = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.Partner).ToListAsync();




            return PartialView(retentionData);
        }
        public async Task<IActionResult> Retention()
        {
            
            return View();
        }
    }
}