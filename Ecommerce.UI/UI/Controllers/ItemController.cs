using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace UI.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration configuration;

        public ItemController(ILogger<ItemController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration iConfig)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            configuration = iConfig;
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index");
            try
            {
                var data = await WebApiClient.Get<List<Item>>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/Item/GetList/", HttpContext.Session.GetString("Token"));
                #region TempDataRegion
                if (TempData["Saved"] == null)
                {
                    TempData["Saved"] = false;
                }
                if (TempData["Update"] == null)
                {
                    TempData["Update"] = false;
                }
                if (TempData["Delete"] == null)
                {
                    TempData["Delete"] = false;
                }
                if (TempData["Cannot"] == null)
                {
                    TempData["Cannot"] = false;
                }
                #endregion
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("UserLogin", "Login");
            }

        }

        public async Task<IActionResult> Create()
        {
            var data = await WebApiClient.Get<List<ItemCategory>>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/GetList", HttpContext.Session.GetString("Token"));
            ViewBag.ItemCategory = new SelectList(data,"Id","Name");
            return View();
        }

        // POST: ItemCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemTrans itemTrans)
        {
            try
            {

                var data = await WebApiClient.Post<ItemTrans>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemStockTransaction/Post", HttpContext.Session.GetString("Token"), itemTrans);
                #region TempDataRegion
                TempData["Saved"] = true;
                TempData["Update"] = false;
                TempData["Delete"] = false;
                TempData["Cannot"] = false;
                #endregion
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Item");
            }
        }

        // GET: ItemCategoryController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                ItemTrans trans = new ItemTrans();
                var data = await WebApiClient.Get<ItemTrans>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemStockTransaction/GetByItemId/" + id + "", HttpContext.Session.GetString("Token"));
                trans.item = data.item;
                trans.itemDetails = data.itemDetails;
                var Getdata = await WebApiClient.Get<List<ItemCategory>>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/GetList", HttpContext.Session.GetString("Token"));
                ViewBag.ItemCategory = new SelectList(Getdata, "Id", "Name", trans.item.ItemCategoryId);
                trans.item.Id = id;
                return View("Create", trans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Item");
            }

           
        }

        // POST: ItemCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemTrans itemTrans)
        {
            try
            {
                var data = await WebApiClient.Put<ItemTrans>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemStockTransaction/Put", HttpContext.Session.GetString("Token"), itemTrans );
                #region TempDataRegion
                if (data.item.Errors.Count < 1)
                {

                    TempData["Delete"] = false;
                    TempData["Saved"] = false;
                    TempData["Update"] = true;
                    TempData["Cannot"] = false;

                }
                else
                {
                    TempData["Delete"] = false;
                    TempData["Saved"] = false;
                    TempData["Update"] = false;
                    TempData["Cannot"] = true;
                }
                #endregion
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Item");
            }
        }

        // GET: ItemCategoryController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var data = await WebApiClient.Get<Item>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/Item/GetById/" + id + "", HttpContext.Session.GetString("Token"));
                return View("Create", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Item");
            }
           
        }

        // POST: ItemCategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ItemTrans itemTrans)
        {
            try
            {
                itemTrans.item.IsDeleted = true;
                var data = await WebApiClient.Post<ItemTrans>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemStockTransaction/Post", HttpContext.Session.GetString("Token"), itemTrans);
                #region TempDataRegion
                if (data.item.Errors.Count < 1)
                {
                    TempData["Delete"] = true;
                    TempData["Saved"] = false;
                    TempData["Update"] = false;
                    TempData["Cannot"] = false;
                }
                else
                {
                    TempData["Delete"] = false;
                    TempData["Saved"] = false;
                    TempData["Update"] = false;
                    TempData["Cannot"] = true;
                }
                #endregion
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Item");
            }
        }
    }
}
