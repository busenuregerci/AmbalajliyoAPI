using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.BLL.Services;
using Ambalajliyo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }

        /// <summary>
        /// Bütün sıkça sorulan soruları getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllFaqs")]
        public async Task<IActionResult> GetAllFaqs()
        {
            try
            {
                var faqs = await _faqService.GetAllFaqAsync();
                return Ok(faqs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait sıkça sorulan soruyu getirir.
        /// </summary>
        /// <param name="faqId"></param>
        /// <returns></returns>
        [HttpGet("GetFaqById/{faqId}")]
        public async Task<IActionResult> GetFaqById(Guid faqId)
        {
            try
            {
                var faq = await _faqService.GetFaqByIdAsync(faqId);
                if (faq == null)
                {
                    return BadRequest($"Kategori bulunamadı: {faqId}");
                }
                return Ok(faq);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni bir sıkça sorulan soru ekler.
        /// </summary>
        /// <param name="faqDto"></param>
        /// <returns></returns>
        [HttpPost("CreateFaq")]
        public async Task<IActionResult> CreateFaq([FromBody] FaqDto faqDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tüm soruları alıyoruz ve soru adının mevcut olup olmadığını kontrol ediyoruz
                var allFaqs = await _faqService.GetAllFaqAsync();
                if (allFaqs.Any(c => c.Question.Equals(faqDto.Question, StringComparison.OrdinalIgnoreCase)))
                {
                    // Aynı ada sahip kategori varsa 400 Bad Request dönüyoruz
                    return BadRequest(new { message = "Soru başlığı zaten mevcuttur" });
                }
                await _faqService.CreateFaqAsync(faqDto);
                return CreatedAtAction(nameof(GetFaqById), new { faqId = faqDto.Id }, faqDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }            
        }

        /// <summary>
        /// Belirtilen id'ye ait sıkça sorulan soruyu günceller.
        /// </summary>
        /// <param name="faqId"></param>
        /// <param name="faqDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateFaq/{faqId}")]
        public async Task<IActionResult> UpdateFaq(Guid faqId, [FromBody] FaqDto faqDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingFaq = await _faqService.GetFaqByIdAsync(faqId);
                if (existingFaq == null)
                {
                    return NotFound($"Kategori bulunamadı: {faqId}");
                }

                await _faqService.UpdateFaqAsync(faqId, faqDto);
                return Ok(faqDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait sıkça sorulan soruyu siler.
        /// </summary>
        /// <param name="faqId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFaq/{faqId}")]
        public async Task<IActionResult> DeleteFaq(Guid faqId)
        {
            try
            {
                var existingFaq = await _faqService.GetFaqByIdAsync(faqId);
                if (existingFaq == null)
                {
                    return NotFound($"Kategori bulunamadı: {faqId}");
                }

                await _faqService.DeleteFaqAsync(faqId);
                return NoContent(); // Başarılı silme işlemi için NoContent kullanılır
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}