using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using EmailApp.Domain.Interfaces;
using EmailApp.Repositories;
using Encrypted.EmailApp.Services;
using Encrypted.EmailApp.Services.Interfaces;

namespace Encrypted.EmailApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BuildIoC();
        }

        private IContainer BuildIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .InstancePerRequest();

            #region Setup a common pattern

            // placed here before RegisterControllers as last one wins
            builder.RegisterAssemblyTypes()
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();
            builder.RegisterAssemblyTypes()
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();

            builder.RegisterType<EmailMessageService>().As<IEmailMessageService>();
            builder.RegisterType<EmailMessageRepository>().As<IEmailMessageRepository>();
            #endregion

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;

        }
    }
}
