using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.IO;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductsRepository productsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productsRepository = productsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();
            return View(products);
        }

        // ========== ДОБАВЛЕНИЕ ==========
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile? uploadedFile)
        {
            if (ModelState.IsValid)
            {
                // Обработка загруженного файла
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    product.PhotoPath = await SaveImageAsync(uploadedFile);
                }
                else
                {
                    product.PhotoPath = "/img/anyProduct.png";
                }

                _productsRepository.Add(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // ========== РЕДАКТИРОВАНИЕ ==========
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productsRepository.TryGetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? uploadedFile)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _productsRepository.TryGetById(product.Id);
                if (existingProduct == null)
                    return NotFound();

                // Обновляем поля
                existingProduct.Name = product.Name;
                existingProduct.Cost = product.Cost;
                existingProduct.Description = product.Description;

                // Если загружено новое изображение
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    // Удаляем старое изображение, если оно не стандартное
                    if (!string.IsNullOrEmpty(existingProduct.PhotoPath) &&
                        existingProduct.PhotoPath != "/img/anyProduct.png")
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            existingProduct.PhotoPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    existingProduct.PhotoPath = await SaveImageAsync(uploadedFile);
                }

                _productsRepository.Update(existingProduct);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _productsRepository.TryGetById(id);
            if (product != null)
            {
                // Удаляем файл изображения, если он не стандартный
                if (!string.IsNullOrEmpty(product.PhotoPath) && product.PhotoPath != "/img/anyProduct.png")
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }
                _productsRepository.Delete(id);
            }
            return RedirectToAction("Index");
        }

        // ========== ВСПОМОГАТЕЛЬНЫЙ МЕТОД ДЛЯ СОХРАНЕНИЯ ФАЙЛА ==========
        private async Task<string> SaveImageAsync(IFormFile uploadedFile)
        {
            // Генерируем уникальное имя файла
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
            string relativePath = Path.Combine("images", "products", fileName).Replace('\\', '/');
            string absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

            // Создаём директорию, если её нет
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));

            using (var fileStream = new FileStream(absolutePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            return "/" + relativePath; // возвращаем путь, начинающийся с "/"
        }
    }
}