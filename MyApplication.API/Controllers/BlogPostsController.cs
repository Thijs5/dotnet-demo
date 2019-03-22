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
        [HttpGet]
        public ActionResult<IEnumerable<BlogPost>> Get()
        {
            return _blogPosts;
        }
        #endregion
        
        #region GET     /{id}
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
        [HttpPost]
        public void Create([FromBody] CreateBlogPost createBlogPost)
        {
            var newBlogPost = new BlogPost()
            {
                Id = ++_blogPostId,
                Title = createBlogPost.Title,
                Text = createBlogPost.Text,
            };
            _blogPosts.Add(newBlogPost);
        }
        #endregion

        #region PUT     /{id}
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
                return BadRequest();
            }
        }
        #endregion
    }
}
