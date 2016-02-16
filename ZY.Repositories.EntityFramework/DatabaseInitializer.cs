using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Repositories.EntityFramework.Migrations;

namespace ZY.Repositories.EntityFramework
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void Initializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BaseDbContext, AutoMigrationsConfiguration>());
        }
    }
}
