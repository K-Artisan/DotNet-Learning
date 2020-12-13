using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.CustomProvider
{
    public class EntityConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public EntityConfigurationSource(
         Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        #region IConfigurationSource 成员
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EntityConfigurationProvider(_optionsAction);
        } 
        #endregion
    }
}
