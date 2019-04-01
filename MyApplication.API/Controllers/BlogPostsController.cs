using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using MyApplication.API.Models;
using MyApplication.Core.Exceptions;
using MyApplication.Services;
using DTO = MyApplication.Services.Models;

namespace MyApplication.API.Controllers
{
    /// <summary>
    /// Endpoint for blog posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BlogPostsController : ControllerBase
    {
        /// <summary>
        /// Constructor containing all required dependencies.
        /// </summary>
        /// <param name="blogPostsDataService">An instance of a BlogPostsDataService.</param>
        public BlogPostsController(IBlogPostsDataService blogPostsDataService)
        {
            _blogPostsDataService = blogPostsDataService;
        }

        #region SERVICES
        private readonly IBlogPostsDataService _blogPostsDataService;
        #endregion

        #region GET     /
        /// <summary>
        /// Get a list of all blog posts.
        /// </summary>
        /// <returns>A list of all blog posts.</returns>
        /// <response code="200">Returns the list of all blog posts</response>
        [HttpGet]
        public ActionResult<IEnumerable<BlogPost>> GetAll()
        {
            var blogPosts = _blogPostsDataService.GetAll();
            // return blogPosts;
            return null;
        }
        #endregion
        
        #region GET     /{id}
        /// <summary>
        /// Get a single blog post.
        /// </summary>
        /// <param name="id">Id of the blog post</param>
        /// <returns>A single blog post.</returns>
        /// <response code="200">Returns a single blog post</response>
        /// <response code="404">No blog post found for the given id</response>
        [HttpGet("{id}")]
        public ActionResult<BlogPost> Get(int id)
        {
            var blogPost = _blogPostsDataService.GetById(id);
            if (blogPost == null) {
                return NotFound();
            }
            // return blogPost;
            return null;
        }
        #endregion

        #region POST    /
        /// <summary>
        /// Create a new blog post.
        /// </summary>
        /// <param name="createBlogPost">Data for the new blog post</param>
        /// <returns>The newly created blog post</returns>
        /// <response code="200">Blog post successfully created</response>
        /// <response code="400">Validation error</response>
        [HttpPost]
        public ActionResult<BlogPost> Create([FromBody] CreateBlogPost createBlogPost)
        {
            var newBlogPost = new DTO.CreateBlogPost()
            {
                Title = createBlogPost.Title,
                Text = createBlogPost.Text,
            };
            var createdBlogPost = _blogPostsDataService.Create(newBlogPost);
            // return createdBlogPost;
            return null;
        }
        #endregion

        #region PUT     /{id}
        /// <summary>
        /// Update an existing blog post.
        /// </summary>
        /// <param name="id">Id of the blog post to update</param>
        /// <param name="updatedBlogPost">New values for the blog post</param>
        /// <returns>Returns the updated blog post.</returns>
        /// <response code="200">Blog post successfully updated</response>
        /// <response code="400">Validation error</response>
        /// <response code="404">No blog posts found with the given id</response>
        [HttpPut("{id}")]
        public ActionResult<BlogPost> Update(
            int id,
            [FromBody] UpdateBlogPost updatedBlogPost
        )
        {
            try
            {
                var updateBlogPostDto = new DTO.UpdateBlogPost()
                {
                    Text = updatedBlogPost.Text,
                };
                var blogPost = _blogPostsDataService.Update(id, updateBlogPostDto);
                // return blogPost;
                return null;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(EntityNotFoundException))
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
        #endregion

        #region DELETE  /{id}
        /// <summary>
        /// Delete an existing blog post.
        /// </summary>
        /// <param name="id">Id of the blog post to delete</param>
        /// <response code="200">Blog post successfully deleted</response>
        /// <response code="404">No blog posts found with the given id</response>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _blogPostsDataService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(EntityNotFoundException))
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
        #endregion
    }
}
