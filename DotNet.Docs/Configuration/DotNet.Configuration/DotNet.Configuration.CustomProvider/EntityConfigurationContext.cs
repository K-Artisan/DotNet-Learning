using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.CustomProvider
{
    /// <summary>
    /// 内存数据库
    /// </summary>
    public class EntityConfigurationContext : DbContext
    {
        public DbSet<Settings> Settings { get; set; }

        public EntityConfigurationContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
