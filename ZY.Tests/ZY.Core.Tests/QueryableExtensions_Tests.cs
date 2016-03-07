using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using ZY.Core.Page;
using ZY.Core.Extensions;
using ZY.Core.Sort;
using ZY.Core.Web;

namespace ZY.Core.Tests
{
    [TestClass]
    public class QueryableExtensions_Tests
    {
        
        [TestMethod]
        public void QueryableExtensions_Test_ToPage()
        {
            IList<User> list = new List<User>();
            list.Add(new User(){Id = 1,Name = "zhangsan1",Age = 18,Address = "四川"});
            list.Add(new User() { Id = 2, Name = "zhangsan2", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 3, Name = "zhangsan3", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 4, Name = "zhangsan4", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 5, Name = "zhangsan5", Age = 18, Address = "四川" });

            IQueryable<User> query = list.AsQueryable();

            PagedResult<User> page = query.ToPage<User>();
            PagedResult<User> page1 = query.ToPage<User>(new Pager(1,1));
            SortCondition[] sort = new SortCondition[] { new SortCondition<User>(o => o.Id, ListSortDirection.Descending) };
            PagedResult<User> page2 = query.ToPage<User>(o=>o.Name.Contains("zhangsan"), new Pager(1, 1, sort));

            Assert.AreEqual(list.Count, page.Data.Count);
            Assert.AreEqual(1, page1.Data.Count);
            Assert.AreEqual(1, page2.Data.Count);
            Assert.AreEqual(5, page2.Data.First().Id);

            var entity = query.FirstOrDefault();
            var entity1 = query.FirstOrDefault(o => o.Age == 20);
            var entity2 = query.Single();
        }

        [TestMethod]
        public void FirstOrDefaultAndSigle_Test()
        {
            IList<User> list = new List<User>();
            list.Add(new User() { Id = 1, Name = "zhangsan1", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 2, Name = "zhangsan2", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 3, Name = "zhangsan3", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 4, Name = "zhangsan4", Age = 18, Address = "四川" });
            list.Add(new User() { Id = 5, Name = "zhangsan5", Age = 18, Address = "四川" });

            IQueryable<User> query = list.AsQueryable();

            Assert.AreEqual(list.First(), query.FirstOrDefault());
            Assert.AreEqual(null, query.FirstOrDefault(o => o.Age == 2));
            Assert.AreEqual(list.Single(), query.Single());
            Assert.AreEqual(null, query.Single(o => o.Age == 2));

        }

    }
}
