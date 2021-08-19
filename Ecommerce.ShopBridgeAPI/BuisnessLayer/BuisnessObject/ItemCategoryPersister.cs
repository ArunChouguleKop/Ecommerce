using DataAccessLayer.IDal;
using Ecommerce.Model;
using System;
using System.Collections.Generic;

namespace BusinessPersister.BuisnessObject
{
    public  class ItemCategoryPersister 
    {
        private IItemCategory dal;
        public ItemCategoryPersister(IItemCategory dal)
        {
            this.dal = dal;
        }
        public ItemCategoryPersister()
        {
            
        }

        public virtual List<ItemCategory> Get( bool IsUsed = true) 
        {
            List<ItemCategory> List = new List<ItemCategory>();
            List = dal.Fetch();
            return List;
        }

        public virtual ItemCategory Insert(ItemCategory itemCategory) 
        {
            if (itemCategory.Name.Length > 20) 
            {
                itemCategory.Errors.Add("Only 20 Charectors allowed.");
            }
            else 
            {
                itemCategory.IsUsed = true;
                itemCategory = dal.Insert(itemCategory);
            }
           
            return itemCategory;
        }

        public virtual ItemCategory Update(ItemCategory itemCategory)
        {
            if (itemCategory.Name.Length > 20)
            {
                itemCategory.Errors.Add("Only 20 Charectors allowed.");
            }
            else 
            {
                if (!dal.CheckRefereceInItem(itemCategory.Id, true))
                {
                    itemCategory.IsUsed = true;
                    itemCategory = dal.Update(itemCategory);
                }
                else
                {
                    itemCategory.Errors.Add("cannot update item listed under this category");
                }
            }
         
            return itemCategory;
        }
        public virtual ItemCategory Delete(ItemCategory itemCategory)
        {
            if (!dal.CheckRefereceInItem(itemCategory.Id, true))
            {
                itemCategory.IsUsed = false;
                itemCategory = dal.Update(itemCategory);
            }
            else 
            {
               
                
                itemCategory.Errors.Add("cannot delete item listed under this category");
            }
            return itemCategory;
        }

        public virtual ItemCategory GetById(Guid Id, bool IsUsed = true) 
        {
            ItemCategory data = new ItemCategory();
            data = dal.FetchById(Id);
            return data;
        }

        public virtual bool CheckRefereceInItem(Guid Id, bool IsUsed = true) 
        {
            var IsRefered = dal.CheckRefereceInItem(Id, IsUsed);
            return IsRefered;
        }
    }
}
