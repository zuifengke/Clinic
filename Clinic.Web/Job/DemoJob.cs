using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2.Job
{
    public class DemoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //This method will run every 10 seconds which setted in JobScheduler class with following code
            //ITrigger trigger = TriggerBuilder.Create()
            //.WithIdentity("trigger1", "group1")
            //.StartNow()
            //.WithSimpleSchedule(x => x
            //.WithIntervalInSeconds(10)
            //.RepeatForever())
            //.Build();
            //GlobalMethod.log.Info("demojob-" + DateTime.Now.ToString());
            //Console.WriteLine("hello1");
        }
    }
}