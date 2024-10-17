using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        /// <summary>
        /// Bütün ürünleri getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni bir ürün ekler.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }      

            try
            {
                await _productService.CreateProduct(productDto);
                return CreatedAtAction(nameof(GetByIdProduct), new { productId = productDto.Id }, productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Belirtilen id'ye ait ürünü getirir.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("GetByIdProduct/{productId}")]
        public async Task<IActionResult> GetByIdProduct(Guid productId)
        {
            try
            {
                var product = await _productService.GetProductById(productId);
                if (product == null)
                {
                    return NotFound($"Ürün bulunamadı: {productId}");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait kategorinin ürünlerini filtreler.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("filter-products-bycategory/{categoryId}")]
        public async Task<IActionResult> FilterProductsByCategory(Guid categoryId)
        {
            try
            {
                // Ürünleri al
                var products = await _productService.GetAllProducts();

                // Belirtilen kategoriye göre filtrele
                var filteredProducts = products.Where(p => p.CategoryId == categoryId).ToList();
               

                return Ok(filteredProducts);
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = $"Sunucu hatası: {ex.Message}" });
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait ürünü siler.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                var existingProduct = await _productService.GetProductById(productId);
                if (existingProduct == null)
                {
                    return NotFound($"Ürün bulunamadı: {productId}");
                }

                await _productService.DeleteProduct(productId);
                return NoContent(); // Başarılı silme işlemi için NoContent kullanılır
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait ürünü günceller.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingProduct = await _productService.GetProductById(productId);
                if (existingProduct == null)
                {
                    return NotFound($"Ürün bulunamadı: {productId}");
                }

                await _productService.UpdateProduct(productId, productDto);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}