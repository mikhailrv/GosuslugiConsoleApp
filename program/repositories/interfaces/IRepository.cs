namespace GosuslugiWinForms.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Save(T entity);
        T? FindById(Guid id);
        void Update(T entity);
        void Delete(Guid id);
    }
}