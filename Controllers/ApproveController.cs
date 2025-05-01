using E_Document.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;



namespace E_Document.Controllers
{

    [Authorize]
    public class ApproveController : Controller
    {
        private readonly AutoPdfContext _context;

        // Constructor
        public ApproveController(AutoPdfContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Approve(int id)
        {
            // ดึงข้อมูล approval พร้อมทั้งข้อมูลของ Approver
            var approval = await _context.Approvals
                .Include(a => a.Document)
                .Include(a => a.Approver) // ดึงข้อมูล Approver มาด้วย
                .FirstOrDefaultAsync(a => a.Id == id);

            if (approval == null)
            {
                TempData["ErrorMessage"] = "ไม่พบข้อมูลการอนุมัติ";
                return RedirectToAction(nameof(Index));
            }

            // อัปเดตสถานะของการอนุมัติ
            approval.Status = "Approved"; // หรือสถานะที่คุณต้องการให้เปลี่ยน
            approval.LastApprover = approval.Approver.Username;
            approval.ApprovedAt = DateTime.Now;

            // อัปเดต LastApprover ของผู้อนุมัติคนแรก
            if (approval.Approver != null)
            {
                approval.LastApprover = approval.Approver.Username; // ชื่อของผู้อนุมัติ
            }
            else
            {
                approval.LastApprover = "Unknown"; // ถ้าไม่มีข้อมูลผู้อนุมัติ
            }

            // บันทึกการอนุมัติลงในฐานข้อมูล
            _context.Approvals.Update(approval);
            await _context.SaveChangesAsync(); // บันทึกการอนุมัติของผู้ปัจจุบัน

            // 🔥 เพิ่มการหาผู้อนุมัติคนถัดไป
            var nextApprover = FindNextApprover(approval.Approver?.ApprovalOrder ?? 0);

            if (nextApprover != null)
            {
                // ถ้ามีผู้อนุมัติคนถัดไป ใช้ Approval ตัวเดิมและอัปเดตข้อมูล
                approval.ApproverId = nextApprover.Id;
                approval.Status = "Pending"; // รอการอนุมัติ
                approval.LastApprover = approval.Approver.Username;

                // บันทึกการอัปเดตสำหรับผู้อนุมัติคนถัดไป
                _context.Approvals.Update(approval);
                await _context.SaveChangesAsync();
            }
            else
            {
                // หากไม่มีผู้อนุมัติคนถัดไปให้เปลี่ยนสถานะเอกสารเป็น "Approved"
                var document = await _context.Documents.FindAsync(approval.DocumentId);
                if (document != null)
                {
                    document.Status = "Approved";
                    _context.Documents.Update(document);
                    await _context.SaveChangesAsync();
                }
            }

            // แจ้งข้อความสำเร็จ
            TempData["SuccessMessage"] = "อนุมัติเอกสารเรียบร้อยแล้ว";
            return RedirectToAction(nameof(Index));
        }

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




    }
}
