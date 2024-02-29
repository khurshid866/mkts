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
    public class TrainingData
    {
        //District
        public List<string> Districts { get; set; }
        public List<int> DistrictFemales { get; set; }
        public List<int> DistrictMales{ get; set; }

        //Type
        public List<string> Type { get; set; }
        public List<int> TypeEnroll { get; set; }

        //Partner
        public List<string> Partners { get; set; }
        public List<int> PartnerFemales { get; set; }
        public List<int> partnerMales { get; set; }

        //Theme 
        public List<string> ThemeName { get; set; }
        public List<int> ThemeMales { get; set; }
        public List<int> ThemeFemales { get; set; }
        //TargetGroup 
        public List<string> TargetGroup { get; set; }
        public List<int> TargetGroupMales { get; set; }
        public List<int> TargetGroupFemales { get; set; }
        //ConductedBy 
        public List<string> ConductedBy { get; set; }
        public List<int> ConductedByMales { get; set; }
        public List<int> ConductedByFemales { get; set; }
    }

    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingController(ApplicationDbContext context)
        {
            //_hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            
            List<string> ProvinceList = await _context.Training.Select(a => a.ProvinceName).Distinct().ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["Province"] = new SelectList(ProvinceList, "ProvinceName");

         
            //Type wise 
            List<string> Type = new List<string> { "Over All", "In-Class", "Online" };
            //Type.Insert(0, "Over All");
            ViewData["Type"] = new SelectList(Type, "Type");
            return View();
        }

        public async Task<IActionResult> Filter(string ProvinceID, string DistrictID, string TypeID)
        {
           // trainingData trainingData = new trainingData();

            TrainingData trainingData = new TrainingData();
            //YearID = YearID == null ? 0 : YearID;
            ProvinceID = ProvinceID == null ? "Over All" : ProvinceID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            var TrainingRaw = _context.Training.Where(a => a.GroupName =="Teachers").OrderBy(a => a.ProvinceName).ThenBy(a => a.DistrictName);
            if (TypeID == "Online")
                TrainingRaw = TrainingRaw.Where(a => a.TrainingType == "Online").OrderBy(a => a.DistrictName);
            else if (TypeID == "In-Class")
                TrainingRaw = TrainingRaw.Where(a => a.TrainingType == "In-Class").OrderBy(a => a.DistrictName);

            
            if (DistrictID != "Over All")
            {
                TrainingRaw = TrainingRaw.Where(a => a.DistrictName == DistrictID).OrderBy(a => a.DistrictName);
            }
            if (ProvinceID != "Over All")
            {
                TrainingRaw = TrainingRaw.Where(a => a.ProvinceName == ProvinceID).OrderBy(a => a.ProvinceName);
            }
            //////////**** District wise Enrollment
            var districtQry = TrainingRaw.GroupBy(a => a.DistrictName).Select(g => new
            {
                Districts = g.Key,
                male = g.Sum(a => a.Male1),
                female = g.Sum(a => a.Female1),
            });
            trainingData.DistrictMales = await districtQry.Select(a => a.male).ToListAsync();
            // trainingData.BoysEnroll = await EnrollmentRaw.GroupBy(a => a.District).Select(g => g.Sum(a=>a.EnrolledBoys)).ToListAsync();
            //Select(a=> new  a.EnrolledBoys).
            trainingData.DistrictFemales = await districtQry.Select(a => a.female).ToListAsync();
            trainingData.Districts = await districtQry.Select(a => a.Districts).ToListAsync();

            ////**** Type wise Enrollment
            //var TypeQry = TrainingRaw.GroupBy(a => a.EducationType).Select(g => new
            //{
            //    EducationType = g.Key,
            //    Ednrolled = g.Sum(a => a.EnrolledBoys + a.EnrolledGirls),
            //    //GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            //});
            //trainingData.Type = await TypeQry.Select(a => a.EducationType).ToListAsync();
            //trainingData.TypeEnroll = await TypeQry.Select(a => a.Ednrolled).ToListAsync();

            //////**** Partner Chart
            ///
            var PartnerQry = TrainingRaw.GroupBy(a => a.Partner).Select(g => new
            {
                Partner = g.Key,
                male = g.Sum(a => a.Male1),
                female = g.Sum(a => a.Female1),
            });
            trainingData.Partners = await PartnerQry.Select(a => a.Partner).ToListAsync();
            trainingData.partnerMales = await PartnerQry.Select(a => a.male).ToListAsync();
            trainingData.PartnerFemales = await PartnerQry.Select(a => a.female).ToListAsync();


            ////// ****** Theme wise 
            ///
            var ThemeQry = TrainingRaw.GroupBy(a => a.ThemeName).Select(g => new
            {
                theme = g.Key,
                male = g.Sum(a => a.Male1),
                female = g.Sum(a => a.Female1),
            });
            trainingData.ThemeName = await ThemeQry.Select(a => a.theme).ToListAsync();
            trainingData.ThemeMales = await ThemeQry.Select(a => a.male).ToListAsync();
            trainingData.ThemeFemales = await ThemeQry.Select(a => a.female).ToListAsync();

            ////// ****** Target Group wise 
            ///
            var TargetGroupQry = TrainingRaw.GroupBy(a => a.GroupName).Select(g => new
            {
                targetGroup = g.Key,
                male = g.Sum(a => a.Male1),
                female = g.Sum(a => a.Female1),
            });
            trainingData.TargetGroup = await TargetGroupQry.Select(a => a.targetGroup).ToListAsync();
            trainingData.TargetGroupMales = await TargetGroupQry.Select(a => a.male).ToListAsync();
            trainingData.TargetGroupFemales = await TargetGroupQry.Select(a => a.female).ToListAsync();

            ////// ****** ConductedBy 
            ///
            var ConductedByQry = TrainingRaw.GroupBy(a => a.ConductedBy).Select(g => new
            {
                ConductedBy = g.Key,
                male = g.Sum(a => a.Male1),
                female = g.Sum(a => a.Female1),
            });
            trainingData.ConductedBy = await ConductedByQry.Select(a => a.ConductedBy).ToListAsync();
            trainingData.ConductedByMales = await ConductedByQry.Select(a => a.male).ToListAsync();
            trainingData.ConductedByFemales = await ConductedByQry.Select(a => a.female).ToListAsync();


            return PartialView(trainingData);
        }

        public JsonResult GetDistrict(string ProvinceID)
        {
            List<string> districtList = _context.Training.Where(a => a.ProvinceName == ProvinceID).Select(a => a.DistrictName).Distinct().ToList();
            // districtList = _context.District.Where(a => a.ProvinceName == ProvinceID).ToList();
            districtList.Insert(0, "Over All");
            return Json(new SelectList(districtList, "DistrictName"));
        }

    }
}