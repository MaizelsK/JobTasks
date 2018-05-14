using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs
{
    public class JobContext : DbContext
    {
        public JobContext() : base("JobDb") { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
