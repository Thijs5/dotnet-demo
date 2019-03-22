namespace MyApplication.API.Models
{
    /// <summary>
    /// A blog post entity.
    /// </summary>
    public class BlogPost
    {
        /// <summary>
        /// Id of the blog post.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Title of the blog post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The text of the blog post.
        /// </summary>
        public string Text { get; set; }
    }
}