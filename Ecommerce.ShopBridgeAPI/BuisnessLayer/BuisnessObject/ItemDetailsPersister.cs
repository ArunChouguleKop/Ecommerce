using DataAccessLayer.IDal;
using Ecommerce.Model;
using System;
using System.Collections.Generic;

namespace BusinessPersister.BuisnessObject
{
    public class ItemDetailsPersister
    {
        private IItemDetails dal;
        public ItemDetailsPersister(IItemDetails dal)
        {
            this.dal = dal;
        }
        public ItemDetailsPersister()
        {
          
        }

        public virtual List<ItemDetails> Get(bool IsUsed = true)
        {
            List<ItemDetails> List = new List<ItemDetails>();
            List = dal.Fetch();
            return List;
        }

        public virtual ItemDetails Insert(ItemDetails ItemDetails)
        {
            ItemDetails.IsUsed = true;
            ItemDetails = dal.Insert(ItemDetails);
            return ItemDetails;
        }

        public virtual ItemDetails Update(ItemDetails ItemDetails)
        {

            ItemDetails.IsUsed = true;
            ItemDetails = dal.Update(ItemDetails);
           
            return ItemDetails;
        }
        public virtual ItemDetails Delete(ItemDetails ItemDetails)
        {

            ItemDetails.IsUsed = false;
            ItemDetails = dal.Update(ItemDetails);
            
            return ItemDetails;
        }

        public virtual ItemDetails GetByItemId(Guid ItemId, bool IsUsed = true)
        {
            ItemDetails data = new ItemDetails();
            data = dal.FetchByItemId(ItemId);
            return data;
        }

       
    }
}
