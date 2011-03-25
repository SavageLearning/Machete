using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;

namespace Machete.Data.Infrastructure
{
public abstract class RepositoryBase<T> where T : class
{
    private MacheteContext dataContext;
    private readonly IDbSet<T> dbset;
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
        return dbset.ToList();
    }
    public virtual IEnumerable<T> GetMany(Func<T, bool> where)
    {
        return dbset.Where(where).ToList();
    }
    public T Get(Func<T, Boolean> where)
    {
        return dbset.Where(where).FirstOrDefault<T>();
    } 
}
}
