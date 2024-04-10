using Domain;
using Exceptions.LogicExceptions;
using Logic;
using LogicInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryInterface;

namespace LogicTest
{
    [TestClass]
    public class ProductLogicTest
    {
        private IProductLogic _productLogic;
        private Mock<IProductRepository> _productRepositoryMock;

        private Product product;
        private Product product2;

        [TestInitialize]
        public void Setup() 
        {
            _productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
            _productLogic = new ProductLogic(_productRepositoryMock.Object);

            Color color1 = new Color() { colorName = "Test" };
            Color color2 = new Color() { colorName = "Test2" };

            product = new Product()
            {
                Name = "Test",
                Price = 1,
                Stock = 10,
                Brand = new Brand() { brandName = "Test" },
                Category = new Category() { categoryName = "Test" },
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
                Colors = new List<Color> { color2 },
                Description = "Test2",
                PromotionExcluded = false
            };
        }

        [TestMethod]
        public void CreateProductOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.AddProduct(product)).Returns(product);

            // Act
            Product result = _productLogic.AddNewProduct(product);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(product.Equals(result));
        }

        [TestMethod]
        public void CreateProductWithNegativeStock()
        {
            // Arrange
            ProductLogic productLogic = new ProductLogic(null);
            product.Stock = -10;

            // Act
            Exception ex = null;
            try
            {
                productLogic.AddNewProduct(product);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
        }

        [TestMethod]
        public void CreateProductServerError()
        {
            // Arrange
            ProductLogic productLogic = new ProductLogic(null);

            // Act
            Exception ex = null;
            try
            {
                productLogic.AddNewProduct(product);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }

        [TestMethod]
        public void GetAllProductsOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo
                .GetAllProducts(null, null, null, null, null, null)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts(null, null, null, null, null, null);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetAllProductsServerError()
        {
            // Arrange
            ProductLogic productLogic = new ProductLogic(null);

            // Act
            Exception ex = null;
            try
            {
                IEnumerable<Product> result = productLogic
                    .GetAllProducts(null, null, null, null, null, null);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }

        [TestMethod]
        public void GetProductWithNonExistentId()
        {
            // Arrange
            Guid productId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetProductById(productId)).Returns((Product)null);

            // Act
            Exception ex = null;
            try
            {
                Product result = _productLogic.GetProductById(productId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
        }

        [TestMethod]
        public void GetProductByIdServerError()
        {
            // Arrange
            ProductLogic productLogic = new ProductLogic(null); 

            // Act
            Exception ex = null;
            try
            {
                productLogic.GetProductById(Guid.NewGuid());
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }


        [TestMethod]
        public void GetAllProductsByCategoryOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.
                GetAllProducts("Test", null, null, null, null, null)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts("Test", null, null, null, null, null);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetAllProductsByBrandOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.
                GetAllProducts(null, "Test", null, null, null, null)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts(null, "Test", null, null, null, null);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetAllProductsByTextOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.
                GetAllProducts(null, null, "Test", null, null, null)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts(null, null, "Test", null, null, null);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetAllProductsByMinPriceOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.
                GetAllProducts(null, null, null, 1, null, null)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts(null, null, null, 1, null, null);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetAllProductsByMaxPriceOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.
                GetAllProducts(null, null, null, null, 1, null)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts(null, null, null, null, 1, null);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetAllProductsThatIncludePromotionOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.
                GetAllProducts(null, null, null, null, null, true)).Returns(products);

            // Act
            IEnumerable<Product> result = _productLogic.GetAllProducts(null, null, null, null, null, true);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(products));
        }

        [TestMethod]
        public void GetSpecificProductsOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product, product2
            };

            _productRepositoryMock.Setup(repo => repo.GetProductById(product.Id)).Returns(product);

            IProductLogic _productLogic = new ProductLogic(_productRepositoryMock.Object);

            // Act
            Product result = _productLogic.GetProductById(product.Id);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(product.Equals(result));
        }

        [TestMethod]
        public void UpdateProductOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product
            };

            _productRepositoryMock.Setup(repo => repo.UpdateProduct(product.Id, product2)).Returns(product2);

            IProductLogic _productLogic = new ProductLogic(_productRepositoryMock.Object);

            // Act
            Product result = _productLogic.UpdateProduct(product.Id, product2);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsTrue(product2.Equals(result));
        }

        [TestMethod]
        public void UpdateNonExistentProduct()
        {
            // Arrange
            Guid productId = Guid.NewGuid();

            _productRepositoryMock.Setup(repo => repo.UpdateProduct(productId, product)).Returns((Product) null);

            // Act
            Exception ex = null;
            try
            {
                _productLogic.UpdateProduct(productId, product);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
        }

        [TestMethod]
        public void UpdateProductServerError()
        {
            // Arrange
            ProductLogic productLogic = new ProductLogic(null);

            // Act
            Exception ex = null;
            try
            {
                productLogic.UpdateProduct(Guid.NewGuid(), product);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }

        [TestMethod]
        public void DeleteProductOk()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.DeleteProduct(product.Id)).Returns(true);

            // Act
            _productLogic.DeleteProduct(product.Id);

            // Assert
            _productRepositoryMock.VerifyAll();
            _productRepositoryMock.Verify(repo => repo.DeleteProduct(product.Id), Times.Once());
        }

        [TestMethod]
        public void DeleteNonExistentProduct()
        {
            // Arrange
            Guid productId = Guid.NewGuid();

            _productRepositoryMock.Setup(repo => repo.DeleteProduct(productId))
                         .Returns(false);

            // Act
            Exception ex = null;
            try
            {
                _productLogic.DeleteProduct(productId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
        }

        [TestMethod]
        public void DeleteProductServerError()
        {
            // Arrange
            ProductLogic productLogic = new ProductLogic(null);

            // Act
            Exception ex = null;
            try
            {
                productLogic.DeleteProduct(Guid.NewGuid());
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }
    }
}