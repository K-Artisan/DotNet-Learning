using System;
using DotNet.Configuration.CustomProvider;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// IConfigurationBuilder 扩展方法
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddEntityConfiguration(this IConfigurationBuilder builder,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return builder.Add(new EntityConfigurationSource(optionsAction));
        }
    }
}