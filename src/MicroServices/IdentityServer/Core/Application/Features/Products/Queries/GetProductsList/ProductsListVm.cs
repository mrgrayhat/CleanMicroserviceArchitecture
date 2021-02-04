using System.Collections.Generic;

namespace STS.Application.Features.Products.Queries.GetProductsList
{
    public class ProductsListVm
    {
        public IList<ProductDto> Products { get; set; }

        public bool CreateEnabled { get; set; }
    }
}
