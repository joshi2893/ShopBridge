using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopBridgeApi.Controllers;
using ShopBridgeApi.Models;
using ShopBridgeApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ShopBridge.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        private static Mock<IProductRepository> repositoryMock;
        private static Mock<ILogger<ProductController>> loggerMock;
        private static Mock<IMapper> mapperMock;
        
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            repositoryMock = new Mock<IProductRepository>();
            loggerMock = new Mock<ILogger<ProductController>>();
            mapperMock = new Mock<IMapper>();
        }
        
        [TestMethod]
        public void FetchingAllProducts_ExceptionOccurs_ServerErrorReturned()
        {
            // Arrange
         
            repositoryMock.Setup(x => x.Get()).Throws<Exception>();

            ProductController controller = new ProductController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

            // Act
            var result = (ObjectResult)controller.Get().Result;

            // Assert
            Assert.AreEqual(result.StatusCode, (int)HttpStatusCode.InternalServerError);
            repositoryMock.Verify(x => x.Get(), Times.Once);
        }
        [TestMethod]
        public void FetchingAllProducts_ReturnedListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name ="Product1", Price = 123 },
                new Product { Id = 2, Name ="Product2", Price = 1233 },
                new Product { Id = 3, Name ="Product3", Price = 1233 }

            };
            repositoryMock.Setup(x => x.Get()).Returns(Task.FromResult(products));

            ProductController controller = new ProductController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

            // Act
            var result = controller.Get().Result as OkObjectResult;

            // Assert
            var productList = result.Value as List<Product>;
            Assert.IsNotNull(productList);
            Assert.AreEqual(products.Count(), productList.Count());
            foreach (var product in productList)
            {
                var existing = products.FirstOrDefault(p => p.Id == product.Id);
                Assert.IsNotNull(existing);
                Assert.IsTrue(product.Equals(existing));
            }
        }

        [TestMethod]
        public void FetchProduct_UsingId()
        {
            // Arrange
            var product = new Product { Id = 2, Name = "Product2", Price = 1233 };

            repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult(product));
            ProductController controller = new ProductController(repositoryMock.Object, loggerMock.Object, mapperMock.Object);

            // Act
            var result = controller.Get(2).Result as OkObjectResult;

            // Assert
            var returnProduct = result.Value as Product;
            Assert.IsNotNull(returnProduct);
            Assert.AreEqual(product.Name, returnProduct.Name);
            Assert.AreEqual(product.Price, returnProduct.Price);

        }

    }
}
