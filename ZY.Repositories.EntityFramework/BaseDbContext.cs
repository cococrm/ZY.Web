using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace ZY.Repositories.EntityFramework
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext()
            : base("Default")
        { }

        public BaseDbContext(string connectionString)
            :base(connectionString)
        { }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            IEnumerable<IEntityMapper> entityMappers = GetEntityMappers().Select(type => Activator.CreateInstance(type) as IEntityMapper).ToList();

            foreach (IEntityMapper mapper in entityMappers)
            {
                mapper.RegistorTo(modelBuilder.Configurations);
            }
        }

        /// <summary>
        /// 获取所有实体映射对象
        /// </summary>
        /// <returns></returns>
        private Type[] GetEntityMappers()
        {
            Type[] mapperTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
            type.BaseType.GetInterface(typeof(IEntityMapper).Name) == typeof(IEntityMapper)).ToArray();
            return mapperTypes;
        }
    }
}
