using GosuslugiWinForms.Data;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories.Interfaces;


namespace GosuslugiWinForms.Repositories
{
    public class ParameterTypeRepository : IRepository<ParameterType>
    {
        private readonly GosuslugiContext _context;

        public ParameterTypeRepository(GosuslugiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(ParameterType parameterType)
        {
            _context.ParameterTypes.Add(parameterType);
            _context.SaveChanges();
        }

        public ParameterType? FindById(Guid id)
        {
            return _context.ParameterTypes.FirstOrDefault(pt => pt.Id == id);
        }

        public void Update(ParameterType parameterType)
        {
            _context.ParameterTypes.Update(parameterType);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var parameterType = FindById(id);
            if (parameterType != null)
            {
                _context.ParameterTypes.Remove(parameterType);
                _context.SaveChanges();
            }
        }
        public List<ParameterType> FindAll()
        {
            return _context.ParameterTypes
                .ToList();
        }
    }
}