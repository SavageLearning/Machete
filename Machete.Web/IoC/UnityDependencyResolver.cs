using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Machete.Web.IoC
{
public class UnityDependencyResolver : IDependencyResolver
{        
    IUnityContainer container;       
    public UnityDependencyResolver(IUnityContainer container)
    {
        this.container = container;
    }

    public object GetService(Type serviceType)
    {
        try
        {
            return container.Resolve(serviceType);
        }
        catch
        {               
            return null;
        }
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        try
        {
            return container.ResolveAll(serviceType);
        }
        catch
        {                
            return new List<object>();
        }
    }
}
}