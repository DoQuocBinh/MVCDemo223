using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //To Load all categories into memory
            //context.Categories.ToList();
            return View(context.Products.Include(p => p.CategoryNavigation).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> NewProductAsync(IFormFile postedFile,Product product)
        {
            MyToysDBContext context = new MyToysDBContext();
            if (product.ProductName.Length <5)
            {
                ModelState.AddModelError("ProductName", "Product name length must >=5");
                              
                var catogories = context.Categories.ToList();
                ViewBag.Categories = catogories;
                return View("NewProduct");
            }
            //check productName by using ML service
            
            //Load sample data
            var sampleData = new MLModel.ModelInput()
            {
                Col0 = product.ProductName,
            };

            //Load model and predict output
            var result = MLModel.Predict(sampleData);
            if (result.Prediction==0)
            {
                ModelState.AddModelError("ProductName", "Product name is not valid!!");

                var catogories = context.Categories.ToList();
                ViewBag.Categories = catogories;
                return View("NewProduct");
            }

            if (postedFile == null)
            {
                ModelState.AddModelError("ProductImg", "You must select a file to upload!");

                var catogories = context.Categories.ToList();
                ViewBag.Categories = catogories;
                return View("NewProduct");
            }

            //update product image for the Product
            using (var dataStream = new MemoryStream())
            {
                await postedFile.CopyToAsync(dataStream);
                product.ProductImg = dataStream.ToArray();
            }
            //add new Product to database
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