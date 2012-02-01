using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;

namespace Machete.Data.Infrastructure
{
// class-declaration is a type-declaration that declares a new class
//
// public   -- [ class-modifier ]
//                  Access not limited
// abstract -- [ class-modifier ]
//                  class is incomplete; used as a base class.
//                  cannot be instantiated directly
//                  cannot be sealed
// <T>      -- [ type parameter ]
//                  placeholder for a type-argument supplied to created a 
//                  constructed type
//                  type-argument -- actual type when constructed
public abstract class RepositoryBase<T> where T : class  //class declaration
{
    // private -- [ method-modifier ]
    //             decorates a method, specifies its accessibility
    //             Access limited to this class
    private MacheteContext dataContext;
    private readonly IDbSet<T> dbset;
    // protected -- [ method-modifier ]
    //                  Access limited to this class or classes derived from this class
    protected RepositoryBase(IDatabaseFactory databaseFactory)
    {
        DatabaseFactory = databaseFactory;
        dbset = DataContext.Set<T>();
    }

    protected IDatabaseFactory DatabaseFactory
    {
        get; private set;
    }

    protected MacheteContext DataContext
    {
        get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
    }
    // virtual -- [ method-modifier ] 
    //              runtime type of the instance determines implementation to invoke
    // abstract -- [method-modifier] 
    //              a virtual method with no implementation; only permitted in abstract classes
    // non-virtual -- (Not virtual or Abstract)
    //              compile-time type of the instance determines implem. to invoke             
    public virtual T Add(T entity)
    {
        return dbset.Add(entity);           
    }
      
    public virtual void Delete(T entity)
    {
        dbset.Remove(entity);
    }
    public void Delete(Func<T, Boolean> where)
    {
        IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
        foreach (T obj in objects)
            dbset.Remove(obj);
    } 
    public virtual T GetById(long id)
    {
        return dbset.Find(id);
    }

    public virtual IEnumerable<T> GetAll()
    {
        return dbset.AsEnumerable();
    }
    public virtual IQueryable<T> GetAllQ()
    {
        return dbset.AsNoTracking().AsQueryable();
    }
    public virtual IEnumerable<T> GetMany(Func<T, bool> where)
    {
        return dbset.Where(where);
    }
    public virtual IQueryable<T> GetManyQ(Func<T, bool> where)
    {
        return dbset.Where(where).AsQueryable();
    }
    public virtual IQueryable<T> GetManyQ()
    {
        return dbset.AsQueryable();
    }
    public T Get(Func<T, bool> where)
    {
        return dbset.Where(where).FirstOrDefault<T>();
    }
    public T GetQ(Func<T, bool> where)
    {
        return dbset.AsQueryable().First<T>(where);
    }
}
}
