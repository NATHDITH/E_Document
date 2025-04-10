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

        // ฟังก์ชันแสดงรายการเอกสารที่รอการอนุมัติ
        public async Task<IActionResult> Index()
        {
            var approvals = await _context.Approvals
                .Include(a => a.Document)  // โหลดข้อมูล Document ที่เชื่อมโยงกับ Approval
                .Include(a => a.Approver)  // โหลดข้อมูล Approver ที่เชื่อมโยงกับ Approval
                .ToListAsync();

            foreach (var approval in approvals)
            {
                // หาผู้อนุมัติล่าสุดสำหรับเอกสาร
                var lastApprover = await _context.Approvals
                    .Where(a => a.DocumentId == approval.DocumentId)
                    .OrderByDescending(a => a.ApprovalOrder)
                    .FirstOrDefaultAsync();

                if (lastApprover != null)
                {
                    approval.LastApprover = lastApprover.Approver?.Username; // กำหนด LastApprover ในแต่ละ Approval
                }
            }

            // ส่งข้อมูลไปยัง View (List<Approval>)
            return View(approvals);  // ส่งข้อมูลแบบที่ถูกต้อง
        }



        public async Task<IActionResult> Approve(int id)
        {
            var approval = await _context.Approvals
                .Include(a => a.Document)
                .Where(a => a.Id == id && a.Status == "Pending")
                .FirstOrDefaultAsync();

            if (approval == null)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสารที่ต้องการอนุมัติ";
                return RedirectToAction("Index");
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("กรุณาเข้าสู่ระบบก่อน");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("ไม่พบข้อมูลผู้อนุมัติ");
            }

            if (!int.TryParse(userId, out var approverId))
            {
                return BadRequest("User ID ไม่ถูกต้อง");
            }

            // ✅ อนุมัติเอกสารนี้
            approval.Status = "Approved";
            approval.ApprovedAt = DateTime.Now;
            _context.Approvals.Update(approval);
            await _context.SaveChangesAsync();

            // ✅ ค้นหา Approver ถัดไป
            var nextApprover = await _context.Approvals
                .Where(a => a.DocumentId == approval.DocumentId && a.ApprovalOrder == approval.ApprovalOrder + 1)
                .FirstOrDefaultAsync();

            if (nextApprover != null)
            {
                // ส่งเอกสารให้ Approver ถัดไป
                nextApprover.Status = "Pending";
                _context.Approvals.Update(nextApprover);
            }
            else
            {
                // ✅ หากไม่มี Approver ถัดไป ให้เปลี่ยนสถานะเอกสารเป็น "Final Approved"
                var document = await _context.Documents.FindAsync(approval.DocumentId);
                if (document != null)
                {
                    document.Status = "Approved";
                    _context.Documents.Update(document);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }










        // ฟังก์ชันปฏิเสธเอกสาร
        // ฟังก์ชันปฏิเสธเอกสาร
        public async Task<IActionResult> Reject(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            // ✅ ค้นหา Approval ที่ยังค้างในสถานะ 'Pending' และเป็นของ Approver ปัจจุบัน
            var approval = await _context.Approvals
                .Where(a => a.DocumentId == document.Id && a.Status == "Pending")
                .FirstOrDefaultAsync();

            if (approval == null)
            {
                TempData["ErrorMessage"] = "ไม่พบเอกสารที่ต้องการปฏิเสธ หรือเอกสารไม่ได้อยู่ในสถานะ Pending";
                return RedirectToAction("Index");
            }

            // ✅ ตรวจสอบว่า Approver ที่ทำรายการเป็นคนที่ถูกต้อง
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var approverId) || approverId != approval.ApproverId)
            {
                return Unauthorized("คุณไม่มีสิทธิ์ปฏิเสธเอกสารนี้");
            }

            // ✅ เปลี่ยนสถานะ Approval เป็น 'Rejected'
            approval.Status = "Rejected";
            _context.Approvals.Update(approval);

            // ✅ เปลี่ยนสถานะเอกสารหลักเป็น 'Rejected'
            document.Status = "Rejected";
            _context.Documents.Update(document);

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "เอกสารถูกปฏิเสธเรียบร้อยแล้ว";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message;
            }

            return RedirectToAction("Index");
        }



    }
}
