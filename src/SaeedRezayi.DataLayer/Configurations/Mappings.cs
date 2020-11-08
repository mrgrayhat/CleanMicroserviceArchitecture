using Microsoft.EntityFrameworkCore;

namespace SaeedRezayi.DataLayer.Configurations
{
    public static class Mappings
    {
        public static void AddCustomMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Mappings).Assembly);

        }
    }
}
