using System.Configuration;
using Autofac;
using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RabbitPoc
{
    public static class AutoFacBuilderExtensions
    {
        public static void RegisterConfigSettings<TSettings>(this ContainerBuilder builder)
        {
            var factory = new DictionaryAdapterFactory();
           
            builder.RegisterInstance((object)factory.GetAdapter<TSettings>(ConfigurationManager.AppSettings))
                .As<TSettings>()
                .SingleInstance();
        }
        
        public static string ToJson(this object obj)
        {
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(),  ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }
    }
}