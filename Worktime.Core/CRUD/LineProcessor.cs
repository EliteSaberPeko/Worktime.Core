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
        public void Create(WTLine line)
        {
            var task = _db.Tasks.Find(line.WTTaskId);
            if (task == null)
            {
                string msg = "Task is not found!";
                throw new ArgumentException(msg);
            }
            if(line.BeginTime > line.EndTime)
                line.EndTime = line.BeginTime;
            double time = (line.EndTime - line.BeginTime).Minutes;
            if (time > 0)
                time /= 60;
            line.Time = time;

            _db.Lines.Add(line);
        }
        public void Update(WTLine newLine)
        {
            var task = _db.Tasks.Find(newLine.WTTaskId);
            var line = _db.Lines.Find(newLine.Id);
            if (task == null)
            {
                string msg = "Task is not found!";
                throw new ArgumentException(msg);
            }
            if (line == null)
            {
                string msg = "Line is not found!";
                throw new ArgumentException(msg);
            }
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
        }
        public void Delete(ulong id)
        {
            var line = _db.Lines.Find(id);
            if (line == null)
            {
                string msg = "Line is not found!";
                throw new ArgumentException(msg);
            }
            _db.Lines.Remove(line);
        }
    }
}
