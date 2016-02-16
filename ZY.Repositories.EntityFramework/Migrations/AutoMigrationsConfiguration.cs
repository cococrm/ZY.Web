
using System.Data.Entity.Migrations;


namespace ZY.Repositories.EntityFramework.Migrations
{
    /// <summary>
    /// 自动迁移设置
    /// </summary>
    public class AutoMigrationsConfiguration : DbMigrationsConfiguration<BaseDbContext>
    {
        public AutoMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
