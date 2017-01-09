using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace InsPlanIcdEditApi.IoC
{
    public class DependencyResolverUnity : DependencyScopeUnity, IDependencyResolver
    {
        public DependencyResolverUnity(IUnityContainer container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new DependencyScopeUnity(child);
        }
    }
}