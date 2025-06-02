namespace GosuslugiWinForms.Models
{
    public class Rule
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public Guid ParameterTypeId { get; set; }
        public ParameterType ParameterType { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public Operator Operator { get; set; }
        public TimeSpan DeadlineInterval { get; set; }
    }
}