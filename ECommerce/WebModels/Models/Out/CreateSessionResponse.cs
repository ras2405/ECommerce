using Domain;

namespace WebModels.Models.Out
{
    public class CreateSessionResponse
    {
        public Guid Token { get; set; }
        public User.Roles Rol { get; set; }

        public Guid Id { get; set; }

        public CreateSessionResponse(Guid token, User.Roles role, Guid id)
        {
            Token = token;
            Rol = role;
            Id = id;
        }
    }
}
