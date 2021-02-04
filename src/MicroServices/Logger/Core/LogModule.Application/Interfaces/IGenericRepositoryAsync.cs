using System.Threading.Tasks;
using System.Collections.Generic;
using LogModule.Application.Enums;

namespace LogModule.Application.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<LogModule.Domain.Entities.Log> GetByIdAsync(string id);
        Task<IReadOnlyList<LogModule.Domain.Entities.Log>> GetAllAsync();
        Task<IReadOnlyList<LogModule.Domain.Entities.Log>> GetPagedReponseAsync(int pageNumber, int pageSize, LogLevels logLevel = LogLevels.ALL, string sorOrder = "desc");
        Task<LogModule.Domain.Entities.Log> AddAsync(LogModule.Domain.Entities.Log entity);
        Task DeleteAsync(LogModule.Domain.Entities.Log entity);
    }
}
