using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Worktime.Core.Models;

namespace Worktime.Core.CRUD
{
    public class LineProcessor
    {
        private readonly ApplicationContext _db;
        public LineProcessor(ApplicationContext db)
        {
            _db = db;
        }

        #region Create
        private Result<WTLine> CreateOne(WTLine line, Result<WTLine> result)
        {
            var task = _db.Tasks.Find(line.WTTaskId);
            if (task == null)
                return new Result<WTLine>() { Success = false, Message = "Task was not found!" };

            if (line.BeginTime > line.EndTime)
                line.EndTime = line.BeginTime;
            double time = Math.Round((line.EndTime - line.BeginTime).TotalHours, 2);
            line.Time = time;

            result.Items.Add(_db.Lines.Add(line).Entity);
            result.Success = true;
            return result;
        }
        public Result<WTLine> Create(WTLine line)
        {
            Result<WTLine> result = new();
            result = CreateOne(line, result);
            if (result.Success)
            {
                _db.SaveChanges();
                RecalculateTotalTime(result.Items.First());
            }
            return result;
        }
        public Result<WTLine> Create(IEnumerable<WTLine> lines)
        {
            Result<WTLine> result = new();
            result = ResultGeneric.Execute(lines, result, CreateOne);
            if (result.Success && lines.Any())
            {
                _db.SaveChanges();
                RecalculateTotalTime(result.Items.First());
            }
            return result;
        }
        #endregion

        #region Read
        public IEnumerable<WTLine> ReadAsIEnumerable(int taskId) => _db.Lines.Where(x => x.WTTaskId == taskId).AsEnumerable();
        public IEnumerable<WTLine> ReadAsIEnumerable(Guid userId) => _db.Lines.Include(x => x.Task).Where(x => x.Task.WTUserId == userId).AsEnumerable();
        public IQueryable<WTLine> ReadAsIQueryable(Guid userId) => _db.Lines.Include(x => x.Task).Where(x => x.Task.WTUserId == userId).AsQueryable();
        public IQueryable<WTLine> ReadAsIQueryable(int taskId) => _db.Lines.Where(x => x.WTTaskId == taskId).AsQueryable();
        #endregion

        #region Update
        private Result<WTLine> UpdateOne(WTLine newLine, Result<WTLine> result)
        {
            var task = _db.Tasks.Find(newLine.WTTaskId);
            var line = _db.Lines.Find(newLine.Id);
            if (line == null)
                return new Result<WTLine>() { Success = false, Message = "Line was not found!" };

            if (task == null)
                return new Result<WTLine>() { Success = false, Message = "Task was not found!" };

            line.Date = newLine.Date;
            line.BeginTime = newLine.BeginTime;
            line.EndTime = newLine.EndTime;
            if (line.BeginTime > line.EndTime)
                line.EndTime = line.BeginTime;
            double time = Math.Round((line.EndTime - line.BeginTime).TotalHours, 2);
            line.Time = time;
            line.WTTaskId = newLine.WTTaskId;
            line.Task = task;

            _db.Tasks.Update(task);
            result.Items.Add(_db.Lines.Update(line).Entity);
            result.Success = true;
            return result;
        }
        public Result<WTLine> Update(WTLine newLine)
        {
            Result<WTLine> result = new();
            result = UpdateOne(newLine, result);
            if (result.Success)
            {
                _db.SaveChanges();
                RecalculateTotalTime(result.Items.First());
            }
            return result;
        }
        public Result<WTLine> Update(IEnumerable<WTLine> lines)
        {
            Result<WTLine> result = new();
            result = ResultGeneric.Execute(lines, result, UpdateOne);
            if (result.Success && lines.Any())
            {
                _db.SaveChanges();
                RecalculateTotalTime(result.Items.First());
            }
            return result;
        }
        #endregion

        #region Delete
        private Result<WTLine> DeleteOne(WTLine item, Result<WTLine> result)
        {
            var task = _db.Tasks.Find(item.WTTaskId);
            var line = _db.Lines.Find(item.Id);
            if (line == null)
                return new Result<WTLine>() { Success = false, Message = "Line was not found!" };
            if (task == null)
                return new Result<WTLine>() { Success = false, Message = "Task was not found!" };

            result.Items.Add(_db.Lines.Remove(line).Entity);
            result.Success = true;
            return result;
        }
        public Result<WTLine> Delete(WTLine item)
        {
            Result<WTLine> result = new();
            result = DeleteOne(item, result);
            if (result.Success)
            {
                _db.SaveChanges();
                RecalculateTotalTime(result.Items.First());
            }
            return result;
        }
        public Result<WTLine> Delete(IEnumerable<WTLine> items)
        {
            Result<WTLine> result = new();
            result = ResultGeneric.Execute(items, result, DeleteOne);
            if (result.Success && items.Any())
            {
                _db.SaveChanges();
                RecalculateTotalTime(result.Items.First());
            }
            return result;
        }
        #endregion

        private void RecalculateTotalTime(WTLine item)
        {
            var task = _db.Tasks.Find(item.WTTaskId);
            if(task != null)
            {
                task.TotalTime = _db.Lines.Where(x => x.WTTaskId == task.Id).Sum(x => x.Time);
                _db.Tasks.Update(task);
                _db.SaveChanges();
            }
        }
    }
}
