using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.CustomProvider
{
    public class EntityConfigurationProvider : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;
        public EntityConfigurationProvider(
        Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        /// <summary>
        /// Loads (or reloads) the data for this provider.
        /// </summary>
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<EntityConfigurationContext>();

            _optionsAction(builder);

            //创建一个内存数据
            using var dbContext = new EntityConfigurationContext(builder.Options);
            dbContext.Database.EnsureCreated();

            //ConfigurationProvider的数据源
            this.Data = dbContext.Settings.Any()
                ? dbContext.Settings.ToDictionary(c => c.Id, c => c.Value)
                : CreateAndSaveDefaultValues(dbContext);
        }

        static IDictionary<string, string> CreateAndSaveDefaultValues(
                    EntityConfigurationContext context)
        {
            var settings = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase) //不缺乏大小写
            {
                ["EndpointId"] = "b3da3c4c-9c4e-4411-bc4d-609e2dcc5c67",
                ["DisplayLabel"] = "Widgets Incorporated, LLC.",
                ["WidgetRoute"] = "api/widgets"
            };

            context.Settings.AddRange(
                settings.Select(kvp => new Settings(kvp.Key, kvp.Value))
                        .ToArray());

            context.SaveChanges();

            return settings;
        }
    }
}