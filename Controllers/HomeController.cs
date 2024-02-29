using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MKTS.Models;
using MKTS.Data;
using Microsoft.EntityFrameworkCore;
using MKTS.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MKTS.Controllers
{
    //public class DashboardData
    //{
    //    public int TotalEnroll { get; set; }
    //    public List<int> Enrollment { get; set; }
    //    public List<string> Province { get; set; }
    //    public List<string> Supplies = new List<string> { "Plastic Mat", " Water Cooler", "Black Board", "Teacher Chair", "Attendance Registar", "Water Tank", "	Desk", "Cupboard/Rack", "Electric Cooler" };

    //    public int SupplySchools { get; set; }
    //    public int InfraSchools { get; set; }
    //    //public List<string> Infra = new List<string> { }
    //    public List<int> InfraVal { get; set; }
    //    // public List<int> SupAjk = new List<int> { 41, 31, 39, 31, 34, 35, 32, 36, 30 };
    //    //public List<int> SupBalo = new List<int> { 43, 49, 36, 49, 48, 49, 41, 44, 32 };
    //    //public List<int> SupGB = new List<int> { 21, 28, 20, 17, 31, 27, 31, 29, 28 };
    //    //public List<int> SupICT = new List<int> { 49, 40, 44, 48, 41, 48, 46, 40, 43 };
    //    //public List<int> SupKP = new List<int> { 31, 30, 32, 39, 35, 36, 32, 32, 36 };
    //    //public List<int> SupPunjab = new List<int> { 30, 28, 22, 28, 27, 22, 25, 29, 23 };
    //    //public List<int> SupSindh = new List<int> { 19, 22, 15, 22, 36, 30, 18, 50, 46 };


    //    public List<ProvinceSupply> ProvinceSupplies = new List<ProvinceSupply>();
    //    public List<string> SupplyPartnerName { get; set; }
    //    public List<int> SupplyPartnerVal { get; set; }

    //    public List<int> retention { get; set; }

    //    //Training

    //    public List<string> TrainingType { get; set; }
    //    public List<int> TrainingMale { get; set; }
    //    public List<int> TrainingFemale { get; set; }

    //    //public int[] Enrollment { get; set; }

    //}

    //public class EnrollmentData
    //{
    //    //District
    //    public List<string> Districts { get; set; }
    //    public List<int> DistrictGirls { get; set; }
    //    public List<int> DistrictBoys { get; set; }

    //    //Type
    //    public List<string> Type { get; set; }
    //    public List<int> TypeEnroll { get; set; }

    //    //Partner
    //    public List<string> Partners { get; set; }
    //    public List<int> PartnerGirls { get; set; }
    //    public List<int> partnerBoys { get; set; }

    //    //Class 
    //    public List<short> ClassName { get; set; }
    //    public List<int> ClassBoys { get; set; }
    //    public List<int> ClassGirls { get; set; }
    //}
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private readonly ILogger<HomeController> _logger;


        public static DashboardData dashboardData;
       
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            dashboardData = new DashboardData();
        }
      
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Account", "Login");
            //}
            //if (User.Identity.Name == null)
            //{ return RedirectToAction("Login", "Account"); }

            DashboardData dd = new DashboardData();

            var enrollment = from Enrollment in _context.Enrollment
                             group Enrollment by new
                             {
                                 Enrollment.Province,
                                 //Enrollment.Year
                             } into g
                             //where g.Key.Year==2019
                             orderby
                               g.Key.Province
                             select new
                             {
                                 Province = g.Key.Province,
                                 PEnroll = (g.Sum(p => p.EnrolledGirls) + g.Sum(p => p.EnrolledBoys))
                             }; ;
            dd.TotalEnroll = enrollment.Sum(a => a.PEnroll);
            dd.Enrollment = await enrollment.Select(a => a.PEnroll).ToListAsync();
            dd.Province = enrollment.Select(a => a.Province).ToList();

            // Retention
            int dropOut = Convert.ToInt32(_context.Retention.Average(a => a.DropoutBoys + a.DropoutGirls));
            dd.retention = new List<int>();
            dropOut = dropOut / 2;
            dd.retention.Add(100 - dropOut);
            dd.retention.Add(dropOut);

            //Training
            var TrainingQry = _context.Training.Where(a => a.GroupName == "Teachers").GroupBy(a => a.TrainingType).Select(g => new
            {
                type = g.Key,
                Male = g.Sum(a => a.Male1),
                Female = g.Sum(a => a.Female1),
                //female = g.Sum(a => a.Female1),
            });
            dd.TrainingType = await TrainingQry.Select(a => a.type).ToListAsync();
            dd.TrainingMale = await TrainingQry.Select(a => a.Male).ToListAsync();
            dd.TrainingFemale = await TrainingQry.Select(a => a.Female).ToListAsync();

            // Supply
            dd.ProvinceSupplies = _context.ProvinceSupply.ToList();
            dd.SupplySchools = _context.schoolSupply.Count(a => a.PlasticMat == 1);

            //var SupplyQry = from SchoolSupplies in _context.schoolSupply
            //                group SchoolSupplies by new
            //                {
            //                    SchoolSupplies.Partner
            //                } into g
            //                select new
            //                {
            //                    PartnerName = g.Key.Partner,
            //                    total = g.Sum(p => p.PlasticMat ==1?1:0)
            //};
            var SupplyQry = _context.schoolSupply.GroupBy(a => a.Partner).Select(g => new
            {
                PartnerName = g.Key,
                total = g.Sum(p => p.PlasticMat == 1 ? 1 : 0),
                //female = g.Sum(a => a.Female1),
            });

            dd.SupplyPartnerName = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.PartnerName).ToListAsync();
            dd.SupplyPartnerVal = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.total).ToListAsync();


            //Infrastructure
            var InfraQry = _context.schoolSupply.Where(a => a.Partner == "SED").ToList();
            dd.InfraSchools = InfraQry.Count();
            dd.InfraVal = new List<int>();
            dd.InfraVal.Add(InfraQry.Sum(a => a.WashroomRepair));
            dd.InfraVal.Add(InfraQry.Sum(a => a.ConstructionWall));
            dd.InfraVal.Add(InfraQry.Sum(a => a.ConstructionWashroom));
            dd.InfraVal.Add(InfraQry.Sum(a => a.InstallationHandpump));
            dd.InfraVal.Add(InfraQry.Sum(a => a.RefurbishmentSchool));
            dd.InfraVal.Add(InfraQry.Sum(a => a.InstallationSolarPanel));
            // dd.InfraVal.Add(InfraQry.Sum(a => a.RepairSchoolGate));
            dd.InfraVal.Add(InfraQry.Sum(a => a.InstallationFiltrationPlant));
            dd.InfraVal.Add(InfraQry.Sum(a => a.WhiteWash));
            dd.InfraVal.Add(InfraQry.Sum(a => a.ElectricRepairments));
            dd.InfraVal.Add(InfraQry.Sum(a => a.Flooring));


            return View(dd);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Enrollment()
        {

            List<string> ProvinceList = await _context.Province.Select(a => a.ProvinceName).ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["Province"] = new SelectList(ProvinceList, "ProvinceName");

            //Year
            List<string> Year = await _context.Enrollment.Select(a => a.Year.ToString()).Distinct().ToListAsync();
            Year.Insert(0, "Over All");
            ViewData["Year"] = new SelectList(Year, "Year");

            //Type wise 
            List<string> Type = new List<string> { "Over All", "Formal", "Non Formal" };
            //Type.Insert(0, "Over All");
            ViewData["Type"] = new SelectList(Type, "Type");
            return View();
        }

        public async Task<IActionResult> EnrollFilter(string ProvinceID, string DistrictID, int? YearID, string TypeID)
        {
            EnrollmentData enrollmentData = new EnrollmentData();

            YearID = YearID == null ? 0 : YearID;
            ProvinceID = ProvinceID == null ? "Over All" : ProvinceID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            var EnrollmentRaw = _context.Enrollment.Where(a => a.Year > 0).OrderBy(a => a.Province).ThenBy(a => a.District);
            if (TypeID == "Formal")
                EnrollmentRaw = EnrollmentRaw.Where(a => a.EducationType == "Formal").OrderBy(a => a.District);
            else if (TypeID != "Over All")
                EnrollmentRaw = EnrollmentRaw.Where(a => a.EducationType != "Formal").OrderBy(a => a.District);

            if (YearID > 0)
                EnrollmentRaw = EnrollmentRaw.Where(a => a.Year == YearID).OrderBy(a => a.District);
            if (DistrictID != "Over All")
            {
                EnrollmentRaw = EnrollmentRaw.Where(a => a.District == DistrictID).OrderBy(a => a.District);
            }
            else if (ProvinceID != "Over All")
            {
                EnrollmentRaw = EnrollmentRaw.Where(a => a.Province == ProvinceID).OrderBy(a => a.Province);
            }
            //////////**** District wise Enrollment
            var districtQry = EnrollmentRaw.GroupBy(a => a.District).Select(g => new
            {
                Districts = g.Key,
                Province = g.Max(a => a.Province),
                BoysEnrolled = g.Sum(a => a.EnrolledBoys),
                GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            });
            enrollmentData.DistrictBoys = await districtQry.OrderBy(a => a.Province).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.DistrictGirls = await districtQry.OrderBy(a => a.Province).Select(a => a.GirlsEnrolled).ToListAsync();
            enrollmentData.Districts = await districtQry.OrderBy(a => a.Province).Select(a => a.Districts).ToListAsync();

            //**** Type wise Enrollment
            var TypeQry = EnrollmentRaw.GroupBy(a => a.EducationType).Select(g => new
            {
                EducationType = g.Key,
                Ednrolled = g.Sum(a => a.EnrolledBoys + a.EnrolledGirls),
                //GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            });
            enrollmentData.Type = await TypeQry.Select(a => a.EducationType).ToListAsync();
            enrollmentData.TypeEnroll = await TypeQry.Select(a => a.Ednrolled).ToListAsync();

            //////**** Partner Chart
            ///
            var PartnerQry = EnrollmentRaw.OrderBy(a => a.Partner).GroupBy(a => a.Partner).Select(g => new
            {
                Partner = g.Max(a => a.Partner),
                BoysEnrolled = g.Sum(a => a.EnrolledBoys),
                GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            });
            enrollmentData.Partners = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.Partner).ToListAsync();
            enrollmentData.partnerBoys = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.PartnerGirls = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.GirlsEnrolled).ToListAsync();


            ////// ****** Class wise 
            ///
            var ClassQry = EnrollmentRaw.OrderBy(a => a.Class).GroupBy(a => a.Class).Select(g => new
            {
                Class = g.Max(a => a.Class),
                BoysEnrolled = g.Sum(a => a.EnrolledBoys),
                GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            });
            enrollmentData.ClassName = await ClassQry.OrderBy(a => a.Class).Select(a => a.Class).ToListAsync();
            enrollmentData.ClassBoys = await ClassQry.OrderBy(a => a.Class).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.ClassGirls = await ClassQry.OrderBy(a => a.Class).Select(a => a.GirlsEnrolled).ToListAsync();


            return PartialView(enrollmentData);
        }



        public JsonResult GetDistrict(string ProvinceID)
        {
            List<string> districtList = _context.District.Where(a => a.ProvinceName == ProvinceID).Select(a => a.DistrictName).ToList();
            // districtList = _context.District.Where(a => a.ProvinceName == ProvinceID).ToList();
            districtList.Insert(0, "Over All");
            return Json(new SelectList(districtList, "DistrictName"));
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
