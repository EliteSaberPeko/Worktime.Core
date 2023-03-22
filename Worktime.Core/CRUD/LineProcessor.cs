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
        private Result CreateOne(WTLine line)
        {
            var task = _db.Tasks.Find(line.WTTaskId);
            if (task == null)
                return new Result() { Success = false, Message = "Task was not found!" };

            if (line.BeginTime > line.EndTime)
                line.EndTime = line.BeginTime;
            double time = (line.EndTime - line.BeginTime).Minutes;
            if (time > 0)
                time /= 60;
            line.Time = time;

            _db.Lines.Add(line);
            return new Result() { Success = true };
        }
        public Result Create(WTLine line)
        {
            var result = CreateOne(line);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result Create(IEnumerable<WTLine> lines)
        {
            var result = ResultGeneric.Execute(lines, CreateOne);
            if (result.Success && lines.Any())
                _db.SaveChanges();
            return result;
        }
        #endregion

        #region Read
        public IEnumerable<WTLine> ReadAsIEnumerable(int taskId) => _db.Lines.Where(x => x.WTTaskId == taskId).AsEnumerable();
        public IQueryable<WTLine> ReadAsIQueryable(int taskId) => _db.Lines.Where(x => x.WTTaskId == taskId).AsQueryable();
        #endregion

        #region Update
        private Result UpdateOne(WTLine newLine)
        {
            var task = _db.Tasks.Find(newLine.WTTaskId);
            var line = _db.Lines.Find(newLine.Id);
            if (line == null)
                return new Result() { Success = false, Message = "Line was not found!" };

            if (task == null)
                return new Result() { Success = false, Message = "Task was not found!" };

            line.Date = newLine.Date;
            line.BeginTime = newLine.BeginTime;
            line.EndTime = newLine.EndTime;
            if (line.BeginTime > line.EndTime)
                line.EndTime = line.BeginTime;
            double time = (line.EndTime - line.BeginTime).Minutes;
            if (time > 0)
                time /= 60;
            line.Time = time;
            line.WTTaskId = newLine.WTTaskId;
            line.Task = task;
            task.TotalTime += time;

            _db.Tasks.Update(task);
            _db.Lines.Update(line);
            return new Result() { Success = true };
        }
        public Result Update(WTLine newLine)
        {
            var result = UpdateOne(newLine);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result Update(IEnumerable<WTLine> lines)
        {
            var result = ResultGeneric.Execute(lines, UpdateOne);
            if (result.Success && lines.Any())
                _db.SaveChanges();
            return result;
        }
        #endregion

        #region Delete
        private Result DeleteOne(ulong id)
        {
            var line = _db.Lines.Find(id);
            if (line == null)
                return new Result() { Success = false, Message = "Line was not found!" };

            _db.Lines.Remove(line);
            return new Result() { Success = true };
        }
        public Result Delete(ulong id)
        {
            var result = DeleteOne(id);
            if (result.Success)
                _db.SaveChanges();
            return result;
        }
        public Result Delete(IEnumerable<ulong> ids)
        {
            var result = ResultGeneric.Execute(ids, DeleteOne);
            if (result.Success && ids.Any())
                _db.SaveChanges();
            return result;
        } 
        #endregion
    }
}
