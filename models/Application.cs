using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GosuslugiWinForms.Models
{
    public class Application
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("Service")]
        public Guid ServiceId { get; set; }
        public virtual Service? Service { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.PENDING;
        public string? Result { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime Deadline { get; set; }
    }
}