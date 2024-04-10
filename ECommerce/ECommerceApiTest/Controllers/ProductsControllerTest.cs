using Domain;
using ECommerceApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebModels.Models.Out;
using LogicInterfaces;
using WebModels.Models.In;

namespace ECommerceApiTest.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        private ProductsController _productsController;
        private Mock<IProductLogic> _productsLogicMock;

        private Product product;
        private Product product2;

        private Color color1;

        [TestInitialize]
        public void Setup()
        {
            _productsLogicMock = new Mock<IProductLogic>(MockBehavior.Strict);
            _productsController = new ProductsController(_productsLogicMock.Object);

            color1 = new Color() { colorName = "Test" };

            product = new Product()
            {
                Id = new Guid(),
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
                Id = new Guid(),
                Name = "Test2",
                Price = 2,
                Stock = 20,
                Brand = new Brand() { brandName = "Test2" },
                Category = new Category() { categoryName = "Test2" },
                Colors = new List<Color> { color1 },
                Description = "Test",
                PromotionExcluded = false
            };
        }

        [TestMethod]
        public void CreateProductOk()
        {
            // Arrange
            CreateProductRequest received = new CreateProductRequest(product2);

            GetProductResponse productExpected = new GetProductResponse(product);

            _productsLogicMock.Setup(logic => logic.AddNewProduct(It.IsAny<Product>())).Returns(product);

            // Act 
            IActionResult result = _productsController.CreateProduct(received);
            CreatedAtActionResult resultObject = result as CreatedAtActionResult;
            GetProductResponse resultProduct = resultObject.Value as GetProductResponse;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.IsTrue(productExpected.Equals(resultProduct));
        }

        [TestMethod]
        public void GetAllProductsOk()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts(null, null, null, null, null, null)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts(null, null, null, null, null, null) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(expectedObject.First().Id, objectResult.First().Id);
        }

        [TestMethod]
        public void GetAllProductsByCategoryOk()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts("Test2", null, null, null, null, null)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts("Test2", null, null, null, null, null) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(product2.Category, objectResult.First().Category);
        }

        [TestMethod]
        public void GetAllProductsByBrandOk()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts(null, "Test2", null, null, null, null)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts(null, "Test2", null, null, null, null) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(product2.Brand, objectResult.First().Brand);
        }

        [TestMethod]
        public void GetAllProductsByTextOk()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts(null, null, "Test2", null, null, null)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts(null, null, "Test2", null, null, null) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(product2.Name.Contains("Test2"), objectResult.First().Name.Contains("Test2"));
        }

        [TestMethod]
        public void GetAllProductsByMinPriceOk()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts(null, null, null, 2, null, null)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts(null, null, null, 2, null, null) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(product2.Category, objectResult.First().Category);
        }

        [TestMethod]
        public void GetAllProductsByMaxPriceOk()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts(null, null, null, null, 2, null)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts(null, null, null, null, 2, null) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(product2.Category, objectResult.First().Category);
        }

        [TestMethod]
        public void GetAllProductsThatIncludePromo()
        {
            // Arrange
            IEnumerable<Product> productList = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic
                .GetAllProducts(null, null, null, null, null, true)).Returns(productList);

            OkObjectResult expected = new OkObjectResult(new List<GetProductResponse>()
            {
                new GetProductResponse(productList.First())
            });
            List<GetProductResponse> expectedObject = expected.Value as List<GetProductResponse>;

            // Act
            OkObjectResult result = _productsController
                .GetAllProducts(null, null, null, null, null, true) as OkObjectResult;
            List<GetProductResponse> objectResult = result.Value as List<GetProductResponse>;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(product2.Category, objectResult.First().Category);
        }

        [TestMethod]
        public void GetSpecificProductOk()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>()
            {
                product2
            };

            _productsLogicMock.Setup(logic => logic.GetProductById(It.IsAny<Guid>())).Returns(product2);

            // Act
            OkObjectResult result = _productsController.GetProduct(product2.Id) as OkObjectResult;
            GetProductResponse objectResult = result.Value as GetProductResponse;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(product2.Id, objectResult.Id);
        }

        [TestMethod]
        public void UpdateProductOk()
        {
            // Arrange
            UpdateProductRequest request = new UpdateProductRequest(product2);

            GetProductResponse responseExpected = new GetProductResponse(product2);

            _productsLogicMock.Setup(logic => logic.UpdateProduct((It.IsAny<Guid>()),
                (It.IsAny<Product>()))).Returns(product2);

            // Act 
            OkObjectResult result = _productsController.UpdateProduct(product.Id, request);
            GetProductResponse resultProduct = result.Value as GetProductResponse;

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.IsTrue(responseExpected.Equals(resultProduct));
        }

        [TestMethod]
        public void DeleteProductOk()
        {
            // Arrange
            _productsLogicMock.Setup(logic => logic.DeleteProduct(It.IsAny<Guid>()));

            // Act 
            IActionResult result = _productsController.DeleteProduct(product.Id);
            OkObjectResult resultValue = result as OkObjectResult;

            OkObjectResult expectedProducts = new OkObjectResult("Product deleted");

            // Assert
            _productsLogicMock.VerifyAll();
            Assert.AreEqual(expectedProducts.Value, resultValue.Value);
        }
    }
}
