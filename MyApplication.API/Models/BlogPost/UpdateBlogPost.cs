using System.ComponentModel.DataAnnotations;

namespace MyApplication.API.Models
{
    /// <summary>
    /// Update an existing blog post.
    /// </summary>
    public class UpdateBlogPost
    {
        /// <summary>
        /// The text of the blog post.
        /// </summary>
        [Required]
        public string Text { get; set; }
    }
}