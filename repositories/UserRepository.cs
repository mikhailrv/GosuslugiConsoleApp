using GosuslugiWinForms.Data;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GosuslugiWinForms.Repositories
{
    public class UserRepository : IRepository<Account>
    {
        private readonly GosuslugiContext _context;

        public UserRepository(GosuslugiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public Account? FindById(Guid id)
        {
            return _context.Accounts
                .Include(a => (a as User)!.Parameters)
                .Include(a => (a as User)!.Applications)
                .FirstOrDefault(a => a.Id == id);
        }

        public List<Account> FindAll()
        {
            return _context.Accounts.ToList();
        }

        public Account? FindByLogin(string login)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Login == login);

            if (account is User user)
            {
                _context.Entry(user).Collection(u => u.Parameters).Load();
                _context.Entry(user).Collection(u => u.Applications).Load();
            }

            return account;
        }

        public List<Parameter> FindParameters(Guid userId)
        {
            return _context.Parameters
                .Include(p => p.ParameterType)
                .Where(p => p.UserId == userId)
                .ToList();
        }

        public void Update(Account account)
        {
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var account = FindById(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }
        public List<User> FindPendingUsers()
        {
            return _context.Accounts
                .OfType<User>()
                .Where(u => !u.IsActive)
                .ToList();
        }
        public List<Account> FindAllAccounts()
        {
            return _context.Accounts.ToList();
        }
    }
}