using Domain;
using Exceptions.LogicExceptions;
using LogicInterfaces;
using RepositoryInterface;
using System.Security.Authentication;

namespace Logic
{
    public class SessionLogic : ISessionLogic
    {
        private User? _currentUser;
        private ISessionRepository _sessionRepository;
        private IUserRepository _userRepository;
        public SessionLogic(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public (Guid, User.Roles, Guid)  Authenticate(string email, string password)
        {
            if (_sessionRepository == null)
            {
                throw new Exception("Authenticate failed.");
            }

            User user = _userRepository.Authenticate(email, password);

            if (user == null)
                throw new NotFoundException("User not found. " +
                    "Please check your email address and try again or register for a new account.");
            if (user.Email == "Invalid")
                throw new InvalidCredentialException("Invalid credentials: " +
                    "Please check your email address and password again or register for a new account.");

            Session session = new Session() { User = user };
            _sessionRepository.Insert(session);

            return (session.AuthToken, user.Rol, user.Id);
        }

        public void UserLogOut(Guid token)
        {
            if (_sessionRepository != null)
            {
                _sessionRepository.UserLogOut(token);
            }
            else
            {
                throw new Exception("UserLogOut failed.");
            }
        }

        public User GetCurrentUser(Guid authToken)
        {
            if (_sessionRepository != null)
            {
                Session session = _sessionRepository.GetSession(authToken);

                if (session != null)
                    _currentUser = session.User;

                return _currentUser;
            }
            else
            {
                throw new Exception("GetCurrentUser failed.");
            }
        }
    }
}