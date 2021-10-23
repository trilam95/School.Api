using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using School.Application.Catalog.Student;
using School.ViewModels.Catalog.Student.Request;

namespace School.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private const string UPLOAD_EXCEL = "UploadExcel";
        private readonly string _webHostEnvironment;

        public StudentController(IStudentService studentService, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;
            _webHostEnvironment = Path.Combine(webHostEnvironment.WebRootPath, UPLOAD_EXCEL);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentViewModelRequest request)
        {
            var result = await _studentService.Add(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] StudentViewModelRequest request)
        {
            var result = await _studentService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _studentService.Delete(Id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagingStudentViewModelRequest request)
        {
            var result = await _studentService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _studentService.GetById(Id);
            return Ok(result);
        }

        [HttpGet("get-student-and-list-enrollment/{Id}")]
        public async Task<IActionResult> GetStudentAndListEnrollment(Guid Id)
        {
            var result = await _studentService.GetStudentAndListEnrollment(Id);
            return Ok(result);
        }

        [HttpGet("import")]
        public ActionResult Import()
        {
            IFormFile file = Request.Form.Files[0];
            string newPath = _webHostEnvironment;
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<table class='table table-bordered'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
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

        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            string sWebRootFolder = _webHostEnvironment;
            string sFileName = @"Student.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("student");
                IRow row = excelSheet.CreateRow(0);

                var result = await _studentService.GetAll();

                row.CreateCell(0).SetCellValue("FirstName");
                row.CreateCell(1).SetCellValue("LastName");
                row.CreateCell(2).SetCellValue("EnrollmentDate");

                for (int i = 0; i < result.Count; i++)
                {
                    row = excelSheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(result[i].FirstName);
                    row.CreateCell(1).SetCellValue(result[i].LastName);
                    row.CreateCell(2).SetCellValue(result[i].EnrollmentDate.ToString());
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        [HttpGet("download")]
        public ActionResult Download()
        {
            string Files = "wwwroot/UploadExcel/Student.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "student_download.xlsx");
        }
    }
}
