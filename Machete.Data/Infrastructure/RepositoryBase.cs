#region COPYRIGHT
// File:     RepositoryBase.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Machete.Data.Infrastructure
{
    // where T -- [ constraint ] on constructed type
    // : class -- [ reference type constaint ] 
    //                  specifies that type argument must be a reference type.
    //                  class types, interface types, delegate types, array types, 
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Delete(T entity);
        void Delete(Func<T, Boolean> predicate);
        T GetById(int Id);
        IQueryable<T> GetAllQ();
        IQueryable<T> GetManyQ(Func<T, bool> where);
    }
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
        private DbSet<T> _dbset; // IDbSet (Framework)
        // protected -- [ method-modifier ]
        //                  Access limited to this class or classes derived from this class
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            dbFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory dbFactory
        {
            get; private set;
        }

        protected MacheteContext DataContext
        {
            get
            {
                if (dataContext == null || dataContext.IsDead)
                {
                    dataContext = dbFactory.Get();
                }
                return dataContext;
            }
        }
        protected DbSet<T> dbset
        {
            get
            {
                if (dataContext.IsDead)
                {
                    _dbset = DataContext.Set<T>();
                }
                return _dbset;
            }
        }
        // virtual -- [ method-modifier ] 
        //              runtime type of the instance determines implementation to invoke
        // abstract -- [method-modifier] 
        //              a virtual method with no implementation; only permitted in abstract classes
        // non-virtual -- (Not virtual or Abstract)
        //              compile-time type of the instance determines implem. to invoke             
        public virtual T Add(T entity)
        {
            var poofball = _dbset.Add(entity).Entity;
            return poofball;
        }
      
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public void Delete(Func<T, Boolean> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        } 
        public virtual T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public virtual IQueryable<T> GetAllQ()
        {
            return _dbset.AsNoTracking().AsQueryable();
        }

        public virtual IQueryable<T> GetManyQ(Func<T, bool> where)
        {
            return _dbset.Where(where).AsQueryable();
        }
    }
}
