using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MKTS.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Controllers.Gaamzan
{
    public class GaamzanController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<GaamzanController> _logger;


        public static DashboardData dashboardData;


        public GaamzanController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            //_logger = logger;
            _context = context;
            dashboardData = new DashboardData();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
         
            DashboardData dd = new DashboardData();

            var enrollment = from Enrollment in _context.GaamzanEnrollment
                             group Enrollment by new
                             {
                                 Enrollment.District,
                                 //Enrollment.Year
                             } into g
                             //where g.Key.Year==2019
                             orderby
                               g.Key.District
                             select new
                             {
                                 Province = g.Key.District,
                                 PEnroll = g.Count(p => p.EnrollmentID>0) 
                             }; ;
            dd.TotalEnroll = enrollment.Sum(a => a.PEnroll);
            dd.Enrollment = await enrollment.Select(a => a.PEnroll).ToListAsync();
            dd.Province = enrollment.Select(a => a.Province).ToList();


            //Training
            var TrainingQry = _context.GaamzanTraining.Where(a => a.TrainingID>0).GroupBy(a => a.Partner).Select(g => new
            {
                Partner = g.Key,
                male = g.Count(a => a.Gender == "Male"),
                female = g.Count(a => a.Gender == "Female"),
            });
            dd.TrainingType = await TrainingQry.Select(a => a.Partner).ToListAsync();
            dd. TrainingMale = await TrainingQry.Select(a => a.male).ToListAsync();
            dd.TrainingFemale = await TrainingQry.Select(a => a.female).ToListAsync();


            var SupplyRaw = _context.SchoolSupported.Where(a => a.SchoolSupportID > 0).OrderBy(a => a.District);
           
            dd.SupplySchools = SupplyRaw.Sum(a => a.PlasticMat == 1 ? 1 : 0);

            var SupplyQry = SupplyRaw.GroupBy(a => a.Partner).Select(g => new
            {
                PartnerName = g.Key,
                total = g.Sum(a => a.PlasticMat == 1 ? 1 : 0),
                //female = g.Sum(a => a.Female1),
            });
            // supplyData.Supplies = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.PartnerName).ToListAsync();

            //  supplyData.SuppliesVal = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.total).ToListAsync();
            dd.SupplyPartnerName = new List<string> { "Alight", "MF" };
            dd.SupplyPartnerVal = new List<int>();
            dd.SupplyPartnerVal.Add(SupplyRaw.Count(a => a.Partner == "Alight"));
            dd.SupplyPartnerVal.Add(SupplyRaw.Count(a => a.Partner == "MF"));
            // supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "CAR").Sum(a => a.total));
            //supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "NCHD").Sum(a => a.total));

            var InfraQry = SupplyRaw.Where(a => a.SchoolSupportID > 0).ToList();

            dd.InfraSchools = InfraQry.Count();

            dd.InfraVal = new List<int>();
            dd.InfraVal.Add(InfraQry.Sum(a => a.Washroom));
            dd.InfraVal.Add(InfraQry.Sum(a => a.SolarPanel));
            dd.InfraVal.Add(InfraQry.Sum(a => a.WaterTank));
            dd.InfraVal.Add(InfraQry.Sum(a => a.PlasticMat));
            dd.InfraVal.Add(InfraQry.Sum(a => a.WaterCooler));
            dd.InfraVal.Add(InfraQry.Sum(a => a.BlackBoard));
            // supplyData.InfraVal.Add(InfraQry.Sum(a => a.RepairSchoolGate));
            dd.InfraVal.Add(InfraQry.Sum(a => a.TeacherChair));
            dd.InfraVal.Add(InfraQry.Sum(a => a.Registar));
            dd.InfraVal.Add(InfraQry.Sum(a => a.LearningMaterial));
            dd.InfraVal.Add(InfraQry.Sum(a => a.LCDs));
            dd.InfraVal.Add(InfraQry.Sum(a => a.Tablets));


            var YouthRaw = _context.YouthFacilitation.Where(a => a.YouthFacilitationID > 0).OrderBy(a => a.District);
            //else if (TypeID != "Over All")
            //    EnrollmentRaw = EnrollmentRaw.Where(a => a.EducationType != "Formal").OrderBy(a => a.District);


           
            var PartnerQry = YouthRaw.OrderBy(a => a.ModeFacilitation).GroupBy(a => a.ModeFacilitation).Select(g => new
            {
                Partner = g.Max(a => a.ModeFacilitation),
                BoysEnrolled = g.Count(a => a.Gender == "Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            dd.YouthFacilitation= await PartnerQry.OrderBy(a => a.Partner).Select(a => a.Partner).ToListAsync();
            dd.YouthBoys = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.BoysEnrolled).ToListAsync();
            dd.YouthGirls = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.GirlsEnrolled).ToListAsync();



            return View(dd);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Enrollment()
        {

            List<string> ProvinceList = await _context.GaamzanEnrollment.Select(a => a.District).Distinct().ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["District"] = new SelectList(ProvinceList, "District");

            //Year
            List<string> Year = await _context.GaamzanEnrollment.Select(a => a.AdmnYear.ToString()).Distinct().ToListAsync();
            Year.Insert(0, "Over All");
            ViewData["Year"] = new SelectList(Year, "Year");

            //Type wise 
            List<string> Type = await _context.GaamzanEnrollment.Select(a => a.SchoolLevel).Distinct(). ToListAsync();
            Type.Insert(0, "Over All");
            ViewData["Type"] = new SelectList(Type, "Type");
            return View();
        }

        public async Task<IActionResult> EnrollFilter( string DistrictID, int? YearID, string TypeID)
        {
            EnrollmentData enrollmentData = new EnrollmentData();

            YearID = YearID == null ? 0 : YearID;
           // ProvinceID = ProvinceID == null ? "Over All" : ProvinceID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            var EnrollmentRaw = _context.GaamzanEnrollment.Where(a => a.AdmnYear > 0).OrderBy(a => a.District);
            if (TypeID != "Over All")
                EnrollmentRaw = EnrollmentRaw.Where(a => a.SchoolLevel == TypeID).OrderBy(a => a.District);
            //else if (TypeID != "Over All")
            //    EnrollmentRaw = EnrollmentRaw.Where(a => a.EducationType != "Formal").OrderBy(a => a.District);

            if (YearID > 0)
                EnrollmentRaw = EnrollmentRaw.Where(a => a.AdmnYear == YearID).OrderBy(a => a.District);
            if (DistrictID != "Over All")
            {
                EnrollmentRaw = EnrollmentRaw.Where(a => a.District == DistrictID).OrderBy(a => a.District);
            }
            //else if (ProvinceID != "Over All")
            //{
            //    EnrollmentRaw = EnrollmentRaw.Where(a => a.Province == ProvinceID).OrderBy(a => a.Province);
            //}
            //////////**** District wise Enrollment
            var districtQry = EnrollmentRaw.GroupBy(a => a.District).Select(g => new
            {
                Districts = g.Key,
                //Province = g.Max(a => a.Province),
                BoysEnrolled = g.Count(a => a.Gender=="Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            enrollmentData.DistrictBoys = await districtQry.OrderBy(a => a.Districts).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.DistrictGirls = await districtQry.OrderBy(a => a.Districts).Select(a => a.GirlsEnrolled).ToListAsync();
            enrollmentData.Districts = await districtQry.OrderBy(a => a.Districts).Select(a => a.Districts).ToListAsync();

            ////**** Type wise Enrollment
            //var TypeQry = EnrollmentRaw.GroupBy(a => a.EducationType).Select(g => new
            //{
            //    EducationType = g.Key,
            //    Ednrolled = g.Sum(a => a.EnrolledBoys + a.EnrolledGirls),
            //    //GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            //});
            //enrollmentData.Type = await TypeQry.Select(a => a.EducationType).ToListAsync();
            //enrollmentData.TypeEnroll = await TypeQry.Select(a => a.Ednrolled).ToListAsync();

            //////**** Partner Chart
            ///
            var PartnerQry = EnrollmentRaw.OrderBy(a => a.SchoolLevel).GroupBy(a => a.SchoolLevel).Select(g => new
            {
                Partner = g.Max(a => a.SchoolLevel),
                BoysEnrolled = g.Count(a => a.Gender == "Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            enrollmentData.Partners = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.Partner).ToListAsync();
            enrollmentData.partnerBoys = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.PartnerGirls = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.GirlsEnrolled).ToListAsync();


            ////// ****** Class wise 
            ///
            var ClassQry = EnrollmentRaw.OrderBy(a => a.Class).GroupBy(a => a.Class).Select(g => new
            {
                Class = g.Max(a => a.Class),
                BoysEnrolled = g.Count(a => a.Gender == "Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            enrollmentData.ClassName = await ClassQry.OrderBy(a => a.Class).Select(a => a.Class).ToListAsync();
            enrollmentData.ClassBoys = await ClassQry.OrderBy(a => a.Class).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.ClassGirls = await ClassQry.OrderBy(a => a.Class).Select(a => a.GirlsEnrolled).ToListAsync();


            return PartialView(enrollmentData);
        }




        ////// Training

        public async Task<IActionResult> Training()
        {


            List<string> ProvinceList = await _context.GaamzanTraining.Select(a => a.District).Distinct().ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["District"] = new SelectList(ProvinceList, "District");


            //Type wise 
            List<string> Type = await _context.GaamzanTraining.Select(a => a.TrainingMode).Distinct().ToListAsync();
            Type.Insert(0, "Over All");
            ViewData["Type"] = new SelectList(Type, "Type");
            return View();
        }

        public async Task<IActionResult> TrainingFilter(string DistrictID, string TypeID)
        {
            // trainingData trainingData = new trainingData();

            TrainingData trainingData = new TrainingData();
            //YearID = YearID == null ? 0 : YearID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            //DistrictID = DistrictID == null ? "Over All" : DistrictID;
            TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            var TrainingRaw = _context.GaamzanTraining.Where(a => a.TrainingID >0).OrderBy(a => a.District);
            if (TypeID != "Over All")
            { TrainingRaw = TrainingRaw.Where(a => a.TrainingMode == TypeID).OrderBy(a => a.District); }
            //if (TypeID == "Online")
            //    TrainingRaw = TrainingRaw.Where(a => a.TrainingType == "Online").OrderBy(a => a.DistrictName);
            //else if (TypeID == "In-Class")
            //    TrainingRaw = TrainingRaw.Where(a => a.TrainingType == "In-Class").OrderBy(a => a.DistrictName);


            //if (DistrictID != "Over All")
            //{
            //    TrainingRaw = TrainingRaw.Where(a => a.DistrictName == DistrictID).OrderBy(a => a.DistrictName);
            //}
            if (DistrictID != "Over All")
            {
                TrainingRaw = TrainingRaw.Where(a => a.District == DistrictID).OrderBy(a => a.District);
            }
            //////////**** District wise Enrollment
            var districtQry = TrainingRaw.GroupBy(a => a.District).Select(g => new
            {
                Districts = g.Key,
                male = g.Count(a => a.Gender=="Male"),
                female = g.Count(a => a.Gender == "Female"),
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
                male = g.Count(a => a.Gender == "Male"),
                female = g.Count(a => a.Gender == "Female"),
            });
            trainingData.Partners = await PartnerQry.Select(a => a.Partner).ToListAsync();
            trainingData.partnerMales = await PartnerQry.Select(a => a.male).ToListAsync();
            trainingData.PartnerFemales = await PartnerQry.Select(a => a.female).ToListAsync();


            ////// ****** Theme wise 
            ///
            var ThemeQry = TrainingRaw.GroupBy(a => a.TrainingTheme).Select(g => new
            {
                theme = g.Key,
                male = g.Count(a => a.Gender == "Male"),
                female = g.Count(a => a.Gender == "Female"),
            });
            trainingData.ThemeName = await ThemeQry.Select(a => a.theme).ToListAsync();
            trainingData.ThemeMales = await ThemeQry.Select(a => a.male).ToListAsync();
            trainingData.ThemeFemales = await ThemeQry.Select(a => a.female).ToListAsync();

            ////// ****** Target Group wise 
            ///
            //var TargetGroupQry = TrainingRaw.GroupBy(a => a.GroupName).Select(g => new
            //{
            //    targetGroup = g.Key,
            //    male = g.Sum(a => a.Male1),
            //    female = g.Sum(a => a.Female1),
            //});
            //trainingData.TargetGroup = await TargetGroupQry.Select(a => a.targetGroup).ToListAsync();
            //trainingData.TargetGroupMales = await TargetGroupQry.Select(a => a.male).ToListAsync();
            //trainingData.TargetGroupFemales = await TargetGroupQry.Select(a => a.female).ToListAsync();

            ////// ****** ConductedBy 
            ///
            //var ConductedByQry = TrainingRaw.GroupBy(a => a.ConductedBy).Select(g => new
            //{
            //    ConductedBy = g.Key,
            //    male = g.Sum(a => a.Male1),
            //    female = g.Sum(a => a.Female1),
            //});
            //trainingData.ConductedBy = await ConductedByQry.Select(a => a.ConductedBy).ToListAsync();
            //trainingData.ConductedByMales = await ConductedByQry.Select(a => a.male).ToListAsync();
            //trainingData.ConductedByFemales = await ConductedByQry.Select(a => a.female).ToListAsync();


            return PartialView(trainingData);
        }


        ///  Training End



        ///// School Supported
        [AllowAnonymous]
        public async Task<IActionResult> SchoolSupport()
        {

            ////Province
            //List<string> ProvinceList = await _context.schoolSupply.Select(a => a.Province).Distinct().ToListAsync();
            //ProvinceList.Insert(0, "Over All");
            //ViewData["Province"] = new SelectList(ProvinceList, "Province");

            //District
            List<string> DistrictList = await _context.SchoolSupported.Select(a => a.District).Distinct().ToListAsync();
            DistrictList.Insert(0, "Over All");
            ViewData["District"] = new SelectList(DistrictList, "District");

            //Partner
            List<string> PartnerList = await _context.SchoolSupported.Select(a => a.Partner).Distinct().ToListAsync();
            PartnerList.Insert(0, "Over All");
            ViewData["Partner"] = new SelectList(PartnerList, "Partner");

            //Year
            //Year
            List<string> Year = await _context.SchoolSupported.Select(a => a.Year.ToString()).Distinct().ToListAsync();
            Year.Insert(0, "Over All");
            ViewData["Year"] = new SelectList(Year, "Year");



            return View();
        }
        public async Task<IActionResult> SupportFilter(string DistrictID, int? YearID, string PartnerID)
        {
            // trainingData trainingData = new trainingData();

            SupplyData supplyData = new SupplyData
            {
                Infrastructure = new List<string>(),

                Supplies = new List<string>(),
            };


            //YearID = YearID == null ? 0 : YearID;
            //ProvinceID = ProvinceID == null ? "Over All" : ProvinceID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            PartnerID = PartnerID == null ? "Over All" : PartnerID;
            //  YearID = YearID == null ? "Over All" : YearID;
            // TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            //SqlParameter ProvincePara = new SqlParameter("@Province", ProvinceID == "Over All" ? (object)DBNull.Value : ProvinceID);
            //SqlParameter DistPara = new SqlParameter("@District", DistrictID == "Over All" ? (object)DBNull.Value : DistrictID);
            //SqlParameter YearPara = new SqlParameter("@Year", YearID == null ? (object)DBNull.Value : YearID);
            //SqlParameter PartnerPara = new SqlParameter("@Partner", PartnerID == "Over All" ? (object)DBNull.Value : PartnerID);

            //var InfraGenderQuery = _context.GenderSP.FromSqlRaw("exec InfraGenderSP" +
            //                   " @Province, @District, @Year,@Partner",
            //                      ProvincePara, DistPara, YearPara, PartnerPara

            //                   ); ; //.ToList<IndicatorsTotalTarget>(); ;





            var SupplyRaw =  _context.SchoolSupported.Where(a => a.SchoolSupportID > 0).OrderBy(a => a.District);

            if (DistrictID != "Over All")
            {
                SupplyRaw = SupplyRaw.Where(a => a.District == DistrictID).OrderBy(a => a.District);
            }
            //if (ProvinceID != "Over All")

            //    SupplyRaw = SupplyRaw.Where(a => a.Province == ProvinceID).OrderBy(a => a.Province);

            if (PartnerID != "Over All")
                SupplyRaw = SupplyRaw.Where(a => a.Partner == PartnerID).OrderBy(a => a.District);

            if (YearID != null)
                SupplyRaw = SupplyRaw.Where(a => a.Year == YearID).OrderBy(a => a.District);

            supplyData.SupplySchools = SupplyRaw.Sum(a => a.PlasticMat == 1 ? 1 : 0);

            var SupplyQry = SupplyRaw.GroupBy(a => a.Partner).Select(g => new
            {
                PartnerName = g.Key,
                total = g.Sum(a => a.PlasticMat == 1 ? 1 : 0),
                //female = g.Sum(a => a.Female1),
            });
            // supplyData.Supplies = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.PartnerName).ToListAsync();

            //  supplyData.SuppliesVal = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.total).ToListAsync();
            supplyData.SuppliesVal = new List<int>();
            supplyData.SuppliesVal.Add(SupplyRaw.Count(a => a.Partner == "Alight"));
            supplyData.SuppliesVal.Add(SupplyRaw.Count(a => a.Partner == "MF"));
           // supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "CAR").Sum(a => a.total));
            //supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "NCHD").Sum(a => a.total));

            var InfraQry = SupplyRaw.Where(a => a.SchoolSupportID >0).ToList();

            supplyData.InfraSchools = InfraQry.Count();

            supplyData.InfrastructureVal = new List<int>();
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.Washroom));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.SolarPanel));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.WaterTank));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.PlasticMat));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.WaterCooler));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.BlackBoard));
            // supplyData.InfraVal.Add(InfraQry.Sum(a => a.RepairSchoolGate));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.TeacherChair));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.Registar));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.LearningMaterial));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.LCDs));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.Tablets));

            ///// ****** 
            ///
            //supplyData.Total =(int) SupplyRaw.Count();
            //supplyData.TotalFormal = (int)SupplyRaw.Where(a=>a.SchoolType== "Formal").Count();
            //supplyData.TotalNonFormal = (int)SupplyRaw.Where(a=>a.SchoolType== "New-Non-Formal").Count();

            return PartialView(supplyData);
        }

        /// School Supported End
        /// 

        ///// Youth Facilitation

        [AllowAnonymous]
        public async Task<IActionResult> Youth()
        {

            List<string> ProvinceList = await _context.YouthFacilitation.Select(a => a.District).Distinct().ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["District"] = new SelectList(ProvinceList, "District");

            ////Year
            //List<string> Year = await _context.YouthFacilitation.Select(a => a.AdmnYear.ToString()).Distinct().ToListAsync();
            //Year.Insert(0, "Over All");
            //ViewData["Year"] = new SelectList(Year, "Year");

            //Type wise 
            List<string> Type = await _context.YouthFacilitation.Select(a => a.ModeFacilitation).Distinct().ToListAsync();
            Type.Insert(0, "Over All");
            ViewData["Type"] = new SelectList(Type, "Type");
            return View();
        }

        public async Task<IActionResult> YouthFilter(string DistrictID, string TypeID)
        {
            EnrollmentData enrollmentData = new EnrollmentData();

            
            // ProvinceID = ProvinceID == null ? "Over All" : ProvinceID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            var EnrollmentRaw = _context.YouthFacilitation.Where(a => a.YouthFacilitationID > 0).OrderBy(a => a.District);
            if (TypeID != "Over All")
                EnrollmentRaw = EnrollmentRaw.Where(a => a.ModeFacilitation == TypeID).OrderBy(a => a.District);
            //else if (TypeID != "Over All")
            //    EnrollmentRaw = EnrollmentRaw.Where(a => a.EducationType != "Formal").OrderBy(a => a.District);

          
            if (DistrictID != "Over All")
            {
                EnrollmentRaw = EnrollmentRaw.Where(a => a.District == DistrictID).OrderBy(a => a.District);
            }
            //else if (ProvinceID != "Over All")
            //{
            //    EnrollmentRaw = EnrollmentRaw.Where(a => a.Province == ProvinceID).OrderBy(a => a.Province);
            //}
            //////////**** District wise Enrollment
            var districtQry = EnrollmentRaw.GroupBy(a => a.District).Select(g => new
            {
                Districts = g.Key,
                //Province = g.Max(a => a.Province),
                BoysEnrolled = g.Count(a => a.Gender == "Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            enrollmentData.DistrictBoys = await districtQry.OrderBy(a => a.Districts).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.DistrictGirls = await districtQry.OrderBy(a => a.Districts).Select(a => a.GirlsEnrolled).ToListAsync();
            enrollmentData.Districts = await districtQry.OrderBy(a => a.Districts).Select(a => a.Districts).ToListAsync();

            ////**** Type wise Enrollment
            //var TypeQry = EnrollmentRaw.GroupBy(a => a.EducationType).Select(g => new
            //{
            //    EducationType = g.Key,
            //    Ednrolled = g.Sum(a => a.EnrolledBoys + a.EnrolledGirls),
            //    //GirlsEnrolled = g.Sum(a => a.EnrolledGirls),
            //});
            //enrollmentData.Type = await TypeQry.Select(a => a.EducationType).ToListAsync();
            //enrollmentData.TypeEnroll = await TypeQry.Select(a => a.Ednrolled).ToListAsync();

            //////**** Mode Facilitation
            ///
            var PartnerQry = EnrollmentRaw.OrderBy(a => a.ModeFacilitation).GroupBy(a => a.ModeFacilitation).Select(g => new
            {
                Partner = g.Max(a => a.ModeFacilitation),
                BoysEnrolled = g.Count(a => a.Gender == "Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            enrollmentData.Partners = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.Partner).ToListAsync();
            enrollmentData.partnerBoys = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.PartnerGirls = await PartnerQry.OrderBy(a => a.Partner).Select(a => a.GirlsEnrolled).ToListAsync();


            ////// ****** Skill Enhanced
            ///
            var ClassQry = EnrollmentRaw.Where(a=>a.SkillEnhanced!= "Not Applicable") .OrderBy(a => a.SkillEnhanced).GroupBy(a => a.SkillEnhanced).Select(g => new
            {
                Class = g.Max(a => a.SkillEnhanced),
                BoysEnrolled = g.Count(a => a.Gender == "Male"),
                GirlsEnrolled = g.Count(a => a.Gender == "Female"),
            });
            enrollmentData.Skills = await ClassQry.OrderBy(a => a.Class).Select(a => a.Class).ToListAsync();
            enrollmentData.ClassBoys = await ClassQry.OrderBy(a => a.Class).Select(a => a.BoysEnrolled).ToListAsync();
            enrollmentData.ClassGirls = await ClassQry.OrderBy(a => a.Class).Select(a => a.GirlsEnrolled).ToListAsync();


            return PartialView(enrollmentData);
        }


        /// Youth Facilitation End


    }
}
