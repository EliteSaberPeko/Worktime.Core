using Worktime.Core.Models;

namespace Worktime.Core
{
    public class Rows
    {
        private readonly Startup _db;
        public Rows(Startup startup)
        {
            _db = startup;
        }
        public IEnumerable<WTLine> GetToday(Guid userId) => GetOnDate(userId, DateTime.Today);
        public IEnumerable<WTLine> GetOnDate(Guid userId, DateTime date)
        {
            date = date.ToUniversalTime();
            return _db.ReadRowsAsIEnumerable(userId).Where(x => x.Date == date);
        }
        public IEnumerable<WTLine> GetFromTask(Guid userId, int taskId) => _db.ReadRowsAsIEnumerable(userId).Where(x => x.WTTaskId == taskId);
    }
}
