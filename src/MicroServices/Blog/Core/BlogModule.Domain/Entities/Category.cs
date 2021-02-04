using System.Collections.Generic;
using BlogModule.Domain.Common;

namespace BlogModule.Domain.Entities
{
    public class Category : AuditableBaseEntity
    {
        public Category()
        {
            SubCategories = new HashSet<Category>();

        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public Category Parent { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
    }
}
