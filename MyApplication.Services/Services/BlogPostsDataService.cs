using System.Collections.Generic;
using System.Linq;
using MyApplication.Core.Exceptions;
using MyApplication.Services.Models;

namespace MyApplication.Services.Services
{
    /// <summary>
    /// Dataservice for blog posts.
    /// </summary>
    public interface IBlogPostsDataService : IDataService
    {
        /// <summary>
        /// Get a list of all blog posts.
        /// </summary>
        List<BlogPost> GetAll();

        /// <summary>
        /// Get a blog post by its id.
        /// </summary>
        /// <param name="id">Id of the blog post to fetch.</param>
        BlogPost GetById(int id);

        /// <summary>
        /// Create a new blog post.
        /// </summary>
        /// <param name="newBlogPost">Data for the new blog post.</param>
        /// <returns>The newly created blog post.</returns>
        BlogPost Create(CreateBlogPost newBlogPost);

        /// <summary>
        /// Update a blog post.
        /// </summary>
        /// <param name="id">Id of the blog post to update.</param>
        /// <param name="updatedBlogPost">Updated data for the blog post.</param>
        /// <exception cref="MyApplication.Core.Exceptions.EntityNotFoundException">Thrown when no blog post is found with the given id.</exception>
        BlogPost Update(int id, UpdateBlogPost updatedBlogPost);

        /// <summary>
        /// Delete a blog post.
        /// </summary>
        /// <param name="id">Id of the blog post to delete.</param>
        /// <exception cref="MyApplication.Core.Exceptions.EntityNotFoundException">Thrown when no blog post is found with the given id.</exception>
        void Delete(int id);
    }

    /// <inheritdoc />
    public class BlogPostsDataService : IBlogPostsDataService
    {
        static int _blogPostId = 0;
        static List<BlogPost> _blogPosts = new List<BlogPost>()
        {
            new BlogPost() { Id = ++_blogPostId, Title = "Title 1", Text = "Text 1" },
            new BlogPost() { Id = ++_blogPostId, Title = "Title 2", Text = "Text 2" },
        };

        /// <inheritdoc />
        public BlogPost Create(CreateBlogPost newBlogPostData)
        {
            var newBlogPost = new BlogPost()
            {
                Id = ++_blogPostId,
                Title = newBlogPostData.Title,
                Text = newBlogPostData.Text,
            };
            _blogPosts.Add(newBlogPost);
            return newBlogPost;
        }

        /// <inheritdoc />
        public void Delete(int id)
        {
            var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
            if (blogPost == null) {
                throw new EntityNotFoundException($"Can't find a blog post with id '{id}'");
            }
            _blogPosts.Remove(blogPost);
        }

        /// <inheritdoc />
        public List<BlogPost> GetAll()
        {
            return _blogPosts;
        }

        /// <inheritdoc />
        public BlogPost GetById(int id)
        {
            var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
            return blogPost;
        }

        /// <inheritdoc />
        public BlogPost Update(int id, UpdateBlogPost updatedBlogPost)
        {
            var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
            if (blogPost == null) {
                throw new EntityNotFoundException($"Can't find a blog post with id '{id}'");
            }
            blogPost.Text = updatedBlogPost.Text;
            return blogPost;
        }
    }
}