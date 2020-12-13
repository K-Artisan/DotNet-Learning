
# 参考文档
https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration



# 配置文件三要素

- IConfiguration：提供应用程序使用

- IConfigurationBuilder：IConfiguration的创建者

- IConfigurationSource：其对象实例代表配置数据的来源

  例如：MeoryConfigurationSource

 使用流程，

1. 若干个不同或相同类型的`IConfigurationSource`注册到`IConfigurationBuilder`,
2. `IConfigurationBuilder`创建出`IConfiguration`对象
3. 应用程序使用`IConfiguration`对象访问配置信息



【示例：DotNet.Configuration.Overview1】



MyOption.cs

```C#
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
```

main.cs

```C#
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

            var configration = cfgBuilder.Build();

            var myOption = new MyOption(configration);
            Console.WriteLine($"Option1: {myOption.Option1}");
            Console.WriteLine($"Option2: {myOption.Option2}");

        }

    }
```





# 控制台程序的配置

```C#
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Console.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
    }
}
```



The [Host.CreateDefaultBuilder(String[\])](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder#Microsoft_Extensions_Hosting_Host_CreateDefaultBuilder_System_String___) method provides default configuration for the app in the following order:

1. [ChainedConfigurationProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.chainedconfigurationsource) : Adds an existing `IConfiguration` as a source.
2. *appsettings.json* using the [JSON configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#file-configuration-provider).
3. *appsettings.*`Environment`*.json* using the [JSON configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#file-configuration-provider). For example, *appsettings*.***Production***.*json* and *appsettings*.***Development***.*json*.
4. App secrets when the app runs in the `Development` environment.
5. Environment variables using the [Environment Variables configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#environment-variable-configuration-provider).
6. Command-line arguments using the [Command-line configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#command-line-configuration-provider).

![image-20201213174020656](images/DotNet.Configuration-Notes/image-20201213174020656.png)



【示例：DotNet.Configuration.Overview2】

```C#
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration
    /// </summary>
    class Program
    {
        static Task Main(string[] args)
        {
            using (IHost host = CreateHostBuilder(args).Build())
            {
                ExemplifyConfiguration(host.Services);

                return host.RunAsync();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    //configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("myoption.json", optional: false); //appsettings.json 存在同名节点，优先取appsettings.json中的配置
                    //configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    //configHost.AddCommandLine(args);
                });
        }

        static void ExemplifyConfiguration(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            //依赖注入中获取IConfiguration
            var configuration = provider.GetRequiredService<IConfiguration>();

            //自己创建获取IConfiguration
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", false)
            //    .Build();

            //---------------------获取配置值的方法--------------------------------
            //GetSection(xxx:x1)
            var myOption1 = configuration.GetSection("MyOption:Option1")?.Value;
            Console.WriteLine($"Option1: {myOption1}");

            //GetSection(xxx)[x1]
            var myOption2 = configuration.GetSection("MyOption")?["Option2"];
            Console.WriteLine($"Option2: {myOption2}");

            //创建对象
            var myOption = configuration.GetSection("MyOption").Get<MyOption>();
            Console.WriteLine($"Option1: {myOption?.Option1}");
            Console.WriteLine($"Option2: {myOption?.Option2}");
        }
    }
```





# Configuration providers

## File configuration provider

[FileConfigurationProvider](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.fileconfigurationprovider) is the base class for loading configuration from the file system. The following configuration providers derive from `FileConfigurationProvider`:

- [JSON configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#json-configuration-provider)
- [XML configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#xml-configuration-provider)
- [INI configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#ini-configuration-provider)



【示例：DotNet.Configuration.Provider1】

`appsettings.json`:

```C#
{
  "MyOption": {
    "Option1": "Option-01 in appsettings.json",
    "Option2": "Option-02 in appsettings.json"
  }
}

```

`appsettings.xml`:

```C#
<configuration>
  <SecretKey>Secret key value</SecretKey>
  <TransientFaultHandlingOptions>
    <Enabled>true</Enabled>
    <AutoRetryDelay>00:00:07</AutoRetryDelay>
  </TransientFaultHandlingOptions>
  <Logging>
    <LogLevel>
      <Default>Information</Default>
      <Microsoft>Warning</Microsoft>
    </LogLevel>
  </Logging>
</configuration>
```

Program.cs:

```C#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DotNet.Configuration.Provider1
{
    class Program
    {

        static async Task Main(string[] args)
        {

            using IHost host = CreateHostBuilder(args).Build();

            // Application code should start here.

            await host.RunAsync();
        }


        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) //using JSON configuration provider
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddXmlFile("appsettings.xml", optional: true, reloadOnChange: true); //using xml provider

                    IConfigurationRoot configurationRoot = configuration.Build();

                    //--------Json Configuration Provider--------
                    var optionsJson = new MyOption();
                    configurationRoot.GetSection(nameof(MyOption))
                                     .Bind(optionsJson);

                    Console.WriteLine("--------Json Configuration Provider--------");
                    Console.WriteLine($"MyOption->Option1: {optionsJson?.Option1}");
                    Console.WriteLine($"MyOption->Option2: {optionsJson?.Option2}");


                    //--------Xml Configuration Provider--------
                    var optionsXml = new TransientFaultHandlingOptions();
                    configurationRoot.GetSection(nameof(TransientFaultHandlingOptions))
                                     .Bind(optionsXml);
                    Console.WriteLine("--------Xml Configuration Provider--------");
                    Console.WriteLine($"TransientFaultHandlingOptions->Option1: {optionsXml?.Enabled}");
                    Console.WriteLine($"TransientFaultHandlingOptions->Option2: {optionsXml?.AutoRetryDelay}");

                });
    }
}

```

输出：

```powershell
--------Json Configuration Provider--------
MyOption->Option1: Option-01 in appsettings.json
MyOption->Option2: Option-02 in appsettings.json
--------Xml Configuration Provider--------
TransientFaultHandlingOptions->Option1: True
TransientFaultHandlingOptions->Option2: 00:00:07
```



### JSON configuration provider

源码：https://github.com/dotnet/runtime/tree/master/src/libraries/Microsoft.Extensions.Configuration.Json/src



## 自定义 configuration provider

文档：https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-configuration-provider



[示例：DotNet.Configuration.CustomProvider]

跟踪源码分析：

```C#
configuration.AddEntityConfiguration(
                    options => options.UseInMemoryDatabase("InMemoryDb")); //EF Core 内存数据库
    public static IConfigurationBuilder AddEntityConfiguration(this IConfigurationBuilder builder,
        Action<DbContextOptionsBuilder> optionsAction)
    {
        return builder.Add(new EntityConfigurationSource(optionsAction));
    }
```

-->

**builder.Add(new EntityConfigurationSource(optionsAction));**

```C#
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        /// <summary>
        /// Returns the sources used to obtain configuration values.
        /// </summary>
        public IList<IConfigurationSource> Sources { get; } = new List<IConfigurationSource>();
        
        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }
    }
```

-->

**IConfigurationRoot configurationRoot = configuration.Build();**

```C#
        public IConfigurationRoot Build()
        {
            var providers = new List<IConfigurationProvider>();
            foreach (IConfigurationSource source in Sources)
            {
                IConfigurationProvider provider = source.Build(this);
                providers.Add(provider);
            }
            return new ConfigurationRoot(providers);
        }
```

-->source.Build(this);

-->**EntityConfigurationSource.Build(IConfigurationBuilder builder)**

```C#
    public class EntityConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;
        ....
            
        #region IConfigurationSource 成员
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EntityConfigurationProvider(_optionsAction);
        } 
        #endregion
    }
```



-->**return new ConfigurationRoot(providers)**

```C#
    /// <summary>
    /// The root node for a configuration.
    /// </summary>
    public class ConfigurationRoot : IConfigurationRoot, IDisposable
    {
        private readonly IList<IConfigurationProvider> _providers;

        /// <summary>
        /// Initializes a Configuration root with a list of providers.
        /// </summary>
        /// <param name="providers">The <see cref="IConfigurationProvider"/>s for this configuration.</param>
        public ConfigurationRoot(IList<IConfigurationProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }
            _providers = providers;
            _changeTokenRegistrations = new List<IDisposable>(providers.Count);
            foreach (IConfigurationProvider p in providers)
            {
                p.Load();
               _changeTokenRegistrations.Add(ChangeToken.OnChange(() => p.GetReloadToken(), () => RaiseChanged())); //如果有变化重新加载
            }
        }
```

-->p.Load();-->**EntityConfigurationProvider.Load()**

```C#
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
```



- 获取值的方式：

-->   **var myOption1 = configurationRoot.GetSection("EndpointId")?.Value;**

-->configurationRoot.GetSection

```C#
public IConfigurationSection GetSection(string key)
        => new ConfigurationSection(this, key);
```
ConfigurationSection .Value

```C#
public class ConfigurationSection : IConfigurationSection
{
        public ConfigurationSection(IConfigurationRoot root, string path)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _root = root;
            _path = path;
        }
    

        /// <summary>
        /// Gets or sets the section value.
        /// </summary>
        public string Value
        {
            get
            {
                return _root[Path];
            }
            set
            {
                _root[Path] = value;
            }
        }
}
```



​      **var myOption2 = configurationRoot["DisplayLabel"];**

```C#
    public class ConfigurationRoot : IConfigurationRoot, IDisposable
    {    
        public string this[string key]
        {
            get
            {
                for (int i = _providers.Count - 1; i >= 0; i--)
                {
                    IConfigurationProvider provider = _providers[i];

                    if (provider.TryGet(key, out string value))
                    {
                        return value;
                    }
                }

                return null;
            }
            set
            {
                if (!_providers.Any())
                {
                    throw new InvalidOperationException(SR.Error_NoSources);
                }

                foreach (IConfigurationProvider provider in _providers)
                {
                    provider.Set(key, value);
                }
            }
        }
```

