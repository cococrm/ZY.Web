using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZY.Web.MVC.Models;
using ZY.Core.Web.Model;
using ZY.Core.Repositories;
using ZY.Model;
using ZY.Utils;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Net.Http.Formatting;

namespace ZY.Web.MVC.Controllers
{
    /// <summary>
    /// 登陆
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IRepository<User, int> _userRepository;

        public AccountController(
            IRepository<User, int> userRepository)
        {
            this._userRepository = userRepository;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResponse(AjaxResponseStatus.ValidError, "参数错误"));
            }
            var user = await _userRepository.FirstOfDefaultAsync(o => o.UserName == model.UserName);
            if (user == null)
            {
                return Json(new AjaxResponse(AjaxResponseStatus.ValidError, "无效的用户名"));
            }
            //请求webapi登录
            OAuthToken token = await RequestLogin(model.UserName, model.Password);
            if (!token.error.IsNullOrEmpty())
            {
                return Json(new AjaxResponse(AjaxResponseStatus.ValidError, token.error_description));
            }
            //登陆
            SignIn(user);
            HttpCookie cookie = new HttpCookie("token", token.access_token)
            {
                Expires = DateTime.Now.AddSeconds(token.expires_in)
            };
            Response.Cookies.Add(cookie);
            return Json(new AjaxResponse());
        }
        /// <summary>
        /// 请求webapi登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<OAuthToken> RequestLogin(string userName, string password)
        {
            HttpClient client = new HttpClient();
            IDictionary<string, string> paramters = new Dictionary<string, string>();
            paramters.Add("grant_type", "password");
            paramters.Add("username", userName);
            paramters.Add("password", password);
            HttpResponseMessage response = await client.PostAsync("http://localhost:16169/token", new FormUrlEncodedContent(paramters));
            var jsonRes = await response.Content.ReadAsStringAsync();
            OAuthToken token = JsonHelper.ToObject<OAuthToken>(jsonRes);
            return token;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user"></param>
        private void SignIn(User user)
        {
            var identity = new ClaimsIdentity("ApplicationCookie");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));//账号登陆名称
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));//账号Id
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(identity);
        }
    }
}