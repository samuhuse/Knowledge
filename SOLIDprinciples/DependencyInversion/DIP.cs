using Autofac;
using Autofac.Core;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOLIDprinciples.DependencyInversion
{
    public class DIP
    {
        #region Module A
        public class ComponentA
        {
            public interface IService
            {
                public Object Object { get; }
            }

            public interface IServiceFactory<T> where T : IService
            {
                public IService CreateService();
            }

            [Test]
            public void Run() // Assembly A uses concrete implementation of Assembly B indipendently from it
            {
                var builder = new ContainerBuilder();

                builder.RegisterModule((IModule)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes()
                        .Where(t => typeof(IModule).IsAssignableFrom(t) && t.Name.EndsWith("B")).First()));

                var container = builder.Build();

                IServiceFactory<IService> factory = container.Resolve<IServiceFactory<IService>>();
                IService service = factory.CreateService();

                Console.WriteLine(service.GetType());
            }           
        }

        #endregion

        #region Module B

        public class ComponentB
        {
            public class Service : ComponentA.IService
            {
                public object Object { get; set; }                
            }

            public class ServiceFactory : ComponentA.IServiceFactory<ComponentA.IService>
            {
                public ComponentA.IService CreateService()
                {
                    return new Service { Object = new object() };
                }
            }

            public class ModuleB : Autofac.Module
            {
                protected override void Load(ContainerBuilder builder)
                {
                    builder.RegisterType<Service>().As<ComponentA.IService>();
                    builder.RegisterType<ServiceFactory>().As<ComponentA.IServiceFactory<ComponentA.IService>>();
                }
            }
        }

        #endregion
    }
}
