using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ChatForLife.Controllers
{
    public class FileUploadController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            // Eğer ayrı bir upload sayfan varsa burada return View();
            // Yoksa bu metodu kaldırabilirsin.
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçin.";
                return RedirectToAction("UserProfile", "Profile");
            }

            if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
            {
                TempData["Error"] = "Lütfen sadece PDF dosyası yükleyin.";
                return RedirectToAction("UserProfile", "Profile");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, pdfFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            TempData["Success"] = "PDF dosyası başarıyla yüklendi.";
            return RedirectToAction("UserProfile", "Profile");
        }
    }
}
