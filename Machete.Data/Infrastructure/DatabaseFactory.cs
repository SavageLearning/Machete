using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Data.Infrastructure
{
    //
    //
    public interface IDatabaseFactory : IDisposable
    {
        MacheteContext Get();
        void Set(MacheteContext context);
    }
    //
    //
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private MacheteContext dataContext;
        public MacheteContext Get()
        {
            return dataContext ?? (dataContext = new MacheteContext());
        }
        public void Set(MacheteContext context)
        {
            dataContext = context;
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
