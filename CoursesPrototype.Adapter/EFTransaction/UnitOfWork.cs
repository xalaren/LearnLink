using CoursesPrototype.Adapter.EFContexts;
using CoursesPrototype.Application.Transaction;

namespace CoursesPrototype.Adapter.EFTransaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}
