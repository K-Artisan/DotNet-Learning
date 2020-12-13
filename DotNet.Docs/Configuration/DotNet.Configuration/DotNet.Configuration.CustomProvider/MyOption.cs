using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Configuration.CustomProvider
{
    public class MyOption
    {
        public string EndpointId { get; set; }
        public string DisplayLabel { get; set; }
        public string WidgetRoute { get; set; }


    }
}
