using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace StorageManagement.Application.DTOs
{
    /// <summary>
    /// An Storage Item that stored in FileSystem and indexed in database.
    /// </summary>
    public class ItemDto
    {
        /// <summary>
        /// Unique Identify Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// an title/name. can be empty
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// creator identity
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// editor identity
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// first creation time
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// last update time
        /// </summary>
        public DateTime? LastModified { get; set; }
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
        public ContentType ContentType { get; set; }
        /// <summary>
        /// total download count
        /// </summary>
        public long Downloaded { get; set; }
        /// <summary>
        /// Unique Hash to protect against invalid/untracked changes
        /// </summary>
        public string VerifiedHash { get; set; }
        public IFormFile File { get; set; }
    }
}
