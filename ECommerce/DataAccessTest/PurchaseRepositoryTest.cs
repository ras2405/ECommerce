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
    public class PurchaseRepositoryTest
    {
        private IPurchaseRepository _purchaseRepository;
        private Mock<ECommerceContext> _eCommerceContextMock;

        private User _user1;
        private User _user2;
        private Product _product1;
        private Product _product2;
        private PurchasedProduct _purchasedProduct1;
        private PurchasedProduct _purchasedProduct2;
        private Purchase _purchase1;
        private Purchase _purchase2;

        [TestInitialize]
        public void Setup()
        {
            _eCommerceContextMock = new Mock<ECommerceContext>();
            _purchaseRepository = new PurchaseRepository(_eCommerceContextMock.Object);

            Color color1 = new Color() { colorName = "Test" };

            _user1 = new User()
            {
                Id = Guid.NewGuid(),
                Email = "mail@mail.com",
                Password = "password",
                Address = "address",
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

            _product1 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "product1",
                Price = 100,
                Brand = new Brand() { brandName = "brand1" },
                Category = new Category() { categoryName = "category1" },
                Colors = new List<Color> { color1 },
                Description = "description1",
            };

            _product2 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "product2",
                Price = 100,
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
                    _purchasedProduct1,
                    _purchasedProduct2
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
            _eCommerceContextMock
                .Setup(ctx => ctx.Purchases)
                .ReturnsDbSet(new List<Purchase>() { _purchase1 });

            // Act
            Purchase result = _purchaseRepository.CreatePurchase(_purchase1);

            // Assert
            Assert.AreEqual(_purchase1.Id, result.Id);
        }

        [TestMethod]
        public void GetPurchasesOk()
        { 
            // Arrange
            _eCommerceContextMock
                .Setup(ctx => ctx.Purchases)
                .ReturnsDbSet(new List<Purchase>() { _purchase1, _purchase2 });

            // Act
            IEnumerable<Purchase> result = _purchaseRepository.GetPurchases();

            // Assert
            Assert.IsTrue(result.Contains(_purchase1) && result.Contains(_purchase2));
        }

        [TestMethod]
        public void GetPurchaseOk()
        {
            // Arrange
            _eCommerceContextMock
                .Setup(ctx => ctx.Purchases)
                .ReturnsDbSet(new List<Purchase>() { _purchase1, _purchase2 });

            // Act
            Purchase result = _purchaseRepository.GetPurchase(_purchase1.Id);

            // Assert
            Assert.AreEqual(_purchase1.Id, result.Id);
        }

        [TestMethod]
        public void GetUserPurchasesOk()
        {
            // Arrange
            _eCommerceContextMock
                .Setup(ctx => ctx.Purchases)
                .ReturnsDbSet(new List<Purchase>() { _purchase1, });

            // Act
            IEnumerable<Purchase> result = _purchaseRepository.GetUserPurchases(_user1.Id);

            // Assert
            Assert.IsTrue(result.Contains(_purchase1));
        }
    }
}
