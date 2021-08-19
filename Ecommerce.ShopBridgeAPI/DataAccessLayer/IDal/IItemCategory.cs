using Ecommerce.Model;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.IDal
{
    public interface IItemCategory
    {
        #region CURD
        
        ItemCategory Insert(ItemCategory itemCategory);
        ItemCategory Update(ItemCategory itemCategory);
        ItemCategory Delete(ItemCategory itemCategory);

        #endregion
        List<ItemCategory> Fetch(bool IsUsed = true);
        ItemCategory FetchById(Guid Id, bool IsUsed = true);

        bool CheckRefereceInItem(Guid Id, bool IsUsed = true);
    }
}
