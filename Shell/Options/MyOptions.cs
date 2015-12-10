using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shell.Options
{
    public class MyOptions
    {
        public MyOptions()
        {
            this.Setting1 = "default setting";
        }

        public string ConnectionString { get; set; }
        public string Setting1 { get; set; }
    }
}