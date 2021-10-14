using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagement.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/products
        [HttpGet]
        public IEnumerable<ProductDto> GetProducts()
        {
            var products = repository.GetProducts().Select(product => ProductToDTO(product));

            foreach (var product in products)
            {
                if (product.Shops.Contains("None"))
                {
                    product.Shops.Remove("None");
                }
            }
            return products;
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProduct(long id)
        {
            var product = repository.GetProduct(id);

            if (product is null)
            {
                return NotFound();
            }

            if (product.Shops.Contains("None"))
            {
                product.Shops.Remove("None");
            }

            return ProductToDTO(product);
        }

        // POST api/products
        /* Create a new product to be inserted into the database, check the input info and 
         * according to the given shopIds add shopIds and the productId into the junction table. 
         */
        [HttpPost]
        public ActionResult<ProductDto> CreateProduct(CreateProductDto productDto)
        {
            if (!isValidGroupId(productDto.GroupId) || !isValidShopIds(productDto.ShopId))
            {
                return BadRequest();
            }

            if (productDto.Price * (productDto.VatPercentage / 100 + 1) != productDto.VatPrice)
            {
                if (productDto.VatPercentage == 0)
                {
                    productDto.VatPercentage = (long)(100 - (productDto.Price / productDto.VatPrice * 100));
                }
                else if (productDto.Price == 0)
                {
                    productDto.Price = (decimal)(100 - productDto.VatPercentage) / 100 * productDto.VatPrice;
                }
                else if (productDto.VatPrice == 0)
                {
                    productDto.VatPrice = productDto.Price / ((decimal)(100 - productDto.VatPercentage) / 100);
                }
            }

            Product product = new()
            {
                Name = productDto.Name,
                GroupId = productDto.GroupId,
                CreatedDate = System.DateTime.UtcNow,
                Price = productDto.Price,
                VatPrice = productDto.VatPrice,
                VatPercentage = productDto.VatPercentage
            };

            repository.CreateProduct(product);

            long createdProductId = repository.LastInsertId();

            if (productDto.ShopId != null && productDto.ShopId.Any())
            {
                repository.AddProductToShops(createdProductId, productDto.ShopId);
            }
            else
            {
                repository.AddProductToShops(createdProductId, new List<int>() { 4 });
            }

            return CreatedAtAction(nameof(GetProduct), new { id = createdProductId }, GetProduct(createdProductId).Value);
        }

        private static ProductDto ProductToDTO(Product product) =>
            new ProductDto
            {
                Name = product.Name,
                GroupName = product.GroupName,
                CreatedDate = product.CreatedDate,
                Price = product.Price,
                VatPrice = product.VatPrice,
                VatPercentage = product.VatPercentage,
                Shops = product.Shops
            };

        public bool isValidGroupId(long id)
        {
            if (repository.GetGroup(id) == null)
            {
                return false;
            }
            return true;
        }

        public bool isValidShopIds(List<int> ids)
        {
            foreach (var id in ids)
            {
                if (repository.GetShop(id) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}