using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnLineShop.Data;
using OnLineShop.Models;

namespace OnLineShop.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;

        UserManager<IdentityUser> _usermanager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> usermanager)
        {
            _usermanager = usermanager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.ApplicationUsers.ToList());
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser applicationuser)
        {
            if (ModelState.IsValid)
            {
                var result = await _usermanager.CreateAsync(applicationuser, applicationuser.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _usermanager.AddToRoleAsync(applicationuser, role: "User");
                    TempData["Success"] = "User has been created successfully";
                    return View(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View();
        }

        public IActionResult Edit(string? id)
        {
            var result = _context.ApplicationUsers.FirstOrDefault(n => n.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(ApplicationUser appuser)
        {
            var searchuser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == appuser.Id);
            if (searchuser == null)
            {
                return NotFound();
            }

            searchuser.FirstName = appuser.FirstName;
            searchuser.LastName = appuser.LastName;

            var result = await _usermanager.UpdateAsync(searchuser);

            if (result.Succeeded)
            {
                TempData["save"] = "User has been Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(searchuser);

        }

        public IActionResult Details(string? id)
        {
            var SearchUser = _context.ApplicationUsers.FirstOrDefault(h => h.Id == id);
            if (SearchUser == null)
            {
                return View();
            }

            return View(SearchUser);
        }


        public IActionResult Delete(string? id)
        {

            var result = _context.ApplicationUsers.FirstOrDefault(a => a.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }


        [HttpPost]
        public IActionResult Delete(ApplicationUser user)
        {
            var userinfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userinfo == null)
            {
                return NotFound();
            }
            userinfo.LockoutEnd = DateTime.Now.AddYears(100);
            int rowEffected = _context.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["save"] = "User has been Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userinfo);
        }

        public async Task<IActionResult> Active(string? id)
        {
            var searchActiveUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (searchActiveUser == null)
            {
                return NotFound();
            }
            return View(searchActiveUser);
        }



        [HttpPost]
        public IActionResult Active(ApplicationUser user)
        {
            var userinfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userinfo == null)
            {
                return NotFound();
            }
            userinfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowEffected = _context.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["save"] = "User has been Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userinfo);
        }
    }
}
