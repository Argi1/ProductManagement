using ProductManagement.Models;
using System.Collections.Generic;

namespace ProductManagement.Repositories
{
    public interface IProductRepository
    {
        Product GetProduct(long id);
        IEnumerable<Product> GetProducts();

        void CreateProduct(Product product);

        void AddProductToShops(long id, List<int> shopId);

        long LastInsertId();

        Group GetGroup(long id);
        IEnumerable<Group> GetGroups();
        Shop GetShop(long id);

    }
}