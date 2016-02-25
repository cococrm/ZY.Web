using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ZY.Model;
using ZY.Core.Logging;

namespace ZY.Identity
{
    public class UserManager : UserManager<User, int>
    {
        private readonly ILog _log;
        public UserManager(IUserStore<User, int> store)
            : base(store)
        {
            _log = new Log();
            //配置用户名的验证逻辑
            UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            //配置密码的验证逻辑
            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false
            };

            //配置用户锁定默认值
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;
        }
        /// <summary>
        /// 账号登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            //根据用户名查找账号
            var user = await Store.FindByNameAsync(userName);
            if (user == null)
                return new LoginResult(LoginResultType.Failed);
            if (user.State != 0)//判断账号状态
                return new LoginResult(LoginResultType.NotAllowed);
            if (await IsLockedOut(user))//判断账号是否锁定
                return new LoginResult(LoginResultType.LockedOut);
            if (await base.CheckPasswordAsync(user, password))//验证密码
            {
                await ResetLockout(user);
                return new LoginResult(user);
            }
            _log.Warn(string.Format("User {0} failed to provide the correct password.", user.Id));

            if (base.SupportsUserLockout)
            {
                // If lockout is requested, increment access failed count which might lock out the user
                await base.AccessFailedAsync(user.Id);
                if (await base.IsLockedOutAsync(user.Id))
                {
                    return new LoginResult(LoginResultType.LockedOut);
                }
            }
            return new LoginResult(LoginResultType.Failed);
        }

        private async Task<bool> IsLockedOut(User user)
        {
            return base.SupportsUserLockout && await base.IsLockedOutAsync(user.Id);
        }

        private Task ResetLockout(User user)
        {
            if (base.SupportsUserLockout)
            {
                return base.ResetAccessFailedCountAsync(user.Id);
            }
            return Task.FromResult(0);
        }
    }
}
