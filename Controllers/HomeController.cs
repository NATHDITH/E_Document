using E_Document.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
namespace E_Document.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AutoPdfContext _context; // ??????????? _context

        // ??????? constructor ???????? _context ??
        public HomeController(ILogger<HomeController> logger, AutoPdfContext context)
        {
            _logger = logger;
            _context = context; // ?????????????? _context
        }
        [Authorize]
        public IActionResult Index()
        {
            string username = User.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                ViewBag.Username = username;
            }

            var approver = _context.NameApprovers.FirstOrDefault();

            if (approver == null)
            {
                approver = new NameApprover();
                _context.NameApprovers.Add(approver);
                _context.SaveChanges();
            }

            return View(approver);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(NameApprover approver)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.NameApprovers.FirstOrDefault();

                if (existing != null)
                {
                    // ?????????????????????????
                    existing._1Approve = approver._1Approve;
                    existing._2Approve = approver._2Approve;
                    existing._3Approve = approver._3Approve;
                    existing._4Approve = approver._4Approve;
                    existing._5Approve = approver._5Approve;
                    existing._6Approve = approver._6Approve;
                    existing._7Approve = approver._7Approve;
                    existing._8Approve = approver._8Approve;
                    existing._9Approve = approver._9Approve;
                    existing._10Approve = approver._10Approve;
                    existing._11Approve = approver._11Approve;

                    // ???????????????
                    _context.SaveChanges();
                    TempData["Message"] = "??????????????????!";
                }
            }
            else
            {
                TempData["Message"] = "?????????????????????????!";
            }

            return RedirectToAction("Index");
        }



        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public ActionResult Login()
        {
            return View();
        }



        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && user.PasswordHash == password)
            {
                // ??????? Role ???????????? Claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role) // ????? Role ????????? Claim
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "????????????????????????????????";
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }









    }
}
