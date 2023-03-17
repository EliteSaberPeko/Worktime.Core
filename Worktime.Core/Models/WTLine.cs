namespace Worktime.Core.Models
{
    public class WTLine
    {
        public ulong Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Time { get; set; }
    }
}
