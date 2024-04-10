using Domain;

namespace LogicInterfaces
{
    public interface ISessionLogic
    {
        (Guid, User.Roles, Guid) Authenticate(string email, string password);
        User GetCurrentUser(Guid token);
        void UserLogOut(Guid token);
    }
}