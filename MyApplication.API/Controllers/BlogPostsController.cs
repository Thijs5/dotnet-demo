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
        private readonly List<BlogPost> _blogPosts = new List<BlogPost>()
        {
            new BlogPost() { Id = 1, Title = "Title 1", Text = "Text 1" },
            new BlogPost() { Id = 2, Title = "Title 2", Text = "Text 2" },
        };

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
    }
}