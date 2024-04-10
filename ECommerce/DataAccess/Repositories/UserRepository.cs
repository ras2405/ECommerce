using DataAccess.Context;
using Domain;
using RepositoryInterface;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceContext _eCommerceContext;

        public UserRepository(ECommerceContext eCommerceContext)
        {
            _eCommerceContext = eCommerceContext;
        }

        public User Authenticate(string email, string password)
        {
            User user = _eCommerceContext.Users.FirstOrDefault(user => user.Email == email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return user;
                }
                else
                {
                    return new User() { Email = "Invalid"};
                }
            }
            else
            {
                return null; ;
            }
        }

        public User CreateUser(User user)
        {
            _eCommerceContext.Users.Add(user);
            _eCommerceContext.SaveChanges();
            return user;
        }

        public bool DeleteUser(Guid id)
        {
            User user = GetUser(id);
            if (user != null)
            {
                _eCommerceContext.Users.Remove(user);
                _eCommerceContext.SaveChanges();
                return true;
            }
            return false;
        }

        public User EditUser(Guid id, User received)
        {
            User user = _eCommerceContext.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                user.Email = received.Email;
                user.Address = received.Address;
                user.Password = received.Password;
                user.Rol = received.Rol;
                _eCommerceContext.SaveChanges();
            }
            return user;
        }

        public bool Exist(string email)
        {
            return _eCommerceContext.Users.Any(user => user.Email == email);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _eCommerceContext.Users;
        }

        public User GetUser(Guid id)
        {
            User user = _eCommerceContext.Users.FirstOrDefault(user => user.Id == id);
            return user;
        }
    }
}