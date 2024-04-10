using Domain;
using ECommerceApi.Controllers;
using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebModels.Models.Out;

namespace ECommerceApiTest.Controllers
{
    [TestClass]
    public class PromotionControllerTest
    {
        private PromotionController _promotionController;
        private Mock<IPromotionLogic> _promotionLogicMock;

        private Product _product;
        private List<Product> _productList;
        private Promotion _promotion;


        [TestInitialize]
        public void Setup()
        {
            _promotionLogicMock = new Mock<IPromotionLogic>(MockBehavior.Strict);
            _promotionController = new PromotionController(_promotionLogicMock.Object);

            _product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "product1",
                Price = 100,
                Brand = new Brand() { brandName = "brand1" },
                Category = new Category() { categoryName = "category1" },
                Colors = new List<Color> { new Color () },
                Description = "description1",
            };

            _productList = new List<Product>() { _product };

            _promotion = new Promotion()
            {
                Amount = 100,
                Type = Promotion.PromotionType.twenty
            };
        }

        [TestMethod]
        public void CreatePromotionOk()
        {
            // Arrange
            GetPromotionResponse expectedResponse = new GetPromotionResponse(_promotion, "");

            _promotionLogicMock
                .Setup(logic => logic.CalculateBestPromotion(It.IsAny<List<Product>>()))
                .Returns(_promotion);
            _promotionLogicMock
                .Setup(logic => logic.AdjustCartToStock(It.IsAny<List<Product>>()))
                .Returns("");
            
            // Act
            OkObjectResult result = _promotionController.CreatePromotion(_productList) as OkObjectResult;
            GetPromotionResponse resultObject = result.Value as GetPromotionResponse;

            // Assert
            _promotionLogicMock.VerifyAll();
            Assert.AreEqual(expectedResponse.Amount, resultObject.Amount);
            Assert.AreEqual(expectedResponse.Type, resultObject.Type);
        }
    }
}
