using DataAccess.Context;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepositoryInterface;

namespace DataAccess.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ECommerceContext _eCommerceContext;

        public SessionRepository(ECommerceContext eCommerceContext)
        {
            _eCommerceContext = eCommerceContext;
        }

        public Session Insert(Session session)
        {
            _eCommerceContext.Sessions.Add(session);
            _eCommerceContext.SaveChanges();
            return session;
        }

        public void UserLogOut(Guid token)
        {
            Session user = GetSession(token);
            if (user != null)
            {
                _eCommerceContext.Sessions.Remove(user);
                _eCommerceContext.SaveChanges();
            }
        }

        public Session GetSession(Guid token)
        {
            Session session = _eCommerceContext.Sessions
                .Include(s => s.User).FirstOrDefault(s => s.AuthToken == token);
            return session;
        }
    }
}