using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jobs
{
    public partial class MainWindow : Window
    {
        private Random random;
        private ICollection<Job> workingJobs;
        public MainWindow()
        {
            random = new Random();
            workingJobs = new List<Job>();

            InitializeComponent();

            Task[] workers = new Task[4];

            for (int i = 0; i < 4; i++)
            {
                workers[i] = new Task(new Action(StartWork));
                workers[i].Start();
            }
        }

        public void StartWork()
        {
            Job workingJob;

            while (true)
            {
                using (var context = new JobContext())
                {
                    workingJob = context.Jobs.AsNoTracking()
                        .FirstOrDefault(x => x.Status == JobStatus.NotFinished);

                    if (workingJob != null)
                    {
                        if (workingJobs.Where(x => x.Id == workingJob.Id).Any() != true)
                        {
                            workingJobs.Add(workingJob);

                            if ((DateTime.Now - workingJob.Message.CreateDate).TotalMinutes > 2)
                            {
                                workingJob.Status = JobStatus.Overdue;

                                context.Entry(workingJob).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                continue;
                            }
                            else
                                workingJob.Status = JobStatus.InProgress;

                            context.Entry(workingJob).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        else
                            continue;
                    }
                    else
                        continue;
                }

                Thread.Sleep(random.Next(15000, 30000));

                using (var context = new JobContext())
                {
                    Job doneJob = context.Jobs.SingleOrDefault(x => x.Id == workingJob.Id);
                    doneJob.Status = JobStatus.Complete;

                    context.Entry(doneJob).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}
