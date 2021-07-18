using ShopBridgeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Repositories
{
    public interface IProductRepository : IDisposable
    {
        Task<Product> Add(Product product);
        Task<Product> Get(int id);
        Task<List<Product>> Get();
        Task<Product> Update(Product product);
        Task<Product> Delete(int id);

    }
}
