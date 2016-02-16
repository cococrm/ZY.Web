using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using ZY.Core.Repositories;
using ZY.Model;
using ZY.Utils;
using System.Linq;

namespace ZY.Identity
{
    public class UserStore :
        IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserSecurityStampStore<User, int>,
        IUserLockoutStore<User, int>
    {
        private readonly IRepository<User, int> _userRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public UserStore(IRepository<User, int> userRepository,IUnitOfWork unitOfWork)
        {
            this._log = new Log();
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
        }
        #region Implementation of IUserStore<TUser,in TUserKey>
        public IQueryable<User> Users
        {
            get
            {
                return _userRepository.Entities;
            }
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User is Null");
            await _userRepository.InsertAsync(user);
            await _unitOfWork.CommitAsync();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User is Null");
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            if (userName.IsNullOrEmpty())
                throw new ArgumentNullException("Username is Null");
            return await _userRepository.FirstOfDefaultAsync(u => u.UserName == userName);
        }


        public async Task<User> FindByIdAsync(int userId)
        {
            return await _userRepository.GetByKeyAsync(userId);
        }
        #endregion

        #region Implementation of IUserPasswordStore<TUser,in TUserKey>

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.Password != null);
        }

        #endregion

        #region Implementation of IUserSecurityStampStore<TUser,in TUserKey>

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStmp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.FromResult(user.SecurityStmp);
        }

        #endregion

        #region Implementation of IUserLockoutStore<TUser,in TUserKey>

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(user.LockoutEndDateUtc.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
