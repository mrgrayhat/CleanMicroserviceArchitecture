using System.Collections.Generic;
using STS.Application.Features.Products.Queries.GetProductsFile;

namespace STS.Application.Abstractions
{
    public interface ICsvFileBuilder
    {
        byte[] BuildProductsFile(IEnumerable<ProductRecordDto> records);
    }
}
