namespace Worktime.Core.Models
{
    public class WTTask
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Title { get; set; }
        public double TotalTime { get; set; }
        public bool Completed { get; set; }

        public Guid WTUserId { get; set; }
        public WTUser User { get; set; } = null!;

        public List<WTLine> Lines { get; set; } = new List<WTLine>();
    }
}
