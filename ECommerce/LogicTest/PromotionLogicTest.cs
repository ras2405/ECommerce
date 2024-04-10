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
    public class PromotionLogicTests
    {
        private IProductLogic _productLogic;
        private Mock<IProductRepository> _productRepositoryMock;
        private Exception _ext;

        [TestInitialize]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
            _productLogic = new ProductLogic(_productRepositoryMock.Object);
        }

        [TestMethod]
        public void CalculateBestPromotionIsTwenty()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };

            Product mockProduct1 = new Product { 
                Colors = new List<Color> { color1 }, 
                Category = new Category() { categoryName = "Electronics" }, 
                Price = 50, 
                Brand = new Brand() { brandName = "Test1" } };
            Product mockProduct2 = new Product { 
                Colors = new List<Color> { color1 }, 
                Category = new Category() { categoryName = "Electronics" }, 
                Price = 30, 
                Brand = new Brand() { brandName = "Test1" } };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2 };

            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);
            promotionLogic.path = "../../../../PromotionsFiles";

            // Act
            Promotion result = promotionLogic.CalculateBestPromotion(products);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Promotion.PromotionType.twenty, result.Type);
            Assert.AreEqual(10, result.Amount);
        }

        [TestMethod]
        public void CalculateBestPromotionIsTotalLook()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };

            Product mockProduct1 = new Product { 
                Price = 50, 
                Colors = new List<Color> { color1 }, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test1" } };
            Product mockProduct2 = new Product { 
                Price = 30, 
                Colors = new List<Color> { color1 }, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test2" } };
            Product mockProduct3 = new Product { 
                Price = 20, 
                Colors = new List<Color> { color1 }, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test3" } };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2, mockProduct3 };

            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);
            promotionLogic.path = "../../../../PromotionsFiles";

            // Act
            Promotion result = promotionLogic.CalculateBestPromotion(products);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Promotion.PromotionType.total, result.Type);
            Assert.AreEqual(25, result.Amount); 
        }

        [TestMethod]
        public void CalculateBestPromotionIsThreeXtwo()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };

            Product mockProduct1 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test1" } };
            Product mockProduct2 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test2" } };
            Product mockProduct3 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test3" } };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2, mockProduct3 };

            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);
            promotionLogic.path = "../../../../PromotionsFiles";

            // Act
            Promotion result = promotionLogic.CalculateBestPromotion(products);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Promotion.PromotionType.threeXtwo, result.Type);
            Assert.AreEqual(100, result.Amount);
        }

        [TestMethod]
        public void CalculateBestPromotionIsThreeXone()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };

            Product mockProduct1 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" } };
            Product mockProduct2 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" } };
            Product mockProduct3 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" } };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2, mockProduct3 };

            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);
            promotionLogic.path = "../../../../PromotionsFiles";

            // Act
            Promotion result = promotionLogic.CalculateBestPromotion(products);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Promotion.PromotionType.threeXone, result.Type);
            Assert.AreEqual(200, result.Amount);
        }

        [TestMethod]
        public void AdjustCartToStockOK()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };

            Product mockProduct1 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, Stock=10 };
            Product mockProduct2 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, Stock = 10 };
            Product mockProduct3 = new Product { 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, Stock = 10 };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2, mockProduct3 };
            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);

            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct1.Id)).Returns(mockProduct1);
            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct2.Id)).Returns(mockProduct2);
            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct3.Id)).Returns(mockProduct3);

            // Act
            string result = promotionLogic.AdjustCartToStock(products);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsNotNull(result); 
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void AdjustCartToStockOneProductOutOfStock()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };
            Product mockProduct1 = new Product {
                Id = Guid.NewGuid(), 
                Name = "Product1", 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, 
                Stock = 1 };
            Product mockProduct2 = new Product { 
                Id = Guid.NewGuid(), Name = "Product2", 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, 
                Stock = 10 };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2, mockProduct1 };
            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);

            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct1.Id)).Returns(mockProduct1);
            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct2.Id)).Returns(mockProduct2);

            // Act
            string result = promotionLogic.AdjustCartToStock(products);

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual("The following product / s have been removed from your cart due to insuficient stock: \n"
                + "- " + mockProduct1.Name + ": 1 item/s\n", result);
        }

        [TestMethod]
        public void AdjustCartToStockAllProductsOutOfStock()
        {
            // Arrange
            Color color1 = new Color() { colorName = "Red" };
            Product mockProduct1 = new Product { 
                Id = Guid.NewGuid(), 
                Name = "Product1", 
                Colors = new List<Color> { color1 }, 
                Price = 100, 
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, 
                Stock = 0 };
            Product mockProduct2 = new Product { 
                Id = Guid.NewGuid(), 
                Name = "Product2", 
                Colors = new List<Color> { color1 }, 
                Price = 100,
                Category = new Category() { categoryName = "Electronics" }, 
                Brand = new Brand() { brandName = "Test" }, 
                Stock = 0 };
            List<Product> products = new List<Product> { mockProduct1, mockProduct2};
            PromotionLogic promotionLogic = new PromotionLogic(_productLogic);

            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct1.Id)).Returns(mockProduct1);
            _productRepositoryMock.Setup(repo => repo.GetProductById(mockProduct2.Id)).Returns(mockProduct2);

            // Act
            try
            {
                promotionLogic.AdjustCartToStock(products);
            }
            catch (Exception ex)
            {
                _ext = ex;
            }

            // Assert
            _productRepositoryMock.VerifyAll();
            Assert.IsNotNull(_ext);
            Assert.IsInstanceOfType(_ext, typeof(BadRequestException));
            Assert.AreEqual("Invalid cart: all products are sold out. Sorry!", _ext.Message);
        }
    }
}