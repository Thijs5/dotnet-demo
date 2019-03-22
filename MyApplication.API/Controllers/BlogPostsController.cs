using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyApplication.API.Models;

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
        #region SERVICES
        static int _blogPostId = 0;
        static List<BlogPost> _blogPosts = new List<BlogPost>()
        {
            new BlogPost() { Id = ++_blogPostId, Title = "Title 1", Text = "Text 1" },
            new BlogPost() { Id = ++_blogPostId, Title = "Title 2", Text = "Text 2" },
        };
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
            return _blogPosts;
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
            var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
            if (blogPost == null) {
                return NotFound();
            }
            return blogPost;
        }
        #endregion

        #region POST    /
        /// <summary>
        /// Create a new blog post.
        /// </summary>
        /// <param name="createBlogPost">Data for the new blog post</param>
        /// <returns>The newly created blog post</returns>
        /// <response code="200">Blog post succesfully created</response>
        /// <response code="400">Validation error</response>
        [HttpPost]
        public ActionResult<BlogPost> Create([FromBody] CreateBlogPost createBlogPost)
        {
            var newBlogPost = new BlogPost()
            {
                Id = ++_blogPostId,
                Title = createBlogPost.Title,
                Text = createBlogPost.Text,
            };
            _blogPosts.Add(newBlogPost);
            return newBlogPost;
        }
        #endregion

        #region PUT     /{id}
        /// <summary>
        /// Update an existing blog post.
        /// </summary>
        /// <param name="id">Id of the blog post to update</param>
        /// <param name="updatedBlogPost">New values for the blog post</param>
        /// <returns>Returns the updated blog post.</returns>
        /// <response code="200">Blog post succesfully updated</response>
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
                var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
                if (blogPost == null) { throw new Exception("NOT_FOUND"); }
                blogPost.Text = updatedBlogPost.Text;
                return blogPost;
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT_FOUND")
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
        /// <response code="200">Blog post succesfully deleted</response>
        /// <response code="404">No blog posts found with the given id</response>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
                if (blogPost == null) { throw new Exception("NOT_FOUND"); }
                _blogPosts.Remove(blogPost);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT_FOUND")
                {
                    return NotFound();
                }
                throw;
            }
        }
        #endregion
    }
}
