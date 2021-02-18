using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StorageManagement.Application.Extensions
{
    public static class FileSecurityExtensions
    {
        public static string CalculateMD5ByteHashAsync(this byte[] bytes)
        {
            using var md5 = MD5.Create();
            return Encoding.ASCII.GetString(md5.ComputeHash(bytes));
        }

        public static string CalculateMD5Hash(this string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
        public static async Task<string> CalculateFileHashAsync(this string filePath, CancellationToken cancellationToken)
        {
            using FileStream fStream = File.OpenRead(filePath);

            return await GetHashAsync<MD5>(fStream, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<bool> CalculateIntegrityAsync(this IFormFile file, string hash,
            CancellationToken cancellationToken)
        {
            using var fStream = file.OpenReadStream();
            var hashed = await GetHashAsync<MD5>(fStream, cancellationToken).ConfigureAwait(false);

            return Equals(hashed,hash);
        }
        public static async Task<string> CalculateSHA256FileHashAsync(this IFormFile file, CancellationToken cancellationToken)
        {
            using var fStream = file.OpenReadStream();
            return await GetHashAsync<SHA256>(fStream, cancellationToken).ConfigureAwait(false);
        }
        public static async Task<string> CalculateMD5FileHashAsync(this IFormFile file, CancellationToken cancellationToken)
        {
            using var fStream = file.OpenReadStream();
            return await GetHashAsync<MD5>(fStream, cancellationToken).ConfigureAwait(false);
        }


        public static async Task<string> GetHashAsync<T>(Stream stream, CancellationToken cancellationToken) where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();
            MethodInfo create = typeof(T).GetMethod("Create", Array.Empty<Type>());
            using T crypt = (T)create.Invoke(null, null);

            byte[] hashBytes = await crypt.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
