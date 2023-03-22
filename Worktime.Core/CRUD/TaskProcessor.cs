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

        #region Create
        private Result CreateOne(WTTask task)
        {
            var user = _db.Users.Find(task.WTUserId);

            if (string.IsNullOrWhiteSpace(task.Name))
                return new Result() { Success = false, Message = "Name is empty!" };

            if (user == null)
                return new Result() { Success = false, Message = "User was not found!" };

            _db.Tasks.Add(task);
            return new Result() { Success = true };
        }
        public Result Create(WTTask task)
        {
            var result = CreateOne(task);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result Create(IEnumerable<WTTask> task)
        {
            var result = ResultGeneric.Execute(task, CreateOne);
            if (result.Success && task.Any())
                _db.SaveChanges();
            return result;
        }
        #endregion

        #region Read
        public IEnumerable<WTTask> ReadAsIEnumerable(Guid userId) => _db.Tasks.Where(x => x.WTUserId == userId).AsEnumerable();
        public IQueryable<WTTask> ReadAsIQueryable(Guid userId) => _db.Tasks.Where(x => x.WTUserId == userId).AsQueryable();
        #endregion

        #region Update
        private Result UpdateOne(WTTask newTask)
        {
            var task = _db.Tasks.Find(newTask.Id);
            var user = _db.Users.Find(newTask.WTUserId);

            if (task == null)
                return new Result() { Success = false, Message = "Task was not found!" };

            if (string.IsNullOrWhiteSpace(task.Name))
                return new Result() { Success = false, Message = "Name is empty!" };

            if (user == null)
                return new Result() { Success = false, Message = "User was not found!" };

            task.Name = newTask.Name;
            task.Description = newTask.Description;
            task.TotalTime = newTask.TotalTime;
            task.Completed = newTask.Completed;
            task.WTUserId = newTask.WTUserId;
            task.User = user;

            _db.Tasks.Update(task);
            return new Result() { Success = true };
        }
        public Result Update(WTTask newTask)
        {
            var result = UpdateOne(newTask);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result Update(IEnumerable<WTTask> tasks)
        {
            var result = ResultGeneric.Execute(tasks, UpdateOne);
            if (result.Success && tasks.Any())
                _db.SaveChanges();
            return result;
        }
        #endregion

        #region Delete
        private Result DeleteOne(int id)
        {
            var task = _db.Tasks.Find(id);
            if (task == null)
                return new Result() { Success = false, Message = "Task was not found!" };

            _db.Tasks.Remove(task);
            return new Result() { Success = true };
        }
        public Result Delete(int id)
        {
            var result = DeleteOne(id);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result Delete(IEnumerable<int> ids)
        {
            var result = ResultGeneric.Execute(ids, DeleteOne);
            if (result.Success && ids.Any())
                _db.SaveChanges();
            return result;
        } 
        #endregion
    }
}
