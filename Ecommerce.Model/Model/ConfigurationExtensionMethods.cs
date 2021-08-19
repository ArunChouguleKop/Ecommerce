using Ecommerce.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Configuration
{
    public static class ConfigurationExtensionMethods
    {
        public static void AddBuisnessModel(this IServiceCollection services)
        {
            services.AddTransient<CommonProperties, CommonProperties>();
            services.AddTransient<Item, Item>();
            services.AddTransient<ItemCategory, ItemCategory>();
            services.AddTransient<ItemDetails, ItemDetails>();
            services.AddTransient<ItemTrans, ItemTrans>();

        }
    }
}
