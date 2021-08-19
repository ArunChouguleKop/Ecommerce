using BusinessPersister.BuisnessObject;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ShopBridge.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemCategoryController : ControllerBase
    {
       
        private ItemCategoryPersister _itemCategoryPersister;
        public ItemCategoryController(ItemCategoryPersister itemCategoryPersister) 
        {
            this._itemCategoryPersister = itemCategoryPersister;
        }

        [HttpGet]
       public IActionResult GetList()
        {
            List<ItemCategory> List = _itemCategoryPersister.Get();
            return Ok(List);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetById(Guid Id)
        {
            ItemCategory data = _itemCategoryPersister.GetById(Id);
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post(ItemCategory itemCategory)
        {
            itemCategory= _itemCategoryPersister.Insert(itemCategory);
            return Ok(itemCategory);
        }

        [HttpPut]
        public IActionResult Put(ItemCategory itemCategory)
        {
            itemCategory = _itemCategoryPersister.Update(itemCategory);
            return Ok(itemCategory);
        }
        [HttpPut]
        public IActionResult Delete(ItemCategory itemCategory)
        {
            itemCategory = _itemCategoryPersister.Delete(itemCategory);
            return Ok(itemCategory);
        }
    }
}
