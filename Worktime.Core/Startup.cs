using Worktime.Core.CRUD;
using Worktime.Core.Models;

namespace Worktime.Core
{
    public class Startup
    {
        private readonly ApplicationContext _db;
        public Startup()
        {
            _db = new ApplicationContext();
        }

        #region Create
        #region One
        public Result<WTUser> Create(WTUser user)
        {
            UserProcessor processor = new(_db);
            var result = processor.Create(user);
            return result;
        }
        public Result<WTTask> Create(WTTask task)
        {
            TaskProcessor processor = new(_db);
            var result = processor.Create(task);
            return result;
        }
        public Result<WTLine> Create(WTLine line)
        {
            LineProcessor processor = new(_db);
            var result = processor.Create(line);
            return result;
        } 
        #endregion
        #region Many
        public Result<WTTask> Create(IEnumerable<WTTask> tasks)
        {
            TaskProcessor processor = new(_db);
            var result = processor.Create(tasks);
            return result;
        }
        public Result<WTLine> Create(IEnumerable<WTLine> lines)
        {
            LineProcessor processor = new(_db);
            var result = processor.Create(lines);
            return result;
        } 
        #endregion
        #endregion

        #region Read
        public WTUser? Read(Guid id)
        {
            UserProcessor processor = new(_db);
            return processor.Read(id);
        }
        public IEnumerable<WTTask> ReadAsIEnumerable(Guid userId)
        {
            TaskProcessor processor = new(_db);
            return processor.ReadAsIEnumerable(userId);
        }
        public IQueryable<WTTask> ReadAsIQueryable(Guid userId)
        {
            TaskProcessor processor = new(_db);
            return processor.ReadAsIQueryable(userId);
        }
        public IEnumerable<WTLine> ReadAsIEnumerable(int taskId)
        {
            LineProcessor processor = new(_db);
            return processor.ReadAsIEnumerable(taskId);
        }
        public IQueryable<WTLine> ReadAsIQueryable(int taskId)
        {
            LineProcessor processor = new(_db);
            return processor.ReadAsIQueryable(taskId);
        }
        public IEnumerable<WTLine> ReadRowAsIEnumerable(Guid userId)
        {
            LineProcessor processor = new(_db);
            return processor.ReadAsIEnumerable(userId);
        }
        public IQueryable<WTLine> ReadRowAsIQueryable(Guid userId)
        {
            LineProcessor processor = new(_db);
            return processor.ReadAsIQueryable(userId);
        }
        public IEnumerable<WTLine> GetRowsToday(Guid userId) => GetRowsOnDate(userId, DateTime.Today);
        public IEnumerable<WTLine> GetRowsOnDate(Guid userId, DateTime date)
        {
            date = date.ToUniversalTime();
            LineProcessor processor = new(_db);
            return processor.ReadAsIEnumerable(userId).Where(x => x.Date == date);
        }
        #endregion

        #region Update
        #region One
        public Result<WTTask> Update(WTTask newTask)
        {
            TaskProcessor processor = new(_db);
            var result = processor.Update(newTask);
            return result;
        }
        public Result<WTLine> Update(WTLine newLine)
        {
            LineProcessor processor = new(_db);
            var result = processor.Update(newLine);
            return result;
        } 
        #endregion
        #region Many
        public Result<WTTask> Update(IEnumerable<WTTask> newTasks)
        {
            TaskProcessor processor = new(_db);
            var result = processor.Update(newTasks);
            return result;
        }
        public Result<WTLine> Update(IEnumerable<WTLine> newLines)
        {
            LineProcessor processor = new(_db);
            var result = processor.Update(newLines);
            return result;
        }
        #endregion
        #endregion

        #region Delete
        #region One
        public Result<WTUser> Delete(Guid id)
        {
            UserProcessor processor = new(_db);
            var result = processor.Delete(id);
            return result;
        }
        public Result<WTTask> Delete(WTTask task)
        {
            TaskProcessor processor = new(_db);
            var result = processor.Delete(task);
            return result;
        }
        public Result<WTLine> Delete(WTLine line)
        {
            LineProcessor processor = new(_db);
            var result = processor.Delete(line);
            return result;
        } 
        #endregion
        #region Many
        public Result<WTTask> Delete(IEnumerable<WTTask> tasks)
        {
            TaskProcessor processor = new(_db);
            var result = processor.Delete(tasks);
            return result;
        }
        public Result<WTLine> Delete(IEnumerable<WTLine> lines)
        {
            LineProcessor processor = new(_db);
            var result = processor.Delete(lines);
            return result;
        } 
        #endregion
        #endregion
    }
}