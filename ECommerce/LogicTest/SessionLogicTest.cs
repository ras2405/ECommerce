using Domain;
using Exceptions.LogicExceptions;
using Logic;
using LogicInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryInterface;
using System.Security.Authentication;

namespace LogicTest
{
    [TestClass]
    public class SessionLogicTest
    {
        private ISessionLogic _sessionLogic;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ISessionRepository> _sessionRepositoryMock;
        private Guid authToken = Guid.NewGuid();

        private Session session;
        private User expected;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            _sessionLogic = new SessionLogic(_sessionRepositoryMock.Object, _userRepositoryMock.Object);

            expected = new User()
            {
                Email = "Test@gmail.com",
                Password = "password",
                Address = "Address",
                Rol = User.Roles.Buyer
            };
            session = new Session()
            {
                User = expected,
                AuthToken = authToken
            };
        }

        [TestMethod]
        public void AuthenticateOK()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.Authenticate(expected.Email, expected.Password)).Returns(expected);
            _sessionRepositoryMock.Setup(repo => repo.Insert(It.IsAny<Session>()))
                .Callback<Session>(session => session.AuthToken = authToken).Returns(session);

            SessionLogic sessionLogic = new SessionLogic(_sessionRepositoryMock.Object, _userRepositoryMock.Object);

            // Act
            (Guid token, User.Roles role, Guid id) = sessionLogic.Authenticate(expected.Email, expected.Password);

            // Assert
            _userRepositoryMock.VerifyAll();
            _sessionRepositoryMock.VerifyAll();
            Assert.AreEqual(authToken, token);
            Assert.AreEqual(expected.Rol, role);
            Assert.AreEqual(expected.Id, id);
        }

        [TestMethod]
        public void AuthenticateWhenTheresNoUser()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.Authenticate(It.IsAny<string>(), 
                It.IsAny<string>())).Returns((User)null);
            SessionLogic sessionLogic = new SessionLogic(_sessionRepositoryMock.Object, _userRepositoryMock.Object);

            // Act
            Exception ex = null;
            try
            {
                sessionLogic.Authenticate("test@gmail.com", "wrongpassword");
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
        public void AuthenticateWithInvalidEmail()
        {
            User invalid = new User() { Email = "Invalid" };
            // Arrange
            _userRepositoryMock.Setup(repo => repo.Authenticate(It.IsAny<string>(), 
                It.IsAny<string>())).Returns((User)invalid);
            SessionLogic sessionLogic = new SessionLogic(_sessionRepositoryMock.Object, _userRepositoryMock.Object);

            // Act
            Exception ex = null;
            try
            {
                sessionLogic.Authenticate("wrong", "wrong");
            }
            catch (Exception e)
            {
                ex = e;
            }

            // Assert
            _userRepositoryMock.VerifyAll();
            Assert.IsNotNull(ex);
            Assert.IsInstanceOfType(ex, typeof(InvalidCredentialException));
        }


        [TestMethod]
        public void DeleteSessionOk()
        {
            // Arrange
            _sessionRepositoryMock.Setup(repo => repo.UserLogOut(session.AuthToken));

            // Act
            _sessionLogic.UserLogOut(session.AuthToken);

            // Assert
            _sessionRepositoryMock.VerifyAll();
            _sessionRepositoryMock.Verify(repo => repo.UserLogOut(session.AuthToken), Times.Once());
        }

        [TestMethod]
        public void GetCurrentUserOK()
        {
            // Arrange
            List<Session> sessionsInDB = new List<Session>() { session };
            _sessionRepositoryMock.Setup(repo => repo.GetSession(authToken)).Returns(session);

            // Act
            User result = _sessionLogic.GetCurrentUser(authToken);

            // Assert
            _sessionRepositoryMock.VerifyAll();
            Assert.AreEqual(expected, result, "Expected user is not returned.");
        }

        [TestMethod]
        public void AuthenticateServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _sessionLogic.Authenticate(expected.Email, expected.Password);
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
        public void UserLogOutServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _sessionLogic.UserLogOut(expected.Id);
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
        public void GetCurrentUserServerError()
        {
            // Act
            Exception ex = null;
            try
            {
                _sessionLogic.GetCurrentUser(expected.Id);
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