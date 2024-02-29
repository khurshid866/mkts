using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MKTS.Models.Data;
using MKTS.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using EFCore.BulkExtensions;
using System.Data.SqlClient;
using MKTS.Models;

namespace MKTS.Controllers
{
    public class WizardController : Controller
    {
        private readonly ApplicationDbContext _context;

        private IHostingEnvironment _hostingEnvironment;
        public WizardController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Account", "Login");
            }
            if (id>0)
            {
                //IFormFile file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                string sFileExtension = ".xlsx";
                string fullPath = Path.Combine(newPath, "UploadedFile" + sFileExtension);
                //
                string[] Province = await _context.Province.Select(a => a.ProvinceName).ToArrayAsync();
                string[] District = await _context.District.Select(a => a.DistrictName).ToArrayAsync();
                string[] Year = await _context.Year.Select(a => a.YearName.ToString()).ToArrayAsync();
                string[] Season = await _context.Season.Select(a => a.SeasonName).ToArrayAsync();
                string[] TypeOfEducation = await _context.TypeOfEducation.Select(a => a.TypeEducation).ToArrayAsync();
                string[] Partner = await _context.Partner.Select(a => a.PartnerName).ToArrayAsync();
                string[] Class = await _context.Class.Select(a => a.ClassName.ToString()).ToArrayAsync();
                string[] TrainingTheme = await _context.TrainingTheme.Select(a => a.ThemeName).ToArrayAsync();
                string[] Course = new string[] { "New", "Refresher" };
                string[] TargetGroup = await _context.TargetGroup.Select(a => a.GroupName).ToArrayAsync();
                string[] TrainingConductedBy = await _context.TrainingConductedBy.Select(a => a.ConductedBy).ToArrayAsync();
                string[] TypeOfParticipant = await _context.TypeOfParticipant.Select(a => a.Participant).ToArrayAsync();

                string[] Level = new string[] { "Primary", "Middle", "High" };
                string[] Gender = new string[] { "Boys", "Girls", "CoEducation" };
                string[] DateString = new string[] { "date" };
                string[] TrainingType = new string[] { "In-Class", "Online" };
                string[] emptyString = new string[] { };
                string[][] data = new string[][]
                {
             /*0*/   emptyString,
             /*1*/   Province,
             /*2*/   District,
             /*3*/   Year,
             /*4*/   Season,
             /*5*/   TypeOfEducation,
             /*6*/   Partner,
             /*7*/   Class,
             /*8*/   emptyString,
             /*9*/   emptyString,
            };

                StringBuilder sb = new StringBuilder();
                     
                    ISheet sheet1, sheet2, sheet3, sheet4;

                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        //file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet1 = hssfwb.GetSheetAt(0);
                            sheet2 = hssfwb.GetSheetAt(1);
                            sheet3 = hssfwb.GetSheetAt(2);
                            sheet4 = hssfwb.GetSheetAt(3);
                        }
                        else //This will read 2007 Excel format    
                        {
                            try
                            {
                                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                                sheet1 = hssfwb.GetSheetAt(0);
                                sheet2 = hssfwb.GetSheetAt(1);
                                sheet3 = hssfwb.GetSheetAt(2);
                                sheet4 = hssfwb.GetSheetAt(3);
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Error = "Uploaded file should have 4 sheets (Enrollment, Retention, Supplies & Training)";
                                return View();
                            }

                        }

                        #region Sheet 1 Enrollment
                        {
                            IRow headerRow = sheet1.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            if (cellCount != 10)
                            {
                                ViewBag.Error = "Enrollment sheet doest not have correct columns";
                                return View();
                            }
                            // Start creating the html which would be displayed in tabular format on the screen  
                            sb.Append("<table class='table'><tr>");
                            for (int j = 0; j < cellCount; j++)
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                                sb.Append("<th>" + cell.ToString() + "</th>");
                            }
                            sb.Append("</tr>");
                            sb.AppendLine("<tr>");
                            for (int i = (sheet1.FirstRowNum + 1); i <= sheet1.LastRowNum; i++)
                            {
                                IRow row = sheet1.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                        sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 1));
                                // sb.Append("<td>" + row.GetCell(j).ToString() +"</td>");
                                else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                            }
                                sb.AppendLine("</tr>");
                            }
                            sb.Append("</table>");
                            ViewData["EnrollmentData"] = sb.ToString();
                        }
                        #endregion

                        #region Sheet 2 Retention
                        {
                            sb = new StringBuilder();
                            data = new string[][]
                                {
                             /*0*/   emptyString,
                             /*1*/   Province,
                             /*2*/   Partner,
                             /*3*/   Year,
                             ///*4*/   Season,
                             /*5*/   emptyString,
                             /*6*/   emptyString,
                             /*7*/   emptyString,
                             /*8*/   emptyString,
                            };
                            IRow headerRow = sheet2.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            if (cellCount != 8)
                            {
                                ViewBag.Error = "Retention sheet does not have correct columns";
                                return View();
                            }
                            // Start creating the html which would be displayed in tabular format on the screen  
                            sb.Append("<table class='table'><tr>");
                            for (int j = 0; j < cellCount; j++)
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                                sb.Append("<th>" + cell.ToString() + "</th>");
                            }
                            sb.Append("</tr>");
                            sb.AppendLine("<tr>");
                            for (int i = (sheet2.FirstRowNum + 1); i <= sheet2.LastRowNum; i++)
                            {
                                IRow row = sheet2.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                        sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 2));
                                // sb.Append("<td>" + row.GetCell(j).ToString() +"</td>");
                                else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                            }
                                sb.AppendLine("</tr>");
                            }
                            sb.Append("</table>");
                            ViewData["RetentionData"] = sb.ToString();
                        }
                        #endregion

                        #region Sheet 3 Supplies
                        {
                            sb = new StringBuilder();
                        data = new string[][]
                         {
                             /*0*/   emptyString,
                             /*1*/   emptyString,
                             /*2*/   TypeOfEducation,
                             /*3*/   Level,
                             /*4*/   Gender,
                             /*5*/   Province,
                             /*6*/   District,
                             /*7*/   Partner,
                             /*8*/   Year,
                             /*9*/   emptyString,
                             /*10*/   emptyString,
                             /*11*/   emptyString,
                             /*12*/   emptyString,
                             /*13*/   emptyString,
                             /*14*/   emptyString,
                             /*15*/   emptyString,
                             /*16*/   emptyString,
                             /*17*/   emptyString,
                             /*18*/   emptyString,
                             /*19*/   emptyString,
                             /*20*/   emptyString,
                             /*21*/   emptyString,
                             /*22*/   emptyString,
                             /*23*/   emptyString,
                             /*24*/   emptyString,
                             /*25*/   emptyString,
                             /*26*/   emptyString,
                             /*27*/   emptyString,
                             /*28*/   emptyString,
                             ///*29*/   emptyString,
                             ///*30*/   emptyString,

                         };
                        IRow headerRow = sheet3.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            if (cellCount != 29)
                            {
                                ViewBag.Error = "Supplies sheet does not have correct number of columns. Current columns="+cellCount;
                                return View();
                            }
                            // Start creating the html which would be displayed in tabular format on the screen  
                            sb.Append("<table class='table'><tr>");
                            for (int j = 0; j < cellCount; j++)
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                                sb.Append("<th>" + cell.ToString() + "</th>");
                            }
                            sb.Append("</tr>");
                            sb.AppendLine("<tr>");
                            for (int i = (sheet3.FirstRowNum + 1); i <= sheet3.LastRowNum; i++)
                            {
                                IRow row = sheet3.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                        if (j == 1 || j >8) // ignore these columns 
                                            sb.Append("<td>" + row.GetCell(j).ToString().Trim() + "</td>");
                                        else
                                            sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 3));
                                else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                            }
                                sb.AppendLine("</tr>");
                            }
                            sb.Append("</table>");
                            ViewData["suppliesData"] = sb.ToString();
                        }
                        #endregion 3 Supplies


                        #region Sheet 4 Training
                        {
                            sb = new StringBuilder();
                            data = new string[][]
                                {
                             /*0*/   emptyString,
                             /*1*/   Province,
                             /*2*/   District,
                             /*3*/   TrainingType,
                             /*4*/   TrainingTheme,
                             /*5*/   Partner,
                             /*6*/   TargetGroup,
                             /*7*/   emptyString,
                             /*7*/   DateString,
                             /*8*/   DateString,
                             /*10*/   TrainingConductedBy,
                             ///*10*/   TypeOfParticipant,
                             /*11*/   emptyString,
                             /*12*/   emptyString,
                             ///*13*/   TypeOfParticipant,
                             ///*14*/   emptyString,
                             ///*15*/   emptyString,
                            };
                            IRow headerRow = sheet4.GetRow(0);
                            int cellCount = headerRow.LastCellNum;
                            if (cellCount != 13)
                            {
                                ViewBag.Error = "Training sheet does not have correct number of columns Existing Column is "+cellCount;
                                return View();
                            }
                            // Start creating the html which would be displayed in tabular format on the screen  
                            sb.Append("<table class='table'><tr>");
                            for (int j = 0; j < cellCount; j++)
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                                sb.Append("<th>" + cell.ToString() + "</th>");
                            }
                            sb.Append("</tr>");
                            sb.AppendLine("<tr>");
                            for (int i = (sheet4.FirstRowNum + 1); i <= sheet4.LastRowNum; i++)
                            {
                                IRow row = sheet4.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    //if (row.GetCell(j) != null)
                                    //    if (j == 6 || j == 11 || j == 12 || j == 14 || j == 15) // get formula values 
                                    //        sb.Append(ValidateEnrollment(row.GetCell(j).NumericCellValue.ToString(), data[j], j, 2));
                                    //    else
                                            sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 2));
                                }
                                sb.AppendLine("</tr>");
                            }
                            sb.Append("</table>");
                            ViewData["TrainingData"] = sb.ToString();
                        }
                        #endregion 4 Training

                    }
                

                if (ViewData["EnrollmentData"].ToString().Contains("style")
                    || ViewData["RetentionData"].ToString().Contains("style")
                    || ViewData["TrainingData"].ToString().Contains("style")
                    || ViewData["suppliesData"].ToString().Contains("style")
                    )
                { ViewBag.AllClear = false; }
                else
                { ViewBag.allClear = true; }
                ViewBag.Error = "";
                ViewBag.TotEnroll =sheet1.LastRowNum;
                ViewBag.TotReten =sheet2.LastRowNum;
                ViewBag.TotSup =sheet3.LastRowNum;
                ViewBag.TotTrain =sheet4.LastRowNum;
            }
            return View();
        }
        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        public async Task<IActionResult> Index()
        {
            ISheet sheet1, sheet2, sheet3, sheet4;

            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            //
            string[] Province = await _context.Province.Select(a => a.ProvinceName).ToArrayAsync();
            string[] District = await _context.District.Select(a => a.DistrictName).ToArrayAsync();
            string[] Year = await _context.Year.Select(a => a.YearName.ToString()).ToArrayAsync();
            string[] Season = await _context.Season.Select(a => a.SeasonName).ToArrayAsync();
            string[] TypeOfEducation = await _context.TypeOfEducation.Select(a => a.TypeEducation).ToArrayAsync();
            string[] Partner = await _context.Partner.Select(a => a.PartnerName).ToArrayAsync();
            string[] Class = await _context.Class.Select(a => a.ClassName.ToString()).ToArrayAsync();
            string[] TrainingTheme = await _context.TrainingTheme.Select(a => a.ThemeName).ToArrayAsync();
            string[] Course = new string[] { "New", "Refresher" };
            string[] TargetGroup = await _context.TargetGroup.Select(a => a.GroupName).ToArrayAsync();
            string[] TrainingConductedBy = await _context.TrainingConductedBy.Select(a => a.ConductedBy).ToArrayAsync();
            string[] TypeOfParticipant = await _context.TypeOfParticipant.Select(a => a.Participant).ToArrayAsync();

            string[] Level = new string[] { "Primary", "Middle", "High" };
            string[] Gender = new string[] { "Boys", "Girls", "CoEducation" };
            string[] TrainingType = new string[] { "In-Class", "Online" };
            string[] DateString = new string[] { "date" };
            string[] emptyString = new string[] { };
            string[][] data = new string[][]
            {
             /*0*/   emptyString,
             /*1*/   Province,
             /*2*/   District,
             /*3*/   Year,
             /*4*/   Season,
             /*5*/   TypeOfEducation,
             /*6*/   Partner,
             /*7*/   Class,
             /*8*/   emptyString,
             /*9*/   emptyString,
        };

            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                if (sFileExtension != ".xlsx")//Check file Extention 
                {
                    ModelState.AddModelError("Model", "Please Upload xlsx file only");
                    ViewBag.Error = "Please Upload .xlsx file only";
                    return View();
                }
                string fullPath = Path.Combine(newPath, "UploadedFile" + sFileExtension);


                using (var stream = new FileStream(fullPath, FileMode.Create))

                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet1 = hssfwb.GetSheetAt(0);
                        sheet2 = hssfwb.GetSheetAt(1);
                        sheet3 = hssfwb.GetSheetAt(2);
                        sheet4 = hssfwb.GetSheetAt(3);
                    }
                    else //This will read 2007 Excel format    
                    {
                        try
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            sheet1 = hssfwb.GetSheetAt(0);
                            sheet2 = hssfwb.GetSheetAt(1);
                            sheet3 = hssfwb.GetSheetAt(2);
                            sheet4 = hssfwb.GetSheetAt(3);

                            ViewBag.TotEnroll = sheet1.LastRowNum;
                            ViewBag.TotReten = sheet2.LastRowNum;
                            ViewBag.TotSup = sheet3.LastRowNum;
                            ViewBag.TotTrain = sheet4.LastRowNum;

                        }
                        catch (Exception ex)
                        {
                            ViewBag.Error = "Uploaded file should have 4 sheets (Enrollment, Retention, Supplies & Training)";
                            return View();
                        }

                    }

                    #region Sheet 1 Enrollment
                    {
                        IRow headerRow = sheet1.GetRow(0);
                       
                        if (headerRow==null || headerRow.LastCellNum != 10)
                        {
                            ViewBag.Error = "sheet 1: Enrollment sheet doest not have correct number of columns or Fisrt row should be Headings";
                            return View();
                        }
                        int cellCount = headerRow.LastCellNum;
                        // Start creating the html which would be displayed in tabular format on the screen  
                        sb.Append("<table class='table'><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");
                        for (int i = (sheet1.FirstRowNum + 1); i <= sheet1.LastRowNum; i++)
                        {
                            IRow row = sheet1.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 1));
                                // sb.Append("<td>" + row.GetCell(j).ToString() +"</td>");
                                else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                            }
                            sb.AppendLine("</tr>");
                        }
                        sb.Append("</table>");
                        ViewData["EnrollmentData"] = sb.ToString();
                    }
                    #endregion

                    #region Sheet 2 Retention
                    {
                        sb = new StringBuilder();
                        data = new string[][]
                            {
                             /*0*/   emptyString,
                             /*1*/   Province,
                             /*2*/   Partner,
                             /*3*/   Year,
                             ///*4*/   Season,
                             /*5*/   emptyString,
                             /*6*/   emptyString,
                             /*7*/   emptyString,
                             /*8*/   emptyString,
                        };
                        IRow headerRow = sheet2.GetRow(0);
                        if (headerRow==null || headerRow.LastCellNum != 8)
                        {
                            ViewBag.Error = "Retention sheet does not have correct number of columns or First row should be Headings";
                            return View();
                        }
                        int cellCount = headerRow.LastCellNum;

                        // Start creating the html which would be displayed in tabular format on the screen  
                        sb.Append("<table class='table'><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");
                        for (int i = (sheet2.FirstRowNum + 1); i <= sheet2.LastRowNum; i++)
                        {
                            IRow row = sheet2.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    if (j >5) // get formula values 
                                        sb.Append(ValidateEnrollment(row.GetCell(j).NumericCellValue.ToString(), data[j], j, 2));
                                    else
                                        sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 2));
                                else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                                // sb.Append("<td>" + row.GetCell(j).ToString() +"</td>");
                            }
                            sb.AppendLine("</tr>");
                        }
                        sb.Append("</table>");
                        ViewData["RetentionData"] = sb.ToString();
                    }
                    #endregion

                    #region Sheet 3 Supplies
                    {
                        sb = new StringBuilder();
                        data = new string[][]
                            {
                             /*0*/   emptyString,
                             /*1*/   emptyString,
                             /*2*/   TypeOfEducation,
                             /*3*/   Level,
                             /*4*/   Gender,
                             /*5*/   Province,
                             /*6*/   District,
                             /*7*/   Partner,
                             /*8*/   Year,
                             /*9*/   emptyString,
                             /*10*/   emptyString,
                             /*11*/   emptyString,
                             /*12*/   emptyString,
                             /*13*/   emptyString,
                             /*14*/   emptyString,
                             /*15*/   emptyString,
                             /*16*/   emptyString,
                             /*17*/   emptyString,
                             /*18*/   emptyString,
                             /*19*/   emptyString,
                             /*20*/   emptyString,
                             /*21*/   emptyString,
                             /*22*/   emptyString,
                             /*23*/   emptyString,
                             /*24*/   emptyString,
                             /*25*/   emptyString,
                             /*26*/   emptyString,
                             /*27*/   emptyString,
                             /*28*/   emptyString,
                             ///*29*/   emptyString,
                             ///*30*/   emptyString,

                        };
                        IRow headerRow = sheet3.GetRow(0);
                        if (headerRow==null ||headerRow.LastCellNum != 29)
                        {
                            ViewBag.Error = "Supplies sheet does not have correct number of columns or 1st row should be Heading";
                            return View();
                        }
                        int cellCount = headerRow.LastCellNum;

                        // Start creating the html which would be displayed in tabular format on the screen  
                        sb.Append("<table class='table'><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");
                        for (int i = (sheet3.FirstRowNum + 1); i <= sheet3.LastRowNum; i++)
                        {
                            IRow row = sheet3.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    if (j==1 ||j >8) // ignore these columns 
                                        sb.Append("<th>" + row.GetCell(j).ToString().Trim() + "</th>");
                                    else
                                        sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 3));
                               else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                            }
                            sb.AppendLine("</tr>");
                        }
                        sb.Append("</table>");
                        ViewData["suppliesData"] = sb.ToString();
                    }
                    #endregion 3 Supplies

                    #region Sheet 4 Training
                    {
                        sb = new StringBuilder();
                        data = new string[][]
                            {
                               /*0*/   emptyString,
                             /*1*/   Province,
                             /*2*/   District,
                             /*3*/   TrainingType,
                             /*4*/   TrainingTheme,
                             /*5*/   Partner,
                             /*6*/   TargetGroup,
                             /*7*/   emptyString,
                             /*7*/   DateString,
                             /*8*/   DateString,
                             /*10*/   TrainingConductedBy,
                             ///*10*/   TypeOfParticipant,
                             /*11*/   emptyString,
                             /*12*/   emptyString,
                             ///*13*/   TypeOfParticipant,
                             ///*14*/   emptyString,
                             ///*15*/   emptyString,
                        };
                        IRow headerRow = sheet4.GetRow(0);
                        if (headerRow==null ||headerRow.LastCellNum != 13)
                        {
                            ViewBag.Error = "Training sheet does not have correct number of columns or 1st row should be Heading";
                            return View();
                        }
                        int cellCount = headerRow.LastCellNum;

                        // Start creating the html which would be displayed in tabular format on the screen  
                        sb.Append("<table class='table'><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");
                        for (int i = (sheet4.FirstRowNum + 1); i <= sheet4.LastRowNum; i++)
                        {
                            IRow row = sheet4.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    //if (j == 6 || j == 11 || j == 12 || j == 14 || j == 15) // get formula values 
                                    //    sb.Append(ValidateEnrollment(row.GetCell(j).NumericCellValue.ToString(), data[j], j, 2));
                                    //else
                                        sb.Append(ValidateEnrollment(row.GetCell(j).ToString().Trim(), data[j], j, 2));
                                else
                                    sb.Append(" <td style=\"background-color:coral\" > </td> ");
                            }
                            sb.AppendLine("</tr>");
                        }
                        sb.Append("</table>");
                        ViewData["TrainingData"] = sb.ToString();
                    }
                    #endregion 4 Training

                }
            }

            if (ViewData["EnrollmentData"].ToString().Contains("style"))
                { ViewBag.EnrollError = "1. Enrolled Sheet has error"; }

            if (ViewData["RetentionData"].ToString().Contains("style"))
            { ViewBag.RetenError = "2. Retention Sheet has error"; }

            if (ViewData["suppliesData"].ToString().Contains("style"))
            { ViewBag.SupplyError = "3. Supplies Sheet has error"; }

            if (ViewData["TrainingData"].ToString().Contains("style"))
            { ViewBag.TrainError = "4. Training Sheet has error"; }

            if (ViewData["EnrollmentData"].ToString().Contains("style")
                || ViewData["RetentionData"].ToString().Contains("style")
                || ViewData["TrainingData"].ToString().Contains("style")
                || ViewData["suppliesData"].ToString().Contains("style")
                )
            { ViewBag.AllClear = false; }

            else
            { ViewBag.allClear = true; }
            ViewBag.Error = "";
            //return this.Content(sb.ToString());
            return View();

        }

        public async Task<IActionResult> DuplicatesOld(short id)
        {
            ISheet sheet1, sheet2, sheet3, sheet4;
            //StringBuilder sb = new StringBuilder();

            RawDataCollection rawDataCol = new RawDataCollection()
            { Enrollments = new List<Enrollment>(),
                Retentions = new List<Retention>(),
                Trainings = new List<Training>(),
                id = id
            };
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string sFileExtension = ".xlsx";
            string fullPath = Path.Combine(newPath, "UploadedFile" + sFileExtension);
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                //file.CopyTo(stream);
                stream.Position = 0;
                if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                    sheet1 = hssfwb.GetSheetAt(0);
                    sheet2 = hssfwb.GetSheetAt(1);
                    sheet3 = hssfwb.GetSheetAt(2);
                    sheet4 = hssfwb.GetSheetAt(3);
                }
                else //This will read 2007 Excel format    
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                    sheet1 = hssfwb.GetSheetAt(0);
                    sheet2 = hssfwb.GetSheetAt(1);
                    sheet3 = hssfwb.GetSheetAt(2);
                    sheet4 = hssfwb.GetSheetAt(3);
                }
                switch (id)
                {
                    case 1: // Sheet 1 Enrollment

                        for (int i = (sheet1.FirstRowNum + 1); i <= sheet1.LastRowNum; i++)
                        {
                            IRow row = sheet1.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            /// compare each record with database and mark Dup if exist. 
                            Enrollment CurEnroll = new Enrollment()
                            {
                                SerialNo = short.Parse(row.GetCell(0).ToString()),
                                Province = row.GetCell(1).ToString(),
                                District = row.GetCell(2).ToString(),
                                Year = (int)row.GetCell(3).NumericCellValue,
                                Season = row.GetCell(4).ToString(),
                                EducationType = row.GetCell(5).ToString(),
                                Partner = row.GetCell(6).ToString(),
                                Class = (short)row.GetCell(7).NumericCellValue,
                                EnrolledBoys = (int)row.GetCell(8).NumericCellValue,
                                EnrolledGirls = (int)row.GetCell(9).NumericCellValue,
                                IsDuplicate = false,
                            };
                            // bool result = _context.StoredProc(parameters);
                            //return (result == true) ? true : false;
                            //var query = await _context.Enrollment.Where(a => a.Province ==CurEnroll.Province
                            //                                        && a.District == CurEnroll.District
                            //                                        && a.Year == CurEnroll.Year
                            //                                        && a.Season == CurEnroll.Season
                            //                                        && a.EducationType== CurEnroll.EducationType
                            //                                        && a.Partner == CurEnroll.Partner
                            //                                        && a.Class == CurEnroll.Class
                            //).Select(a=>a.EnrollmentID).FirstOrDefaultAsync();


                            CheckDuplicate query = await _context.checkDuplicates.FromSqlRaw("exec EnrollmentCheckDuplicate" +
                                " @Province, @District, @Year, @Season, @EducationType, @Partner, @Class",
                                new SqlParameter("@Province", CurEnroll.Province),
                                new SqlParameter("@District", CurEnroll.District),
                                new SqlParameter("@Year", CurEnroll.Year),
                                new SqlParameter("@Season", CurEnroll.Season),
                                new SqlParameter("@EducationType", CurEnroll.EducationType),
                                new SqlParameter("@Partner", CurEnroll.Partner),
                                new SqlParameter("@Class", CurEnroll.Class)
                                ).FirstOrDefaultAsync(); ; //.ToList<IndicatorsTotalTarget>(); ;


                            if (query.Id > 0)
                            {
                                CurEnroll.EnrollmentID = (int)query.Id;
                                CurEnroll.IsDuplicate = true;
                            }
                            //if(rawDataCol.Enr.Any(a=>a.District== CurEnroll.District && a.year==CurEnroll.year))
                            rawDataCol.Enrollments.Add(CurEnroll);

                            // sb.AppendLine("</tr>");
                        }

                        break;

                    case 2: // Sheet 2 Retention

                        for (int i = (sheet2.FirstRowNum + 1); i <= sheet2.LastRowNum; i++)
                        {
                            IRow row = sheet2.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            /// compate each record with database and mark Dup if exist. 
                            Retention CurReten = new Retention()
                            {
                                SerialNo = (short)row.GetCell(0).NumericCellValue,
                                Province = row.GetCell(1).ToString(),
                                Partner = row.GetCell(2).ToString(),
                                Year = (int)row.GetCell(3).NumericCellValue,
                                DropoutBoys = (int)row.GetCell(4).NumericCellValue,
                                DropoutGirls = (int)row.GetCell(5).NumericCellValue,
                                IsDuplicate = false,
                            };
                            var query = await _context.checkDuplicates.FromSqlRaw("exec RetentionCheckDuuplicate" +
                                " @Province, @Partner,@Year,  @DropoutBoys,@DropoutGirls",
                                new SqlParameter("@Province", CurReten.Province),
                                new SqlParameter("@Partner", CurReten.Partner),
                                new SqlParameter("@Year", CurReten.Year),

                                new SqlParameter("@DropoutBoys", CurReten.DropoutBoys),
                                new SqlParameter("@DropoutGirls", CurReten.DropoutGirls)
                                ).FirstOrDefaultAsync(); ; //.ToList<IndicatorsTotalTarget>(); ;

                            //Retention query = await _context.Retention.Where(a => a.Province == CurReten.Province
                            //                                       && a.Partner == CurReten.Partner
                            //                                       && a.Year == CurReten.Year
                            //                                       && a.Dropout == CurReten.Dropout
                            //                                       && a.RetentionPercent == CurReten.RetentionPercent
                            //    ).FirstOrDefaultAsync(); ; //.ToList<IndicatorsTotalTarget>(); ;

                            if (query.Id > 0)
                            {
                                CurReten.RetentionID = (int)query.Id;
                                CurReten.IsDuplicate = true;
                            }
                            rawDataCol.Retentions.Add(CurReten);

                            // sb.AppendLine("</tr>");
                        }

                        break;
                    case 3: break;
                    case 4: break;
                }


                // Sheet 4 Training
                for (int i = (sheet4.FirstRowNum + 1); i <= sheet4.LastRowNum; i++)
                {
                    IRow row = sheet4.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    /// compate each record with database and mark Dup if exist. 
                    Training CurTrain = new Training()
                    {
                        SerialNo = short.Parse(row.GetCell(0).ToString()),
                        ProvinceName = row.GetCell(1).ToString(),
                        DistrictName = row.GetCell(2).ToString(),
                        TrainingType = row.GetCell(3).ToString(),
                        ThemeName = row.GetCell(4).ToString(),
                        Partner = row.GetCell(5).ToString(),
                        GroupName = row.GetCell(6).ToString(),
                        Duration = (short) row.GetCell(7).NumericCellValue,
                       StartDate  = row.GetCell(8).DateCellValue,
                       EndDate  = row.GetCell(9).DateCellValue,
                        ConductedBy = row.GetCell(10).ToString(),
                        Male1 = (short)row.GetCell(11).NumericCellValue,
                        Female1 = (short)row.GetCell(12).NumericCellValue,
                      

                        IsDuplicate = false,
                    };
                    Training query = await _context.Training.Where(a => a.ProvinceName == CurTrain.ProvinceName
                                                           && a.DistrictName == CurTrain.DistrictName
                                                           && a.ThemeName == CurTrain.ThemeName
                                                           && a.Partner == CurTrain.Partner
                                                           && a.StartDate == CurTrain.StartDate
                    ).FirstOrDefaultAsync();

                    if (query != null)
                    {
                        CurTrain.TrainingID = query.TrainingID;
                        CurTrain.IsDuplicate = true;
                    }
                    rawDataCol.Trainings.Add(CurTrain);

                    // sb.AppendLine("</tr>");
                }
                //sb.Append("</table>");
            }
            // ViewData["ImportedData"] = sb.ToString();
            rawDataCol.IsRecDup = true;
            return View(rawDataCol);
        }

        public IActionResult Duplicates (short id, int tot)
        {
            

            ViewBag.id = id;
            ViewBag.tot = tot;
            int t = (int)(tot*.0066);
            ViewBag.Time = t > 1 ? t - 1 : t;
            return View();
        }
        public async Task<IActionResult> DuplicatePartial(short id)
        {
                ISheet sheet1, sheet2, sheet3, sheet4;
                //StringBuilder sb = new StringBuilder();

                RawDataCollection rawDataCol = new RawDataCollection()
                {
                    Enrollments = new List<Enrollment>(),
                    Retentions = new List<Retention>(),
                    Trainings = new List<Training>(),
                    SchoolSupplies = new List<SchoolSupply>(),
                    id = id
                };
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                string sFileExtension = ".xlsx";
                string fullPath = Path.Combine(newPath, "UploadedFile" + sFileExtension);
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    //file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet1 = hssfwb.GetSheetAt(0);
                        sheet2 = hssfwb.GetSheetAt(1);
                        sheet3 = hssfwb.GetSheetAt(2);
                        sheet4 = hssfwb.GetSheetAt(3);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet1 = hssfwb.GetSheetAt(0);
                        sheet2 = hssfwb.GetSheetAt(1);
                        sheet3 = hssfwb.GetSheetAt(2);
                        sheet4 = hssfwb.GetSheetAt(3);
                    }
                    switch (id)
                    {
                        case 1: // Sheet 1 Enrollment

                            for (int i = (sheet1.FirstRowNum + 1); i <= sheet1.LastRowNum; i++)
                            {
                                IRow row = sheet1.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                                /// compare each record with database and mark Dup if exist. 
                                Enrollment CurEnroll = new Enrollment()
                                {
                                    SerialNo = short.Parse(row.GetCell(0).ToString()),
                                    Province = row.GetCell(1).ToString(),
                                    District = row.GetCell(2).ToString(),
                                    Year = (int)row.GetCell(3).NumericCellValue,
                                    Season = row.GetCell(4).ToString(),
                                    EducationType = row.GetCell(5).ToString(),
                                    Partner = row.GetCell(6).ToString(),
                                    Class = (short)row.GetCell(7).NumericCellValue,
                                    EnrolledBoys = (int)row.GetCell(8).NumericCellValue,
                                    EnrolledGirls = (int)row.GetCell(9).NumericCellValue,
                                    IsDuplicate = false,
                                };
                            // bool result = _context.StoredProc(parameters);
                            //return (result == true) ? true : false;
                            var query = await _context.Enrollment.Where(a => a.Province == CurEnroll.Province
                                                                    && a.District == CurEnroll.District
                                                                    && a.Year == CurEnroll.Year
                                                                    && a.Season == CurEnroll.Season
                                                                    && a.EducationType == CurEnroll.EducationType
                                                                    && a.Partner == CurEnroll.Partner
                                                                    && a.Class == CurEnroll.Class
                            ).Select(a => a.EnrollmentID).FirstOrDefaultAsync();


                            //CheckDuplicate query = await _context.checkDuplicates.FromSqlRaw("exec EnrollmentCheckDuplicate" +
                            //    " @Province, @District, @Year, @Season, @EducationType, @Partner, @Class",
                            //    new SqlParameter("@Province", CurEnroll.Province),
                            //    new SqlParameter("@District", CurEnroll.District),
                            //    new SqlParameter("@Year", CurEnroll.Year),
                            //    new SqlParameter("@Season", CurEnroll.Season),
                            //    new SqlParameter("@EducationType", CurEnroll.EducationType),
                            //    new SqlParameter("@Partner", CurEnroll.Partner),
                            //    new SqlParameter("@Class", CurEnroll.Class)
                            //    ).FirstOrDefaultAsync(); ; //.ToList<IndicatorsTotalTarget>(); ;


                            if ( query > 0)
                                {
                                    CurEnroll.EnrollmentID = (int)query;
                                    CurEnroll.IsDuplicate = true;
                                CurEnroll.UpdatedBy = User.Identity.Name;
                                CurEnroll.UpdatedDate = DateTime.Now;
                                }
                                else
                            {
                                CurEnroll.CreatedBy = User.Identity.Name;
                                CurEnroll.CreatedDate = DateTime.Now;
                            }
                                //if(rawDataCol.Enr.Any(a=>a.District== CurEnroll.District && a.year==CurEnroll.year))
                                rawDataCol.Enrollments.Add(CurEnroll);

                                // sb.AppendLine("</tr>");
                            }
                            break;

                        case 2: // Sheet 2 Retention

                            for (int i = (sheet2.FirstRowNum + 1); i <= sheet2.LastRowNum; i++)
                            {
                                IRow row = sheet2.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                                /// compate each record with database and mark Dup if exist. 
                                Retention CurReten = new Retention()
                                {
                                    SerialNo = (short)row.GetCell(0).NumericCellValue,
                                    Province = row.GetCell(1).ToString(),
                                    Partner = row.GetCell(2).ToString(),
                                    Year = (int)row.GetCell(3).NumericCellValue,
                                    DropoutBoys = (int)row.GetCell(4).NumericCellValue,
                                    DropoutGirls = (int)row.GetCell(5).NumericCellValue,
                                    IsDuplicate = false,
                                };
                            //var query = await _context.checkDuplicates.FromSqlRaw("exec RetentionCheckDuuplicate" +
                            //    " @Province, @Partner,@Year,  @DropoutBoys,@DropoutGirls",
                            //    new SqlParameter("@Province", CurReten.Province),
                            //    new SqlParameter("@Partner", CurReten.Partner),
                            //    new SqlParameter("@Year", CurReten.Year),

                            //    new SqlParameter("@DropoutBoys", CurReten.DropoutBoys),
                            //    new SqlParameter("@DropoutGirls", CurReten.DropoutGirls)
                            //    ).FirstOrDefaultAsync() ; //.ToList<IndicatorsTotalTarget>(); ;

                            Retention query = await _context.Retention.Where(a => a.Province == CurReten.Province
                                                                   && a.Partner == CurReten.Partner
                                                                   && a.Year == CurReten.Year
                                                                   && a.DropoutBoys == CurReten.DropoutBoys
                                                                   && a.DropoutGirls == CurReten.DropoutGirls
                                ).FirstOrDefaultAsync(); ; //.ToList<IndicatorsTotalTarget>(); ;

                            if (query!=null && query.RetentionID>0)
                                {
                                    CurReten.RetentionID = (int)query.RetentionID;
                                    CurReten.IsDuplicate = true;

                                CurReten.CreatedBy = _context.Retention.Where(a => a.RetentionID==CurReten.RetentionID) .Select(a=>a.CreatedBy).ToString();
                                CurReten.CreatedDate = await _context.Retention.Where(a => a.RetentionID == CurReten.RetentionID).Select(a => a.CreatedDate).FirstOrDefaultAsync(); ;

                                CurReten.UpdatedBy = User.Identity.Name;
                                CurReten.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                CurReten.CreatedBy = User.Identity.Name;
                                CurReten.CreatedDate = DateTime.Now;
                            }
                            rawDataCol.Retentions.Add(CurReten);

                                // sb.AppendLine("</tr>");
                            }
                            break;
                        case 3:
                        // Sheet 3 Supplies
                        for (int i = (sheet3.FirstRowNum + 1); i <= sheet3.LastRowNum; i++)
                        {
                            IRow row = sheet3.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            /// compare each record with database and mark Dup if exist. 
                            SchoolSupply CurSupply = new SchoolSupply()
                            {
                                SerialNo = short.Parse(row.GetCell(0).ToString()),
                                SchoolName = row.GetCell(1).ToString(),
                                SchoolType = row.GetCell(2).ToString(),
                                SchoolLevel = row.GetCell(3).ToString(),
                                Gender = row.GetCell(4).ToString(),
                                Province = row.GetCell(5).ToString(),
                                District = row.GetCell(6).ToString(),
                                Partner = row.GetCell(7).ToString(),
                                Year = (short)row.GetCell(8).NumericCellValue,
                                WashroomRepair = (short) row.GetCell(9).NumericCellValue,
                                ConstructionWall = (short)row.GetCell(10).NumericCellValue,
                                ConstructionWashroom = (short)row.GetCell(11).NumericCellValue,
                                InstallationHandpump = (short)row.GetCell(12).NumericCellValue,
                                RefurbishmentSchool = (short)row.GetCell(13).NumericCellValue,
                                InstallationSolarPanel = (short)row.GetCell(14).NumericCellValue,
                                RepairSchoolGate = (short)row.GetCell(15).NumericCellValue,
                                InstallationFiltrationPlant = (short)row.GetCell(16).NumericCellValue,
                                WhiteWash = (short)row.GetCell(17).NumericCellValue,
                                ElectricRepairments = (short)row.GetCell(18).NumericCellValue,
                                Flooring = (short)row.GetCell(19).NumericCellValue,
                                PlasticMat = (short)row.GetCell(20).NumericCellValue,
                          //      WaterCooler = (short)row.GetCell(21).NumericCellValue,
                          //      BlackBoard = (short)row.GetCell(22).NumericCellValue,
                          //      TeacherChair = (short)row.GetCell(23).NumericCellValue,
                           //     AttendanceRegister = (short)row.GetCell(24).NumericCellValue,
                           //     WaterTank = (short)row.GetCell(25).NumericCellValue,
                           //     Desk = (short)row.GetCell(26).NumericCellValue,
                           //     CupboardRack = (short)row.GetCell(27).NumericCellValue,
                           //     ElectricCooler = (short)row.GetCell(28).NumericCellValue,


                                IsDuplicate = false,
                            };
                            SchoolSupply query = await _context.schoolSupply.Where(a => a.SchoolName == CurSupply.SchoolName
                                                                   && a.SchoolType == CurSupply.SchoolType
                                                                   && a.SchoolLevel == CurSupply.SchoolLevel
                                                                   && a.Gender == CurSupply.Gender
                                                                   && a.Province == CurSupply.Province
                                                                   && a.District == CurSupply.District
                                                                   && a.Partner == CurSupply.Partner
                                                                   && a.Year == CurSupply.Year

                            ).FirstOrDefaultAsync();

                            if (query != null)
                            {
                                CurSupply.SSID = query.SSID;
                                CurSupply.IsDuplicate = true;

                                CurSupply.CreatedBy = query.CreatedBy;
                                CurSupply.CreatedDate = query.CreatedDate;

                                CurSupply.UpdatedBy = User.Identity.Name;
                                CurSupply.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                CurSupply.CreatedBy = User.Identity.Name;
                                CurSupply.CreatedDate = DateTime.Now;
                            }
                            rawDataCol.SchoolSupplies.Add(CurSupply);

                            // sb.AppendLine("</tr>");
                        }

                        break;
                        case 4:
                        // Sheet 4 Training
                        for (int i = (sheet4.FirstRowNum + 1); i <= sheet4.LastRowNum; i++)
                        {
                            IRow row = sheet4.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            /// compate each record with database and mark Dup if exist. 
                            Training CurTrain = new Training()
                            {
                                SerialNo = short.Parse(row.GetCell(0).ToString()),
                                ProvinceName = row.GetCell(1).ToString(),
                                DistrictName = row.GetCell(2).ToString(),
                                TrainingType = row.GetCell(3).ToString(),
                                ThemeName = row.GetCell(4).ToString(),
                                Partner = row.GetCell(5).ToString(),
                                GroupName = row.GetCell(6).ToString(),
                                Duration = short.Parse(row.GetCell(7).ToString()),
                                StartDate = row.GetCell(8).DateCellValue,
                                //EndDate = row.GetCell(9).DateCellValue,
                                ConductedBy = row.GetCell(10).ToString(),
                               Male1 = short.Parse(row.GetCell(11).ToString()),
                                Female1 = short.Parse(row.GetCell(12).ToString()),


                                IsDuplicate = false,
                            };
                            Training query = await _context.Training.Where(a => a.ProvinceName == CurTrain.ProvinceName
                                                                   && a.DistrictName == CurTrain.DistrictName
                                                                   && a.ThemeName == CurTrain.ThemeName
                                                                   && a.Partner == CurTrain.Partner
                                                                   && a.StartDate == CurTrain.StartDate
                            ).FirstOrDefaultAsync();

                            if (query != null)
                            {
                                CurTrain.TrainingID = query.TrainingID;
                                CurTrain.IsDuplicate = true;

                                CurTrain.CreatedBy = query.CreatedBy;
                                CurTrain.CreatedDate = query.CreatedDate;

                                CurTrain.UpdatedBy = User.Identity.Name;
                                CurTrain.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                CurTrain.CreatedBy = User.Identity.Name;
                                CurTrain.CreatedDate = DateTime.Now;
                            }
                            rawDataCol.Trainings.Add(CurTrain);

                            // sb.AppendLine("</tr>");
                        }

                        break;

                    }


                    //sb.Append("</table>");
                }
                // ViewData["ImportedData"] = sb.ToString();
                rawDataCol.IsRecDup = true;


            return PartialView(rawDataCol);
        }
       [HttpPost]

        // public async Task<IActionResult> Duplicates(IFormCollection form)
        public async Task<IActionResult> DuplicatePartial(RawDataCollection rawDataCol)
        {
            int c = 1;
            //EnrollList enrollments = new EnrollList();
            try
            {
                switch(rawDataCol.id)
                {
                    case 1:
                        //EFBulk.EFBulk.Update()
                        if (rawDataCol.Enrollments.Any())
                        {
                            try
                            {
                                _context.BulkInsert(rawDataCol.Enrollments.Where(a => a.IsDuplicate == false).ToList());
                            } catch (Exception ex) { }

                            try
                            {
                                _context.BulkUpdate(rawDataCol.Enrollments.Where(a => a.IsDuplicate == true).ToList());
                            }
                            catch( Exception ex) {
                                foreach (var enroll in rawDataCol.Enrollments.Where(a=>a.IsDuplicate==true))
                                {
                                    _context.Update(enroll);
                                }
                                await _context.SaveChangesAsync();

                                //    if (enroll.IsDuplicate)
                                //    {
                                //           _context.Update(enroll);
                                //    }
                                //    else
                                //    {
                                //           _context.Add(enroll);
                                //    }
                                //    try
                                //    {
                                //await _context.SaveChangesAsync();

                                //    }
                                //    catch(Exception ex)
                                //    {
                                //       // ViewBag.Error = ex.Message;
                                //    }
                                //}
                            }
                            ViewBag.SuccessMessage = "Enrollment data updated successfully";
              }

                        break;

                    case 2:
                        if (rawDataCol.Retentions.Any())
                        {
                            try {_context.BulkInsert(rawDataCol.Retentions.Where(a => a.IsDuplicate == false).ToList());
                            }
                            catch (Exception ex) { }
                          try{  _context.BulkUpdate(rawDataCol.Retentions.Where(a => a.IsDuplicate == true).ToList());
                            }
                            catch (Exception ex) { }
                            ViewBag.SuccessMessage = "Retention data updated successfully";
                     }

                        break;

                    case 3:
                        if (rawDataCol.SchoolSupplies.Any())
                        {
                            try {                            _context.BulkInsert(rawDataCol.SchoolSupplies.Where(a => a.IsDuplicate == false).ToList());
                        } catch (Exception ex) { }

                            try
                            {
                            _context.BulkUpdate(rawDataCol.SchoolSupplies.Where(a => a.IsDuplicate == true).ToList());

                            }
                            catch (Exception ex)
                            { }
                             ViewBag.SuccessMessage = "Supplies data updated successfully";
                   }

                        break;
  
                    case 4:
                        if(rawDataCol.Trainings.Any())
                        {
                          try{  _context.BulkInsert(rawDataCol.Trainings.Where(a => a.IsDuplicate == false).ToList());
                            }
                            catch (Exception ex) { }
                        try{    _context.BulkUpdate(rawDataCol.Trainings.Where(a => a.IsDuplicate == true).ToList());
                            }
                            catch (Exception ex) { }
                            ViewBag.SuccessMessage = "Training data updated successfully";
                    }
                        break;

                }
                // 1. Enrollment
                //foreach (var enroll in rawDataCol.Enrollments)
                //{
                //    if (enroll.IsDuplicate)
                //    {
                //        //   _context.Update(enroll);
                //    }
                //    else
                //    {
                //        //   _context.Add(enroll);
                //    }
                //    await _context.SaveChangesAsync();
                //}
                // 2. Retention
                //foreach (var reten in rawDataCol.Retentions)
                //{
                //    if (reten.IsDuplicate)
                //    {
                //        _context.Update(reten);
                //    }
                //    else
                //    {
                //        _context.Add(reten);
                //    }
                //    c++;
                //    await _context.SaveChangesAsync();
                //}


            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(rawDataCol);
            }
            ViewBag.Error = "";
            //if(rawDataCol.id ==4)
            //    return RedirectToAction("Report");
            //else
                return RedirectToAction("index", new { id = rawDataCol.id+1 });
        }
   [HttpPost]
        public async Task<IActionResult> Duplicates2(RawDataCollection enrollmentsPost)
        {
            ISheet sheet;
           // StringBuilder sb = new StringBuilder();

            RawDataCollection enrollments = new RawDataCollection()
            {
                Enrollments = new List<Enrollment>()
            };
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string sFileExtension = ".xlsx";
            string fullPath = Path.Combine(newPath, "UploadedFile" + sFileExtension);
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                //file.CopyTo(stream);
                stream.Position = 0;
                if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                    sheet = hssfwb.GetSheetAt(0);
                }
                else //This will read 2007 Excel format    
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                    sheet = hssfwb.GetSheetAt(0);
                }
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                // Start creating the html which would be displayed in tabular format on the screen  
                //sb.Append("<table class='table'><tr>");
                for (int j = 0; j < cellCount; j++)
                {
                    NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                   // sb.Append("<th>" + cell.ToString() + "</th>");
                }
                //sb.Append("</tr>");
               // sb.AppendLine("<tr>");
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    /// compate each record with database and mark Dup if exist. 
                    Enrollment CurEnroll = new Enrollment()
                    {
                        SerialNo = short.Parse(row.GetCell(0).ToString()),
                        Province = row.GetCell(1).ToString(),
                        District = row.GetCell(2).ToString(),
                        Year = int.Parse(row.GetCell(3).ToString()),
                        Season = row.GetCell(4).ToString(),
                        EducationType = row.GetCell(5).ToString(),
                        Partner = row.GetCell(6).ToString(),
                        Class = short.Parse(row.GetCell(7).ToString()),
                        EnrolledBoys = int.Parse(row.GetCell(8).ToString()),
                        EnrolledGirls = int.Parse(row.GetCell(9).ToString()),
                        IsDuplicate = false,
                    };
                    Enrollment query = _context.Enrollment.Where(a => a.Province == CurEnroll.Province
                                                           && a.District == CurEnroll.District
                                                           && a.Year == CurEnroll.Year
                                                           && a.Season == CurEnroll.Season
                                                           && a.EducationType == CurEnroll.EducationType
                                                           && a.Partner == CurEnroll.Partner
                                                           && a.Class == CurEnroll.Class

                    ).FirstOrDefault();

                    if (query != null)
                    {
                        CurEnroll.EnrollmentID = CurEnroll.EnrollmentID;
                        CurEnroll.IsDuplicate = true;
                        _context.Update(CurEnroll);
                    }
                    else
                    {
                        _context.Add(CurEnroll);
                        enrollments.Enrollments.Add(CurEnroll);
                    }

                    //sb.AppendLine("</tr>");
                }
                //sb.Append("</table>");
            }

            return RedirectToAction("");
        }


        public ActionResult Report()
        {
            return View();

        }
       private string ValidateEnrollment(string value, string[] data, int i, int sheet)
        {
            string v2 = " <td style=\"background-color:coral\" > " + value + " </td> ";
            //var qry= _context.Province.Select(a => a.ProvinceName); ;
            
            //if (i == 0 ||
            //    (sheet==1 && i>7) ||
            //    (sheet==2 && i>3) )
            if(data.Length==0) // Number Validation
            {
                if(int.TryParse(value, out int n))
                    v2 = "<td>" + value + "</td>";
            }
            else if(data.Length==1)
            {
                if(DateTime.TryParse(value, out DateTime dt)) v2 = "<td>" + value + "</td>";
            }
            else
            {
                if(data.Contains(value)) v2 = "<td>" + value + "</td>";
            }
            return v2;
            }


        public ActionResult OnPostImportFromExcel()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //This will read 2007 Excel format    
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;
                    // Start creating the html which would be displayed in tabular format on the screen  
                    sb.Append("<table class='table'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            return this.Content(sb.ToString());
        }
    }
}