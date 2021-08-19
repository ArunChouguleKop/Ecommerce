using BusinessPersister.BuisnessObject;
using BusinessPersister.TransactionObject;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Configuration
{
    public static  class ConfigurationExtensionMethodscs
    {
        public static void AddBusinessPersister( this IServiceCollection services)
        {
            services.AddTransient<ItemCategoryPersister, ItemCategoryPersister>();
            services.AddTransient<ItemDetailsPersister, ItemDetailsPersister>();
            services.AddTransient<ItemPersister, ItemPersister>();
            services.AddTransient<UserPersister, UserPersister>();
            services.AddTransient<ItemStockTransaction, ItemStockTransaction>();
        }
    }
}
