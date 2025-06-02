using GosuslugiWinForms.Data;
using GosuslugiWinForms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GosuslugiWinForms.Repositories
{
    public class ApplicationRepository : IRepository<Models.Application>
    {
        private readonly GosuslugiContext _context;

        public ApplicationRepository(GosuslugiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(Models.Application application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        public Models.Application? FindById(Guid id)
        {
            return _context.Applications
                .Include(a => a.Service)
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);
        }

        public List<Models.Application> FindAll()
        {
            return _context.Applications.ToList();
        }

        public List<Models.Application> GetByUser(Guid userId)
        {
            return _context.Applications
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public void Update(Models.Application application)
        {
            _context.Applications.Update(application);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var application = FindById(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
                _context.SaveChanges();
            }
        }
    }
}