using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnLineShop.Areas.Admin.Models;
using OnLineShop.Data;

namespace OnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {

        RoleManager<IdentityRole> _roleManger;

        private readonly ApplicationDbContext _context;
        UserManager<IdentityUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _context = context;
        }

        public IActionResult Index()
        {
            var userRole = _roleManger.Roles.ToList();
            ViewBag.Roles = userRole;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isexist = await _roleManger.RoleExistsAsync(role.Name);
            if (isexist)
            {
                ViewBag.msg = "This role is already exist";
                ViewBag.name = name;
                return View();


            }
            var result = await _roleManger.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "User has been created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();

        }


        public async Task<IActionResult> Edit(string id)
        {
            var findrole = await _roleManger.FindByIdAsync(id);
            if (findrole == null)
            {
                return NotFound();
            }
            ViewBag.id = findrole.Id;
            ViewBag.name = findrole.Name;
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManger.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;

            var isexist = await _roleManger.RoleExistsAsync(role.Name);
            if (isexist)
            {
                ViewBag.msg = "This role has already exist";
                ViewBag.name = name;
                return View();
            }

            var result = await _roleManger.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "user updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        public async Task<IActionResult> Delete(string id)
        {
            var findrole = await _roleManger.FindByIdAsync(id);
            if (findrole == null)
            {
                return NotFound();
            }
            ViewBag.name = findrole.Name;
            ViewBag.id = findrole.Id;
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]

        public async Task<IActionResult> DeleteConfrom(string id)
        {
            var finduser = await _roleManger.FindByIdAsync(id);
            if (finduser == null)
            {
                return NotFound();
            }

            var result = await _roleManger.DeleteAsync(finduser);

            if (result.Succeeded)
            {
                TempData["save"] = "User Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManger.Roles.ToList(), "Name", "Name");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVm roleuser)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == roleuser.UserId);
            var ischeckRoleAssign = await _userManager.IsInRoleAsync(user, roleuser.RoleId);


            if (ischeckRoleAssign)
            {
                ViewBag.msg = "This user assign this role";
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, roleuser.RoleId);

            if (role.Succeeded)
            {
                TempData["save"] = "User Role assign successfully";
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        public ActionResult AssignUserRole()
        {
            var result = from ur in _context.UserRoles
                         join r in _context.Roles on ur.RoleId equals r.Id
                         join a in _context.ApplicationUsers on ur.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;

            return View();
        }
    }


}
