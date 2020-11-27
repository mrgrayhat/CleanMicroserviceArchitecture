using System;
using System.ComponentModel.DataAnnotations;

namespace SaeedRezayi.DomainClasses.Common
{
    public class CommonEntityProperties<T>
    {
        /// <summary>
        /// pk
        /// </summary>
        [Key]
        public virtual T Id { get; set; }
        /// <summary>
        /// create datetime
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        /// <summary>
        /// last update datetime
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
