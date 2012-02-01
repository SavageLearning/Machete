using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Data.Infrastructure
{
    //
    //
    public interface IUnitOfWork
    {
        void Commit();
    }
    //
    //
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;
        private MacheteContext dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected MacheteContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}
