using System.ComponentModel.DataAnnotations;

namespace GosuslugiWinForms.Models
{
    public class ParameterType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string? Name { get; set; }

        public ValueType ValueType { get; set; }
    }
}