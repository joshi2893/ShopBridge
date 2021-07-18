using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridgeApi.Models;
using ShopBridgeApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Tests.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {

        private IProductRepository GetRepository()
        {
            ShopBridgeDbContext dbContext = new ShopBridgeDbContext(
                                                    new DbContextOptionsBuilder<ShopBridgeDbContext>()
                                                    .UseSqlite("DataSource =:memory:")
                                                    .Options
                                                );
            IProductRepository repository = new ProductRepository(dbContext);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            if (dbContext.Products.Count() > 0)
            {
                dbContext.Products.RemoveRange(dbContext.Products);
                dbContext.SaveChanges();
            }
            dbContext.AddRange(products);
            dbContext.SaveChanges();

            return repository;
        }

        [TestMethod]
        public void Adding_ValidProduct_IsAddedSuccessfully()
        {
            // Arrange
            Product product = new Product()
            {
                Name = "Product1001",
                Price = 10101.11
            };
            IProductRepository repository = GetRepository();

            // Act
            var result = repository.Add(product).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != 0);
            Assert.IsTrue(product.Equals(result));

            repository.Dispose();
        }

        [TestMethod]
        public void Adding_NonValidProduct_ShouldFail()
        {
            // Arrange
            Product product = new Product()
            {
                Price = 10101.11,
                Description = "description"
            };
            IProductRepository repository = GetRepository();

            // Act
            var result = repository.Add(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            repository.Dispose();
        }

        [TestMethod]
        public void Fetching_ValidProduct()
        {
            // Arrange
            var id = 1;
            IProductRepository repository = GetRepository();

            // Act
            var result = repository.Get(id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Product1", result.Name);
            Assert.AreEqual(1, result.Id);
            repository.Dispose();
        }

        [TestMethod]
        public void Fetching_NonValidProduct()
        {
            // Arrange
            var id = 5;
            IProductRepository repository = GetRepository();

            // Act
            var result = repository.Get(id).Result;

            // Assert
            Assert.IsNull(result);
            repository.Dispose();
        }

        [TestMethod]
        public void Fetching_AllProducts()
        {
            // Arrange
            IProductRepository repository = GetRepository();

            // Act
            var result = repository.Get().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            repository.Dispose();
        }

        [TestMethod]
        public void Update_ProductWithValidData()
        {
            // Arrange
            IProductRepository repository = GetRepository();
            var product = new Product
            {
                Id = 1,
                Name = "Product11"
            };

            // Act
            var result = repository.Update(product).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Product11", result.Name);
            repository.Dispose();
        }

        [TestMethod]
        public void Update_ProductWith_NonValidId()
        {
            // Arrange
            IProductRepository repository = GetRepository();
            var product = new Product
            {
                Id = 5,
                Name = "Product11"
            };


            // Act
            var result = repository.Update(product).Result;

            // Assert
            Assert.IsNull(result);
            repository.Dispose();
        }

        [TestMethod]
        public void Update_ProductWith_NonValidData()
        {
            // Arrange
            IProductRepository repository = GetRepository();
            var product = new Product
            {
                Id = 1,
            };


            // Act
            var result = repository.Update(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            repository.Dispose();
        }

        [TestMethod]
        public void Delete_ProductWith_ValidId()
        {
            // Arrange
            IProductRepository repository = GetRepository();
            var id = 1;

            // Act
            var result = repository.Delete(id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Product1", result.Name);
            Assert.AreEqual(1, result.Id);
            repository.Dispose();
        }

        [TestMethod]
        public void Delete_ProductWith_NonValidId()
        {
            // Arrange
            IProductRepository repository = GetRepository();
            var id = 5;

            // Act
            var result = repository.Delete(id).Result;

            // Assert
            Assert.IsNull(result);
            repository.Dispose();
        }


        #region TestData

        private static List<Product> products => new List<Product>
        {
            new Product()
            {
                Name = "Product1",
                Price = 1.0,
            },
            new Product()
            {
                Name = "Product2",
                Price = 1.0,
            },
            new Product()
            {
                Name = "Product3",
                Price = 1.0,
            },
        };

        #endregion

    }
}
