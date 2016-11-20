using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Top.Api.Response;
using Windy.WebMVC.Web2.Helpers;
using Windy.WebMVC.Web2.Utility;

namespace Windy.WebMVC.Web2
{
    public class ProductServices
    {
        /// <summary>
        /// 获取分页文章列表
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<Models.Product> GetProducts(string keyword,string categoryCode,string order, int page, int size, out int totalCount)
        {
            int skip = (page - 1) * size;
            var result = EnterRepository.GetRepositoryEnter().ProductRepository.GetProducts(keyword, categoryCode,order,skip,size,out totalCount).ToList();
            return result;
        }
        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Models.Product GetProduct(int id)
        {

            //获取用户及角色已授权的权限
            var result = EnterRepository.GetRepositoryEnter().ProductRepository.GetProduct(id);

            return result;
        }
        public static bool Import(int nCategoryID,string filePath, string szTemplateValue)
        {
            try
            {
                GlobalMethod.log.Info("开始导入");
                EnterRepository.GetRepositoryEnter().ProductRepository.db.Configuration.AutoDetectChangesEnabled = false;
                EnterRepository.GetRepositoryEnter().ProductRepository.db.Configuration.ValidateOnSaveEnabled = false;
                Hashtable htProducts = new Hashtable();
                var list = EnterRepository.GetRepositoryEnter().ProductRepository.LoadEntities().ToList();
                foreach (var item in list)
                {
                    if (!htProducts.ContainsKey(item.TaobaoID))
                        htProducts.Add(item.TaobaoID, item.ID);
                }
                string[] values = szTemplateValue.Split(',');
                ExcelProvider excelProvider = ExcelProvider.Create(filePath, "Page1");
                int passCount = 0;
                int importCount = 0;
                foreach (ExcelRow row in excelProvider)
                {
                    int ID = 0;
                    Models.Product product = new Models.Product();
                    for (int index = 0; index < values.Length; index++)
                    {
                        switch (values[index])
                        {
                            case "商品id":
                                if (!string.IsNullOrEmpty(row.GetString("商品id")))
                                    product.TaobaoID = row.GetString(values[index]);
                                break;
                            case "商品名称":
                                product.Title = row.GetString(values[index]);
                                break;
                            case "商品主图":
                                product.ImagePath = row.GetString(values[index]);
                                break;
                            case "店铺名称":
                                product.ShopName = row.GetString(values[index]);
                                break;
                            case "商品价格(单位：元)":
                                product.Price = float.Parse(row.GetString(values[index]));
                                break;
                            case "收入比率(%)":
                                product.Percent = float.Parse(row.GetString(values[index]));
                                break;
                            case "佣金":
                                product.Income = float.Parse(row.GetString(values[index]));
                                break;
                            case "商品月销量":
                                product.Sales = int.Parse(row.GetString(values[index]));
                                break;
                            case "淘宝客链接":
                                product.Url = row.GetString(values[index]);
                                break;
                            case "商品详情页链接地址":
                                product.DetailUrl = row.GetString(values[index]);
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(product.TaobaoID))
                    {
                        passCount++;
                        GlobalMethod.log.Warn(string.Format("导入excel,淘宝商品{0}ID号码为空，跳过", product.Title));
                        continue;
                    }
                    product.Keywords = product.Title;
                    if (!htProducts.ContainsKey(product.TaobaoID))
                    {
                        product.CategoryID = nCategoryID;
                        product.CreateID = WebCookieHelper.GetEmployeeId();
                        product.CreateTime = DateTime.Now;
                        product.ModifyID = WebCookieHelper.GetEmployeeId();
                        product.ModifyTime = DateTime.Now;
                        EnterRepository.GetRepositoryEnter().ProductRepository.AddEntity(product);
                    }
                    else
                    {
                        product.CategoryID = nCategoryID;
                        product.ModifyTime = DateTime.Now;
                        product.ID = int.Parse(htProducts[product.TaobaoID].ToString());
                        EnterRepository.GetRepositoryEnter().ProductRepository.EditEntity(
                            product, new string[] {"Title","ModifyTime","Percent","Url", "DetailUrl", "Income","Price","ShopName","ImagePath", "CategoryID", "Sales","Keywords" });
                    }
                }
                importCount = EnterRepository.GetRepositoryEnter().SaveChange();
                GlobalMethod.log.Info(string.Format("本次导入成功，共导入{0}商品,应数据异常跳过{1}个"
                       , importCount.ToString()
                       , passCount.ToString()));
                return true;

            }
            catch (Exception ex)
            {
                GlobalMethod.log.Error(ex);
                throw;
            }
            finally
            {

                EnterRepository.GetRepositoryEnter().UsersRepository.db.Configuration.AutoDetectChangesEnabled = true;
                EnterRepository.GetRepositoryEnter().UsersRepository.db.Configuration.ValidateOnSaveEnabled = true;
            }
        }
        /// <summary>
        /// 通过淘宝API导入选品库商品信息
        /// </summary>
        public static void ImportByTopApi()
        {

            GlobalMethod.log.Info("开始通过Api导入淘宝推广商品");
            EnterRepository.GetRepositoryEnter().ProductRepository.db.Configuration.AutoDetectChangesEnabled = false;
            EnterRepository.GetRepositoryEnter().ProductRepository.db.Configuration.ValidateOnSaveEnabled = false;
            Hashtable htProducts = new Hashtable();
            var list = EnterRepository.GetRepositoryEnter().ProductRepository.LoadEntities().ToList();
            foreach (var item in list)
            {
                if (!htProducts.ContainsKey(item.TaobaoID))
                    htProducts.Add(item.TaobaoID, item.ID);
            }
            //获取选品库列表
            TbkUatmFavoritesGetResponse res = TopSDKHelper.GetTbkUatmFavoritesGetResponse(null);
            //取最新的选品库，将商品导入数据库
            if (res == null || res.Results.Count == 0)
            {
                GlobalMethod.log.Info("选品库列表为空");
                return;
            }
            long favoritesId = res.Results[0].FavoritesId;
            TbkUatmFavoritesItemGetResponse res2 = TopSDKHelper.GetTbkUatmFavoritesItemGetResponse(favoritesId);
            if (res2 == null || res2.Results.Count == 0)
            {
                GlobalMethod.log.Info("选品库商品为空");
                return;
            }
            int passCount = 0;
            int importCount = 0;
            int nCategoryID = EnterRepository.GetRepositoryEnter().CategoryRepository.LoadEntities(m => m.Code == SystemConst.CategoryCode.AdvertTaobao).FirstOrDefault().ID;
            foreach (var item in res2.Results)
            {
                Models.Product product = new Models.Product();
                product.TaobaoID = item.NumIid.ToString();
                product.CategoryID = nCategoryID;
                product.DetailUrl = item.ItemUrl;
                product.Percent = float.Parse(item.TkRate);
                product.ImagePath = item.PictUrl;
                product.Income = float.Parse(item.ZkFinalPrice) * product.Percent / 100;
                product.Price = float.Parse(item.ZkFinalPrice);
                product.Sales = int.Parse(item.Volume.ToString());
                product.Url = item.ClickUrl;
                product.ShopName = item.ShopTitle;
                product.Title = item.Title;

                if (string.IsNullOrEmpty(product.TaobaoID))
                {
                    passCount++;
                    GlobalMethod.log.Warn(string.Format("导入excel,淘宝商品{0}ID号码为空，跳过", product.Title));
                    continue;
                }
                product.Keywords = product.Title;
                if (!htProducts.ContainsKey(product.TaobaoID))
                {
                    product.CreateID = WebCookieHelper.GetEmployeeId();
                    product.CreateTime = DateTime.Now;
                    product.ModifyID = WebCookieHelper.GetEmployeeId();
                    product.ModifyTime = DateTime.Now;
                    EnterRepository.GetRepositoryEnter().ProductRepository.AddEntity(product);
                }
                else
                {
                    product.ModifyTime = DateTime.Now;
                    product.ID = int.Parse(htProducts[product.TaobaoID].ToString());
                    EnterRepository.GetRepositoryEnter().ProductRepository.EditEntity(
                        product, new string[] { "Title", "ModifyTime", "Percent", "Url", "DetailUrl", "Income", "Price", "ShopName", "ImagePath", "CategoryID", "Sales", "Keywords" });
                }
            }
            importCount = EnterRepository.GetRepositoryEnter().SaveChange();
            GlobalMethod.log.Info(string.Format("本次导入成功，共导入{0}商品,应数据异常跳过{1}个"
                   , importCount.ToString()
                   , passCount.ToString()));

        }
    }
}