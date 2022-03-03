using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IActionResult AllProducts()
        {
            MyToysDBContext context = new MyToysDBContext();
            return View(context.Products.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> NewProductAsync(IFormFile postedFile,Product product)
        {
            //update product image for the Product
            using (var dataStream = new MemoryStream())
            {
                await postedFile.CopyToAsync(dataStream);
                product.ProductImg = dataStream.ToArray();
            }
            //add new Product to database
            MyToysDBContext context = new MyToysDBContext();
            context.Products.Add(product);
            context.SaveChanges();
            //go to index Page
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NewProduct()
        {
            MyToysDBContext context = new MyToysDBContext();    
            var catogories = context.Categories.ToList();
            ViewBag.Categories = catogories;
            return View();
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}