using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IDal
{
    public interface IItem
    {
        #region CURD

        Item Insert(Item Item);
        Item Update(Item Item);
        Item Delete(Item Item);
       
        #endregion
        List<Item> Fetch( bool IsUsed = true);
        Item FetchById(Guid Id, bool IsUsed = true);
        Item AddStock(Item Item);
        Item ConsumeStock(Item Item);
    }
}
