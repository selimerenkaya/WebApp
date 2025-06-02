using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        [HttpPost("analyze-gender")]
        public IActionResult AnalyzeGender(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("⚠️ Dosya bulunamadı.");

            // Yükleme klasörünü oluştur
            var uploadsFolder = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Dosyayı kaydet
            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Model dosya yolu
            var modelPath = Path.Combine("wwwroot", "models", "model_quantized.onnx");

            // AI tahmini
            var predictor = new GenderPredictor(modelPath);
            var result = predictor.PredictGender(filePath);

            return Ok(new
            {
                file = file.FileName,
                gender = result
            });
        }
    }
}
