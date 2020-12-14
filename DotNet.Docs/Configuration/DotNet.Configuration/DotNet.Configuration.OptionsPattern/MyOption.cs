using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.OptionsPattern
{
    public class MyOption
    {
        public string Id { get; set; } 
        public string Option1 { get; set; }
        public string Option2 { get; set; }

        public MyOption()
        {
            Id = Guid.NewGuid().ToString()[^4..];
        }

    }
}
