using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bah.Core.Site.Configuration
{
    /// <summary>
    /// This is hacked up from https://github.com/aspnet/Options/blob/dev/src/Microsoft.Extensions.OptionsModel/OptionsManager.cs
    /// because I'm too lazy (for now) to copy the lazy loading, ironically enough.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class OptionsManager<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        private TOptions _options;

        public OptionsManager(TOptions options)
        {
            _options = options;
        }

        public virtual TOptions Value
        {
            get
            {
                return _options;
            }
        }
    }
}
