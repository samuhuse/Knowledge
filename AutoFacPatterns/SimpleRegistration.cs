using Autofac;

using AutoFacPatterns.Model;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFacPatterns
{
    public class SimpleRegistration
    {
        ContainerBuilder builder = new ContainerBuilder();
        private void LogSample(IContainer container)
        {
            void Log(ILogger logger)
            {
                logger.Log("First Message");
                logger.Log("Second Message");
                logger.Log("Third Message");
            }            

            Log(container.Resolve<ILogger>());
            Log(container.Resolve<ILogger>());
            Log(container.Resolve<ILogger>());
        }

        [Test]
        public void TryOneInstancePerRequest()
        {
            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            var container = builder.Build();

            LogSample(container);
        }

        [Test]
        public void TryOneInstance()
        {
            builder.RegisterInstance(new ConsoleLogger()).As<ILogger>();

            var container = builder.Build();

            LogSample(container);
        }

    }
}
