using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Application.DTOs.Conversions
{
    public static class ProductConversions
    {
        public static Product ToEntity(ProductDTO product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            var singleProduct = product is null ? null : new ProductDTO(product.Id, product.Name!, product.Quantity, product.Price);
            var productList = products is null ? null : products?.Select(pDTO => new ProductDTO(pDTO.Id, pDTO.Name!, pDTO.Quantity, pDTO.Price)).ToList();

            return (singleProduct, productList);
        }
    }
}
