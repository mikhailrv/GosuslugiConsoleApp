namespace GosuslugiWinForms.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ModifiedById { get; set; }
        public List<Rule>? Rules { get; set; } = new List<Rule>();
        public List<Application> Applications { get; set; } = new List<Application>();
    }
}