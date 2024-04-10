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
    public class PurchaseLogicTest
    {
        private IPurchaseLogic _purchaseLogic;
        private Mock<IUserLogic> _userLogicMock;
        private Mock<IPromotionLogic> _promotionLogicMock;
        private Mock<IProductLogic> _productLogicMock;
        private Mock<IPurchaseRepository> _purchaseRepositoryMock;
        private Exception _ext;

        private User _user1;
        private User _user2;
        private Product _product1;
        private Product _product2;
        private Promotion _promotion;
        private Promotion _promotion2;
        private PurchasedProduct _purchasedProduct1;
        private PurchasedProduct _purchasedProduct2;
        private Purchase _purchase1;
        private Purchase _purchase2;
        private Purchase _purchase3;
        private int initialStock1;
        private int initialStock2;

        [TestInitialize]
        public void Setup()
        {
            _purchaseRepositoryMock = new Mock<IPurchaseRepository>(MockBehavior.Strict);
            _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _productLogicMock = new Mock<IProductLogic>(MockBehavior.Strict);
            _promotionLogicMock = new Mock<IPromotionLogic>(MockBehavior.Strict);
            _purchaseLogic = new PurchaseLogic(_purchaseRepositoryMock.Object, 
                _userLogicMock.Object, _productLogicMock.Object, _promotionLogicMock.Object);

            Color color1 = new Color() { colorName = "Test" };
            Color color2 = new Color() { colorName = "Test2" };

            initialStock1 = 1;
            initialStock2 = 2;

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
                Stock = initialStock1,
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
                Stock = initialStock2,
                Brand = new Brand() { brandName = "brand2" },
                Category = new Category() { categoryName = "category2" },
                Colors = new List<Color> { color1 },
                Description = "description2",
            };

            _promotion = new Promotion();
            _promotion2 = new Promotion()
            {
                Amount = 100,
                Type = Promotion.PromotionType.twenty
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

            _purchase3 = new Purchase()
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
                PaymentMethod = Purchase.PaymentType.Paganza
            };
        }

        [TestMethod]
        public void CreatePurchaseOk()
        {
            // Arrange
            _purchaseRepositoryMock
                .Setup(repo => repo.CreatePurchase(It.IsAny<Purchase>()))
                .Returns(_purchase2);
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(It.IsAny<Guid>()))
                .Returns(_product1);
            _promotionLogicMock
                .Setup(logic => logic.CalculateBestPromotion(It.IsAny<List<Product>>()))
                .Returns(_promotion);
            _productLogicMock
                .Setup(logic => logic.UpdateProduct(It.IsAny<Guid>(), It.IsAny<Product>()))
                .Returns(_product1);

            // Act
            (Purchase, string) res = _purchaseLogic.CreatePurchase(_purchase2);
            Purchase result = res.Item1;

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            _userLogicMock.VerifyAll();
            _productLogicMock.VerifyAll();
            _promotionLogicMock.VerifyAll();
            Assert.AreEqual(_purchase2.Id, result.Id);
            Assert.AreEqual(_purchase2.Products.Count, result.Products.Count);
            Assert.AreEqual(res.Item2, "");
        }

        [TestMethod]
        public void CreatePurchaseWithInvalidUserId()
        { 
            // Arrange
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Throws(new NotFoundException());

            try
            {
                // Act
                _purchaseLogic.CreatePurchase(_purchase1);
            }
            catch (Exception ex)
            {
                _ext = ex;
            }

            // Assert
            _userLogicMock.VerifyAll();
            Assert.IsNotNull(_ext);
            Assert.IsInstanceOfType(_ext, typeof(BadRequestException));
            Assert.AreEqual("Invalid user id.", _ext.Message);
        }

        [TestMethod]
        public void CreatePurchaseWithInvalidProduct()
        {
            // Arrange
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(It.IsAny<Guid>()))
                .Throws(new NotFoundException());

            try
            {
                // Act
                _purchaseLogic.CreatePurchase(_purchase1);
            }
            catch (Exception ex)
            {
                _ext = ex;
            }

            _productLogicMock.VerifyAll();
            _userLogicMock.VerifyAll();
            Assert.IsNotNull(_ext);
            Assert.IsInstanceOfType(_ext, typeof(BadRequestException));
            Assert.AreEqual("Invalid product.", _ext.Message);
        }

        [TestMethod]
        public void CreatePurchaseWithSingleSoldOutItem()
        {
            // Arrange
            _product1.Stock = 0;
            _purchasedProduct1.Stock = 0;

            _purchaseRepositoryMock
                .Setup(repo => repo.CreatePurchase(It.IsAny<Purchase>()))
                .Returns(_purchase1);
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(_product1.Id))
                .Returns(_product1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(_product2.Id))
                .Returns(_product2);
            _productLogicMock
                .Setup(logic => logic.UpdateProduct(It.IsAny<Guid>(), It.IsAny<Product>()))
                .Returns(_product1);
            _promotionLogicMock
                .Setup(logic => logic.CalculateBestPromotion(It.IsAny<List<Product>>()))
                .Returns(_promotion);

            // Act
            (Purchase, string) res = _purchaseLogic.CreatePurchase(_purchase1);
            Purchase result = res.Item1;

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            _userLogicMock.VerifyAll();
            _productLogicMock.VerifyAll();
            _promotionLogicMock.VerifyAll();
            Assert.AreEqual(_purchase1.Id, result.Id);
            Assert.AreEqual(result.Products.Count, 1);
            Assert.AreEqual(res.Item2, 
                "The following product / s have been removed from your purchase due to insuficient stock: \n"
                + "- " + _purchasedProduct1.Name + ": 1 item/s" + "\n");
        }

        [TestMethod]
        public void CreatePurchaseWithOnlySoldOutItems()
        {
            // Arrange
            _product1.Stock = 0;
            _product2.Stock = 0;
            _purchasedProduct1.Stock = 0;
            _purchasedProduct2.Stock = 0;

            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(_product1.Id))
                .Returns(_product1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(_product2.Id))
                .Returns(_product2);

            // Act
            try
            {
                (Purchase, string) result = _purchaseLogic.CreatePurchase(_purchase1);
            }
            catch (Exception ex)
            {
                _ext = ex;
            }

            // Assert
            _userLogicMock.VerifyAll();
            _productLogicMock.VerifyAll();
            Assert.IsNotNull(_ext);
            Assert.IsInstanceOfType(_ext, typeof(BadRequestException));
            Assert.AreEqual("Invalid purchase: all products are sold out. Sorry!", _ext.Message);
        }

        [TestMethod]
        public void CreatePurchaseWithRepetedSoldOutItem()
        {
            // Arrange
            _product1.Stock = 2;
            _purchase1.Products.Add(_purchasedProduct1);
            _purchase1.Products.Add(_purchasedProduct1);
            _purchase1.Products.Add(_purchasedProduct1);

            _purchaseRepositoryMock
                .Setup(repo => repo.CreatePurchase(It.IsAny<Purchase>()))
                .Returns(_purchase1);
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(_product1.Id))
                .Returns(_product1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(_product2.Id))
                .Returns(_product2);
            _productLogicMock
                .Setup(logic => logic.UpdateProduct(It.IsAny<Guid>(), It.IsAny<Product>()))
                .Returns(_product1);
            _promotionLogicMock
                .Setup(logic => logic.CalculateBestPromotion(It.IsAny<List<Product>>()))
                .Returns(_promotion);

            // Act
            (Purchase, string) res = _purchaseLogic.CreatePurchase(_purchase1);
            Purchase result = res.Item1;

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            _userLogicMock.VerifyAll();
            _productLogicMock.VerifyAll();
            _promotionLogicMock.VerifyAll();
            Assert.AreEqual(_purchase1.Id, result.Id);
            Assert.AreEqual(result.Products.Count, 3);
            Assert.AreEqual(res.Item2, 
                "The following product / s have been removed from your purchase due to insuficient stock: \n"
                + "- " + _product1.Name + ": 2 item/s" + "\n");
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

            _purchaseRepositoryMock
                .Setup(repo => repo.GetPurchases())
                .Returns(purchases);

            // Act
            IEnumerable<Purchase> result = _purchaseLogic.GetPurchases();

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            Assert.AreEqual(purchases, result);
        }

        [TestMethod]
        public void GetPurchaseOk()
        {
            // Arrange
            _purchaseRepositoryMock
                .Setup(repo => repo.GetPurchase(It.IsAny<Guid>()))
                .Returns(_purchase1);

            // Act
            Purchase result = _purchaseLogic.GetPurchase(_purchase1.Id);

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            Assert.AreEqual(_purchase1, result);
        }

        [TestMethod]
        public void GetPurchaseWithNonExistentId()
        {
            // Arrange
            _purchaseRepositoryMock
                .Setup(repo => repo.GetPurchase(It.IsAny<Guid>()))
                .Returns((Purchase)null);

            try
            {
                // Act
                _purchaseLogic.GetPurchase(_purchase1.Id);
            }
            catch (Exception ex)
            {
                _ext = ex;
            }

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            Assert.IsNotNull(_ext);
            Assert.IsInstanceOfType(_ext, typeof(NotFoundException));
            Assert.AreEqual("Purchase not found.", _ext.Message);
        }

        [TestMethod]
        public void GetUserPurchasesOk()
        {
            // Arrange
            IEnumerable<Purchase> purchases = new List<Purchase>()
            {
                _purchase1,
            };

            _purchaseRepositoryMock
                .Setup(repo => repo.GetUserPurchases(It.IsAny<Guid>()))
                .Returns(purchases);
            _userLogicMock                
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);

            // Act
            IEnumerable<Purchase> result = _purchaseLogic.GetUserPurchases(_user1.Id);

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            _userLogicMock.VerifyAll();
            Assert.AreEqual(purchases, result);
        }

        [TestMethod]
        public void GetUserPurchasesWithNonExistentId()
        {
            // Arrange
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Throws(new NotFoundException());

            try
            {
                // Act
                _purchaseLogic.GetUserPurchases(_user1.Id);
            }
            catch (Exception ex)
            {
                _ext = ex;
            }

            // Assert
            _userLogicMock.VerifyAll();
            Assert.IsNotNull(_ext);
            Assert.IsInstanceOfType(_ext, typeof(BadRequestException));
            Assert.AreEqual("Invalid user id.", _ext.Message);
        }

        [TestMethod]
        public void CreatePaganzaPurchaseOk()
        {
            // Arrange
            double expectedDiscount = ((_product1.Price + _product2.Price)
                - ((_product1.Price + _product2.Price) * 0.2)) * 0.1;

            _purchaseRepositoryMock
                .Setup(repo => repo.CreatePurchase(It.IsAny<Purchase>()))
                .Returns(_purchase3);
            _userLogicMock
                .Setup(logic => logic.GetUser(It.IsAny<Guid>()))
                .Returns(_user1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(It.IsAny<Guid>()))
                .Returns(_product1);
            _productLogicMock
                .Setup(logic => logic.GetProductById(It.IsAny<Guid>()))
                .Returns(_product2);
            _promotionLogicMock
                .Setup(logic => logic.CalculateBestPromotion(It.IsAny<List<Product>>()))
                .Returns(_promotion2);
            _productLogicMock
                .Setup(logic => logic.UpdateProduct(It.IsAny<Guid>(), It.IsAny<Product>()))
                .Returns(_product1);
            _productLogicMock
                .Setup(logic => logic.UpdateProduct(It.IsAny<Guid>(), It.IsAny<Product>()))
                .Returns(_product2);

            // Act
            (Purchase, string) res = _purchaseLogic.CreatePurchase(_purchase3);
            Purchase result = res.Item1;

            // Assert
            _purchaseRepositoryMock.VerifyAll();
            _userLogicMock.VerifyAll();
            _productLogicMock.VerifyAll();
            _promotionLogicMock.VerifyAll();
            Assert.AreEqual(_promotion2.Amount, result.Promotion.Amount);
        }
    }
}