using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambalajliyo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Bütün haberleri getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPost")]
        public async Task<IActionResult> GetAllPost()
        {
            try
            {
                var posts = await _postService.GetAllPost();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni bir haber ekler.
        /// </summary>
        /// <param name="postDto"></param>
        /// <returns></returns>
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _postService.CreatePost(postDto);
                return CreatedAtAction(nameof(GetByIdPost), new { postId = postDto.Id }, postDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait haberi getirir.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("GetByIdPost/{postId}")]
        public async Task<IActionResult> GetByIdPost(Guid postId)
        {
            try
            {
                var post = await _postService.GetPostById(postId);
                if (post == null)
                {
                    return NotFound($"Gönderi bulunamadı: {postId}");
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait haberi siler.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete("DeletePost/{postId}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            try
            {
                var existingPost = await _postService.GetPostById(postId);
                if (existingPost == null)
                {
                    return NotFound($"Gönderi bulunamadı: {postId}");
                }

                await _postService.DeletePost(postId);
                return NoContent(); // Başarılı silme işlemi için NoContent kullanılır
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen id'ye ait haberi günceller.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="postDto"></param>
        /// <returns></returns>
        [HttpPut("UpdatePost/{postId}")]
        public async Task<IActionResult> UpdatePost(Guid postId, [FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingPost = await _postService.GetPostById(postId);
                if (existingPost == null)
                {
                    return NotFound($"Gönderi bulunamadı: {postId}");
                }

                await _postService.UpdatePost(postId, postDto);
                return Ok(postDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}