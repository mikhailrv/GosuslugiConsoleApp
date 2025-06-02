using GosuslugiWinForms.Data;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GosuslugiWinForms.Repositories
{
    public class ServiceRepository : IRepository<Service>
    {
        private readonly GosuslugiContext _context;

        public ServiceRepository(GosuslugiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public Service? FindById(Guid id)
        {
            return _context.Services
                .Include(s => s.Rules)
                .Include(s => s.Applications)
                .FirstOrDefault(s => s.Id == id);
        }

        public List<Service> FindAll()
        {
            return _context.Services.ToList();
        }

        public List<Service> FindAllActive()
        {
            return _context.Services
                .Where(s => s.EndDate == null || s.EndDate > DateTimeOffset.UtcNow)
                .ToList();
        }

        public void Update(Service service)
        {
            var existingService = FindById(service.Id)
                ?? throw new ArgumentException("Услуга не найдена.");
            existingService.EndDate = service.EndDate;
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var service = FindById(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
        }
    }
}