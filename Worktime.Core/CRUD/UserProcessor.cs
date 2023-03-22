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
        public Result Create(WTUser user)
        {
            if (user == null || user.Id == Guid.Empty)
                return new Result() { Message = "User is empty!", Success = false };

            if (_db.Users.Find(user.Id) != null)
                return new Result() { Message = "User is exist!", Success = false };

            _db.Users.Add(user);
            _db.SaveChanges();
            return new Result() { Success = true };
        }
        public WTUser? Read(Guid id)
        {
            var user = _db.Users.Find(id);
            //if(user == null)
            //{
            //    string msg = "User is not found!";
            //    throw new ArgumentException(msg);
            //}
            return user;
        }
        public Result Delete(Guid id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
                return new Result() { Success = false, Message = "User was not found!" };

            _db.Users.Remove(user);
            _db.SaveChanges();
            return new Result() { Success = true };
        }
    }
}
