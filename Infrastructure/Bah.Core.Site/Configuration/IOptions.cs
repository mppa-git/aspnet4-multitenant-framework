using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bah.Core.Site.Configuration
{
    /// <summary>
    /// Intentionally mimicking ASP.NET 5 options so we can swap out when we want to
    /// https://github.com/aspnet/Options/blob/dev/src/Microsoft.Extensions.OptionsModel/IOptions.cs
    /// 
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IOptions<out TOptions> where TOptions : class, new()
    {
        TOptions Value { get; }
    }
}
