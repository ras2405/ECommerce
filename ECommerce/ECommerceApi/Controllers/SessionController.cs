using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebModels.Models.Out;
using ECommerceApi.Filters;
using WebModels.Models.In;

namespace ECommerceApi.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionLogic _sessionLogic;

        public SessionController(ISessionLogic sessionLogic)
        {
            _sessionLogic = sessionLogic;
        }

        [HttpPost]
        public OkObjectResult Login([FromBody] CreateSessionRequest request)
        {
            (Guid token, Domain.User.Roles role, Guid id) = _sessionLogic.Authenticate(request.Email, request.Password);
            CreateSessionResponse response = new CreateSessionResponse(token, role, id);
            return Ok(response);
        }

        [HttpDelete]
        [AuthenticationFilter]
        public OkObjectResult Logout()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _sessionLogic.UserLogOut(Guid.Parse(token));
            return Ok(new { message = "Successful Logout" });
        }
    }
}
