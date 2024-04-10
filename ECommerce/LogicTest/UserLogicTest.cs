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
    public class UserLogicTest
    {
        private IUserLogic _userLogic;
        private Mock<IUserRepository> _userRepositoryMock;

        private User user;
        private User user2;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _userLogic = new UserLogic(_userRepositoryMock.Object);

            user = new User()
            {
                Email = "Test@gmail.com",
                Password = "password",
                Address = "Address",
                Rol = User.Roles.Buyer
            };
            user2 = new User()
            {
                Id = Guid.NewGuid(),
                Email = "Test2@gmail.com",
                Password = "password2",
                Address = "Address2",
                Rol = User.Roles.Buyer
            };
        }

        [TestMethod]
        public void CreateUserOk()
        {
            //Arrage 
            _userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(user);
            _userRepositoryMock.Setup(repo => repo.Exist(user.Email)).Returns(false);

            // Act
            User result = _userLogic.CreateUser(user);

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.AreEqual(result.Id, user.Id);
        }

        [TestMethod]
        public void GetAllUsersOk()
        {
            //Arrage 
            IEnumerable<User> users = new List<User>()
            {
                user
            };

            _userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            IEnumerable<User> result = _userLogic.GetAllUsers();

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsTrue(result.SequenceEqual(users));
        }

        [TestMethod]
        public void GetSpecificUserOk()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUser(user.Id)).Returns(user);

            // Act
            User result = _userLogic.GetUser(user.Id);

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.AreEqual(user.Id, result.Id);
        }

        [TestMethod]
        public void EditUserOk()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUser(user.Id)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.Exist(user2.Email)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.EditUser(user.Id, user2)).Returns(user2);

            // Act
            User result = _userLogic.EditUser(user.Id, user2);

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsTrue(user2.Equals(result));
        }

        [TestMethod]
        public void DeleteUserOk()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.DeleteUser(user.Id)).Returns(true);

            // Act
            _userLogic.DeleteUser(user.Id);

            // Assert
            _userRepositoryMock.VerifyAll();
            _userRepositoryMock.Verify(repo => repo.DeleteUser(user.Id), Times.Once());
        }

        [TestMethod]
        public void CreateUserAlreadyExists()
        {
            //Arrage 
            Exception ex = null;
            try
            {
                _userRepositoryMock.Setup(repo => repo.Exist(user.Email)).Returns(true);
                
                // Act
                User result = _userLogic.CreateUser(user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Email already has a registered account");
        }


        [TestMethod]
        public void CreateUserWithInvalidEmailOne()
        {
            //Arrage 
            Exception ex = null;
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "invalid@gmail",
                Password = "password1",
                Address = "Adress1",
                Rol = User.Roles.Buyer
            };

            try
            {
                // Act
                User result = _userLogic.CreateUser(user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid Email format");
        }

        [TestMethod]
        public void CreateUserWithInvalidEmailTwo()
        {
            //Arrage 
            Exception ex = null;
            try
            {
                User user = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "@gmail.com",
                    Password = "password1",
                    Address = "Adress1",
                    Rol = User.Roles.Buyer
                };
                // Act
                User result = _userLogic.CreateUser(user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid Email format");
        }

        [TestMethod]
        public void CreateUserWithInvalidEmailThree()
        {
            //Arrage 
            Exception ex = null;
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };
            try
            {
                // Act
                User result = _userLogic.CreateUser(user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid Email format");
        }

        [TestMethod]
        public void CreateUserWithBlankPassword()
        {
            //Arrage 
            Exception ex = null;
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Password = "",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };
            try
            {
                // Act
                User result = _userLogic.CreateUser(user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid password: User password can not be blank");
        }

        [TestMethod]
        public void CreateUserWithBlankAddress()
        {
            //Arrage 
            Exception ex = null;
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Password = "Password",
                Address = "",
                Rol = User.Roles.Buyer
            };
            try
            {
                // Act
                User result = _userLogic.CreateUser(user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid address: User address can not be blank");
        }

        [TestMethod]
        public void CreateUserServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _userLogic.CreateUser(user);
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
        public void EditUserServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _userLogic.EditUser(user.Id, user);
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
        public void EditNonExistentUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            
            _userRepositoryMock.Setup(repo => repo.GetUser(userId)).Returns((User)null);

            // Act
            Exception ex = null;
            try
            {
                User result = _userLogic.EditUser(userId, user);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
            Assert.AreEqual(ex.Message, "User not found");
        }


        [TestMethod]
        public void EditUser_EmailAlreadyExists()
        {
            //Arrage 
            Exception ex = null;
            try
            {
                _userRepositoryMock.Setup(repo => repo.GetUser(user.Id)).Returns(user);
                _userRepositoryMock.Setup(repo => repo.Exist(user2.Email)).Returns(true);

                // Act
                var result = _userLogic.EditUser(user.Id, user2);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Email already has a registered account");
        }

        [TestMethod]
        public void EditUser_InvalidEmailOne()
        {
            //Arrage 
            Exception ex = null;
            User received = new User()
            {
                Id = Guid.NewGuid(),
                Email = "invalid@gmail",
                Password = "password1",
                Address = "Adress1",
                Rol = User.Roles.Buyer
            };

            try
            {
                // Act
                var result = _userLogic.EditUser(received.Id, received);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid Email format");
        }

        [TestMethod]
        public void EditUser_InvalidEmailTwo()
        {
            //Arrage 
            Exception ex = null;
            try
            {
                User received = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "@gmail.com",
                    Password = "password1",
                    Address = "Adress1",
                    Rol = User.Roles.Buyer
                };
                // Act
                var result = _userLogic.EditUser(received.Id, received);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid Email format");
        }

        [TestMethod]
        public void EditUser_InvalidEmailThree()
        {
            //Arrage 
            Exception ex = null;
            User received = new User()
            {
                Id = Guid.NewGuid(),
                Email = "",
                Password = "password1",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };
            try
            {
                var _userLogic = new UserLogic(_userRepositoryMock.Object);

                // Act
                var result = _userLogic.EditUser(received.Id, received);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid Email format");
        }

        [TestMethod]
        public void EditUser_BlankPassword()
        {
            //Arrage 
            Exception ex = null;
            User received = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Password = "",
                Address = "Address1",
                Rol = User.Roles.Buyer
            };
            try
            {
                // Act
                var result = _userLogic.EditUser(received.Id, received);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid password: User password can not be blank");
        }

        [TestMethod]
        public void EditUser_BlankAddress()
        {
            //Arrage 
            Exception ex = null;
            User received = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Password = "Password",
                Address = "",
                Rol = User.Roles.Buyer
            };
            try
            {
                // Act
                var result = _userLogic.EditUser(received.Id, received);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(BadRequestException));
            Assert.AreEqual(ex.Message, "Invalid address: User address can not be blank");
        }

        [TestMethod]
        public void DeleteUserServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _userLogic.DeleteUser(user.Id);
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
        public void DeleteNonExistentUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.DeleteUser(userId))
                         .Returns(false);

            // Act
            Exception ex = null;
            try
            {
                _userLogic.DeleteUser(userId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
        }

        [TestMethod]
        public void GetUserServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _userLogic.GetUser(user.Id);
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
        public void GetAllUsersWhenThereAreNoUsers()
        {
            // Act
            Exception ex = null;
            try
            {
                _userLogic.GetAllUsers();
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
        public void GetNonExistentUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            _userRepositoryMock.Setup(repo => repo.GetUser(userId)).Returns((User) null);

            // Act
            Exception ex = null;
            try
            {
                _userLogic.GetUser(userId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(NotFoundException));
        }
    }
}