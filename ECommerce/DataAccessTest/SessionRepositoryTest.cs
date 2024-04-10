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
    public class SessionRepositoryTest
    {
        [TestMethod]
        public void InsertSessionOk()
        {
            // Arrange
            Session session = new Session
            {
                AuthToken = Guid.NewGuid(),
                User = new User()
                {
                    Address = "address",
                    Email = "test@gmail.com",
                    Rol = User.Roles.Both,
                    Password = "test"
                }
            };

            Mock<ECommerceContext> _eCommerceContext = new Mock<ECommerceContext>();
            _eCommerceContext.Setup(ctx => ctx.Sessions).ReturnsDbSet(new List<Session>());
            ISessionRepository sessionRepository = new SessionRepository(_eCommerceContext.Object);
            // Act
            Session result = sessionRepository.Insert(session);

            // Assert
            _eCommerceContext.Verify(ctx => ctx.Sessions.Add(session), Times.Once);
            _eCommerceContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
            Assert.AreEqual(session, result);
        }

        [TestMethod]
        public void DeleteSessionOk()
        {
            // Arrange
            Session session = new Session
            {
                AuthToken = Guid.NewGuid(),
                User = new User()
                {
                    Address = "address",
                    Email = "test@gmail.com",
                    Rol = User.Roles.Both,
                    Password = "test"
                }
            };

            List<Session> sessionsInDB = new List<Session>() { session };
            Mock<ECommerceContext> ecommerceContext = new Mock<ECommerceContext>();
            ecommerceContext.Setup(ctx => ctx.Sessions).ReturnsDbSet(sessionsInDB);
            ecommerceContext.Setup(ctx => ctx.Sessions.Remove(It.IsAny<Session>()))
                .Callback<Session>((entity) => sessionsInDB.Remove(entity));
            ISessionRepository sessionRepository = new SessionRepository(ecommerceContext.Object);

            // Act
            sessionRepository.UserLogOut(session.AuthToken);

            // Assert
            Assert.AreEqual(0, sessionsInDB.Count);
        }
    }
}