using Domain;
using static Domain.User;

namespace WebModels.Models.In
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public Roles Rol { get; set; }

        public User ToEntity()
        {
            return new User
            {
                Address = Address,
                Email = Email,
                Password = Password,
                Rol = Rol
            };
        }

        public CreateUserRequest(User user)
        {
            Address = user.Address;
            Email = user.Email;
            Password = user.Password;
            Rol = user.Rol;
        }

        public CreateUserRequest() { }
    }
}
