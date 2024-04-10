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
    public class UsersControllerTest
    {
        private UsersController _userController;

        [TestMethod]
        public void CreateUserOk()
        {
            // Arrange
            CreateUserRequest received = new CreateUserRequest()
            {
                Email = "Test@gmail.com",
                Password = "password",
                Address = "Address",
                Rol = User.Roles.Buyer
            };
            User expected = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test@gmail.com",
                Password = "password",
                Address = "Address",
                Rol = User.Roles.Buyer
            };

            CreateUserResponse expectedMappedResult = new CreateUserResponse(expected);
            
            Mock<IUserLogic> _userLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _userLogicMock.Setup(logic => logic.CreateUser(It.IsAny<User>())).Returns(expected);
            _userController = new UsersController(_userLogicMock.Object);

            CreatedAtActionResult expectedObjectResult = 
                new CreatedAtActionResult("CreateUser", "CreateUser", new { id = 5 }, expectedMappedResult);

            // Act
            IActionResult result = _userController.CreateUser(received);

            // Assert
            _userLogicMock.VerifyAll();
            CreatedAtActionResult resultObject = result as CreatedAtActionResult;
            CreateUserResponse resultValue = resultObject.Value as CreateUserResponse;
            Assert.AreEqual(resultObject.StatusCode, expectedObjectResult.StatusCode);
            Assert.AreEqual(resultValue.Id, expectedMappedResult.Id);
        }

        [TestMethod]
        public void GetAllUsersOk()
        {
            // Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test@gmail.com",
                Password = "password",
                Address = "Address",
                Rol = User.Roles.Buyer
            };

            IEnumerable<User> users = new List<User>()
            {
                user
            };

            Mock<IUserLogic> _usersLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _usersLogicMock.Setup(logic => logic.GetAllUsers()).Returns(users);
            _userController = new UsersController(_usersLogicMock.Object);

            OkObjectResult expected = new OkObjectResult(new List<CreateUserResponse>()
            {
                new CreateUserResponse(users.First())
            });
            List<CreateUserResponse> expectedObject = expected.Value as List<CreateUserResponse>;

            // Act
            OkObjectResult result = _userController.GetAllUsers();
            List<CreateUserResponse> objectResult = result.Value as List<CreateUserResponse>;

            // Assert
            _usersLogicMock.VerifyAll();
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
            Assert.AreEqual(expectedObject.First().Id, objectResult.First().Id);
        }

        [TestMethod]
        public void GetSpecificUserOk()
        {
            // Arrange
            Guid id = new Guid();

            User userExpected = new User()
            {
                Id = id,
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };          

            Mock<IUserLogic> _usersLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _usersLogicMock.Setup(logic => logic.GetUser(It.IsAny<Guid>())).Returns(userExpected);
            _userController = new UsersController(_usersLogicMock.Object);

            // Act
            OkObjectResult result = _userController.GetUser(id);
            CreateUserResponse returnedProduct = result.Value as CreateUserResponse;

            // Assert
            _usersLogicMock.VerifyAll();
            Assert.AreEqual(userExpected.Id, returnedProduct.Id);
        }

        [TestMethod]
        public void EditUserOk()
        {
            // Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };

            User user2 = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test2@gmail.com",
                Password = "password2",
                Address = "Address2",
                Rol = User.Roles.Buyer
            };

            CreateUserRequest userExpected = new CreateUserRequest(user2);

            Mock<IUserLogic> _userslogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _userslogicMock.Setup(logic => logic.EditUser((It.IsAny<Guid>()), (It.IsAny<User>()))).Returns(user2);
            _userController = new UsersController(_userslogicMock.Object);

            // Act 
            OkObjectResult result = _userController.EditUser(user.Id, userExpected);
            CreateUserResponse resultProduct = result.Value as CreateUserResponse;

            // Assert
            _userslogicMock.VerifyAll();
            Assert.AreEqual(user2.Id, resultProduct.Id);
            Assert.AreEqual(user2.Email, resultProduct.Email);
            Assert.AreEqual(user2.Password, resultProduct.Password);
            Assert.AreEqual(user2.Address, resultProduct.Address);
            Assert.AreEqual(user2.Rol, resultProduct.Rol);
        }

        [TestMethod]
        public void DeleteUsertOk()
        {
            // Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };

            Mock<IUserLogic> _usersLogicMock = new Mock<IUserLogic>(MockBehavior.Strict);
            _usersLogicMock.Setup(logic => logic.DeleteUser(user.Id));
            _userController = new UsersController(_usersLogicMock.Object);

            // Act 
            OkObjectResult result = _userController.DeleteUser(user.Id);
            OkObjectResult expectedUser = new OkObjectResult("User deleted");

            // Assert
            _usersLogicMock.VerifyAll();
            Assert.AreEqual(expectedUser.Value, result.Value);
        }
    }
}
