﻿using System.Threading.Tasks;
using System.Collections.Generic;

namespace StorageManagement.Application.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        ValueTask<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsNoTrackingAsync();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
