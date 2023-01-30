using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnLineShop.Data;
using OnLineShop.Models;

namespace OnLineShop.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]

    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index(bool issuccess = false)
        {
            ViewBag.Isuccess = issuccess;
            var producttypes = _context.ProductTypes.ToList();
            return View(producttypes);
        }



        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(ProductTypes producttypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Add(producttypes);
                await _context.SaveChangesAsync();
                TempData["save"] = "Product Type has been save Sucessfully";
                return RedirectToAction(nameof(Index), new { issuccess = true });
            }

            return View(producttypes);

        }

        public IActionResult Edit(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var produttypesfind = _context.ProductTypes.Find(id);
            if (produttypesfind == null)
            {
                return NotFound();
            }

            return View(produttypesfind);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]

        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Update(productTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        public IActionResult Detail(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var result = _context.ProductTypes.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]


        public IActionResult Detail(ProductTypes producttypes)
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var deleteproduct = _context.ProductTypes.Find(id);
            if (deleteproduct == null)
            {
                return NotFound();
            }
            return View(deleteproduct);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes producttypes)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != producttypes.Id)
            {
                return NotFound();
            }
            var findproduct = _context.ProductTypes.Find(id);
            if (findproduct == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Remove(findproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producttypes);
        }
    }
}
