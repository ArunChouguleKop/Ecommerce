using BusinessPersister.BuisnessObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ShopBridge.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemController : ControllerBase
    {
        private ItemPersister _itemPersister;
        public ItemController(ItemPersister itemPersister)
        {
            this._itemPersister = itemPersister;
        }
        [HttpGet]
        
        public IActionResult GetList()
        {
            List<Item> List = _itemPersister.Get();
            return Ok(List);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetById(Guid Id)
        {
            Item data = _itemPersister.GetById(Id);
            return Ok(data);
        }

        [HttpPost]
       
        public IActionResult Post(Item item)
        {
            item = _itemPersister.Insert(item);
            return Ok(item);
        }

        [HttpPut]
       
        public IActionResult Put(Item item)
        {
            item = _itemPersister.Update(item);
            return Ok(item);
        }
        [HttpPut]
        
        public IActionResult Delete(Item item)
        {
            item = _itemPersister.Delete(item);
            return Ok(item);
        }
    }
}
