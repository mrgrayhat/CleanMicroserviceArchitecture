using System;
using BlogModule.Domain.Common;

namespace BlogModule.Domain.Entities
{
    public class Attachment //: AuditableBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public long Size { get; set; }
        public string Description { get; set; }

    }
}
