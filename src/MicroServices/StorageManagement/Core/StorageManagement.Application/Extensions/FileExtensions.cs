using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace StorageManagement.Application.Extensions
{
    public static class FileExtensions
    {
        
        public static string GetContentType(this IFormFile file)
        {
            return file.ContentType;
        }
        /// <summary>
        /// extract file extension from <see cref="IFormFile"/> input.
        /// </summary>
        /// <param name="file">the full name/path of the file</param>
        /// <returns>image.png => png</returns>
        public static string GetFileExtension(this IFormFile file)
        {
            string fileExtension;
            fileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
            return fileExtension;
        }
        public static string GetFileNameWithoutExtension(this IFormFile file)
        {
            return Path.GetFileNameWithoutExtension(file.FileName);
        }
        /// <summary>
        /// extract file extension via it's full name, for example: image.png => png
        /// </summary>
        /// <param name="fileName">the full name/path of the file</param>
        /// <returns>image.png => png</returns>
        public static string GetFileExtension(this string fileName)
        {
            string fileExtension;
            fileExtension = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
            return fileExtension;
        }
    }
}