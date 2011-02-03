using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machete.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        MacheteContext Get();
    }
}
