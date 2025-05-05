using E_Document.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting; // เพิ่ม namespace นี้
using Microsoft.Extensions.Hosting;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;



namespace E_Document.Controllers
{

    [Authorize]
    public class ApproveController : Controller
    {
        private readonly AutoPdfContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;  // เพิ่มตัวแปรนี้
        // Constructor
        public ApproveController(AutoPdfContext context, IWebHostEnvironment webHostEnvironment)  // เพิ่มตัวแปรนี้ใน constructor
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;  // อินเจ็กต์ใน constructor
        }

        public async Task<IActionResult> Index()
        {
            var approverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var approvals = await _context.Approvals
                .Include(a => a.Document)
                .Where(a => a.ApproverId == approverId && a.Status == "Pending")
                .ToListAsync();

            return View(approvals);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Approve(int id)
        //{
        //    // ดึงข้อมูล approval พร้อมทั้งข้อมูลของ Approver
        //    var approval = await _context.Approvals
        //        .Include(a => a.Document)
        //        .Include(a => a.Approver) // ดึงข้อมูล Approver มาด้วย
        //        .FirstOrDefaultAsync(a => a.Id == id);

        //    if (approval == null)
        //    {
        //        TempData["ErrorMessage"] = "ไม่พบข้อมูลการอนุมัติ";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    // อัปเดตสถานะของการอนุมัติ
        //    approval.Status = "Approved"; // หรือสถานะที่คุณต้องการให้เปลี่ยน
        //    approval.LastApprover = approval.Approver.Username;
        //    approval.ApprovedAt = DateTime.Now;

        //    // อัปเดต LastApprover ของผู้อนุมัติคนแรก
        //    if (approval.Approver != null)
        //    {
        //        approval.LastApprover = approval.Approver.Username; // ชื่อของผู้อนุมัติ
        //    }
        //    else
        //    {
        //        approval.LastApprover = "Unknown"; // ถ้าไม่มีข้อมูลผู้อนุมัติ
        //    }

        //    // บันทึกการอนุมัติลงในฐานข้อมูล
        //    _context.Approvals.Update(approval);
        //    await _context.SaveChangesAsync(); // บันทึกการอนุมัติของผู้ปัจจุบัน

        //    // 🔥 เพิ่มการหาผู้อนุมัติคนถัดไป
        //    var nextApprover = FindNextApprover(approval.Approver?.ApprovalOrder ?? 0);

        //    if (nextApprover != null)
        //    {
        //        // ถ้ามีผู้อนุมัติคนถัดไป ใช้ Approval ตัวเดิมและอัปเดตข้อมูล
        //        approval.ApproverId = nextApprover.Id;
        //        approval.Status = "Pending"; // รอการอนุมัติ
        //        approval.LastApprover = approval.Approver.Username;

        //        // บันทึกการอัปเดตสำหรับผู้อนุมัติคนถัดไป
        //        _context.Approvals.Update(approval);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        // หากไม่มีผู้อนุมัติคนถัดไปให้เปลี่ยนสถานะเอกสารเป็น "Approved"
        //        var document = await _context.Documents.FindAsync(approval.DocumentId);
        //        if (document != null)
        //        {
        //            document.Status = "Approved";
        //            _context.Documents.Update(document);
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    // แจ้งข้อความสำเร็จ
        //    TempData["SuccessMessage"] = "อนุมัติเอกสารเรียบร้อยแล้ว";
        //    return RedirectToAction(nameof(Index));
        //}

        private User FindNextApprover(int currentOrder)
        {
            using (var db = new AutoPdfContext())
            {
                return db.Users
                    .Where(u => u.ApprovalOrder.HasValue && u.ApprovalOrder > currentOrder)
                    .OrderBy(u => u.ApprovalOrder)
                    .FirstOrDefault();
            }
        }









        // ฟังก์ชันปฏิเสธเอกสาร
        // ฟังก์ชันปฏิเสธเอกสาร
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            // ค้นหาเอกสาร
            var approval = await _context.Approvals
                .Include(a => a.Document)  // รวมข้อมูลเอกสาร
                .Where(a => a.Id == id && a.Status == "Pending")
                .FirstOrDefaultAsync();

            if (approval == null)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสารที่ต้องการปฏิเสธ";
                return RedirectToAction("Index");
            }

            // เปลี่ยนสถานะ Approval เป็น 'Rejected'
            approval.Status = "Rejected";
            approval.ApprovedAt = DateTime.Now; // สามารถบันทึกวันเวลาได้
            _context.Approvals.Update(approval);

            // เปลี่ยนสถานะ Document เป็น 'Rejected'
            var document = await _context.Documents.FindAsync(approval.DocumentId);
            if (document != null)
            {
                document.Status = "Rejected";
                _context.Documents.Update(document);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "เอกสารถูกปฏิเสธเรียบร้อยแล้ว";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SaveSignature(int documentId, string signatureData)
        {
            if (string.IsNullOrEmpty(signatureData))
            {
                TempData["ErrorMessage"] = "ไม่พบลายเซ็น";
                return RedirectToAction("Index");
            }

            var document = await _context.Documents.FindAsync(documentId);
            if (document == null)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสาร";
                return RedirectToAction("Index");
            }

            // 1. บันทึกลายเซ็น
            string fileName = Guid.NewGuid().ToString() + ".png";
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "signatures");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);
            byte[] imageBytes = Convert.FromBase64String(signatureData.Replace("data:image/png;base64,", ""));
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
            var approverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var signature = new Signature
            {
                DocumentId = documentId,
                ApproverId = approverId,
                SignaturePath = "/signatures/" + fileName,
                SignedAt = DateTime.Now
            };
            _context.Signatures.Add(signature);
            // 2. ใส่ลายเซ็น PNG ลงใน PDF
            // 2. ใส่ลายเซ็น PNG ลงใน PDF และเขียนทับไฟล์เดิม

            string pdfPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", document.FileName);
            string tempPdfPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", Guid.NewGuid().ToString() + "_temp.pdf"); // 🔄 temp file
            string signatureImagePath = Path.Combine(_webHostEnvironment.WebRootPath, signature.SignaturePath.TrimStart('/'));

            using (PdfReader pdfReader = new PdfReader(pdfPath))
            using (PdfWriter pdfWriter = new PdfWriter(tempPdfPath)) // 🔄 เขียนลงไฟล์ชั่วคราว
            using (PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter))
            {
                var page = pdfDoc.GetFirstPage();
                var canvas = new Canvas(new PdfCanvas(page), page.GetPageSize());  // ✅ ใช้ 2 พารามิเตอร์

                ImageData imageData = ImageDataFactory.Create(signatureImagePath);

                float x;
                float y;

                switch (approverId)
                {
                    case 3:
                        x = 90; y = 420;
                        break;
                    case 5:
                        x = 350; y = 420;
                        break;
                    default:
                        x = 400; y = 200;
                        break;
                }

                Image image = new Image(imageData);
                image.SetFixedPosition(x, y);
                image.ScaleToFit(150, 100);
                canvas.Add(image);
            }

            // 🔁 เขียนทับไฟล์เดิม
            System.IO.File.Delete(pdfPath);
            System.IO.File.Move(tempPdfPath, pdfPath);

            // ✅ ไม่ต้องเปลี่ยนชื่อไฟล์ใน database แล้ว
            // document.FileName = "signed_" + document.FileName; <-- ลบบรรทัดนี้ทิ้ง
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();


            // 4. อัปเดต Approval ว่าอนุมัติแล้ว

            var approval = await _context.Approvals
                .Include(a => a.Document)
                .Include(a => a.Approver)
                .FirstOrDefaultAsync(a => a.DocumentId == documentId && a.ApproverId == approverId && a.Status == "Pending");

            if (approval == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูลการอนุมัติของคุณ";
                return RedirectToAction("Index");
            }

            approval.Status = "Approved";
            approval.ApprovedAt = DateTime.Now;
            approval.LastApprover = approval.Approver?.Username ?? "Unknown";

            // 5. หา Approver ถัดไป
            var nextApprover = FindNextApprover(approval.Approver?.ApprovalOrder ?? 0);
            if (nextApprover != null)
            {
                approval.ApproverId = nextApprover.Id;
                approval.Status = "Pending";
                approval.LastApprover = approval.Approver.Username;
            }
            else
            {
                document.Status = "Approved";
                _context.Documents.Update(document);
            }

            _context.Approvals.Update(approval);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "ลายเซ็นและการอนุมัติถูกบันทึกเรียบร้อยแล้ว";
            return RedirectToAction("Index");
        }





    }
}
