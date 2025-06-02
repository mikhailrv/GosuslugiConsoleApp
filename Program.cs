using System.Windows.Forms;
using GosuslugiWinForms.UI;

namespace GosuslugiWinForms
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}