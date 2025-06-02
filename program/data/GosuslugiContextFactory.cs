using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GosuslugiWinForms.Data
{
    public class GosuslugiContextFactory : IDesignTimeDbContextFactory<GosuslugiContext>
    {
        public GosuslugiContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GosuslugiContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gosuslugi;Username=postgres;Password=P@ssw0rd;Encoding=UTF8");

            return new GosuslugiContext(optionsBuilder.Options);
        }
    }
}