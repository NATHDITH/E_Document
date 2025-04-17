using System;
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
using iText.Layout;  // เพิ่ม namespace นี้
using iTextDocument = iText.Layout.Document;
using System.Text.Json;

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
            _context = context ?? throw new ArgumentNullException(nameof(context));  // ตรวจสอบ context ที่ส่งมา
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));  // ตรวจสอบ webHostEnvironment ที่ส่งมา
        }

        // ✅ ฟังก์ชันส่งเอกสาร

        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents.ToListAsync();
            return View(documents);
        }
        //public IActionResult Principles()
        //{
        //    return View();
        //}




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
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
                    document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
                    document.Add(new Paragraph()
                        .Add(new Text("ที่ สภ.มก.ศรช. ").SetFont(thaiFont)) // ข้อความ "ที่"
                        .Add(new Text(model.DocNo ?? "N/A").SetFont(thaiFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)) // ฟอนต์ไทยและจัดตรงกลาง
                        .Add(new Text(" / 2568 ").SetFont(thaiFont)) // ข้อความ "/2567"
                    );
                    var culture = new System.Globalization.CultureInfo("th-TH");

                    string formattedDatenow = model.CreateDate.HasValue
                        ? model.CreateDate.Value.ToString("dd MMMM yyyy", culture)
                        : "N/A";

                    string formattedDatestart = model.StartDate.HasValue
                        ? model.StartDate.Value.ToString("ddddที่ d MMMM yyyy", culture)
                        : "N/A";

                    string formattedDateend = model.EndDate.HasValue
                        ? model.EndDate.Value.ToString("ddddที่ d MMMM yyyy", culture)
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
                        .Add(new Paragraph($"\n{approve6 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph("หัวหน้างานบริหารกิจการนิสิตและการกีฬา").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                        .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                    //ลายเซ็นทางขวา(จัดตรงกลาง)
                    Cell rightCell = new Cell()
                        .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                        .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                        .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
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



                    document.Add(new Paragraph("แบบเสนอโครงการกิจกรรมนิสิต\n")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("มหาวิทยาลัยเกษตรศาสตร์ วิทยาเขตศรีราชา\n")
                            .SetFont(thaiFont)
                            .SetMultipliedLeading(1.0f)
                            .SetTextAlignment(TextAlignment.CENTER));

                    Paragraph committee2Paragraph = new Paragraph()
        .SetTextAlignment(TextAlignment.LEFT)
        .SetMultipliedLeading(1.5f) // ระยะห่างระหว่างบรรทัด
        .Add(new Tab()) // ขยับเลขไปที่ตำแหน่งแรก
        .Add(new Text("1. ").SetFont(thaiFont))
        .Add(new Tab()) // ขยับชื่อไปที่ตำแหน่งที่ 2
        .Add(new Text("ผศ.ดร.นัฎฐวิกา จันทร์ศรี").SetFont(thaiFont))

        .Add(new Text("\n"))
        .Add(new Tab())
        .Add(new Text("2. ").SetFont(thaiFont))
        .Add(new Tab())
        .Add(new Text("นายนนทกร บุญมาก").SetFont(thaiFont))

        .Add(new Text("\n"))
        .Add(new Tab())
        .Add(new Text("3. ").SetFont(thaiFont))
        .Add(new Tab())
        .Add(new Text("นายชนัญชัย ดอนมงคล").SetFont(thaiFont))
        ;

                    //กำหนดตำแหน่งของแต่ละ Tab
                    committee2Paragraph.AddTabStops(
                        new TabStop(-100, TabAlignment.LEFT),  // เลข 1, 2, 3
                        new TabStop(50, TabAlignment.LEFT) // ชื่อ

                    );

                    document.Add(committeeParagraph);




                    document.Close();
                    byte[] fileBytes = stream.ToArray();
                    string safeFileName = string.Concat(model.ProjectName.Split(Path.GetInvalidFileNameChars()));
                    return File(stream.ToArray(), "application/pdf", $"{safeFileName}.pdf");

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

                document.Add(new Paragraph()
    .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
    .Add(new Text("บัดนี้การดำเนินโครงการดังกล่าวเสร็จสิ้นเรียบร้อยแล้ว จึงใคร่ขอรายงานผลการดำเนินงานและขออนุมัติเบิกค่าใช้จ่ายเงินบำรุงกิจกรรมนิสิต ปีการศึกษา 2568 เป็นจำนวนเงินทั้งสิ้น ").SetFont(thaiFont))
    .Add(new Text(" ตามแผนการใช้เงินบำรุงกิจกรรมนิสิต ปีการศึกษา 2568 โดยมีวงเงินค่าใช้จ่ายจำนวน  ").SetFont(thaiFont)).Add(new Text($"{(documentDetail.Maintenancemoney > 0 ? ((decimal)documentDetail.Maintenancemoney).ToString("N2") : "N/A")} บาท").SetFont(thaiFont))

    .SetFontSize(12)
    .SetMultipliedLeading(1.0f)
    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง



                document.Add(new Paragraph()
    .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
    .Add(new Text("จึงเรียนมาเพื่อโปรดพิจารณา  ").SetFont(thaiFont)));
                document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
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

                // ช่องที่ 2

                // ช่องที่ 2 (2 ลายเซ็น ซ้าย-ขวา และอยู่ตรงกลาง)
                Table innerTable = new Table(2); // ตารางย่อย 2 คอลัมน์
                innerTable.SetWidth(UnitValue.CreatePercentValue(100));

                // ลายเซ็นทางซ้าย (จัดตรงกลาง)
                Cell leftCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                    .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                    .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(new Paragraph($"{approve6 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(new Paragraph("หัวหน้างานบริหารกิจการนิสิตและการกีฬา").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                // ลายเซ็นทางขวา (จัดตรงกลาง)
                Cell rightCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // ไม่มีเส้นขอบ
                    .SetTextAlignment(TextAlignment.CENTER) // จัดข้อความตรงกลาง
                    .Add(new Paragraph("ลงชื่อ .................................\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(new Paragraph($"{approve5 ?? "N/A"}").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(new Paragraph("ผู้อำนวยการกองบริหารการศึกษาและพัฒนานิสิต").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(new Paragraph(".../..../....").SetFont(thaiFont).SetMultipliedLeading(1.0f));

                // เพิ่มเซลล์ซ้าย-ขวาเข้าตารางย่อย
                innerTable.AddCell(leftCell);
                innerTable.AddCell(rightCell);

                // เพิ่มตารางย่อยลงในตารางหลัก
                signTable.AddCell(new Cell()
                    .SetTextAlignment(TextAlignment.LEFT) // จัดข้อความของเซลล์หลักให้อยู่ตรงกลาง
                    .Add(new Paragraph("2) ตรวจสอบให้เป็นไปตามแผนการจัดกิจกรรม\n").SetFont(thaiFont).SetMultipliedLeading(1.0f))
                    .Add(innerTable) // ใส่ตารางย่อยที่มี 2 คอลัมน์ (ซ้าย-ขวา)
                );




                // ช่องที่ 3
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

                // ช่องที่ 4
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
                // ช่องที่ 5
                signTable.AddCell(new Cell()
                    .Add(new Paragraph("5)เรียนรองอธิการบดี ฯ\n" +
                    "เพื่อโปรดพิจารณา\n") // ใช้ \n เพื่อขึ้นบรรทัดใหม่
                        .SetFont(thaiFont).SetMultipliedLeading(1.0f))

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

                // ช่องที่ 6
                signTable.AddCell(new Cell()
                    .Add(new Paragraph("อนุมัติ\n\n\n")
                        .SetFont(thaiFont).SetMultipliedLeading(1.0f).SetTextAlignment(TextAlignment.CENTER))

                    .Add(new Paragraph($"{approve1 ?? "N/A"}")
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
                return File(fileBytes, "application/pdf", "ProjectDocument.pdf");
            }
        }
        }


        public async Task<IActionResult> Activity()
        {
            var documents = await _context.Documents.ToListAsync();
            return View(documents); // ✅ ส่ง Model ไปยัง View
        }



        // ✅ ฟังก์ชันสำหรับส่งเอกสาร
        // ✅ ฟังก์ชันสำหรับส่งเอกสารและบันทึก Approval

        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "กรุณาเลือกไฟล์เพื่ออัปโหลด");
                return RedirectToAction("Index");
            }

            // กำหนดโฟลเดอร์ uploads ภายใน wwwroot
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // สร้างโฟลเดอร์ถ้าไม่มี
            }

            // ตั้งชื่อไฟล์ให้ไม่ซ้ำกัน
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // บันทึกไฟล์ลงในเซิร์ฟเวอร์
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // บันทึกข้อมูลเอกสารลงฐานข้อมูล
            var document = new E_Document.Models.Document
            {
                FileName = file.FileName,
                FilePath = "/uploads/" + uniqueFileName,
                Status = "Uploaded",
                UploadedAt = DateTime.Now
            };


            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            // บันทึกข้อมูล Approval (ผู้อนุมัติคนแรก)
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
                    ApprovalOrder = 1,  // ลำดับที่ 1
                    Status = "Pending", // สถานะเริ่มต้น
                    ApprovedAt = null
                };

                _context.Approvals.Add(firstApproval);
                await _context.SaveChangesAsync();


            }
            else
            {
                ModelState.AddModelError("Approver", "ไม่มีผู้อนุมัติในระบบ");
                return RedirectToAction("Index");
            }

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

            // ค้นหาผู้อนุมัติคนแรก (ApprovalOrder = 1)
            var firstApprover = await _context.Approvals
                .Where(a => a.DocumentId == documentId && a.ApprovalOrder == 1 && a.Status == "Pending")
                .FirstOrDefaultAsync();

            if (firstApprover == null)
            {
                return BadRequest("ไม่มีผู้อนุมัติในระบบ");
            }

            // เปลี่ยนสถานะเอกสารเป็น "In Review"
            document.Status = "In Review";
            await _context.SaveChangesAsync();

            // ตรวจสอบว่าเอกสารได้รับการอนุมัติครบทุกคนหรือไม่
            var allApprovals = await _context.Approvals
                .Where(a => a.DocumentId == documentId && a.Status == "Approved")
                .ToListAsync();

            var totalApprovers = await _context.Users.CountAsync(u => u.Role == "Approver");

            if (allApprovals.Count == totalApprovers)
            {
                // ถ้าอนุมัติครบทุกคนแล้ว เปลี่ยนสถานะเอกสารเป็น "Approved"
                document.Status = "Approved";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }




        

        //        [HttpPost]
        //        public IActionResult GenerateActivityPdf([FromForm] DocumentDetail2 model)
        //        {
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                PdfWriter writer = new PdfWriter(stream);
        //                PdfDocument pdf = new PdfDocument(writer);
        //                Document document = new Document(pdf);
        //                iText.Layout.Document document = new iText.Layout.Document(pdf);
        //                string fontPath = "wwwroot/fonts/THSarabunNew.ttf"; // เส้นทางฟอนต์
        //                PdfFont thaiFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H); // ฝังฟอนต์
        //                PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        //                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        //                Cell cell = new Cell().Add(new Paragraph("ข้อความตัวหนา").SetFont(boldFont));
        //                Table table = new Table(2);
        //                table.AddCell(cell);
        //                เพิ่มข้อความโดยใช้ฟอนต์ไทย
        //                int activity1Hours1 = model.Activity1Hours1;
        //                int activity1Hours2 = model.Activity1Hours2;
        //                int subActivity2Hours1 = model.SubActivity2Hours1;
        //                int subActivity2Hours2 = model.SubActivity2Hours2;
        //                int subActivity2_1Hours1 = model.SubActivity2_1Hours1;
        //                int subActivity2_1Hours2 = model.SubActivity2_1Hours2;
        //                int subActivity2_2Hours1 = model.SubActivity2_2Hours1;
        //                int subActivity2_2Hours2 = model.SubActivity2_2Hours2;
        //                int subActivity2_3Hours1 = model.SubActivity2_3Hours1;
        //                int subActivity2_3Hours2 = model.SubActivity2_3Hours2;
        //                int subActivity2_4Hours1 = model.SubActivity2_4Hours1;
        //                int subActivity2_4Hours2 = model.SubActivity2_4Hours2;

        //                int totalHours1 = activity1Hours1 + subActivity2Hours1 + subActivity2_1Hours1 + subActivity2_2Hours1 + subActivity2_3Hours1 + subActivity2_4Hours1;
        //                int totalHours2 = activity1Hours2 + subActivity2Hours2 + subActivity2_1Hours2 + subActivity2_2Hours2 + subActivity2_3Hours2 + subActivity2_4Hours2;
        //                document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง
        //                document.Add(new Paragraph("\n")); // เพิ่มบรรทัดว่าง

        //                เพิ่มข้อมูล DocNo โดยใช้ฟอนต์ไทยทั้งหมด
        //                document.Add(new Paragraph()
        //                    .Add(new Text("ที่ สภ.มก.ศรช. ").SetFont(thaiFont)) // ข้อความ "ที่"
        //                    .Add(new Text(model.DocNo ?? "N/A").SetFont(thaiFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)) // ฟอนต์ไทยและจัดตรงกลาง
        //                    .Add(new Text(" / 2568 ").SetFont(thaiFont)) // ข้อความ "/2567"
        //                );

        //                แปลงวันที่ให้เป็นภาษาไทย
        //                string formattedDatenow = model.ProjectDate?.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("th-TH")) ?? "N/A";

        //                string formattedDatestart = model.ProjectStartDate?.ToString("ddddที่ d MMMM yyyy", new System.Globalization.CultureInfo("th-TH")) ?? "N/A";
        //                string formattedDateend = model.ProjectEndDate?.ToString("ddddที่ d MMMM yyyy", new System.Globalization.CultureInfo("th-TH")) ?? "N/A";


        //                int totalDays = (model.ProjectStartDate.HasValue && model.ProjectEndDate.HasValue)
        //    ? (model.ProjectEndDate.Value - model.ProjectStartDate.Value).Days
        //    : 0;
        //                เพิ่มวันที่
        //                document.Add(new Paragraph("วันที่ ")
        //                    .Add(new Text(formattedDatenow).SetFont(thaiFont))  // แสดงวันที่ที่แปลงแล้ว
        //                    .SetFont(thaiFont)
        //                    .SetFontSize(12)
        //                    .SetMarginLeft(250)); // เพิ่มระยะห่างจากซ้ายเพื่อเลื่อนข้อความไปทางขวา



        //                document.Add(new Paragraph()
        //                    .Add(new Text("เรื่อง ขออนุมัติชั่วโมงกิจกรรมโครงการ  ").SetFont(thaiFont))
        //                    .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont))
        //                    .Add(new Text("\n เรียน ประธานคณะกรรมการองค์การนิสิต มหาวิทยาลัยเกษตรศาสตร์ วิทยาเขตศรีราชา ").SetFont(thaiFont).SetFontSize(12))
        //                    .Add(new Text("\n สิ่งที่แนบมาด้วย      1. สำเนาขออนุมัติหลักการและวงเงินค่าใช้จ่ายโครงการ ").SetFont(thaiFont))  // ใช้ฟอนต์ไทยสำหรับข้อความ
        //                    .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont))


        //                 ใช้ฟอนต์ไทยสำหรับ ProjectName
        //                );
        //                document.Add(new Paragraph()
        //    .SetFirstLineIndent(65).Add(new Text("2. ใบบันทึกชั่วโมงกิจกรรมโครงการ  ").SetFont(thaiFont))  // ใช้ฟอนต์ไทยสำหรับข้อความ
        //                    .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont)));



        //                document.Add(new Paragraph()
        //    .SetFirstLineIndent(65) // เยื้องบรรทัดแรก
        //    .Add(new Text("เนื่องด้วยสภาผู้แทนนิสิต องค์การนิสิต ได้มีการดำเนินการจัดกิจกรรมโครงการ ").SetFont(thaiFont))
        //    .Add(new Text(model.ProjectName ?? "N/A").SetFont(thaiFont))
        //    .Add(new Text("  รหัสโครงการ  ").SetFont(thaiFont))
        //    .Add(new Text(model.ProjectCode ?? "N/A").SetFont(thaiFont))
        //    .Add(new Text("  ในระหว่าง ").SetFont(thaiFont))
        //    .Add(new Text(formattedDatestart).SetFont(thaiFont))
        //    .Add(new Text("  ถึง ").SetFont(thaiFont))
        //    .Add(new Text(formattedDateend).SetFont(thaiFont))
        //    .Add(new Text($" ซึ่งมีระยะเวลาทั้งสิ้น {totalDays} วัน").SetFont(thaiFont))
        //    .Add(new Text(" จำนวนการทำกิจกรรมทั้งหมด  ").SetFont(thaiFont))
        //    .Add(new Text(totalHours1.ToString()).SetFont(thaiFont))

        //    .Add(new Text(" ชั่วโมง โดยมีรายละเอียดการบันทึกชั่วโมงกิจกรรมด้านต่าง ๆ ต่อครั้ง และมีผู้เข้าร่วมกิจกรรมจำแนกตามวัน ดังนี้  ").SetFont(thaiFont))

        //    .SetFontSize(12)
        //    .SetMultipliedLeading(1.0f)
        //    .SetTextAlignment(TextAlignment.LEFT)
        //);









        //                // สร้างตาราง 4 คอลัมน์
        //                Table activityTable = new Table(4);
        //                activityTable.SetWidth(UnitValue.CreatePercentValue(100));


        //                activityTable.AddHeaderCell(CreateHeaderCell("ลำดับ", thaiFont));
        //                activityTable.AddHeaderCell(CreateHeaderCell("ประเภทกิจกรรม", thaiFont));
        //                activityTable.AddHeaderCell(CreateHeaderCell("คณะกรรมการ", thaiFont));
        //                activityTable.AddHeaderCell(CreateHeaderCell("ผู้เข้าร่วมกิจกรรม", thaiFont));


        //                AddActivityRow(activityTable, "1.", "กิจกรรมมหาวิทยาลัย", thaiFont, activity1Hours1, activity1Hours2);
        //                AddSubActivityRow(activityTable, "2.", "กิจกรรมเลือกเข้าร่วม", thaiFont, subActivity2Hours1, subActivity2Hours2);
        //                AddSubActivityRow(activityTable, "2.1", "กิจกรรมทางด้านคุณธรรม จริยธรรมหรือค่านิยม", thaiFont, subActivity2_1Hours1, subActivity2_1Hours2);
        //                AddSubActivityRow(activityTable, "2.2", "กิจกรรมเสริมสร้างทักษะทางวิชาการหรือวิชาชีพ", thaiFont, subActivity2_2Hours1, subActivity2_2Hours2);
        //                AddSubActivityRow(activityTable, "2.3", "กิจกรรมพัฒนาบุคลิกภาพและการแสดงออก", thaiFont, subActivity2_3Hours1, subActivity2_3Hours2);
        //                AddSubActivityRow(activityTable, "2.4", "กิจกรรมทางด้านเสริมสร้างและพัฒนาสุขภาพ", thaiFont, subActivity2_4Hours1, subActivity2_4Hours2);

        //                รวมชั่วโมงกิจกรรม


        //                AddTotalRow(activityTable, thaiFont, totalHours1, totalHours2);



        //                document.Add(activityTable);
        //                document.Close();
        //                byte[] fileBytes = stream.ToArray();
        //                return File(fileBytes, "application/pdf", "ProjectDocument.pdf");
        //            }
        //        }
        //        static void AddActivityRow(Table table, string index, string activityName, PdfFont font, int hours1, int hours2)
        //        {
        //            table.AddCell(new Cell().Add(new Paragraph(index).SetFont(font)));
        //            table.AddCell(new Cell().Add(new Paragraph(activityName).SetFont(font)));
        //            table.AddCell(CreateHourCell(font, hours1));  // ใส่ค่าชั่วโมงที่ 1
        //            table.AddCell(CreateHourCell(font, hours2));  // ใส่ค่าชั่วโมงที่ 2
        //        }

        //        static void AddSubActivityRow(Table table, string index, string subActivityName, PdfFont font, int hours1, int hours2)
        //        {
        //            table.AddCell(new Cell().Add(new Paragraph(index).SetFont(font)));
        //            table.AddCell(new Cell().Add(new Paragraph("    " + subActivityName).SetFont(font)));
        //            table.AddCell(CreateHourCell(font, hours1));  // ใส่ค่าชั่วโมงที่ 1
        //            table.AddCell(CreateHourCell(font, hours2));  // ใส่ค่าชั่วโมงที่ 2
        //        }

        //        static void AddTotalRow(Table table, PdfFont font, int totalHours1, int totalHours2)
        //        {
        //            table.AddCell(new Cell(1, 2).Add(new Paragraph("รวมชั่วโมงกิจกรรมทั้งสิ้น").SetFont(font)));
        //            table.AddCell(CreateHourCell(font, totalHours1));  // รวมชั่วโมงที่ 1
        //            table.AddCell(CreateHourCell(font, totalHours2));  // รวมชั่วโมงที่ 2
        //        }

        //        static Cell CreateHourCell(PdfFont font, int hours)
        //        {
        //            return new Cell().Add(new Paragraph(hours.ToString() + " ชั่วโมง").SetFont(font)).SetTextAlignment(TextAlignment.CENTER);
        //        }

        //        static Cell CreateHeaderCell(string text, PdfFont font)
        //        {
        //            return new Cell()
        //                .SetTextAlignment(TextAlignment.CENTER)

        //                .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
        //                .Add(new Paragraph(text).SetFont(font));
        //        }




    }
}
