using BusinessPersister.BuisnessObject;
using Ecommerce.Model;
using System;
using System.Transactions;

namespace BusinessPersister.TransactionObject
{
    public class ItemStockTransaction
    {
        private ItemPersister itemPersister;
        private ItemDetailsPersister itemDetailsPersister;

        public ItemStockTransaction(ItemPersister itemPersister, ItemDetailsPersister itemDetailsPersister)
        {
            this.itemPersister = itemPersister;
            this.itemDetailsPersister = itemDetailsPersister;

        }
        public ItemStockTransaction()
        {
            
        }
        public virtual ItemTrans Insert(ItemTrans itemTrans)
        {

                //Define the scope for bundling the transaction
                using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {

                    if (itemTrans.itemDetails.Cost > 0)
                    {
                        itemTrans.item.IsUsed = itemTrans.item.IsDeleted == true ? false : true;
                        itemTrans.item = itemPersister.Insert(itemTrans.item);
                        if (itemTrans.itemDetails == null)
                        {
                            itemTrans.itemDetails = new ItemDetails();
                        }
                        itemTrans.itemDetails.ItemId = itemTrans.item.Id;
                        itemTrans.itemDetails.IsUsed = itemTrans.item.IsDeleted == true ? false : true;
                        itemTrans.itemDetails = itemDetailsPersister.Insert(itemTrans.itemDetails);
                    }
                    else
                    {
                        itemTrans.item.Errors.Add("Only Positive Cost Are Allowed.");
                    }
                    if (itemTrans.item.StockIn < 1 && itemTrans.item.StockOut < 1)
                    {
                        itemTrans.item.Errors.Add("Only Positive Stock And Consuption Allowed.");
                    }
 
                        txscope.Complete();
                    }
                    catch
                    {
                    }

                }
            return itemTrans;
        }

        public virtual ItemTrans Update(ItemTrans itemTrans)
        {


            if (itemTrans.item.IsComsumed)
            {
                if (itemTrans.item.StockOut > 0)
                {
                    itemTrans.item = itemPersister.ConsumeStock(itemTrans.item);
                }
                else
                {
                    itemTrans.item.Errors.Add("Only Positive Stock And Consuption Allowed.");
                }

            }
            else
            {
                if (itemTrans.item.StockIn > 0)
                {
                    itemTrans.item = itemPersister.AddStock(itemTrans.item);
                }
                else
                {
                    itemTrans.item.Errors.Add("Only Positive Stock And Consuption Allowed.");
                }
            }







            return itemTrans;
        }



        public virtual ItemTrans GetByItemId(Guid ItemId)
        {
            ItemTrans itemTrans = new ItemTrans();
            itemTrans.item = itemPersister.GetById(ItemId);
            itemTrans.itemDetails = itemDetailsPersister.GetByItemId(ItemId);
            return itemTrans;
        }


    }
}
