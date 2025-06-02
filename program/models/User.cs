namespace GosuslugiWinForms.Models
{
    public class User : Account
    {
        public User()
        {
            Role = Role.CITIZEN;
        }

        public bool IsActive { get; set; } = false;

        public virtual List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public virtual List<Models.Application> Applications { get; set; } = new List<Models.Application>();
    }
}