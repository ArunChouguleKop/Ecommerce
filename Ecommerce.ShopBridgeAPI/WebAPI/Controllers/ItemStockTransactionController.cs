using BusinessPersister.BuisnessObject;
using BusinessPersister.TransactionObject;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ShopBridge.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemStockTransactionController : ControllerBase
    {
        private ItemStockTransaction _itemStockTransaction;
        private ItemPersister itemPersister;
        public ItemStockTransactionController(ItemStockTransaction itemStockTransaction, ItemPersister itemPersister)
        {
            this._itemStockTransaction = itemStockTransaction;
            this.itemPersister = itemPersister;
        }

        [HttpPost]
        public IActionResult Post(ItemTrans itemTrans)
        {
            if (!itemTrans.item.IsDeleted)
            {
                itemTrans = _itemStockTransaction.Insert(itemTrans);
            }
            else 
            {
                itemPersister.Delete(itemTrans.item);
            }
            
            return Ok(itemTrans);
        }


        

        [HttpPut]
       
        public IActionResult Put(ItemTrans itemTrans)
        {
            itemTrans = _itemStockTransaction.Update(itemTrans);
            return Ok(itemTrans);
        }

        [HttpPost]
        public IActionResult ItemStockEffect(ItemTrans itemTrans)
        {
            itemTrans = _itemStockTransaction.Insert(itemTrans);
            return Ok(itemTrans);
        }
        [HttpGet]
        [Route("{ItemId}")]
        public IActionResult GetByItemId(Guid ItemId)
        {
            ItemTrans itemTrans = new ItemTrans();
            itemTrans = _itemStockTransaction.GetByItemId(ItemId);
            return Ok(itemTrans);
        }
    }
}
