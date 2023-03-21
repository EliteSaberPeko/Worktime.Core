using Worktime.Core.Models;

namespace Worktime.Core
{
    public class UserProcessor
    {
        private readonly ApplicationContext _db;
        public UserProcessor(ApplicationContext db)
        {
            _db = db;
        }
        public void Create(WTUser user)
        {
            if(user == null || user.Id == Guid.Empty)
            {
                string msg = "User is empty!";
                throw new ArgumentException(msg);
            }
            _db.Users.Add(user);
        }
        public WTUser Read(Guid id)
        {
            var user = _db.Users.Find(id);
            if(user == null)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }
            return user;
        }
        public void Delete(Guid id)
        {
            var user = _db.Users.Find(id);
            if(user == null)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }
            _db.Users.Remove(user);
        }
    }
}
