using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bah.Core.Site.Configuration
{
    public static class ConfigurationExtensions
    {
        public static ContainerBuilder ConfigureOptions<TOptions>(this ContainerBuilder services, TOptions options)
            where TOptions : class, new()
        {
            var iOption = new OptionsManager<TOptions>(options);
            services.RegisterInstance(iOption)
                .As<IOptions<TOptions>>()
                .SingleInstance();

            return services;
        }
    }
}
