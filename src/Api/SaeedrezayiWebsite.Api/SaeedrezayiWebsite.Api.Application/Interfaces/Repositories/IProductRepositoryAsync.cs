using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SaeedrezayiWebsite.Api.Domain.Entities;

namespace SaeedrezayiWebsite.Api.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
