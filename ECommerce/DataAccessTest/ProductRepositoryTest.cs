using DataAccess.Context;
using DataAccess.Repositories;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using RepositoryInterface;

namespace DataAccessTest
{
    [TestClass]
    public class ProductRepositoryTest
    {
        private Mock<ECommerceContext> _ecommerceContext;
        private IProductRepository _productRepository;

        private Product product1;
        private Product product2;
        private Product product3;

        [TestInitialize] public void Setup() 
        {
            _ecommerceContext = new Mock<ECommerceContext>();
            _productRepository = new ProductRepository(_ecommerceContext.Object);

            Color color1 = new Color() { colorName = "Test" };

            Brand brand = new Brand() { brandName = "Test" };
            Category category = new Category() { categoryName = "Test1" };

            product1 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Test1",
                Price = 1,
                Stock = 10,
                Brand = brand,
                Category = category,
                Colors = new List<Color> { color1 },
                Description = "Test",
                PromotionExcluded = true
            };
            product2 = new Product()
            {
                Name = "Test2",
                Price = 2,
                Stock = 20,
                Brand = new Brand() { brandName = "Test2" },
                Category = new Category() { categoryName = "Test2" },
                Colors = new List<Color> { color1 },
                Description = "Test",
                PromotionExcluded = false
            };
            product3 = new Product()
            {
                Name = "Test3",
                Price = 3,
                Stock = 30,
                Brand = brand,
                Category = category,
                Colors = new List<Color> { color1 },
                Description = "Test",
                PromotionExcluded = false
            };
        }

        [TestMethod]
        public void CreateProductOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products).ReturnsDbSet(new List<Product>() { product1 });

            // Act
            Product actualReturn = _productRepository.AddProduct(product1);

            // Assert
            Assert.IsTrue(product1.Equals(actualReturn));
        }

        [TestMethod]
        public void GetAllProductsOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts(null, null, null, null, null, null);

            // Assert
            Assert.IsTrue(actualReturn.Contains(product1) && actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetAllProductsByCategoryOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts("Test1", null, null, null, null, null);

            // Assert
            Assert.IsTrue(actualReturn.Contains(product1) && !actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetAllProductsByBrandOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts(null, "Test", null, null, null, null);

            // Assert
            Assert.IsTrue(actualReturn.Contains(product1) && !actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetAllProductsByNameFilterOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts(null, null, "1", null, null, null);

            // Assert
            Assert.IsTrue(actualReturn.Contains(product1) && !actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetAllProductsByMinPriceOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts(null, null, null, 2, null, null);

            // Assert
            Assert.IsTrue(!actualReturn.Contains(product1) && actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetAllProductsByMaxPriceOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts(null, null, null, null, 1, null);

            // Assert
            Assert.IsTrue(actualReturn.Contains(product1) && !actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetAllProductsThatIncludePromotionOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act
            IEnumerable<Product> actualReturn = _productRepository
                .GetAllProducts(null, null, null, null, null, true);

            // Assert
            Assert.IsTrue(!actualReturn.Contains(product1) && actualReturn.Contains(product2));
        }

        [TestMethod]
        public void GetSpecificProductOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product1, product2 });

            // Act 
            Product actualReturn = _productRepository.GetProductById(product1.Id);

            // Assert
            Assert.IsTrue(product1.Equals(actualReturn));
        }

        [TestMethod]
        public void UpdateProductOk()
        {
            // Arrange
            _ecommerceContext.Setup(ctx => ctx.Products)
                .ReturnsDbSet(new List<Product>() { product2, product1 , product3});

            // Act
            Product actualReturn = _productRepository.UpdateProduct(product1.Id, product3);

            // Assert
            Assert.IsTrue(product3.Equals(actualReturn));
        }

        [TestMethod]
        public void DeleteProductOk()
        {
            // Arrange
            List<Product> products = new List<Product> { product1 };

            _ecommerceContext.Setup(ctx => ctx.Products).ReturnsDbSet(products);
            _ecommerceContext.Setup(ctx => ctx.Products.Remove(It.IsAny<Product>())).
                Callback<Product>((entity) => products.Remove(entity));

            // Act
            bool result = _productRepository.DeleteProduct(product1.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(products.FirstOrDefault(product => product.Id == product1.Id));
        }

        [TestMethod]
        public void DeleteNonExistentProduct()
        {
            // Arrange
            List<Product> products = new List<Product> { product1 };

            _ecommerceContext.Setup(ctx => ctx.Products).ReturnsDbSet(products);
            
            // Act
            bool result = _productRepository.DeleteProduct(product2.Id);

            // Assert
            Assert.IsFalse(result);
        }
    }
}