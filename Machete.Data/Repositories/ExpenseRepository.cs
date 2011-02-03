using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data.Infrastructure;

namespace Machete.Data
{
    public class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
        {
        public ExpenseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
            }           
        }
    public interface IExpenseRepository : IRepository<Expense>
    {
    }
}
