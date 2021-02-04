using System.Threading.Tasks;

namespace LogModule.Application.Interfaces.Repositories
{
    public interface ILogRepositoryAsync : IGenericRepositoryAsync<Domain.Entities.Log>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
