using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Core.Repositories;
using ZY.Model;
using ZY.Utils;
using ZY.Core.Caching;
using ZY.Core;

namespace ZY.Identity
{
    /// <summary>
    /// 账号权限检查
    /// </summary>
    public class PermissionCheck : IPermissionCheck
    {
        private readonly IRepository<UserRoleMap, int> _userRoleRepository;
        private readonly IRepository<RoleModuleMap, int> _roleModuleRepository;
        private readonly IRepository<UserModuleMap, int> _userModuleRepository;
        private readonly IRepository<SysModule, Guid> _moudleRepository;
        private readonly IRepository<Operation, int> _operationRepository;
        private readonly IRepository<User, int> _userRepository;
        private readonly ICache _cache;

        public PermissionCheck(
            IRepository<UserRoleMap, int> userRoleRepository,
            IRepository<RoleModuleMap, int> roleModuleRepository,
            IRepository<UserModuleMap, int> userModuleRepository,
            IRepository<SysModule, Guid> moudleRepository,
            IRepository<Operation, int> operationRepository,
            IRepository<User, int> userRepository,
            ICache cache)
        {
            this._userRoleRepository = userRoleRepository;
            this._roleModuleRepository = roleModuleRepository;
            this._userModuleRepository = userModuleRepository;
            this._moudleRepository = moudleRepository;
            this._operationRepository = operationRepository;
            this._userRepository = userRepository;
            this._cache = cache;
        }

        /// <summary>
        /// 异步判断账号是否有权限
        /// </summary>
        /// <param name="userId">账号Id</param>
        /// <param name="moudle">模块Code</param>
        /// <param name="operation">操作Code</param>
        /// <returns></returns>
        public async Task<bool> IsGrantedAsync(int userId, string moudle, string operation)
        {
            //获取当前登录账号信息
            var user = await _userRepository.GetByKeyAsync(userId);
            if (user == null)
                return false;
            //超级管理员拥有全部权限
            if (user.IsSuperManager)
                return true;
            //获取权限列表
            var userPermission = await GetUserPermissionCacheItem(userId);
            //判断权限
            return userPermission.UserPermissions.Exists(o => o.Module == moudle && o.Operation == operation);
        }
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moudle"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public bool IsGranted(int userId, string moudle, string operation)
        {
            return AsyncHelper.RunAsync(() => IsGrantedAsync(userId, moudle, operation));
        }

        /// <summary>
        /// 获取当前账号所有操作权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<UserPermissionCacheItem> GetUserPermissionCacheItem(int userId)
        {
            var obj = _cache.Get<UserPermissionCacheItem>(UserPermissionCacheItem.CacheStoreName);
            if (obj == null)
            {
                //查询账号对应模块权限
                var userModule = await _userModuleRepository.QueryAsync(o => o.UserId == userId);
                //查询账号所有角色
                var userRole = await _userRoleRepository.QueryAsync(o => o.UserId == userId);
                //全部模块
                var moudles = await _moudleRepository.GetAllAsync();
                //全部操作类型
                var operations = await _operationRepository.GetAllAsync();
                //查询账户下面角色对应模块权限
                var roleModule = new List<RoleModuleMap>();
                foreach (var role in userRole)
                {
                    roleModule = await _roleModuleRepository.QueryAsync(o => o.RoleId == role.RoleId);
                }
                //去掉重复账号，角色有相同的权限
                foreach (var moudle in roleModule)
                {
                    if (!userModule.Exists(o => o.ModuleId == moudle.ModuleId && o.OperationId == moudle.OperationId))
                    {
                        userModule.Add(new UserModuleMap()
                        {
                            ModuleId = moudle.ModuleId,
                            OperationId = moudle.OperationId
                        });
                    }
                }
                //获取玩家所有权限
                var userPermission = new List<UserPermission>();
                foreach (var moudle in userModule)
                {
                    userPermission.Add(new UserPermission()
                    {
                        UserId = userId,
                        Module = (moudles.FirstOrDefault(o => o.Id == moudle.ModuleId)).Code,
                        Operation = (operations.FirstOrDefault(o => o.Id == moudle.OperationId)).Code
                    });
                }
                obj = new UserPermissionCacheItem(userId)
                {
                    UserPermissions = userPermission
                };
                //添加缓存
                _cache.Set(UserPermissionCacheItem.CacheStoreName, obj, obj.CacheExpireTime);
            }
            return obj;
        }
    }
}
