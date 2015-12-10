using Autofac;
using Autofac.Extras.Multitenant;
using Autofac.Extras.Multitenant.Web;
using Autofac.Integration.Mvc;
using Bah.Core.Site.Configuration;
using Bah.Core.Site.Multitenancy;
using Shell.Models;
using Shell.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public MyOptions GetMyOptions(string tenant)
        {
            var connectionString = "Server=.;Database=aspnet5_" + tenant + ";Trusted_Connection=True;MultipleActiveResultSets=true";

            var o = new MyOptions();
            o.ConnectionString = connectionString;

            switch (tenant)
            {
                case "a":
                    o.Setting1 = "setting overridden for a";
                    break;
                case "b":
                    o.Setting1 = "overridden for b";
                    break;
                default:
                    break;
            }

            return o;
        }

        private void AddTenant(MultitenantContainer mtc, string tenantId)
        {
            mtc.ConfigureTenant(tenantId,
                b =>
                {
                    var options = this.GetMyOptions(tenantId);

                    b.Register(c => new TestDbContext(options.ConnectionString))
                        .As<TestDbContext>()
                        .InstancePerDependency();

                    b.ConfigureOptions(options);
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
