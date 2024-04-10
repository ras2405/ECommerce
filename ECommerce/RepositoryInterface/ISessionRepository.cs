using Domain;

namespace RepositoryInterface
{
    public interface ISessionRepository
    {
        Session Insert(Session session);
        void UserLogOut(Guid token);

        Session GetSession(Guid token);
    }
}