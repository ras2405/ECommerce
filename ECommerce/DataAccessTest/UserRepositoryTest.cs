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
    public class UserRepositoryTest
    {
        [TestMethod]
        public void CreateUserOk()
        {
            // Arrage 
            User expected = new User()
            {
                Email = "Test@gmail.com",
                Password = "password",
                Address = "Address",
                Rol = User.Roles.Buyer
            };

            Mock<ECommerceContext> _ecommerceContext = new Mock<ECommerceContext>();
            _ecommerceContext.Setup(ctx => ctx.Users).ReturnsDbSet(new List<User>());
            IUserRepository userRepository = new UserRepository(_ecommerceContext.Object);
            
            //Act
            User actualReturn = userRepository.CreateUser(expected);

            //Assert
            Assert.AreEqual(actualReturn.Id, expected.Id);
        }

        [TestMethod]
        public void GetAllUsersOk()
        {
            // Arrange
            User user1 = new User()
            {
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };

            User user2 = new User()
            {
                Email = "Test2@gmail.com",
                Password = "password2",
                Address = "Address2",
                Rol = User.Roles.Buyer
            };

            Mock<ECommerceContext> _ecommerceContext = new Mock<ECommerceContext>();
            _ecommerceContext.Setup(ctx => ctx.Users).ReturnsDbSet(new List<User>() { user1, user2 });
            IUserRepository userRepository = new UserRepository(_ecommerceContext.Object);

            // Act
            IEnumerable<User> actualReturn = userRepository.GetAllUsers();

            // Assert
            Assert.IsTrue(actualReturn.Contains(user1) && actualReturn.Contains(user2));
        }

        [TestMethod]
        public void GetSpecificUserOk()
        {
            // Arrange
            User user1 = new User()
            {
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };

            User user2 = new User()
            {
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };

            Mock<ECommerceContext> _ecommerceContext = new Mock<ECommerceContext>();
            _ecommerceContext.Setup(ctx => ctx.Users).ReturnsDbSet(new List<User>() { user1, user2 });
            IUserRepository userRepository = new UserRepository(_ecommerceContext.Object);

            // Act
            User actualReturn = userRepository.GetUser(user1.Id);

            // Assert
            Assert.AreEqual(user1.Id, actualReturn.Id);
        }

        [TestMethod]
        public void EditUserOk()
        {
            // Arrange
            User user1 = new User()
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
                Rol = User.Roles.Admin
            };

            Mock<ECommerceContext> _ecommerceContext = new Mock<ECommerceContext>();
            _ecommerceContext.Setup(ctx => ctx.Users).ReturnsDbSet(new List<User>() { user1, user2 });
            UserRepository userRepository = new UserRepository(_ecommerceContext.Object);

            // Act
            User actualReturn = userRepository.EditUser(user1.Id, user2);
            
            //Assert
            Assert.IsTrue(user2.Equals(actualReturn));
        }

        [TestMethod]
        public void DeleteUserOk()
        {
            // Arrange
            User user1 = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test1@gmail.com",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };

            List<User> usersInDB = new List<User>() { user1 };
            Mock<ECommerceContext> _ecommerceContext = new Mock<ECommerceContext>();
            _ecommerceContext.Setup(ctx => ctx.Users).ReturnsDbSet(usersInDB);
            _ecommerceContext.Setup(ctx => ctx.Users.Remove(It.IsAny<User>()))
                .Callback<User>((entity) => usersInDB.Remove(entity));
            IUserRepository userRepository = new UserRepository(_ecommerceContext.Object);

            // Act
            userRepository.DeleteUser(user1.Id);

            // Assert
            Assert.AreEqual(0, usersInDB.Count);
        }
    }
}