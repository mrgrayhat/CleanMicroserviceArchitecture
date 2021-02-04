using System.Linq;
using System.Threading.Tasks;
using BlogModule.Domain.Entities;
using BlogModule.Infrastructure.Contexts;

namespace BlogModule.Infrastructure.Seeds
{
    public static class DefaultCultures
    {
        public static async Task SeedDefaultCulturesAsync(this BlogDbContext blogDbContext)
        {

            var cultureEnglish = new Culture
            {
                Code = "en-us",
                DisplayName = "English-UnitedState"
            };
            var cultureFarsi = new Culture
            {
                Code = "fa-ir",
                DisplayName = "Farsi-Iran"
            };
            var cultureArabic = new Culture
            {
                Code = "ar-sa",
                DisplayName = "Arabic"
            };
            //var cultureChina = new Culture
            //{
            //    Code = "ch-",
            //    DisplayName = "China"
            //};

            if (!blogDbContext.Cultures.Any(u => u.Code == cultureEnglish.Code))
            {
                await blogDbContext.AddAsync(cultureEnglish);
                await blogDbContext.SaveChangesAsync();
            }
            if (!blogDbContext.Cultures.Any(u => u.Code == cultureFarsi.Code))
            {
                await blogDbContext.AddAsync(cultureFarsi);
                await blogDbContext.SaveChangesAsync();
            }
            if (!blogDbContext.Cultures.Any(u => u.Code == cultureArabic.Code))
            {
                await blogDbContext.AddAsync(cultureArabic);
                await blogDbContext.SaveChangesAsync();
            }
        }
    }
}
