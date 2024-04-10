using Domain;
using static Domain.User;

namespace WebModels.Models.Out
{
    public class CreateUserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string Password { get; set; }
        public Roles Rol { get; set; }
        public CreateUserResponse(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Address = user.Address;
            Password = user.Password;
            Rol = user.Rol;
        }

        public CreateUserResponse(){}
    }
}
