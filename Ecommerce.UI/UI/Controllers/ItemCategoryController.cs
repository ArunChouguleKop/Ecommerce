using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace UI.Controllers
{
    public class ItemCategoryController : Controller
    {
        // GET: ItemCategoryController
        private readonly ILogger<ItemCategoryController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration configuration;

        public ItemCategoryController(ILogger<ItemCategoryController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration iConfig)
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
                var data = await WebApiClient.Get<List<ItemCategory>>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/GetList", HttpContext.Session.GetString("Token"));
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

        // GET: ItemCategoryController/Details/5


        // GET: ItemCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemCategory itemCategory)
        {
            _logger.LogInformation("Create");
            try
            {


                var data = await WebApiClient.Post<ItemCategory>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/Post", HttpContext.Session.GetString("Token"), itemCategory);
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
                return RedirectToAction("Index", "ItemCategory");
            }
        }

        // GET: ItemCategoryController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            _logger.LogInformation("Edit");
            try
            {
                var data = await WebApiClient.Get<ItemCategory>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/GetById/" + id + "",  HttpContext.Session.GetString("Token"));
                return View("Create", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "ItemCategory");
            }

        }

        // POST: ItemCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemCategory itemCategory)
        {
            _logger.LogInformation("Edit");
            try
            {

                var data = await WebApiClient.Put<ItemCategory>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/Put", HttpContext.Session.GetString("Token"), itemCategory);
                #region TempDataRegion
                if (data.Errors.Count < 1)
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
                return RedirectToAction("Index", "ItemCategory");
            }
        }

        // GET: ItemCategoryController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Delete");
            try
            {
                var data = await WebApiClient.Get<ItemCategory>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/GetById/" + id + "", HttpContext.Session.GetString("Token"));
                return View("Create", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "ItemCategory");
            }

        }

        // POST: ItemCategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ItemCategory itemCategory)
        {
            _logger.LogInformation("Delete");
            try
            {
                ItemCategory itemCategory1 = new ItemCategory();
                itemCategory1 = itemCategory;
                var data = await WebApiClient.Put<ItemCategory>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/ItemCategory/Delete", HttpContext.Session.GetString("Token"), itemCategory1);
                #region TempDataRegion
                if (data.Errors.Count < 1)
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
                return RedirectToAction("Index", "ItemCategory");
            }
        }
    }
}
