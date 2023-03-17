namespace Worktime.Core.Models
{
    public class WTUser
    {
        public Guid Id { get; set; }

        public List<WTTask> Tasks { get; set; } = new List<WTTask>();
    }
}
