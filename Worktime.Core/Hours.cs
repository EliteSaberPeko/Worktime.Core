using Microsoft.EntityFrameworkCore;

namespace Worktime.Core
{
    public class Hours
    {
        private readonly Startup _db;
        public Hours(Startup startup)
        {
            _db = startup;
        }
        public double GetFromTask(Guid userId, int taskId) => Math.Round(_db.ReadAsIEnumerable(userId).FirstOrDefault(x => x.Id == taskId)?.TotalTime ?? 0D, 2);
        public double GetOnThisYear(Guid userId) => GetOnYear(userId, DateTime.Today);
        public double GetOnYear(Guid userId, DateTime date)
        {
            return Math.Round(_db.ReadRowsAsIQueryable(userId).
                Where(x => x.Date.ToLocalTime().Year ==  date.Year).
                Sum(x => x.Time), 2);
        }
        public double GetOnThisMonth(Guid userId) => GetOnMonth(userId, DateTime.Today);
        public double GetOnMonth(Guid userId, DateTime date)
        {
            return Math.Round(_db.ReadRowsAsIQueryable(userId).
                Where(x => x.Date.ToLocalTime().Year == date.Year && x.Date.ToLocalTime().Month == date.Month).
                Sum(x => x.Time), 2);
        }
        public double GetOnThisWeek(Guid userId) => GetOnWeek(userId, DateTime.Today);
        public double GetOnWeek(Guid userId, DateTime date)
        {
            int days = 7 - (int)date.DayOfWeek;
            date = date.Date.AddDays(-days).ToUniversalTime();
            return Math.Round(_db.ReadRowsAsIQueryable(userId).
                Where(x => x.Date >= date && x.Date < date.AddDays(7)).
                Sum(x => x.Time), 2);
        }
        public double GetOnThisDay(Guid userId) => GetOnDay(userId, DateTime.Today);
        public double GetOnDay(Guid userId, DateTime date)
        {
            return Math.Round((double)_db.ReadRowsAsIQueryable(userId).
                Where(x => x.Date.ToLocalTime().Date == date.Date).
                Sum(x => x.Time), 2);
        }
    }
}
