using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs
{
    public class Job
    {
        public Guid Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public virtual Message Message { get; set; }
        public JobStatus Status { get; set; }
    }
}
