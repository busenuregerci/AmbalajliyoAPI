using Ambalajliyo.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.BLL.Interfaces
{
    /// <summary>
    /// Defines the contract for post-related operations in the business logic layer.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Retrieves a list of all posts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="PostDto"/> objects.</returns>
        Task<List<PostDto>> GetAllPost();

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="postDto">The post data to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="PostDto"/> object.</returns>
        Task<PostDto> CreatePost(PostDto postDto);

        /// <summary>
        /// Deletes a post by its identifier.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeletePost(Guid postId);

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to be updated.</param>
        /// <param name="postDto">The updated post data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdatePost(Guid postId, PostDto postDto);

        /// <summary>
        /// Retrieves a specific post by its identifier.
        /// </summary>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="PostDto"/> object with the specified identifier.</returns>
        Task<PostDto> GetPostById(Guid postId);
    }
}
