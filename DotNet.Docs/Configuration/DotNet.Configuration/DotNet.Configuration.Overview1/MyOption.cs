using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.Overview1
{
    public class MyOption
    {
        public string Option1 { get; set; }
        public string Option2 { get; set; }

        public MyOption(IConfiguration configuration)
        {
            Option1 = configuration["Option1"];
            Option2 = configuration["Option2"];
        }

    }
}
