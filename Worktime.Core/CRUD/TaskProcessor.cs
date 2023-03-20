using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(string.IsNullOrWhiteSpace(task.Name))
            {
                string msg = "Name is empty!";
                throw new ArgumentException(msg);
            }
            if(task.WTUserId == Guid.Empty)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }
            _db.Tasks.Add(task);
        }
        public void Update(WTTask newTask)
        {
            var task = _db.Tasks.Find(newTask.Id);
            if(task == null)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                string msg = "Name is empty!";
                throw new ArgumentException(msg);
            }
            if (task.WTUserId == Guid.Empty)
            {
                string msg = "User is not found!";
                throw new ArgumentException(msg);
            }
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
