using System.Text;
using System.Text.RegularExpressions;

namespace SaeedRezayi.Common
{
    public static class SeoExtensions
    {
        /// <summary>
        /// Generate Seo Friendly title
        /// </summary>
        private const int MaxLenghtSlug = 45;
        public static string GenerateSlug(string title)
        {
            var slug = RemoveAccent(title).ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9-\u0600-\u06FF]", "-");
            slug = Regex.Replace(slug, @"\s+", "-").Trim();
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Substring(0, slug.Length <= MaxLenghtSlug ? slug.Length : MaxLenghtSlug).Trim();

            return slug;
        }

        private static string RemoveAccent(string text)
        {
            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}

