using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace StorageManagement.Application.Extensions
{
    public static class FileExtensions
    {
        private static readonly IDictionary<string, string> ValidFileMimesDictionary = new Dictionary<string, string>
        {
			// Images
			{ ".bmp", "image/bmp" },
            { ".dib", "image/bmp" },
            { ".gif", "ima	ge/gif" },
            { ".svg", "image/svg+xml" },
            { ".jpe", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".pnz", "image/png" },
			// MultitMedia
			{ ".mp3", "audio/mpeg3" },
            { ".mp4", "video/mpeg" },
			// Documents
			{ ".txt", "file/text" },
            { ".pdf", "document/pdf" },
            { ".doc", "document/doc" },
            { ".docx", "document/docx" },
			// files
			{ ".zip", "archive/zip" },
            { ".rar", "archive/winrar" },
            { ".tar", "archive/tarball" },
            { ".tar.gz", "archive/tarballgzip" },
            { ".gz", "archive/gzip" },
            // executables
            {".bat","" },
            {".cmd","" },
            {".sh","" },
            {".vb","" },
            {".vbs","" },
            {".cs","" },
            {".php","" },
            {".py","" },
            {".o","" },
            {".c","" },
            {".dll","" },
            {".js","" },
            {".exe","binary/executable" },
			// other
			
		};
        public static bool IsValidImage(this IFormFile file)
        {
            try
            {

                // valid image stream
                return true;
            }
            catch
            {
                // not an image

                return false;
            }
        }

        public static bool IsValid(this IFormFile file)
        {
            if (file.Length <= 0)
                throw new ArgumentNullException(nameof(file));

            var extension = Path.GetExtension(file.Name);
            var contentType = file.ContentType;

            return ValidFileMimesDictionary.ContainsKey(extension.ToLower())
                   && ValidFileMimesDictionary.TryGetValue(contentType, out _);
        }

        public static string GetContentType(this IFormFile file)
        {
            return file.ContentType;
        }
    }
}
