using Microsoft.EntityFrameworkCore;
using ShopBridgeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopBridgeDbContext _context;
        public ProductRepository(ShopBridgeDbContext context)
        {
            _context = context;
        }
        public async Task<Product> Add(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Product> Delete(int id)
        {
            try
            {
                var product = await Get(id);
                if (product == null)
                    return product;
                var res = _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            this._context.Database.CloseConnection();
            this._context.Dispose();
        }

        public async Task<Product> Get(int id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Product>> Get()
        {
            try
            {
                return await Task.Run(() => _context.Products.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product> Update(Product product)
        {
            try
            {
                var existingProduct = await Get(product.Id);
                if (existingProduct == null)
                    return null;
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
