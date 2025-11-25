namespace DAL.Repositories.Interfaces.Basic
{
    internal interface ICreateWithId<T>
    {
        Task<int?> CreateReturnIdAsync(T entity);
    }
}
