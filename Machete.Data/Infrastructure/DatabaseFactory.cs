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
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
