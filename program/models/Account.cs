using System;
using System.ComponentModel.DataAnnotations;

namespace GosuslugiWinForms.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}