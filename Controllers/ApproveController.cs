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

        // อนุมัติเอกสาร
        public async Task<IActionResult> Approve(int id)
        {
            var approval = await _context.Approvals.Include(a => a.Document).FirstOrDefaultAsync(a => a.Id == id);
            if (approval == null)
            {
                return NotFound();
            }

            // อัปเดต approval ของคนนี้
            approval.Status = "Approved";
            approval.ApprovedAt = DateTime.Now;
            _context.Update(approval);

            // หา next approver
            var nextApprover = FindNextApprover(approval.ApprovalOrder);

            if (nextApprover != null)
            {
                // ถ้ามีคนถัดไป สร้าง approval ใหม่
                var newApproval = new Approval
                {
                    DocumentId = approval.DocumentId,
                    ApproverId = nextApprover.Id,
                    Status = "Pending",
                    ApprovalOrder = nextApprover.ApprovalOrder ?? 0,
                    
                };
                _context.Approvals.Add(newApproval);

                // อัปเดต document ให้สถานะยังคง In Review
                var document = approval.Document;
                document.Status = "In Review";
                _context.Documents.Update(document);
            }
            else
            {
                // ถ้าไม่มีคนถัดไปแล้ว --> อนุมัติเอกสารเลย
                var document = approval.Document;
                document.Status = "Approved";
                _context.Documents.Update(document);
            }

            await _context.SaveChangesAsync();
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
