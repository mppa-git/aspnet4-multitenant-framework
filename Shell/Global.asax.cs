using Autofac;
using Autofac.Extras.Multitenant;
using Autofac.Extras.Multitenant.Web;
using Autofac.Integration.Mvc;
using Bah.Core.Site.Multitenancy;
using Shell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Shell
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void RegisterServices()
        {
            var tenantIdStrategy = new RouteDataItemTenantIdentificationStrategy("tenant");

            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Create the multitenant container and the tenant overrides.
            var mtc = new MultitenantContainer(tenantIdStrategy, builder.Build());
            AddTenant(mtc, "a");
            AddTenant(mtc, "b");

            DependencyResolver.SetResolver(new AutofacDependencyResolver(mtc));
        }

        private void AddTenant(MultitenantContainer mtc, string tenantId)
        {
            mtc.ConfigureTenant(tenantId,
                b =>
                {
                    b.Register(c =>
                    {
                        var connectionString = "Server=.;Database=aspnet5_" + tenantId + ";Trusted_Connection=True;MultipleActiveResultSets=true";
                        return new TestDbContext(connectionString);
                    })
                      .As<TestDbContext>()
                      .InstancePerDependency();
                    //b.RegisterType<Tenant1Dependency>().As<IDependency>().InstancePerDependency();
                    //b.RegisterType<Tenant1Controller>().As<HomeController>();
                });
        }

        protected void Application_Start()
        {
            this.RegisterServices();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
