
using System.Threading.Tasks;

namespace SaeedRezayi.Services.Core
{
    public interface IDbInitializerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize(bool isTest = false);

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        Task SeedData();
        Task SeedExtraData(int amount = 10);
    }
}
