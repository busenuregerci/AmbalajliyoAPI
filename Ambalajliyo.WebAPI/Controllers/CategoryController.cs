using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.WebAPI.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;

namespace Ambalajliyo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Bütün kategorileri getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni bir kategori ekller.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tüm kategorileri alıyoruz ve kategori adının mevcut olup olmadığını kontrol ediyoruz
                var allCategories = await _categoryService.GetAllCategories();
                if (allCategories.Any(c => c.Name.Equals(categoryDto.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    // Aynı ada sahip kategori varsa 400 Bad Request dönüyoruz
                    return BadRequest(new { message = "Kategori adı zaten mevcuttur!" });
                }
                await _categoryService.CreateCategory(categoryDto);
                return CreatedAtAction(nameof(GetByIdCategory), new { categoryId = categoryDto.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait kategoriyi getirir.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("GetByIdCategory/{categoryId}")]
        public async Task<IActionResult> GetByIdCategory(Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(categoryId);
                if (category == null)
                {
                    return BadRequest($"Kategori bulunamadı: {categoryId}");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait kategoriyi siler.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                var existingCategory = await _categoryService.GetCategoryById(categoryId);
                if (existingCategory == null)
                {
                    return NotFound($"Kategori bulunamadı: {categoryId}");
                }

                await _categoryService.DeleteCategory(categoryId);
                return NoContent(); // Başarılı silme işlemi için NoContent kullanılır
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait kategoriyi günceller.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingCategory = await _categoryService.GetCategoryById(categoryId);
                if (existingCategory == null)
                {
                    return NotFound($"Kategori bulunamadı: {categoryId}");
                }

                await _categoryService.UpdateCategory(categoryId, categoryDto);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}