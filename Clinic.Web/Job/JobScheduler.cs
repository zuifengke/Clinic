using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace Windy.WebMVC.Web2.Job
{
    public class JobScheduler
    {
        public static void Start()
        {
            try
            {

                GlobalMethod.log.Info("start");
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<SpiderPageJob>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "group")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(2)
                .RepeatForever())
                .Build();

                scheduler.ScheduleJob(job, trigger);


                GlobalMethod.log.Info("爬虫任务启动成功");

                IJobDetail job2 = JobBuilder.Create<ClearFileJob>().Build();
                ITrigger trigger2 = TriggerBuilder.Create()
               .WithIdentity("trigger2", "group")
               .StartNow()
               .WithSimpleSchedule(x => x
               .WithIntervalInMinutes(2)
               .RepeatForever())
               .Build();

                scheduler.ScheduleJob(job2, trigger2);

                GlobalMethod.log.Info("清理病毒文件启动成功");

                IJobDetail job3= JobBuilder.Create<UpdateIndexJob>().Build();
                ITrigger trigger3 = TriggerBuilder.Create()
               .WithIdentity("trigger3", "group")
               .StartNow()
               .WithSimpleSchedule(x => x
               .WithIntervalInMinutes(60)
               .RepeatForever())
               .Build();

                scheduler.ScheduleJob(job3, trigger3);

                GlobalMethod.log.Info("更新主页任务启动成功");
                GlobalMethod.log.Info("start end");
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
            }
        }
    }
}