using Worktime.Core.Models;

namespace Worktime.Core.CRUD
{
    public class TaskProcessor
    {
        private readonly ApplicationContext _db;
        public TaskProcessor(ApplicationContext db)
        {
            _db = db;
        }
        public void Create(WTTask task)
        {
            var user = _db.Users.Find(task.WTUserId);
            if(string.IsNullOrWhiteSpace(task.Name))
            {
                string msg = "Name is empty!";
                throw new ArgumentException(msg);
            }
            if(user == null)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }
            _db.Tasks.Add(task);
        }
        public IEnumerable<WTTask> ReadAsIEnumerable(Guid userId) => _db.Tasks.Where(x => x.WTUserId == userId).AsEnumerable();
        public IQueryable<WTTask> ReadAsIQueryable(Guid userId) => _db.Tasks.Where(x => x.WTUserId == userId).AsQueryable();
        public void Update(WTTask newTask)
        {
            var task = _db.Tasks.Find(newTask.Id);
            var user = _db.Users.Find(newTask.WTUserId);
            if(task == null)
            {
                string msg = "Task is not found!";
                throw new ArgumentException(msg);
            }
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                string msg = "Name is empty!";
                throw new ArgumentException(msg);
            }
            if (user == null)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }

            task.Name = newTask.Name;
            task.Description = newTask.Description;
            task.TotalTime = newTask.TotalTime;
            task.Completed = newTask.Completed;
            task.WTUserId = newTask.WTUserId;
            task.User = user;

            _db.Tasks.Update(task);
        }
        public void Delete(int id)
        {
            var task = _db.Tasks.Find(id);
            if (task == null)
            {
                string msg = "Task is not found!";
                throw new ArgumentException(msg);
            }
            _db.Tasks.Remove(task);
        }
    }
}
