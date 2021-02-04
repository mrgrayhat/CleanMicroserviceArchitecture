using BlogModule.Domain.Common;

namespace BlogModule.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string ByIP { get; set; }
        public string ByAgent { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
