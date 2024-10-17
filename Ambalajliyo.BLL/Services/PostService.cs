using Ambalajliyo.BLL.DTOs;
using Ambalajliyo.BLL.Interfaces;
using Ambalajliyo.DAL.AbstractRepository;
using Ambalajliyo.DAL.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Services
{
    /// <summary>
    /// Provides implementation for post-related operations defined in the <see cref="IPostService"/> interface.
    /// </summary>
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="postRepository">The repository to handle post data access.</param>
        public PostService(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="postDto">The post data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="PostDto"/> object.</returns>
        public async Task<PostDto> CreatePost(PostDto postDto)
        {
            var post = postDto.Adapt<Post>(); // Map DTO to entity
            await _postRepository.AddAsync(post);
            // Map entity back to DTO
            return post.Adapt<PostDto>();
        }

        /// <summary>
        /// Deletes a post by its identifier.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeletePost(Guid postId)
        {
            await _postRepository.DeleteAsync(postId);
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="PostDto"/> objects.</returns>
        public async Task<List<PostDto>> GetAllPost()
        {
            var posts = await _postRepository.GetAllAsync();
            // Map list of entities to list of DTOs
            return posts.Adapt<List<PostDto>>();
        }

        /// <summary>
        /// Retrieves a specific post by its identifier.
        /// </summary>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="PostDto"/> object with the specified identifier.</returns>
        public async Task<PostDto> GetPostById(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            // Map entity to DTO
            return post.Adapt<PostDto>();
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to be updated.</param>
        /// <param name="postDto">The updated post data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdatePost(Guid postId, PostDto postDto)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            // Update post properties
            post.Id = postDto.Id;
            post.Title = postDto.Title;
            post.Info = postDto.Info;
            post.IsPublished = postDto.IsPublished;
            post.Image = postDto.Image;

            await _postRepository.UpdateAsync(post);
        }
    }
}
