﻿using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.IO.Font;
using Microsoft.AspNetCore.Http;
using iText.Layout.Borders;
using iText.Layout.Properties;
using E_Document.Models;
using Microsoft.EntityFrameworkCore;
using iTextDocument = iText.Layout.Document;
using System.Text.Json;
using ModelDocument = E_Document.Models.Document;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc.Rendering;
namespace E_Document.Controllers
{
    [Authorize]
    public class CreateController : Controller
    {
        private readonly AutoPdfContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // คอนสตรัคเตอร์ที่รับค่าจาก DI
        public CreateController(AutoPdfContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        // ✅ ฟังก์ชันส่งเอกสาร
        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents.ToListAsync();
            return View(documents);
        }

        public async Task<IActionResult> Principles()
        {
            var documents = await _context.Documents.ToListAsync();
            return View(documents); // ✅ ส่ง Model ไปยัง View
        }


        [HttpPost]
        public async Task<IActionResult> GeneratePrinciplesPdf([FromForm] DocumentDetail model, [FromForm] NameApprover approver)
        {

            var documentDetail = new DocumentDetail
            {
                DocNo = model.DocNo,
                CreateDate = model.CreateDate,
                ProjectName = model.ProjectName,
                ProjectCode = model.ProjectCode,
                StartDate = model.StartDate,
                Projectdate = model.Projectdate,
                EndDate = model.EndDate,
                Location = model.Location,
                Maintenancemoney = model.Maintenancemoney,
                Maintenancemoneytext = model.Maintenancemoneytext,
                Accumulatedmoney = model.Accumulatedmoney,
                Accumulatedmoneytext = model.Accumulatedmoneytext,
                Othermoney = model.Othermoney,
                Othermoneytext = model.Othermoneytext,
                _1universityactivitiesSt = model._1universityactivitiesSt,
                _1universityactivities = model._1universityactivities,
                _211publicspiritactivitiesSt = model._211publicspiritactivitiesSt,
                _211publicspiritactivities = model._211publicspiritactivities,
                _212moralSt = model._212moralSt,
                _212moral = model._212moral,
                _221competencybuildingactivitiesSt = model._221competencybuildingactivitiesSt,
                _221competencybuildingactivities = model._221competencybuildingactivities,
                _222itskillsSt = model._222itskillsSt,
                _222itskills = model._222itskills,
                _223developingSt = model._223developingSt,
                _223developing = model._223developing,
                _231democraticSt = model._231democraticSt,
                _231democratic = model._231democratic,
                _232relationshipsSt = model._232relationshipsSt,
                _24healthSt = model._24healthSt,
                _24health = model._24health,
                _31activitiesSt = model._31activitiesSt,
                _31activities = model._31activities,
                _32socialSt = model._32socialSt,
                _32social = model._32social,

            };

            _context.DocumentDetails.Add(documentDetail);
            await _context.SaveChangesAsync();  // บันทึกข้อมูลลงใน DocumentDetail

            using (var context = new AutoPdfContext())
            {
                var approverData = context.NameApprovers
                              .FirstOrDefault();  // ดึงข้อมูลแถวแรก
                string approve1 = approverData._1Approve;   // คอลัมน์ [1_Approve]
                string approve2 = approverData._2Approve;   // คอลัมน์ [2_Approve]
                string approve3 = approverData._3Approve;   // คอลัมน์ [3_Approve]
                string approve4 = approverData._4Approve;   // คอลัมน์ [4_Approve]
                string approve5 = approverData._5Approve;   // คอลัมน์ [5_Approve]
                string approve6 = approverData._6Approve;  // คอลัมน์ [6_Approve6]
                string approve7 = approverData._7Approve;   // คอลัมน์ [7_Approve]
                string approve8 = approverData._8Approve;// คอลัมน์ [8_Approve]
                string approve9 = approverData._9Approve;   // คอลัมน์ [9_Approve]
                string approve10 = approverData._10Approve; // คอลัมน์ [10_Approve]
                string approve11 = approverData._11Approve; // คอลัมน์ [11_Approve]



                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    //Document document = new Document(pdf);
                    iText.Layout.Document document = new iText.Layout.Document(pdf);
                    string fontPath = "wwwroot/fonts/THSarabunNew.ttf"; // เส้นทางฟอนต์
                    PdfFont thaiFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H); // ฝังฟอนต์
                    PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    
                    document.Add(new Paragraph()
                        .Add(new Text("ที่ สภ.มก.ศรช. ").SetFont(thaiFont)) // ข้อความ "ที่"
                        .Add(new Text(model.DocNo ?? "N/A").SetFont(thaiFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)) // ฟอนต์ไทยและจัดตรงกลาง
                        .Add(new Text(" / 2568 ").SetFont(thaiFont)) // ข้อความ "/2567"
                    );
                    var culture = new System.Globalization.CultureInfo("th-TH");

                    string formattedDatenow = model.CreateDate.HasValue
    ? $"{model.CreateDate.Value.ToString("dd MMMM", culture)} {model.CreateDate.Value.Year + 543}"
    : "N/A";

                    string formattedDatestart = model.StartDate.HasValue
                        ? $"{model.StartDate.Value.ToString("ddddที่ d MMMM", culture)} {model.StartDate.Value.Year + 543}"
                        : "N/A";

                    string formattedDateend = model.EndDate.HasValue
                        ? $"{model.EndDate.Value.ToString("ddddที่ d MMMM", culture)} {model.EndDate.Value.Year + 543}"
                        : "N/A";


                    document.Add(new Paragraph("วันที่ ")
                        .Add(new Text(formattedDatenow).SetFont(thaiFont))  // แสดงวันที่ที่แปลงแล้ว
                        .SetFont(thaiFont)
                        .SetFontSize(12)
                        .SetMarginLeft(250));
                    document.Add(new Paragraph()
                        .Add(new Text("เรื่อง ขออนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ  ").SetFont(thaiFont))
                        .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont))
                        .Add(new Text("\n เรียน ผู้ช่วยอธิการบดีฝ่ายกิจการนิสิต และพัฒนาอย่างยั่งยืน วิทยาเขตศรีราชา").SetFont(thaiFont).SetFontSize(12))
                        .Add(new Text("\n สิ่งที่แนบมาด้วย      รายละเอียดโครงการและกำหนดการโครงการ ").SetFont(thaiFont))  // ใช้ฟอนต์ไทยสำหรับข้อความ
                        .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont))

                    //ใช้ฟอนต์ไทยสำหรับ ProjectName
                    );



                    document.Add(new Paragraph()
        .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
        .Add(new Text("ด้วยสภาผู้แทนนิสิต องค์การนิสิต กำหนดจัดโครงการ ").SetFont(thaiFont))
        .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont))
        .Add(new Text("  รหัสโครงการ  ").SetFont(thaiFont))
        .Add(new Text(model.ProjectCode ?? "N/A").SetFont(thaiFont))
        .Add(new Text("  ในระหว่าง ").SetFont(thaiFont))
        .Add(new Text(formattedDatestart).SetFont(thaiFont))
        .Add(new Text("  ถึง ").SetFont(thaiFont))
        .Add(new Text(formattedDateend).SetFont(thaiFont))
        .Add(new Text(" ตามแผนการใช้เงินบำรุงกิจกรรมนิสิต ปีการศึกษา 2567 โดยมีวงเงินค่าใช้จ่ายจำนวน  ").SetFont(thaiFont))
    .Add(new Text($"{(model.Maintenancemoney.HasValue && model.Maintenancemoney > 0 ? model.Maintenancemoney.Value.ToString("N2") : "N/A")} บาท").SetFont(thaiFont))
    .Add(new Text(" ( ").SetFont(thaiFont))
        .Add(new Text(model.Maintenancemoneytext).SetFont(thaiFont))
        .Add(new Text(" ) ").SetFont(thaiFont))
        .Add(new Text(" ตามรายละเอียดของโครงการดังแนบและขอแต่งตั้งคณะกรรมการตรวจรับ จำนวน 3 คน ได้แก่ ").SetFont(thaiFont))
        .SetFontSize(12)
        .SetMultipliedLeading(1.0f)
        .SetTextAlignment(TextAlignment.LEFT)
    );

                    Paragraph committeeParagraph = new Paragraph()
        .SetTextAlignment(TextAlignment.LEFT)
        .SetMultipliedLeading(1.5f) // ระยะห่างระหว่างบรรทัด
        .Add(new Tab()) // ขยับเลขไปที่ตำแหน่งแรก
        .Add(new Text("1. ").SetFont(thaiFont))
        .Add(new Tab()) // ขยับชื่อไปที่ตำแหน่งที่ 2
        .Add(new Text($"{approve8 ?? "N/A"}").SetFont(thaiFont))
        .Add(new Tab()) // ขยับตำแหน่งไปที่ตำแหน่งที่ 3
        .Add(new Text("ประธานกรรมการ").SetFont(thaiFont))
        .Add(new Text("\n"))
        .Add(new Tab())
        .Add(new Text("2. ").SetFont(thaiFont))
        .Add(new Tab())
        .Add(new Text($"{approve9 ?? "N/A"}").SetFont(thaiFont))
        .Add(new Tab())
        .Add(new Text("กรรมการ").SetFont(thaiFont))
        .Add(new Text("\n"))
        .Add(new Tab())
        .Add(new Text("3. ").SetFont(thaiFont))
        .Add(new Tab())
        .Add(new Text($"{approve10 ?? "N/A"}").SetFont(thaiFont))
        .Add(new Tab())
        .Add(new Text("กรรมการ").SetFont(thaiFont));

                    ////กำหนดตำแหน่งของแต่ละ Tab
                    committeeParagraph.AddTabStops(
                        new TabStop(140, TabAlignment.LEFT),  // เลข 1, 2, 3
                        new TabStop(150, TabAlignment.LEFT), // ชื่อ
                        new TabStop(160, TabAlignment.LEFT)  // ตำแหน่ง
                    );

                    document.Add(committeeParagraph);







                    document.Add(new Paragraph()
        .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
        .Add(new Text("จึงเรียนมาเพื่อโปรดพิจารณา  ").SetFont(thaiFont)));
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง

                    Table signTable = new Table(2); // ตาราง 2 คอลัมน์
                    signTable.SetWidth(UnitValue.CreatePercentValue(100));

                    signTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("ลงชื่อ .................................\n")
                            .SetFont(thaiFont)

                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph($"{approve8 ?? "N/A"}")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("อาจารที่ปรึกษา")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );
                    signTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("ลงชื่อ .................................\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph($"{approve9 ?? "N/A"}")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("ประธานสภาผู้แทนนิสิต องค์การนิสิต")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );



                    ////ช่องที่ 1
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("1)ได้ตรวจสอบรายละเอียดของโครงการแล้ว\n" +
                        "☐ เห็นสมควรอนุมัติ\n" +
                        "☐ อื่น ๆ (ระบุ) ........................................\n" +
                        "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))

                        .Add(new Paragraph($"{approve7 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("นักวิชาการศึกษา ชำนาญการ")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    //ช่องที่ 2

                    //         ช่องที่ 2(2 ลายเซ็น ซ้าย - ขวา และอยู่ตรงกลาง)
                    Table innerTable = new Table(2); // ตารางย่อย 2 คอลัมน์
                    innerTable.SetWidth(UnitValue.CreatePercentValue(100));

                    //ลายเซ็นทางซ้าย(จัดตรงกลาง)
                    Cell leftCell = new Cell()
                        .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                        .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                        .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"\n{approve6 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("หัวหน้างานบริหารกิจการนิสิตและการกีฬา").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                    //ลายเซ็นทางขวา(จัดตรงกลาง)
                    Cell rightCell = new Cell()
                        .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                        .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                        .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"\n{approve5 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("ผู้อำนวยการกองบริหารการศึกษาและพัฒนานิสิต").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                    //เพิ่มเซลล์ซ้าย - ขวาเข้าตารางย่อย
                    innerTable.AddCell(leftCell);
                    innerTable.AddCell(rightCell);

                    //เพิ่มตารางย่อยลงในตารางหลัก
                    signTable.AddCell(new Cell()
                        .SetTextAlignment(TextAlignment.LEFT) // จัดข้อความของเซลล์หลักให้อยู่ตรงกลาง
                        .Add(new Paragraph("2) ตรวจสอบให้เป็นไปตามแผนการจัดกิจกรรม\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(innerTable) // ใส่ตารางย่อยที่มี 2 คอลัมน์ (ซ้าย-ขวา)
                    );




                    //ช่องที่ 3
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("3)ตรวจสอบแผนการใช้งบประมาณ\n" +
                        "☐ 1) เงินบำรุงกิจกรรมนิสิต      \n" +
                        "☐ 2) เงินสะสมองค์กร          \n" +
                        "☐ 3) แหล่งอื่นๆ รายการ...............................\n" +
                        "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))

                        .Add(new Paragraph($"{approve4 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("นักวิชาการเงินและบัญชี")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    //ช่องที่ 4
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("4)เรียนผู้ช่วยอธิการบดีฝ่ายกิจการนิสิต ฯ\n" +
                        "เพื่อโปรดพิจารณา\n\n\n\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))

                        .Add(new Paragraph($"{approve3 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("ผู้ช่วยอธิการบดีฝ่ายเทคโนโลยีสารสนเทศ กายภาพ และสิ่งแวดล้อม วิทยาเขตศรีราชา\r\nรักษาการแทนผู้อำนวยการสำนักงานวิทยาเขตศรีราชา\r\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );
                    //ช่องที่ 5
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("5)เรียนรองอธิการบดี ฯ\n" +
                        "เพื่อโปรดพิจารณา\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"{approve2 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("ผู้ช่วยอธิการบดีฝ่ายกิจการนิสิต และพัฒนาอย่างยั่งยืน\r\nวิทยาเขตศรีราชา\r\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    //ช่องที่ 6
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("อนุมัติ\n\n\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f).SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"\n {approve1 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("รองอธิการบดีวิทยาเขตศรีราชา")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );


                    //เพิ่มตารางลงในเอกสาร
                    document.Add(signTable);


                    document.Close();
                    byte[] fileBytes = stream.ToArray();
                    string safeFileName = string.Concat(model.ProjectName.Split(Path.GetInvalidFileNameChars()));
                    return File(stream.ToArray(), "application/pdf", $"ขออนุมัติ{safeFileName}.pdf");

                }
            }
        }

        public IActionResult ShowDocumentDetails()
        {
            // ดึงข้อมูลทั้งหมดจาก DocumentDetails
            var documentDetails = _context.DocumentDetails.ToList();

            // ส่งข้อมูลไปยัง View ผ่าน ViewBag
            ViewBag.DocumentDetails = documentDetails;

            return View();
        }
        
        
        public IActionResult Disburse()
        {
            var projectDetails = _context.DocumentDetails
                .Select(d => new
                {
                    d.DocNo,
                    d.CreateDate,
                    d.ProjectCode,  // ใช้ชื่อเดิม
                    d.ProjectName,
                    ProjectCodeValue = d.ProjectCode,  // เปลี่ยนชื่อเป็น ProjectCodeValue
                    d.StartDate,
                    d.Projectdate,
                    d.EndDate,
                    d.Location,
                    d.Maintenancemoney,
                    d.Maintenancemoneytext,
                    d.Accumulatedmoney,
                    d.Accumulatedmoneytext,
                    d.Othermoney,
                    d.Othermoneytext,
                    d._1universityactivitiesSt,
                    d._1universityactivities,
                    d._211publicspiritactivitiesSt,
                    d._211publicspiritactivities,
                    d._212moralSt,
                    d._212moral,
                    d._221competencybuildingactivitiesSt,
                    d._221competencybuildingactivities,
                    d._222itskillsSt,
                    d._222itskills,
                    d._223developingSt,
                    d._223developing,
                    d._231democraticSt,
                    d._231democratic,
                    d._232relationshipsSt,
                    d._232relationships,
                    d._24healthSt,
                    d._24health,
                    d._31activitiesSt,
                    d._31activities,
                    d._32socialSt,
                    d._32social

                }).ToList();

            ViewBag.ProjectCodeList = new SelectList(projectDetails, "ProjectCode", "ProjectName");
            ViewBag.ProjectDetailsJson = JsonSerializer.Serialize(projectDetails);

            return View();
        }










        [HttpPost]
        public IActionResult GenerateDisbursePdf([FromForm] DocumentDetail documentDetail, [FromForm] NameApprover approver)
        {



            using (var context = new AutoPdfContext())
            {
                var approverData = context.NameApprovers
                              .FirstOrDefault();  // ดึงข้อมูลแถวแรก
                string approve1 = approverData._1Approve;   // คอลัมน์ [1_Approve]
                string approve2 = approverData._2Approve;   // คอลัมน์ [2_Approve]
                string approve3 = approverData._3Approve;   // คอลัมน์ [3_Approve]
                string approve4 = approverData._4Approve;   // คอลัมน์ [4_Approve]
                string approve5 = approverData._5Approve;   // คอลัมน์ [5_Approve]
                string approve6 = approverData._6Approve;  // คอลัมน์ [6_Approve6]
                string approve7 = approverData._7Approve;   // คอลัมน์ [7_Approve]
                string approve8 = approverData._8Approve;// คอลัมน์ [8_Approve]
                string approve9 = approverData._9Approve;   // คอลัมน์ [9_Approve]
                string approve10 = approverData._10Approve; // คอลัมน์ [10_Approve]
                string approve11 = approverData._11Approve; // คอลัมน์ [11_Approve]

            

            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                //Document document = new Document(pdf);
                iText.Layout.Document document = new iText.Layout.Document(pdf);
                string fontPath = "wwwroot/fonts/THSarabunNew.ttf"; // เส้นทางฟอนต์
                PdfFont thaiFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph()
                    .Add(new Text("ที่ สภ.มก.ศรช. ").SetFont(thaiFont)) // ข้อความ "ที่"
                    .Add(new Text(documentDetail.DocNoDis ?? "N/A").SetFont(thaiFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)) // ฟอนต์ไทยและจัดตรงกลาง
                    .Add(new Text(" / 2568 ").SetFont(thaiFont)) // ข้อความ "/2567"
                );
                var culture = new System.Globalization.CultureInfo("th-TH");

                    string formattedDatenow = documentDetail.CreateDateDis.HasValue
        ? $"{documentDetail.CreateDateDis.Value.ToString("dd MMMM", culture)} {documentDetail.CreateDateDis.Value.Year + 543}"
        : "N/A";

                    string formattedDateCreate = documentDetail.CreateDate.HasValue
                        ? $"{documentDetail.CreateDate.Value.ToString("ddddที่ d MMMM", culture)} {documentDetail.CreateDate.Value.Year + 543}"
                        : "N/A";

                    string formattedDatestart = documentDetail.StartDate.HasValue
                        ? $"{documentDetail.StartDate.Value.ToString("ddddที่ d MMMM", culture)} {documentDetail.StartDate.Value.Year + 543}"
                        : "N/A";

                    string formattedDateend = documentDetail.EndDate.HasValue
                        ? $"{documentDetail.EndDate.Value.ToString("ddddที่ d MMMM", culture)} {documentDetail.EndDate.Value.Year + 543}"
                        : "N/A";


                    //                string formattedDatenow = model.CreateDate.HasValue
                    //? $"{model.CreateDate.Value.ToString("dd MMMM", culture)} {model.CreateDate.Value.Year + 543}"
                    //: "N/A";

                    //                string formattedDatestart = model.StartDate.HasValue
                    //                    ? $"{model.StartDate.Value.ToString("ddddที่ d MMMM", culture)} {model.StartDate.Value.Year + 543}"
                    //                    : "N/A";

                    //                string formattedDateend = model.EndDate.HasValue
                    //                    ? $"{model.EndDate.Value.ToString("ddddที่ d MMMM", culture)} {model.EndDate.Value.Year + 543}"
                    //                    : "N/A";

                    document.Add(new Paragraph("วันที่ ")
                    .Add(new Text(formattedDatenow).SetFont(thaiFont))  // แสดงวันที่ที่แปลงแล้ว
                    .SetFont(thaiFont)
                    .SetFontSize(12)
                    .SetMarginLeft(250)); // เพิ่มระยะห่างจากซ้ายเพื่อเลื่อนข้อความไปทางขวา
                document.Add(new Paragraph()
                    .Add(new Text("เรื่อง ขออนุมัติเบิกค่าใช้จ่ายโครงการ   ").SetFont(thaiFont))
                    .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))
                    .Add(new Text("\n เรียน รองอธิการบดีวิทยาเขตศรีราชา ").SetFont(thaiFont).SetFontSize(12))
                    .Add(new Text("\n สิ่งที่แนบมาด้วย      สำเนาขออนุมัติหลักการและวงเงินค่าใช่จ่ายโครงการ ").SetFont(thaiFont))  // ใช้ฟอนต์ไทยสำหรับข้อความ
                    .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))
                // ใช้ฟอนต์ไทยสำหรับ ProjectName
                );
                document.Add(new Paragraph()
    .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
    .Add(new Text("ตามที่สภาผู้แทนนิสิต องค์กรนิสิต ได้รับอนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ ").SetFont(thaiFont))
    .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))
    .Add(new Text("  ตามหนังสือเลขที่ สภ.มก.ศรช. ").SetFont(thaiFont))


    .Add(new Text(documentDetail.DocNo ?? "N/A").SetFont(thaiFont))


    .Add(new Text(" / 2568 ลว.  ").SetFont(thaiFont))
    .Add(new Text(formattedDateCreate).SetFont(thaiFont)) // ✅ แปลงเป็น string พร้อม formatting
    .Add(new Text(" เรื่อง ขออนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ  ").SetFont(thaiFont)
    ).Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))

    .SetFontSize(12)
    .SetMultipliedLeading(1.0f)
    .SetTextAlignment(TextAlignment.LEFT)
);

                document.Add(new Paragraph()
    .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
    .Add(new Text("บัดนี้การดำเนินโครงการดังกล่าวเสร็จสิ้นเรียบร้อยแล้ว จึงใคร่ขอรายงานผลการดำเนินงานและขออนุมัติเบิกค่าใช้จ่ายเงินบำรุงกิจกรรมนิสิต ปีการศึกษา 2568 เป็นจำนวนเงินทั้งสิ้น ").SetFont(thaiFont))
    .Add(new Text(" ตามแผนการใช้เงินบำรุงกิจกรรมนิสิต ปีการศึกษา 2568 โดยมีวงเงินค่าใช้จ่ายจำนวน  ").SetFont(thaiFont)).Add(new Text($"{(documentDetail.Maintenancemoney > 0 ? ((decimal)documentDetail.Maintenancemoney).ToString("N2") : "N/A")} บาท").SetFont(thaiFont))
    .Add(new Text(" ( ").SetFont(thaiFont))
        .Add(new Text(documentDetail.Maintenancemoneytext).SetFont(thaiFont))
        .Add(new Text(" ) ").SetFont(thaiFont))
        .Add(new Text("พร้อมได้แนบรายงานผลการจัดโครงการและหลักฐานประกอบการเบิกจ่ายมาด้วย ").SetFont(thaiFont))
    .SetFontSize(12)
    .SetMultipliedLeading(1.0f)
    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง



                document.Add(new Paragraph()
    .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
    .Add(new Text("จึงเรียนมาเพื่อโปรดพิจารณา  ").SetFont(thaiFont)));
                document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
                document.Add(new Paragraph("\n")); //
                                                   // 
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง

                    Table signTable = new Table(2); // ตาราง 2 คอลัมน์
                signTable.SetWidth(UnitValue.CreatePercentValue(100));

                    //signTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph("ลงชื่อ .................................\n")
                    //        .SetFont(thaiFont)

                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //    .Add(new Paragraph($"{approve8 ?? "N/A"}")
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //    .Add(new Paragraph("อาจารที่ปรึกษา")
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //    .Add(new Paragraph(".../..../....")
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //);
                    //signTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph("ลงชื่อ .................................\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //    .Add(new Paragraph($"{approve9 ?? "N/A"}")
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //    .Add(new Paragraph("ประธานสภาผู้แทนนิสิต องค์การนิสิต")
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //    .Add(new Paragraph(".../..../....")
                    //        .SetFont(thaiFont)
                    //        .SetMultipliedLeading(1.0f)
                    //        .SetTextAlignment(TextAlignment.CENTER))
                    //);



                    // ช่องที่ 1
                    signTable.AddCell(new Cell()
                            .Add(new Paragraph("1)ได้ตรวจสอบรายละเอียดของโครงการแล้ว\n" +
                            "☐ เห็นสมควรอนุมัติ\n" +
                            "☐ อื่น ๆ (ระบุ) ........................................\n" +
                            "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                                .SetFont(thaiFont).SetMultipliedLeading(1.0f))

                            .Add(new Paragraph($"{approve7 ?? "N/A"}")
                                .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                                .SetTextAlignment(TextAlignment.CENTER))
                            .Add(new Paragraph("นักวิชาการศึกษา ชำนาญการ")
                                .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                                .SetTextAlignment(TextAlignment.CENTER))
                            .Add(new Paragraph(".../..../....")
                                .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                                .SetTextAlignment(TextAlignment.CENTER))
                        );

                    //ช่องที่ 2

                    //         ช่องที่ 2(2 ลายเซ็น ซ้าย - ขวา และอยู่ตรงกลาง)
                    Table innerTable = new Table(2); // ตารางย่อย 2 คอลัมน์
                    innerTable.SetWidth(UnitValue.CreatePercentValue(100));

                    //ลายเซ็นทางซ้าย(จัดตรงกลาง)
                    Cell leftCell = new Cell()
                        .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                        .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                        .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"\n{approve6 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("หัวหน้างานบริหารกิจการนิสิตและการกีฬา").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                    //ลายเซ็นทางขวา(จัดตรงกลาง)
                    Cell rightCell = new Cell()
                        .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                        .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                        .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"\n{approve5 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("ผู้อำนวยการกองบริหารการศึกษาและพัฒนานิสิต").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                    //เพิ่มเซลล์ซ้าย - ขวาเข้าตารางย่อย
                    innerTable.AddCell(leftCell);
                    innerTable.AddCell(rightCell);

                    //เพิ่มตารางย่อยลงในตารางหลัก
                    signTable.AddCell(new Cell()
                        .SetTextAlignment(TextAlignment.LEFT) // จัดข้อความของเซลล์หลักให้อยู่ตรงกลาง
                        .Add(new Paragraph("2) ตรวจสอบให้เป็นไปตามแผนการจัดกิจกรรม\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(innerTable) // ใส่ตารางย่อยที่มี 2 คอลัมน์ (ซ้าย-ขวา)
                    );




                    //ช่องที่ 3
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("3)ตรวจสอบแผนการใช้งบประมาณ\n" +
                        "☐ 1) เงินบำรุงกิจกรรมนิสิต      \n" +
                        "☐ 2) เงินสะสมองค์กร          \n" +
                        "☐ 3) แหล่งอื่นๆ รายการ...............................\n" +
                        "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))

                        .Add(new Paragraph($"{approve4 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("นักวิชาการเงินและบัญชี")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    //ช่องที่ 4
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("4)เรียนผู้ช่วยอธิการบดีฝ่ายกิจการนิสิต ฯ\n" +
                        "เพื่อโปรดพิจารณา\n\n\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))

                        .Add(new Paragraph($"{approve3 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("ผู้ช่วยอธิการบดีฝ่ายเทคโนโลยีสารสนเทศ กายภาพ และสิ่งแวดล้อม วิทยาเขตศรีราชา\r\nรักษาการแทนผู้อำนวยการสำนักงานวิทยาเขตศรีราชา\r\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );
                    //ช่องที่ 5
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("5)เรียนรองอธิการบดี ฯ\n" +
                        "เพื่อโปรดพิจารณา\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"{approve2 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("ผู้ช่วยอธิการบดีฝ่ายกิจการนิสิต และพัฒนาอย่างยั่งยืน\r\nวิทยาเขตศรีราชา\r\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    //ช่องที่ 6
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("อนุมัติ\n\n\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f).SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("\n")) // เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"\n {approve1 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("รองอธิการบดีวิทยาเขตศรีราชา")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    document.Add(signTable);







                

                document.Close();
                    byte[] fileBytes = stream.ToArray();
                    string safeFileName = string.Concat(documentDetail.ProjectName.Split(Path.GetInvalidFileNameChars()));
                    return File(stream.ToArray(), "application/pdf", $"ขออนุมัติเบิกค่าใช้จ่ายโครงการ{safeFileName}.pdf");
                }
        }
        }


        public async Task<IActionResult> Activity()
        {
            var projectDetails = _context.DocumentDetails
                .Select(d => new
                {
                    d.DocNo,
                    d.CreateDate,
                    d.ProjectCode,  // ใช้ชื่อเดิม
                    d.ProjectName,
                    ProjectCodeValue = d.ProjectCode,  // เปลี่ยนชื่อเป็น ProjectCodeValue
                    d.StartDate,
                    d.Projectdate,
                    d.EndDate,
                    d.Location,
                    d.Maintenancemoney,
                    d.Maintenancemoneytext,
                    d.Accumulatedmoney,
                    d.Accumulatedmoneytext,
                    d.Othermoney,
                    d.Othermoneytext,
                    d._1universityactivitiesSt,
                    d._1universityactivities,
                    d._211publicspiritactivitiesSt,
                    d._211publicspiritactivities,
                    d._212moralSt,
                    d._212moral,
                    d._221competencybuildingactivitiesSt,
                    d._221competencybuildingactivities,
                    d._222itskillsSt,
                    d._222itskills,
                    d._223developingSt,
                    d._223developing,
                    d._231democraticSt,
                    d._231democratic,
                    d._232relationshipsSt,
                    d._232relationships,
                    d._24healthSt,
                    d._24health,
                    d._31activitiesSt,
                    d._31activities,
                    d._32socialSt,
                    d._32social

                }).ToList();

            ViewBag.ProjectCodeList = new SelectList(projectDetails, "ProjectCode", "ProjectName");
            ViewBag.ProjectDetailsJson = JsonSerializer.Serialize(projectDetails);

            return View();
        }

        [HttpPost]
        public IActionResult GenerateActivityPdf([FromForm] DocumentDetail documentDetail, [FromForm] NameApprover approver)
        {



            using (var context = new AutoPdfContext())
            {
                var approverData = context.NameApprovers
                              .FirstOrDefault();  // ดึงข้อมูลแถวแรก
                string approve1 = approverData._1Approve;   // คอลัมน์ [1_Approve]
                string approve2 = approverData._2Approve;   // คอลัมน์ [2_Approve]
                string approve3 = approverData._3Approve;   // คอลัมน์ [3_Approve]
                string approve4 = approverData._4Approve;   // คอลัมน์ [4_Approve]
                string approve5 = approverData._5Approve;   // คอลัมน์ [5_Approve]
                string approve6 = approverData._6Approve;  // คอลัมน์ [6_Approve6]
                string approve7 = approverData._7Approve;   // คอลัมน์ [7_Approve]
                string approve8 = approverData._8Approve;// คอลัมน์ [8_Approve]
                string approve9 = approverData._9Approve;   // คอลัมน์ [9_Approve]
                string approve10 = approverData._10Approve; // คอลัมน์ [10_Approve]
                string approve11 = approverData._11Approve; // คอลัมน์ [11_Approve]



                using (MemoryStream stream = new MemoryStream())
                {
                    Console.WriteLine(documentDetail._1universityactivitiesSt);

                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    //Document document = new Document(pdf);
                    iText.Layout.Document document = new iText.Layout.Document(pdf);
                    string fontPath = "wwwroot/fonts/THSarabunNew.ttf"; // เส้นทางฟอนต์
                    PdfFont thaiFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                    PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph()
                        .Add(new Text("ที่ สภ.มก.ศรช. ").SetFont(thaiFont)) // ข้อความ "ที่"
                        .Add(new Text(documentDetail.DocNoDis ?? "N/A").SetFont(thaiFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)) // ฟอนต์ไทยและจัดตรงกลาง
                        .Add(new Text(" / 2568 ").SetFont(thaiFont)) // ข้อความ "/2567"
                    );
                    var culture = new System.Globalization.CultureInfo("th-TH");

                    string formattedDatenow = documentDetail.CreateDateDis.HasValue
                        ? documentDetail.CreateDateDis.Value.ToString("dd MMMM yyyy", culture)
                        : "N/A";

                    string formattedDateCreate = documentDetail.CreateDate.HasValue
                        ? documentDetail.CreateDate.Value.ToString("dd MMMM yyyy", culture)
                        : "N/A";

                    string formattedDatestart = documentDetail.StartDate.HasValue
                        ? documentDetail.StartDate.Value.ToString("ddddที่ d MMMM yyyy", culture)
                        : "N/A";

                    string formattedDateend = documentDetail.EndDate.HasValue
                        ? documentDetail.EndDate.Value.ToString("ddddที่ d MMMM yyyy", culture)
                        : "N/A";

                    


                    document.Add(new Paragraph("วันที่ ")
                        .Add(new Text(formattedDatenow).SetFont(thaiFont))  // แสดงวันที่ที่แปลงแล้ว
                        .SetFont(thaiFont)
                        .SetFontSize(12)
                        .SetMarginLeft(250)); // เพิ่มระยะห่างจากซ้ายเพื่อเลื่อนข้อความไปทางขวา
                    document.Add(new Paragraph()
                        .Add(new Text("เรื่อง ขออนุมัติชั่วโมงกิจกรรมโครงการ   ").SetFont(thaiFont))
                        .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))
                        .Add(new Text("\n เรียน ประธานคณะกรรมการองค์การนิสิต มหาวิทยาลัยเกษตรศาสตร์ วิทยาเขตศรีราชา ").SetFont(thaiFont).SetFontSize(12))
                        .Add(new Text("\n สิ่งที่แนบมาด้วย      1. สำเนาขออนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ  ").SetFont(thaiFont))  // ใช้ฟอนต์ไทยสำหรับข้อความ
                        .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))
                    // ใช้ฟอนต์ไทยสำหรับ ProjectName

                    );
                    document.Add(new Paragraph()
    .SetFirstLineIndent(65).Add(new Text("2. ใบบันทึกชั่วโมงกิจกรรมโครงการ  ").SetFont(thaiFont))  // ใช้ฟอนต์ไทยสำหรับข้อความ
                    .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont)));
                    document.Add(new Paragraph()
        .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
        .Add(new Text("ตามที่สภาผู้แทนนิสิต องค์กรนิสิต ได้รับอนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ ").SetFont(thaiFont))
        .Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))
        .Add(new Text("  ตามหนังสือเลขที่  ").SetFont(thaiFont))


        .Add(new Text(documentDetail.DocNo ?? "N/A").SetFont(thaiFont))


        .Add(new Text(" / 2568 ลว.  ").SetFont(thaiFont))
        .Add(new Text(formattedDateend).SetFont(thaiFont)) // ✅ แปลงเป็น string พร้อม formatting
        .Add(new Text(" เรื่อง ขออนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ  ").SetFont(thaiFont)
        ).Add(new Text(documentDetail.ProjectName ?? "N/A").SetFont(thaiFont))

        .SetFontSize(12)
        .SetMultipliedLeading(1.0f)
        .SetTextAlignment(TextAlignment.LEFT)
    );
                    int totalHoursStA =
    (documentDetail._1universityactivitiesSt ?? 0) +
    (documentDetail._211publicspiritactivitiesSt ?? 0) +
    (documentDetail._212moralSt ?? 0) +
    (documentDetail._221competencybuildingactivitiesSt ?? 0) +
    (documentDetail._222itskillsSt ?? 0) +
    (documentDetail._223developingSt ?? 0) +
    (documentDetail._231democraticSt ?? 0) +
    (documentDetail._232relationshipsSt ?? 0) +
    (documentDetail._24healthSt ?? 0) +
    (documentDetail._31activitiesSt ?? 0) +
    (documentDetail._32socialSt ?? 0);
                    int totalHoursA =
    (documentDetail._1universityactivities ?? 0) +
    (documentDetail._211publicspiritactivities ?? 0) +
    (documentDetail._212moral ?? 0) +
    (documentDetail._221competencybuildingactivities ?? 0) +
    (documentDetail._222itskills ?? 0) +
    (documentDetail._223developing ?? 0) +
    (documentDetail._231democratic ?? 0) +
    (documentDetail._232relationships ?? 0) +
    (documentDetail._24health ?? 0) +
    (documentDetail._31activities ?? 0) +
    (documentDetail._32social ?? 0);
                    // สร้างตาราง 4 คอลัมน์
                    Table activityTableA = new Table(4);
                    activityTableA.SetWidth(UnitValue.CreatePercentValue(100));


                    activityTableA.AddHeaderCell(CreateHeaderCell("ลำดับ", thaiFont));
                    activityTableA.AddHeaderCell(CreateHeaderCell("ประเภทกิจกรรม", thaiFont));
                    activityTableA.AddHeaderCell(CreateHeaderCell("คณะกรรมการ", thaiFont));
                    activityTableA .AddHeaderCell(CreateHeaderCell("ผู้เข้าร่วมกิจกรรม", thaiFont));


                    
                   


                    AddActivityRow(activityTableA, "1.", "กิจกรรมมหาวิทยาลัย", thaiFont,
documentDetail._1universityactivitiesSt.HasValue ? documentDetail._1universityactivitiesSt.Value : 0,
documentDetail._1universityactivities.HasValue ? documentDetail._1universityactivities.Value : 0);

                    AddActivityRow(activityTableA, "2.", "กิจกรรมเสริมสร้างสมรรถนะ", thaiFont,
    null, // คณะกรรมการ = 0 เพื่อแสดงเป็นช่องว่าง
    null); // ผู้เข้าร่วม = 0 เพื่อแสดงเป็นช่องว่าง

                    AddActivityRow(activityTableA, "2.1", "ด้านพัฒนาคุณธรรม", thaiFont,
    null, // คณะกรรมการ = 0 เพื่อแสดงเป็นช่องว่าง
    null); // ผู้เข้าร่วม = 0 เพื่อแสดงเป็นช่องว่าง


                    AddActivityRow(activityTableA, "2.1.1", "กิจกรรมจิตสาธารณะ ", thaiFont,
documentDetail._211publicspiritactivitiesSt.HasValue ? documentDetail._211publicspiritactivitiesSt.Value : 0,
documentDetail._211publicspiritactivities.HasValue ? documentDetail._211publicspiritactivities.Value : 0);

                    AddActivityRow(activityTableA, "2.1.2", "กิจกรรมอื่น ๆ ", thaiFont,
documentDetail._212moralSt.HasValue ? documentDetail._212moralSt.Value : 0,
documentDetail._212moral.HasValue ? documentDetail._212moral.Value : 0);

                    AddActivityRow(activityTableA, "2.2", " ด้านพัฒนาทักษะเสริมสร้างทางการคิดและการเรียนรู้", thaiFont,
    null, // คณะกรรมการ = 0 เพื่อแสดงเป็นช่องว่าง
    null); // ผู้เข้าร่วม = 0 เพื่อแสดงเป็นช่องว่าง
                    AddActivityRow(activityTableA, "2.2.1", "ทักษะภาษาต่างประเทศ", thaiFont,
documentDetail._221competencybuildingactivitiesSt.HasValue ? documentDetail._221competencybuildingactivitiesSt.Value : 0,
documentDetail._221competencybuildingactivities.HasValue ? documentDetail._221competencybuildingactivities.Value : 0);
                    AddActivityRow(activityTableA, "2.1.2", "ทักษะไอที ", thaiFont,
documentDetail._222itskillsSt.HasValue ? documentDetail._222itskillsSt.Value : 0,
documentDetail._222itskills.HasValue ? documentDetail._222itskills.Value : 0);
                    AddActivityRow(activityTableA, "2.1.2", "กิจกรรมอื่น ๆ ", thaiFont,
documentDetail._223developingSt.HasValue ? documentDetail._223developingSt.Value : 0,
documentDetail._223developing.HasValue ? documentDetail._223developing.Value : 0);

                    AddActivityRow(activityTableA, "2.3", "ด้านพัฒนาทักษะเสริมสร้างความสัมพันธ์ระหว่างบุคคล", thaiFont,
    null, // คณะกรรมการ = 0 เพื่อแสดงเป็นช่องว่าง
    null); // ผู้เข้าร่วม = 0 เพื่อแสดงเป็นช่องว่าง


                    AddActivityRow(activityTableA, "2.3.1", "การเสริมสร้างทัศนคติประชาธิปไตย ", thaiFont,
documentDetail._231democraticSt.HasValue ? documentDetail._231democraticSt.Value : 0,
documentDetail._231democratic.HasValue ? documentDetail._231democratic.Value : 0);

                    AddActivityRow(activityTableA, "2.3.2", "กิจกรรมอื่น ๆ ", thaiFont,
documentDetail._232relationshipsSt.HasValue ? documentDetail._232relationshipsSt.Value : 0,
documentDetail._232relationships.HasValue ? documentDetail._232relationships.Value : 0);
                    AddActivityRow(activityTableA, "2.4", "ด้านสุขภาพ ", thaiFont,
documentDetail._24healthSt.HasValue ? documentDetail._24healthSt.Value : 0,
documentDetail._24health.HasValue ? documentDetail._24health.Value : 0);


                    AddActivityRow(activityTableA, "3.", "กิจกรรมเพื่อสังคม", thaiFont,
    null, // คณะกรรมการ = 0 เพื่อแสดงเป็นช่องว่าง
    null); // ผู้เข้าร่วม = 0 เพื่อแสดงเป็นช่องว่าง


                    AddActivityRow(activityTableA, "3.1", "กิจกรรมจิตสาธารณะ ", thaiFont,
documentDetail._31activitiesSt.HasValue ? documentDetail._31activitiesSt.Value : 0,
documentDetail._31activities.HasValue ? documentDetail._31activities.Value : 0);

                    AddActivityRow(activityTableA, "3.2", "กิจกรรมอื่น ๆ ", thaiFont,
documentDetail._32socialSt.HasValue ? documentDetail._32socialSt.Value : 0,
documentDetail._32social.HasValue ? documentDetail._32social.Value : 0);


                    AddTotalRow(activityTableA, thaiFont, totalHoursStA, totalHoursA);



                    document.Add(activityTableA);
                    
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง


                    document.Add(new Paragraph()
        .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
        .Add(new Text("ในการนี้ทาง สภาผู้แทนนิสิต องค์การนิสิต จึงขออนุมัติชั่วโมงกิจกรรมของนิสิต ตามรายชื่อและจำนวนชั่วโมงกิจกรรม ดังกล่าวที่ได้ขออนุมัติโครงการและดำเนินการจัดกิจกรรม ซึ่งขอรับรองว่ารายชื่อดังเอกสารแนบเป็นรายชื่อนิสิตที่เข้าร่วมกิจกรรม  จริงทุกประการ ").SetFont(thaiFont))
        

        .SetFontSize(12)
        .SetMultipliedLeading(1.0f)
        .SetTextAlignment(TextAlignment.LEFT));
                    document.Add(new Paragraph()
        .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
        .Add(new Text("จึงเรียนมาเพื่อโปรดพิจารณา  ").SetFont(thaiFont)));
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
                    document.Add(new Paragraph("\n")); //
                                                       // 
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง

                    Table signTable = new Table(4); // ตาราง 2 คอลัมน์
                    signTable.SetWidth(UnitValue.CreatePercentValue(100));

                    



                    // ช่องที่ 1
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("ความเห็นอาจารย์ที่ปรึกษา \n" +
                        "..............................\n" +
                        "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n"))
                        .Add(new Paragraph("\n"))// เพิ่มบรรทัดว่าง
                        .Add(new Paragraph($"{approve8 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("อาจารย์ที่ปรึกษา")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    // ช่องที่ 2
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("เรียน  ประธานคณะกรรมการองค์การนิสิต\n" +
                        "☐ ตรวจสอบแล้วเห็นควรอนุมัติชั่วโมงกิจกรรม\n" +
                        "☐ ไม่เห็นควรอนุมัติ\n" +
                        
                        "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))
.Add(new Paragraph("\n")).Add(new Paragraph("\n"))
                        .Add(new Paragraph($"{approve11 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );




                    // ช่องที่ 3
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("☐อนุมัติชั่วโมงกิจกรรม\n" +
                        "☐ ไม่อนุมัติ\n" +
                        "\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")).Add(new Paragraph("\n"))

                        .Add(new Paragraph($"{approve9 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("นักวิชาการเงินและบัญชี")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph(".../..../....")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );

                    // ช่องที่ 4
                    signTable.AddCell(new Cell()
                        .Add(new Paragraph("☐ ดำเนินการเรียบร้อยแล้ว\n" +
                        "☐ ยังไม่ได้ดำเนินการ\n" +

                        "\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("\n")).Add(new Paragraph("\n"))
                        .Add(new Paragraph($"{approve7 ?? "N/A"}")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("ผู้ดูแลระบบกิจกรรมฝ่ายกิจการนิสิต\n")
                            .SetFont(thaiFont).SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER))
                    );
                   
                    document.Add(signTable);







                    document.Close();
                    byte[] fileBytes = stream.ToArray();
                    string safeFileName = string.Concat(documentDetail.ProjectName.Split(Path.GetInvalidFileNameChars()));
                    return File(stream.ToArray(), "application/pdf", $"ขออนุมัติชัวโมงกิจกรรมโครงการ{safeFileName}.pdf");
                }
            
            
            }
        }

        static void AddActivityRow(Table table, string index, string activityName, PdfFont font, int? hours1, int? hours2)
        {
            table.AddCell(new Cell().Add(new Paragraph(index).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(activityName).SetFont(font)));
            table.AddCell(CreateHourCell(font, hours1));
            table.AddCell(CreateHourCell(font, hours2));
        }


        static void AddSubActivityRow(Table table, string index, string subActivityName, PdfFont font, int hours1, int hours2)
        {
            table.AddCell(new Cell().Add(new Paragraph(index).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("    " + subActivityName).SetFont(font)));
            table.AddCell(CreateHourCell(font, hours1));  // ใส่ค่าชั่วโมงที่ 1
            table.AddCell(CreateHourCell(font, hours2));  // ใส่ค่าชั่วโมงที่ 2
        }

        static void AddTotalRow(Table table, PdfFont font, int totalHours1, int totalHours2)
        {
            table.AddCell(new Cell(1, 2).Add(new Paragraph("รวมชั่วโมงกิจกรรมทั้งสิ้น").SetFont(font)));
            table.AddCell(CreateHourCell(font, totalHours1));  // รวมชั่วโมงที่ 1
            table.AddCell(CreateHourCell(font, totalHours2));  // รวมชั่วโมงที่ 2
        }

        static Cell CreateHourCell(PdfFont font, int? hours)
        {
            string displayText = hours.HasValue ? $"{hours.Value} ชั่วโมง" : "";
            return new Cell()
                .Add(new Paragraph(displayText).SetFont(font))
                .SetTextAlignment(TextAlignment.CENTER);
        }



        static Cell CreateHeaderCell(string text, PdfFont font)
        {
            return new Cell()
                .SetTextAlignment(TextAlignment.CENTER)

                .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                .Add(new Paragraph(text).SetFont(font));
        }


        
        // UploadDocument: อัปโหลดไฟล์
        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "กรุณาเลือกไฟล์เพื่ออัปโหลด");
                return RedirectToAction("Index");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new E_Document.Models.Document
            {
                FileName = file.FileName,
                FilePath = "/uploads/" + uniqueFileName,
                Status = "Uploaded",
                UploadedAt = DateTime.Now
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> SubmitDocument(int documentId)
        {
            var document = await _context.Documents.FindAsync(documentId);
            if (document == null)
            {
                return NotFound();
            }

            // เช็คว่ามี Approval คนแรกหรือยัง
            var existingFirstApproval = await _context.Approvals
                .Where(a => a.DocumentId == documentId && a.ApprovalOrder == 1)
                .FirstOrDefaultAsync();

            if (existingFirstApproval == null)
            {
                // ถ้ายังไม่มี Approval คนแรก ➔ สร้างใหม่
                var firstApprover = await _context.Users
                    .Where(u => u.Role == "Approver")
                    .OrderBy(u => u.ApprovalOrder)
                    .FirstOrDefaultAsync();

                if (firstApprover != null)
                {
                    var firstApproval = new Approval
                    {
                        DocumentId = document.Id,
                        ApproverId = firstApprover.Id,
                        ApprovalOrder = 1,
                        Status = "Pending",
                        ApprovedAt = null
                    };

                    _context.Approvals.Add(firstApproval);
                }
                else
                {
                    ModelState.AddModelError("Approver", "ไม่มีผู้อนุมัติในระบบ");
                    return RedirectToAction("Index");
                }
            }

            // เปลี่ยนสถานะเอกสารเป็น "In Review"
            document.Status = "In Review";
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
