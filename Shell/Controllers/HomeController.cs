using Bah.Core.Site.Configuration;
using Shell.Models;
using Shell.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shell.Controllers
{
    public class HomeController : Controller
    {
        public TestDbContext TestDbContext { get; private set; }
        //public ITenantService TenantService { get; private set; }
        private MyOptions Options { get; set; }

        public HomeController(TestDbContext db, IOptions<MyOptions> options) //, ITenantService tenantService)
        {
            this.TestDbContext = db;
            //this.TenantService = tenantService;
            this.Options = options.Value;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var row = new Test() { Name = "test" };
            this.TestDbContext.Tests.Add(row);
            this.TestDbContext.SaveChanges();

            //var tenant = this.HttpContext.Features.Get<ITenantFeature>();
            //var tenantName = this.TenantService.Tenant.Name;

            var tenantName = "unknown";
            ViewData["Message"] = string.Format(
                "Your {0} application description page.  {1} tests run.  Setting1 is {2}",
                tenantName,
                this.TestDbContext.Tests.Count(),
                this.Options.Setting1);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}