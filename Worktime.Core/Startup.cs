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
        public void Create(WTUser user)
        {
            UserProcessor processor = new(_db);
            processor.Create(user);
        }
        public void Create(WTTask task)
        {
            TaskProcessor processor = new(_db);
            processor.Create(task);
        }
        public void Create(WTLine line)
        {
            LineProcessor processor = new(_db);
            processor.Create(line);
        }
        #endregion

        #region Update
        public void Update(WTTask newTask)
        {
            TaskProcessor processor = new(_db);
            processor.Update(newTask);
        }
        public void Update(WTLine newLine)
        {
            LineProcessor processor = new(_db);
            processor.Update(newLine);
        }
        #endregion

        #region Delete
        public void Delete<WTUser>(Guid id)
        {
            UserProcessor processor = new(_db);
            processor.Delete(id);
        }
        public void Delete<WTTask>(int id)
        {
            TaskProcessor processor = new(_db);
            processor.Delete(id);
        }
        public void Delete<WTLine>(ulong id)
        {
            LineProcessor processor = new(_db);
            processor.Delete(id);
        } 
        #endregion
    }
}