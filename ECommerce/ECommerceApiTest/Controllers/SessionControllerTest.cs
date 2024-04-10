using ECommerceApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using LogicInterfaces;
using Microsoft.AspNetCore.Http;
using WebModels.Models.In;
using WebModels.Models.Out;

namespace ECommerceApiTest.Controllers
{
    [TestClass]
    public class SessionControllerTest
    {
        private SessionController _sessionController;
        private Mock<ISessionLogic> _sessionLogicMock;

        [TestInitialize]
        public void Setup()
        {
            _sessionLogicMock = new Mock<ISessionLogic>(MockBehavior.Strict);
            _sessionController = new SessionController(_sessionLogicMock.Object);
        }

        [TestMethod]
        public void LoginOk()
        {
            // Arrange
            CreateSessionRequest request = new CreateSessionRequest()
            {
                Email = "test@gmail.com",
                Password = "password123"
            };

            Guid dummyToken = Guid.NewGuid();
            Guid dummyId = Guid.NewGuid();

            CreateSessionResponse expectedResponse = new CreateSessionResponse(dummyToken, Domain.User.Roles.Buyer, dummyId);

            _sessionLogicMock
                .Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((expectedResponse.Token, expectedResponse.Rol, expectedResponse.Id));

            // Act
            OkObjectResult result = _sessionController.Login(request);
            CreateSessionResponse response = result.Value as CreateSessionResponse;

            // Assert
            _sessionLogicMock.VerifyAll();
            Assert.AreEqual(expectedResponse.Token, response.Token);
            Assert.AreEqual(expectedResponse.Rol, response.Rol);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void LogoutSuccessfulLogoutReturnsOk()
        {
            // Arrange
            Guid sampleToken = Guid.NewGuid();
            Mock<ISessionLogic> _sessionLogicMock = new Mock<ISessionLogic>();
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            Mock<HttpRequest> mockHttpRequest = new Mock<HttpRequest>();
            HeaderDictionary mockHeaders = new HeaderDictionary { { "Authorization", "Bearer 3F2504E0-4F89-41D3-9A0C-0305E82C3301" } };

            mockHttpRequest.SetupGet(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);

            _sessionLogicMock.Setup(x => x.UserLogOut(sampleToken));

            SessionController controller = new SessionController(_sessionLogicMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            OkObjectResult result = controller.Logout();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
