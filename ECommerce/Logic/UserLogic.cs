using Domain;
using Exceptions.LogicExceptions;
using LogicInterfaces;
using RepositoryInterface;

namespace Logic
{
    public class UserLogic : IUserLogic
    {
        private IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public User CreateUser(User user)
        {
            if (_userRepository != null)
            {
                user.SelfValidation();
                if (_userRepository.Exist(user.Email))
                    throw new BadRequestException("Email already has a registered account");
                return this._userRepository.CreateUser(user);
            }
            else
            {
                throw new Exception("Can't create user");
            }
        }

        public void DeleteUser(Guid id)
        {
            if (_userRepository != null)
            {
                if (!_userRepository.DeleteUser(id))
                {
                    throw new NotFoundException("User with id:" + id + "does not exist");
                };
            }
            else
            {
                throw new Exception("Can't delete user");
            }

        }

        public User EditUser(Guid id, User received)
        {
            if (_userRepository != null)
            {
                received.SelfValidation();
                User toEdit = _userRepository.GetUser(id);
                if (toEdit != null) {
                    if (received.Email != toEdit.Email && _userRepository.Exist(received.Email))
                    {
                        throw new BadRequestException("Email already has a registered account");
                    }
                    return this._userRepository.EditUser(id, received);
                }
                else
                {
                    throw new NotFoundException("User not found");
                }
            }
            else
            {
                throw new Exception("Can't edit user");
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            if (_userRepository != null)
            {

                return _userRepository.GetAllUsers();
            }
            else
            {
                throw new Exception("Can't get all users");
            }
        }

        public User GetUser(Guid id)
        {

            if (_userRepository != null)
            {
                User ret = _userRepository.GetUser(id);
                if (ret != null)
                    return ret;
                throw new NotFoundException("User not found");
            }
            else
            {
                throw new Exception("Can't get user");
            }
        }
    }
}