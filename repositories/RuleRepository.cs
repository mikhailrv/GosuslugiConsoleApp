using GosuslugiWinForms.Data;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GosuslugiWinForms.Repositories
{
    public class RuleRepository : IRepository<Rule>
    {
        private readonly GosuslugiContext _context;

        public RuleRepository(GosuslugiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(Rule rule)
        {
            _context.Rules.Add(rule);
            _context.SaveChanges();
        }

        public Rule? FindById(Guid id)
        {
            return _context.Rules
                .Include(r => r.Service)
                .Include(r => r.ParameterType)
                .FirstOrDefault(r => r.Id == id);
        }

        public List<Rule> FindAll()
        {
            return _context.Rules
                .Include(r => r.Service)
                .Include(r => r.ParameterType)
                .ToList();
        }

        public void Update(Rule rule)
        {
            _context.Rules.Update(rule);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var rule = FindById(id);
            if (rule != null)
            {
                _context.Rules.Remove(rule);
                _context.SaveChanges();
            }
        }
    }
}