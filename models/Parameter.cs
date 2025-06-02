using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GosuslugiWinForms.Models
{
    public class Parameter
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("ParameterType")]
        public Guid ParameterTypeId { get; set; }
        public virtual ParameterType? ParameterType { get; set; }

        [Required]
        public string? Value { get; set; }
    }
}