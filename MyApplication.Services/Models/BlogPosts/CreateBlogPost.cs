namespace MyApplication.Services.Models
{
    /// <summary>
    /// A blog post entity.
    /// </summary>
    public class CreateBlogPost
    {
        /// <summary>
        /// Title of the blog post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The text of the blog post.
        /// </summary>
        public string Text { get; set; }
    }
}