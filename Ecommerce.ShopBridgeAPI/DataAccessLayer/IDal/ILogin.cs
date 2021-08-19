using Ecommerce.Model;

namespace DataAccessLayer.IDal
{
    public interface ILogin
    {
        User Login(User applicationUser);
    }
}
