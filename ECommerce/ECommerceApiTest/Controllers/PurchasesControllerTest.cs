using Domain;
using ECommerceApi.Controllers;
using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebModels.Models.In;
using WebModels.Models.Out;

namespace ECommerceApiTest.Controllers
{
    [TestClass]
    public class PurchasesControllerTest
    {
        private PurchasesController _purchasesController;
        private Mock<IPurchaseLogic> _purchaseLogicMock;

        private User _user1;
        private User _user2;
        private Product _product1;
        private Product _product2;
        private PurchasedProduct _purchasedProduct1;
        private PurchasedProduct _purchasedProduct2;
        private Purchase _purchase1;
        private Purchase _purchase2;
        private int initialStock1;
        private int initialStock2;

        [TestInitialize]
        public void Setup()
        {
            _purchaseLogicMock = new Mock<IPurchaseLogic>(MockBehavior.Strict);
            _purchasesController = new PurchasesController(_purchaseLogicMock.Object);

            Color color1 = new Color() { colorName = "Test" };

            _user1 = new User()
            {
                Id = Guid.NewGuid(),
                Email = "mail1@mail.com",
                Password = "password1",
                Address = "address1",
                Rol = User.Roles.Buyer
            };

            _user2 = new User()
            {
                Id = Guid.NewGuid(),
                Email = "mail2@mail.com",
                Password = "password2",
                Address = "address2",
                Rol = User.Roles.Buyer
            };

            initialStock1 = 1;
            initialStock2 = 2;

            _product1 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "product1",
                Price = 100,
                Brand = new Brand() { brandName = "brand1" },
                Stock = initialStock1,
                Category = new Category() { categoryName = "category1" },
                Colors = new List<Color> { color1 },
                Description = "description1",
            };

            _product2 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "product2",
                Price = 100,
                Stock = initialStock2,
                Brand = new Brand() { brandName = "brand2" },
                Category = new Category() { categoryName = "category2" },
                Colors = new List<Color> { color1 },
                Description = "description2",
            };

            _purchasedProduct1 = new PurchasedProduct(_product1);
            _purchasedProduct2 = new PurchasedProduct(_product2);

            _purchase1 = new Purchase()
            {
                Id = Guid.NewGuid(),
                UserId = _user1.Id,
                Products = new List<PurchasedProduct>()
                {
                    _purchasedProduct1,
                    _purchasedProduct2
                },
                Promotion = new Promotion(),
                Date = DateTime.Now,
                PaymentMethod = Purchase.PaymentType.Visa
            };

            _purchase2 = new Purchase()
            {
                Id = Guid.NewGuid(),
                UserId = _user2.Id,
                Products = new List<PurchasedProduct>()
                {
                    _purchasedProduct1
                },
                Promotion = new Promotion(),
                Date = DateTime.Now,
                PaymentMethod = Purchase.PaymentType.Santander
            };
        }

        [TestMethod]
        public void CreatePurchaseOk()
        {
            // Arrange
            CreatePurchaseRequest request = new CreatePurchaseRequest()
            {
                UserId = _user1.Id,
                Products = new List<Product>()
                {
                    _product1,
                    _product2
                }
            };

            GetPurchaseResponse expectedResponse = new GetPurchaseResponse(_purchase1, "");

            _purchaseLogicMock
                .Setup(logic => logic.CreatePurchase(It.IsAny<Purchase>()))
                .Returns((_purchase1,""));
            
            // Act
            IActionResult result = _purchasesController.CreatePurchase(request);
            CreatedAtActionResult resultObject = result as CreatedAtActionResult;
            GetPurchaseResponse resultPurchase = resultObject.Value as GetPurchaseResponse;

            // Assert
            _purchaseLogicMock.VerifyAll();
            Assert.AreEqual(expectedResponse.Id, resultPurchase.Id);
        }

        [TestMethod]
        public void GetPurchasesOk()
        {
            // Arrange
            IEnumerable<Purchase> purchases = new List<Purchase>()
            {
                _purchase1,
                _purchase2
            };

            List<GetPurchaseResponse> expectedMappedResult = purchases
                .Select(p => new GetPurchaseResponse(p, "")).ToList();
            OkObjectResult expectedObjectResult = new OkObjectResult(expectedMappedResult);

            _purchaseLogicMock
                .Setup(logic => logic.GetPurchases())
                .Returns(purchases);

            // Act
            OkObjectResult result = _purchasesController.GetPurchases(null);
            List<GetPurchaseResponse> resultObject = result.Value as List<GetPurchaseResponse>;

            // Assert
            _purchaseLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expectedObjectResult.StatusCode);
            Assert.AreEqual(resultObject[0].Id, expectedMappedResult[0].Id);
            Assert.AreEqual(resultObject[1].Id, expectedMappedResult[1].Id);
        }

        [TestMethod]
        public void GetPurchaseOk()
        {
            // Arrange
            GetPurchaseResponse expectedResponse = new GetPurchaseResponse(_purchase1,"");
            OkObjectResult expectedObjectResponse = new OkObjectResult(expectedResponse);

            _purchaseLogicMock
                .Setup(logic => logic.GetPurchase(It.IsAny<Guid>()))
                .Returns(_purchase1);

            // Act
            OkObjectResult result = _purchasesController.GetPurchase(_purchase1.Id);
            GetPurchaseResponse resultObject = result.Value as GetPurchaseResponse;

            // Assert
            _purchaseLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expectedObjectResponse.StatusCode);
            Assert.AreEqual(resultObject.Id, expectedResponse.Id);
        }

        [TestMethod]
        public void GetUserPurchasesOk()
        {
            // Arrange
            IEnumerable<Purchase> purchases = new List<Purchase>()
            {
                _purchase1,
            };

            List<GetPurchaseResponse> expectedMappedResult = purchases
                .Select(p => new GetPurchaseResponse(p, "")).ToList();
            OkObjectResult expectedObjectResult = new OkObjectResult(expectedMappedResult);

            _purchaseLogicMock
                .Setup(logic => logic.GetUserPurchases(It.IsAny<Guid>()))
                .Returns(purchases);

            // Act
            OkObjectResult result = _purchasesController.GetPurchases(_purchase1.UserId);
            List<GetPurchaseResponse> resultObject = result.Value as List<GetPurchaseResponse>;

            // Assert
            _purchaseLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expectedObjectResult.StatusCode);
            Assert.AreEqual(resultObject.First().Id, expectedMappedResult.First().Id);
        }
    }
}
