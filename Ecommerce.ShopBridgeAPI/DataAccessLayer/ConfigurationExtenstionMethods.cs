using BusinessLogics.Bal;
using DataAccessLayer.IDal;
using DataAccessLayer.PgsqlDal;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Configuration
{
    public static class ConfigurationExtenstionMethods
    {
        public static void AddDalPGSQL(this IServiceCollection services)
        {
            services.AddSingleton<IItemCategory, ItemCategoryDal>();
            services.AddSingleton<IItem, ItemDal>();
            services.AddSingleton<IItemDetails, ItemDetailsDal>();
            services.AddSingleton<ILogin, LoginDal>();
        }
        
    }
}
