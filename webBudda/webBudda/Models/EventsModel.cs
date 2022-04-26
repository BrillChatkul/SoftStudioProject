namespace webBudda.Models
{
    public class EventsModel
    {
        public int EventId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string ThemeColor { get; set; }
        public bool isFullDay { get; set; }

    }
}