using Domain;

namespace LogicInterfaces
{
    public interface IUserLogic
    {
        User CreateUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUser(Guid v);
        User EditUser(Guid id, User received);
        void DeleteUser(Guid id);
    }
}