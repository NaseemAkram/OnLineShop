using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnLineShop.Data;
using OnLineShop.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        private IHostingEnvironment _he;

        public ProductController(ApplicationDbContext context, IHostingEnvironment he)
        {
            _context = context;
            _he = he;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.ProductTypes).ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Index(decimal lowamount, decimal highamount)
        {
            var products = _context.Products.Include(m => m.ProductTypes).Where(n => n.Price >= lowamount && n.Price <= highamount).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchproduct = _context.Products.FirstOrDefault(b => b.Name == product.Name);
                if (searchproduct != null)
                {
                    ViewBag.message = "This Product is already exist";
                    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
                    return View();
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));

                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Image/noimg.jpg";
                }

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            return View(product);
        }


        public IActionResult Edit(int? id)
        {

            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            if (id == 0)
            {
                return NotFound();
            }
            var EditProduct = _context.Products.Include(p => p.ProductTypes).FirstOrDefault(u => u.Id == id);
            if (EditProduct == null)
            {
                return NotFound();
            }
            return View(EditProduct);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));

                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Image/noimg.jpg";
                }

                _context.Products.Update(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            if (id == 0)
            {

                return NotFound();
            }

            var result = _context.Products.Include(p => p.ProductTypes).FirstOrDefault(u => u.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var result = _context.Products.Include(u => u.ProductTypes).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        [ActionName("Delete")]

        public ActionResult DeleteConfirm(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var delete = _context.Products.FirstOrDefault(o => o.Id == id);
            if (delete == null)
            {
                return NotFound();
            }

            _context.Products.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
