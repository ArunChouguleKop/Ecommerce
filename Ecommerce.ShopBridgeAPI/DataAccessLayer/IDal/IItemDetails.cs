using Ecommerce.Model;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.IDal
{
    public interface IItemDetails
    {
        #region CURD

        ItemDetails Insert(ItemDetails ItemDetails);
        ItemDetails Update(ItemDetails ItemDetails);
        ItemDetails Delete(ItemDetails ItemDetails);

        #endregion
        List<ItemDetails> Fetch(bool IsUsed = true);
        ItemDetails FetchByItemId(Guid ItemId, bool IsUsed = true);
    }
}
