using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Windy.WebMVC.Web2.Job
{
    public class SpiderPageJob : IJob
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
            try
            {
                //GlobalMethod.log.Info("爬虫运行:"+DateTime.Now.ToString());
                var result = EnterRepository.GetRepositoryEnter().BlogRepository.LoadEntities().OrderByDescending(m => m.ModifyTime).Take(10).ToList();
                foreach (var item in result)
                {
                    SpiderServices.SpiderRelationPage(item.ReprintUrl);
                }

                //GlobalMethod.log.Info("爬虫运行，抓取今日头条" + DateTime.Now.ToString());
                var result2 = EnterRepository.GetRepositoryEnter().ToutiaoRepository.LoadEntities().OrderByDescending(m => m.ModifyTime).Take(10).ToList();
                foreach (var item in result)
                {
                    SpiderServices.SpiderRelationPageToutiao(item.ReprintUrl);
                }
            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
            }
        }
    }
}