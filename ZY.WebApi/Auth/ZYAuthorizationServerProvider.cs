using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Autofac;
using ZY.Identity;
using System;
using ZY.WebApi.Autofac;
using ZY.Model;
using ZY.Core.Autofac;

namespace ZY.WebApi
{
    public class ZYAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly UserManager _userManager;
        public ZYAuthorizationServerProvider()
        {
            var _userStore = IocManager.Resolve<IUserStore<User, int>>(new OAuthModule());
            this._userManager = new UserManager(_userStore);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public async override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                LoginResult result = await _userManager.LoginAsync(context.UserName, context.Password);
                var loginResultType = result.Result;
                if (loginResultType == LoginResultType.Failed)
                {
                    context.SetError("login", "登陆失败");
                    return;
                }
                if (loginResultType == LoginResultType.NotAllowed)
                {
                    context.SetError("login", "账号已经锁定");
                    return;
                }
                if (loginResultType == LoginResultType.LockedOut)
                {
                    context.SetError("login", "账号登陆超过次数");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));//账号登陆名称
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()));//账号Id
                identity.AddClaim(new Claim("sub", context.UserName));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", context.ClientId ?? string.Empty
                    },
                    {
                        "userName", context.UserName
                    }
                });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

            }
            catch
            {
                context.SetError("login", "系统错误");
                return;
            }
        }

        /// <summary>
        /// 刷新客户端令牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            string originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            string currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
                context.Rejected();
                return Task.FromResult(0);
            }
            ClaimsIdentity identity = new ClaimsIdentity(context.Ticket.Identity);
            identity.AddClaim(new Claim("newClaim", "refreshToken"));
            AuthenticationTicket ticket = new AuthenticationTicket(identity, context.Ticket.Properties);
            context.Validated(ticket);
            return base.GrantRefreshToken(context);
        }
    }
}
