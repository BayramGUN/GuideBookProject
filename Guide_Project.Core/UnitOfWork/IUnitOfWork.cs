namespace Guide_Project.Core.UnitOfWork;

public interface IUnitOfWork
{
    Task CommitAsync();
    void Commit();
}