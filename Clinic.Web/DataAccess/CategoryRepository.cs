using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Windy.WebMVC.Web2.Models;

namespace Windy.WebMVC.Web2.EFDao
{
    public class CategoryRepository : BaseRepository<Models.Category>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetCategorys(string categoryCode)
        {
            var result = from p in db.Set<Category>()
                         join a in db.Set<Category>() on new { ID = p.ParentID } equals new { ID = a.ID } into a_into
                         from a in a_into.DefaultIfEmpty()
                         select new
                         {
                             p.ID,
                             p.Name,
                             p.Code,
                             p.ParentID,
                             ParentName = a.Name
                         };
            if (!string.IsNullOrEmpty(categoryCode))
                result = result.Where(m => m.Code.Contains(categoryCode));
            return result.ToList().Select(m => new Category() {  Name = m.Name, ID = m.ID, ParentID = m.ParentID,  Code = m.Code, ParentName = m.ParentName });
        }
        
    }
}