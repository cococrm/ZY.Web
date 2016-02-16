using ZY.Model;

namespace ZY.Identity
{
    /// <summary>
    /// 登陆返回结果
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 返回结果类型
        /// </summary>
        public LoginResultType Result { get; private set; }
        /// <summary>
        /// 返回账号信息
        /// </summary>
        public User User { get; private set; }

        public LoginResult(LoginResultType result)
        {
            this.Result = result;
            this.User = null;
        }

        public LoginResult(User user)
        {
            this.Result = LoginResultType.Success;
            this.User = user;
        }


    }
}
