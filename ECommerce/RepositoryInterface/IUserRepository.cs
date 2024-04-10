using Domain;

namespace RepositoryInterface
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        bool DeleteUser(Guid id);
        User EditUser(Guid id, User received);
        IEnumerable<User> GetAllUsers();
        User GetUser(Guid id);

        bool Exist(string email);
        User Authenticate(string email, string password);
    }
}