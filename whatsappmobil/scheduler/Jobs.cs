using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using whatsappmobil.scheduler;

namespace whatsappmobil.ssl
{
    public class Jobs
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Scheduler>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(10)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);

            //Job Looping Plan
            IScheduler schedulerPlan = StdSchedulerFactory.GetDefaultScheduler().Result;
            schedulerPlan.Start();
            IJobDetail jobPlan = JobBuilder.Create<ScheduledPlan>().WithIdentity("job2", "group2").Build();
            ITrigger triggerPlan = TriggerBuilder.Create().WithIdentity("trigger2", "group2")
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInSeconds(35)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(07, 30)).EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 0))
                  )
                .Build();

            schedulerPlan.ScheduleJob(jobPlan, triggerPlan);

            IScheduler schedulerPlanAfter = StdSchedulerFactory.GetDefaultScheduler().Result;
            schedulerPlanAfter.Start();
            IJobDetail jobPlanAfter = JobBuilder.Create<ScheduledPlanAfter>().WithIdentity("job3", "group3").Build();
            ITrigger triggerPlanAfter = TriggerBuilder.Create().WithIdentity("trigger3", "group3")
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInSeconds(5)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(21, 0)).EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 0))
                  )
                .Build();

            schedulerPlanAfter.ScheduleJob(jobPlanAfter, triggerPlanAfter);

            //IScheduler schedulerMessage = StdSchedulerFactory.GetDefaultScheduler().Result;
            //schedulerMessage.Start();
            //IJobDetail jobMessage = JobBuilder.Create<ScheduledMessage>().WithIdentity("job1", "group1").Build();
            //ITrigger triggerMessage = TriggerBuilder.Create().WithIdentity("trigger1", "group1")
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInMinutes(1)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(07, 0)).EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 30))
            //      )
            //    .Build();

            //schedulerMessage.ScheduleJob(jobMessage, triggerMessage);

        }
    }
}