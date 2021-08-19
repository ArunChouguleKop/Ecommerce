using DataAccessLayer.IDal;
using Ecommerce.Model;

namespace BusinessPersister.BuisnessObject
{
    public class UserPersister
    {
        private ILogin dal;
        public UserPersister(ILogin dal)
        {
            this.dal = dal;
        }

        public User Login(User user) 
        {
            user.IsUsed = true;
            user = dal.Login(user);
            return user;
        }
    }
}
