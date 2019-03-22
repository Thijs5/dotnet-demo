using System.ComponentModel.DataAnnotations;

namespace MyApplication.API.Models
{
    /// <summary>
    /// Create a new blog post.
    /// </summary>
    public class CreateBlogPost
    {
        /// <summary>
        ///  Title of the blog post.
        /// </summary>
        [Required]
        [MaxLength(40)]
        public string Title { get; set; }

        /// <summary>
        /// The text of the blog post.
        /// </summary>
        [Required]
        public string Text { get; set; }
    }
}