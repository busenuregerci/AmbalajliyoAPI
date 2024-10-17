using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Bütün katalogları getirir
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllCatalog")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                var catalogs = await _catalogService.GetAllCatalog();
                return Ok(catalogs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni bir katalog ekler.
        /// </summary>
        /// <param name="catalogDto"></param>
        /// <returns></returns>
        [HttpPost("AddCatalog")]
        public async Task<IActionResult> AddCatalog([FromBody] CatalogDto catalogDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tüm soruları alıyoruz ve soru adının mevcut olup olmadığını kontrol ediyoruz
                var allCatalog = await _catalogService.GetAllCatalog();
                if (allCatalog.Any(c => c.PdfName.Equals(catalogDto.PdfName, StringComparison.OrdinalIgnoreCase)))
                {
                    // Aynı ada sahip kategori varsa 400 Bad Request dönüyoruz
                    return BadRequest(new { message = "Dosya adı zaten mevcuttur!" });
                }
                await _catalogService.CreateCatalog(catalogDto);
                return CreatedAtAction(nameof(GetByIdCatalog), new { catalogId = catalogDto.Id }, catalogDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait kataloğu getirir.
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        [HttpGet("GetByIdCatalog/{catalogId}")]
        public async Task<IActionResult> GetByIdCatalog(Guid catalogId)
        {
            try
            {
                var catalog = await _catalogService.GetCatalogById(catalogId);
                if (catalog == null)
                {
                    return NotFound($"Ürün bulunamadı: {catalogId}");
                }
                return Ok(catalog);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait kataloğu siler.
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCatlog/{catalogId}")]
        public async Task<IActionResult> DeleteCategory(Guid catalogId)
        {
            try
            {
                var existingCategory = await _catalogService.GetCatalogById(catalogId);
                if (existingCategory == null)
                {
                    return NotFound($"Kategori bulunamadı: {catalogId}");
                }

                await _catalogService.DeleteCatalog(catalogId);
                return NoContent(); // Başarılı silme işlemi için NoContent kullanılır
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}