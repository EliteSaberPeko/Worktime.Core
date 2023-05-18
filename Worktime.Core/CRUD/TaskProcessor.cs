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
        private Result<WTTask> CreateOne(WTTask task, Result<WTTask> result)
        {
            var user = _db.Users.Find(task.WTUserId);

            if (string.IsNullOrWhiteSpace(task.Identifier))
                return new Result<WTTask>() { Success = false, Message = $"{nameof(task.Identifier)} is empty!" };

            if (string.IsNullOrWhiteSpace(task.Title))
                return new Result<WTTask>() { Success = false, Message = $"{nameof(task.Title)} is empty!" };

            if (user == null)
                return new Result<WTTask>() { Success = false, Message = "User was not found!" };

            result.Items.Add(_db.Tasks.Add(task).Entity);
            result.Success = true;
            return result;
        }
        public Result<WTTask> Create(WTTask task)
        {
            Result<WTTask> result = new();
            result = CreateOne(task, result);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result<WTTask> Create(IEnumerable<WTTask> task)
        {
            Result<WTTask> result = new();
            result = ResultGeneric.Execute(task, result, CreateOne);
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
        private Result<WTTask> UpdateOne(WTTask newTask, Result<WTTask> result)
        {
            var task = _db.Tasks.Find(newTask.Id);
            var user = _db.Users.Find(newTask.WTUserId);

            if (task == null)
                return new Result<WTTask>() { Success = false, Message = "Task was not found!" };

            if (string.IsNullOrWhiteSpace(task.Identifier))
                return new Result<WTTask>() { Success = false, Message = $"{nameof(task.Identifier)} is empty!" };

            if (string.IsNullOrWhiteSpace(task.Title))
                return new Result<WTTask>() { Success = false, Message = $"{nameof(task.Title)} is empty!" };

            if (user == null)
                return new Result<WTTask>() { Success = false, Message = "User was not found!" };

            task.Identifier = newTask.Identifier;
            task.Title = newTask.Title;
            task.TotalTime = newTask.TotalTime;
            task.Completed = newTask.Completed;
            task.WTUserId = newTask.WTUserId;
            task.User = user;

            result.Items.Add(_db.Tasks.Update(task).Entity);
            result.Success = true;
            return result;
        }
        public Result<WTTask> Update(WTTask newTask)
        {
            Result<WTTask> result = new();
            result = UpdateOne(newTask, result);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result<WTTask> Update(IEnumerable<WTTask> tasks)
        {
            Result<WTTask> result = new();
            result = ResultGeneric.Execute(tasks, result, UpdateOne);
            if (result.Success && tasks.Any())
                _db.SaveChanges();
            return result;
        }
        #endregion

        #region Delete
        private Result<WTTask> DeleteOne(WTTask item, Result<WTTask> result)
        {
            var task = _db.Tasks.Find(item.Id);
            if (task == null)
                return new Result<WTTask>() { Success = false, Message = "Task was not found!" };

            result.Items.Add(_db.Tasks.Remove(task).Entity);
            result.Success = true;
            return result;
        }
        public Result<WTTask> Delete(WTTask task)
        {
            Result<WTTask> result = new();
            result = DeleteOne(task, result);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result<WTTask> Delete(IEnumerable<WTTask> tasks)
        {
            Result<WTTask> result = new();
            result = ResultGeneric.Execute(tasks, result, DeleteOne);
            if (result.Success && tasks.Any())
                _db.SaveChanges();
            return result;
        } 
        #endregion
    }
}
