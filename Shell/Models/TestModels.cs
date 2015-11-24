using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Shell.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public virtual DbSet<Test> Tests { get; set; }
    }
}
