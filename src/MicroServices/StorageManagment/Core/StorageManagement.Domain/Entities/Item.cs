using StorageManagement.Domain.Common;

namespace StorageManagement.Domain.Entities
{
    /// <summary>
    /// An Storage Item that stored in FileSystem and indexed in database.
    /// </summary>
    public class Item: AuditableBaseEntity
    {
        /// <summary>
        /// an title/name. can be empty
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// a short text to describe content
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// total bytes of data
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Type of file/data
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// total download count
        /// </summary>
        public long Downloaded { get; set; } = 0;
        /// <summary>
        /// Unique Hash to protect against invalid/untracked changes
        /// </summary>
        public string VerifiedHash { get; set; }
    }
}
