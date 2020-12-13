using DotNet.Configuration.Overview1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace DotNet.Configuration.Overview2
{

    /*
       Cofiguration三要素:
          - IConfiguration：提供应用程序使用
          - IConfigurationBuilder：IConfiguration的创建者
          - IConfigurationSource：其对象实例代表配置数据的来源,例如：MeoryConfigurationSource


    使用流程，

    1. 若干个不同或相同类型的`IConfigurationSource`注册到`IConfigurationBuilder`,
    2. `IConfigurationBuilder`创建出`IConfiguration`对象
    3. 应用程序使用`IConfiguration`对象访问配置信息

    */

    class Program
    {
        static void Main(string[] args)
        {
            var sourceData = new Dictionary<string, string>
            {
                ["Option1"] = "Option-01",
                ["Option2"] = "Option-02"
            };

            var cfgSource = new MemoryConfigurationSource()
            {
                InitialData = sourceData
            };

            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.Add(cfgSource);

            var configuration = cfgBuilder.Build();

            var myOption = new MyOption(configuration);
            Console.WriteLine($"Option1: {myOption.Option1}");
            Console.WriteLine($"Option2: {myOption.Option2}");

        }

    }
}
