namespace MyApplication.Services.Models
{
    /// <summary>
    /// A base entity holds all the fields every entity in the database holds.
    /// This can be just an ID, or maybe a CreatedAt-field.
    /// </summary>
    /// <typeparam name="TKey">Type of the id of the entity</typeparam>
    public abstract class BaseEntity<TKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public TKey Id { get; set; }
    }
}