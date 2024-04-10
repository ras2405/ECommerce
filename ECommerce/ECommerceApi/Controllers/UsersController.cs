using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebModels.Models.Out;
using WebModels.Models.In;
using Domain;
using ECommerceApi.Filters;

namespace ECommerceApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UsersController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserRequest received)
        {
            User user = received.ToEntity();
            User resultLogic = _userLogic.CreateUser(user);
            CreateUserResponse result = new CreateUserResponse(resultLogic);
            return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
        }

        [HttpGet]
        public OkObjectResult GetAllUsers()
        {
            return Ok(_userLogic.GetAllUsers()
                 .Select(users => new CreateUserResponse(users)).ToList());
        }

        [HttpGet("{id}")]
        [AuthenticationFilter(EditUser = true, RoleNeeded = "Admin")]
        public OkObjectResult GetUser([FromRoute] Guid id)
        {
            CreateUserResponse result = new CreateUserResponse(_userLogic.GetUser(id));
            return Ok(result);
        }

        [HttpPut("{id}")]
        [AuthenticationFilter(EditUser = true,RoleNeeded = "Admin")]
        public OkObjectResult EditUser(Guid id, CreateUserRequest userExpected)
        {
            User user = userExpected.ToEntity();
            User logic = _userLogic.EditUser(id, user);
            CreateUserResponse result = new CreateUserResponse(logic);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthenticationFilter(RoleNeeded = "Admin")]
        public OkObjectResult DeleteUser([FromRoute] Guid id)
        {
            _userLogic.DeleteUser(id);
            return Ok("User deleted");
        }
    }
}
