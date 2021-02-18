using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Wrappers;

namespace StorageManagement.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentCommand : IRequest<Response<ItemDto>>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "at least 1 file is required")]
        public IFormFile File { get; set; }
        /// <summary>
        /// a nickname name
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "No More than 50 character are allowed")]
        public string Name { get; set; }
        public string ContentType { get; private set; }
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public string CreatedBy { get; private set; }
        public DateTime? LastModified { get; private set; }
        public string LastModifiedBy { get; private set; }
        [MaxLength(500, ErrorMessage = "No More that 500 character are allowed")]
        public string Description { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// total bytes of data
        /// </summary>
        public long Size { get; private set; }
        /// <summary>
        /// Unique Hash to protect against invalid/untracked changes
        /// </summary>
        public string VerifiedHash { get; private set; }

    }
}
