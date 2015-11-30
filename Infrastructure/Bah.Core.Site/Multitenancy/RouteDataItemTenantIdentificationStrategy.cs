using Autofac.Extras.Multitenant;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bah.Core.Site.Multitenancy
{
    public class RouteDataItemTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        /// <summary>
        /// Gets the route data item name from which the tenant ID should be retrieved.
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Create a new <see cref="RouteDataItemTenantIdentificationStrategy"/> for
        /// the specified parameter name.
        /// </summary>
        /// <param name="parameterName">
        /// The request parameter name that holds the tenant identifier.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if <paramref name="parameterName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if <paramref name="parameterName" /> is empty.
        /// </exception>
        public RouteDataItemTenantIdentificationStrategy(string parameterName)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            if (parameterName.Length == 0)
            {
                throw new ArgumentException("paramterName");
            }

            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Attempts to identify the tenant from the route data parameters.
        /// </summary>
        /// <param name="tenantId">The current tenant identifier.</param>
        /// <returns>
        /// <see langword="true"/> if the tenant could be identified; <see langword="false"/>
        /// if not.
        /// </returns>
        public bool TryIdentifyTenant(out object tenantId)
        {
            var context = HttpContext.Current;
            try
            {
                if (context == null || context.Request == null)
                {
                    tenantId = null;
                    return false;
                }
            }
            catch (HttpException)
            {
                // This will happen at application startup in MVC3
                // integration since the ILifetimeScopeProvider tries
                // to be resolved from the container at the point where
                // a new AutofacDependencyResolver is created.
                tenantId = null;
                return false;
            }

            tenantId = HttpContext.Current.Request.RequestContext.RouteData.Values[this.ParameterName];
            return tenantId != null;
        }
    }
}
