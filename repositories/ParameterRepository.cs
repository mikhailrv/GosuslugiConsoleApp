using GosuslugiWinForms.Data;
using GosuslugiWinForms.Models;
using Microsoft.EntityFrameworkCore;
using GosuslugiWinForms.Repositories.Interfaces;


namespace GosuslugiWinForms.Repositories
{
    public class ParameterRepository : IRepository<Parameter>
    {
        private readonly GosuslugiContext _context;

        public ParameterRepository(GosuslugiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(Parameter parameter)
        {
            _context.Parameters.Add(parameter);
            _context.SaveChanges();
        }

        public Parameter? FindById(Guid id)
        {
            return _context.Parameters
                .Include(p => p.ParameterType)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Update(Parameter parameter)
        {
            _context.Parameters.Update(parameter);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var parameter = FindById(id);
            if (parameter != null)
            {
                _context.Parameters.Remove(parameter);
                _context.SaveChanges();
            }
        }
    }
}