using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MKTS.Data;
using MKTS.Models.View;

namespace MKTS.Controllers
{
    public class SupplyData
    {
        //District
        public List<int> InfrastructureVal { get; set; }
        public List<string> Infrastructure = new List<string> { "Washroom Repair", "Wall", "New Washroom", "Handpump", "Refurbishment", "Solar Panel", "School Gate", "Filtration Plant", "White Wash", "Electric Repair", "Flooring", };

        public List<string> Supplies = new List<string> { "Plastic Mat", " Water Cooler", "Black Board", "Teacher Chair", "Attendance Registar", "Water Tank", "Desk", "Cupboard/Rack", "Electric Cooler" };
        public List<int> SuppliesVal { get; set; }

        public int SupplySchools { get; set; }
        public int InfraSchools { get; set; }

        //Level


    }

    public class SupplyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplyController(ApplicationDbContext context)
        {
            //_hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
          
            //Province
            List<string> ProvinceList = await _context.schoolSupply.Select(a => a.Province).Distinct().ToListAsync();
            ProvinceList.Insert(0, "Over All");
            ViewData["Province"] = new SelectList(ProvinceList, "Province");

            //District
            List<string> DistrictList = await _context.schoolSupply.Select(a => a.District).Distinct().ToListAsync();
            DistrictList.Insert(0, "Over All");
            ViewData["District"] = new SelectList(DistrictList, "District");

            //Partner
            List<string> PartnerList = await _context.schoolSupply.Select(a => a.Partner).Distinct().ToListAsync();
            PartnerList.Insert(0, "Over All");
            ViewData["Partner"] = new SelectList(PartnerList, "Partner");

            //Year
            //Year
            List<string> Year = await _context.Enrollment.Select(a => a.Year.ToString()).Distinct().ToListAsync();
            Year.Insert(0, "Over All");
            ViewData["Year"] = new SelectList(Year, "Year");



            return View();
        }
        public async Task<IActionResult> Filter(string ProvinceID, string DistrictID, int? YearID, string PartnerID)
        {
            // trainingData trainingData = new trainingData();

            SupplyData supplyData = new SupplyData
            {
                Infrastructure = new List<string>(),
             
                Supplies = new List<string>(),
                          };


            //YearID = YearID == null ? 0 : YearID;
            ProvinceID = ProvinceID == null ? "Over All" : ProvinceID;
            DistrictID = DistrictID == null ? "Over All" : DistrictID;
            PartnerID = PartnerID == null ? "Over All" : PartnerID;
            //  YearID = YearID == null ? "Over All" : YearID;
            // TypeID = TypeID == null ? "Over All" : TypeID;
            //if (YearID==null)
            //{
            //    YearID = (short)_context.Enrollment.Max(a => a.Year);
            //}
            SqlParameter ProvincePara = new SqlParameter("@Province", ProvinceID == "Over All" ? (object)DBNull.Value : ProvinceID);
            SqlParameter DistPara = new SqlParameter("@District", DistrictID == "Over All" ? (object)DBNull.Value : DistrictID);
            SqlParameter YearPara = new SqlParameter("@Year", YearID == null ? (object)DBNull.Value : YearID);
            SqlParameter PartnerPara = new SqlParameter("@Partner", PartnerID == "Over All" ? (object)DBNull.Value : PartnerID);

            var InfraGenderQuery =  _context.GenderSP.FromSqlRaw("exec InfraGenderSP" +
                               " @Province, @District, @Year,@Partner",
                                  ProvincePara, DistPara,YearPara,PartnerPara
                               
                               ); ; //.ToList<IndicatorsTotalTarget>(); ;





            var SupplyRaw =  _context.schoolSupply.Where(a => a.SSID > 0).OrderBy(a => a.Province).ThenBy(a => a.District);

            if (DistrictID != "Over All")
            {
                SupplyRaw = SupplyRaw.Where(a => a.District == DistrictID).OrderBy(a => a.District);
            }
            if (ProvinceID != "Over All")
          
                SupplyRaw = SupplyRaw.Where(a => a.Province == ProvinceID).OrderBy(a => a.Province);

            if(PartnerID!="Over All")
                SupplyRaw = SupplyRaw.Where(a => a.Partner == PartnerID).OrderBy(a => a.Partner);

            if(YearID!=null)
                SupplyRaw = SupplyRaw.Where(a => a.Year == YearID).OrderBy(a => a.Province);

            supplyData.SupplySchools = SupplyRaw.Sum(a => a.PlasticMat == 1?1:0);

            var SupplyQry = SupplyRaw.GroupBy(a => a.Partner).Select(g => new
            {
                PartnerName = g.Key,
                total = g.Sum(a => a.PlasticMat == 1 ? 1 : 0),
                //female = g.Sum(a => a.Female1),
            });
            // supplyData.Supplies = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.PartnerName).ToListAsync();

          //  supplyData.SuppliesVal = await SupplyQry.OrderBy(a => a.PartnerName).Select(a => a.total).ToListAsync();
            supplyData.SuppliesVal = new List<int>();
            supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "Alight NFS").Sum(a => a.total));
            supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "BECS").Sum(a => a.total));
            supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "CAR").Sum(a => a.total));
            supplyData.SuppliesVal.Add(SupplyQry.Where(a => a.PartnerName == "NCHD").Sum(a => a.total));

            var InfraQry = SupplyRaw.Where(a => a.Partner == "SED").ToList();

            supplyData.InfraSchools = InfraQry.Count();

            supplyData.InfrastructureVal = new List<int>();
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.WashroomRepair));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.ConstructionWall));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.ConstructionWashroom));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.InstallationHandpump));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.RefurbishmentSchool));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.InstallationSolarPanel));
            // supplyData.InfraVal.Add(InfraQry.Sum(a => a.RepairSchoolGate));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.InstallationFiltrationPlant));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.WhiteWash));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.ElectricRepairments));
            supplyData.InfrastructureVal.Add(InfraQry.Sum(a => a.Flooring));

            ///// ****** 
            ///
            //supplyData.Total =(int) SupplyRaw.Count();
            //supplyData.TotalFormal = (int)SupplyRaw.Where(a=>a.SchoolType== "Formal").Count();
            //supplyData.TotalNonFormal = (int)SupplyRaw.Where(a=>a.SchoolType== "New-Non-Formal").Count();

            return PartialView(supplyData);
        }

        public JsonResult GetDistrict(string ProvinceID)
        {
            List<string> districtList = _context.schoolSupply.Where(a => a.Province == ProvinceID).Select(a => a.District).Distinct().ToList();
            // districtList = _context.District.Where(a => a.ProvinceName == ProvinceID).ToList();
            districtList.Insert(0, "Over All");
            return Json(new SelectList(districtList, "DistrictName"));
        }
    }
}