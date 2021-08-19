using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using System;
using System.Threading.Tasks;
using UI.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace UI.Controllers
{

    public class LoginController : Controller
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration configuration;

        public LoginController(ILogger<LoginController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration iConfig)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            configuration = iConfig;
        }
        public IActionResult UserLogin()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            _logger.LogInformation("Requested the Get UserLogin");
            string ErrorMsg = "";
            try
            {
                var data = await WebApiClient.Post<User>(configuration.GetValue<string>("AppSettings:WebAPIStoreBridgeDomain"), "api/Token/GetToken", null, user);
                if(data != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("Token", data.Token);
                    ViewBag.Name = data.IsAdmin;
                    if (data.IsUsed)
                    {
                        if (data.IsAdmin)
                        {
                            return RedirectToAction("Index", "ItemCategory");
                        }
                    }


                }
                else
                {
                    ErrorMsg = "User name or password is wrong";
                    ModelState.AddModelError("", ErrorMsg);
                    return View();
                }

                return View("Login", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Caught");
                TempData["error"] = ex.Message;
                return RedirectToAction("UserLogin", "Login");
            }
          
        }
    }
}

