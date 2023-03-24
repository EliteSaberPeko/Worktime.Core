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
        public Result<WTUser> Create(WTUser user)
        {
            if (user == null || user.Id == Guid.Empty)
                return new Result<WTUser>() { Message = "User is empty!", Success = false };

            if (_db.Users.Find(user.Id) != null)
                return new Result<WTUser>() { Message = "User is exist!", Success = false };

            var item = _db.Users.Add(user).Entity;
            _db.SaveChanges();
            return new Result<WTUser>() { Success = true, Items = new() { item } };
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
        public Result<WTUser> Delete(Guid id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
                return new Result<WTUser>() { Success = false, Message = "User was not found!" };

            var item = _db.Users.Remove(user).Entity;
            _db.SaveChanges();
            return new Result<WTUser>() { Success = true, Items = new() { item } };
        }
    }
}
