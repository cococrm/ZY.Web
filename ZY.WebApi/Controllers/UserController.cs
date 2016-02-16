using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ZY.Core.Repositories;
using ZY.Utils;
using ZY.Identity;
using ZY.Model;
using ZY.Core.Web.Model;
using ZY.WebApi.ViewModels;
using ZY.WebApi.Filter;
using ZY.Core.Extensions;

namespace ZY.WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiControllerBase
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<UserRoleMap, int> _userRoleRepository;
        private readonly IRepository<UserModuleMap, int> _userModuleRepository;
        private readonly IUserStore<User, int> _userStore;
        private readonly UserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public UserController(
            IRepository<User, int> userRepository,
            IRepository<Role, int> roleRepository,
            IRepository<UserRoleMap, int> userRoleRepository,
            IRepository<UserModuleMap, int> userModuleRepository,
            IUserStore<User, int> userStore,
            IUnitOfWork unitOfWork)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._userRoleRepository = userRoleRepository;
            this._userModuleRepository = userModuleRepository;
            this._unitOfWork = unitOfWork;
            this._userStore = userStore;
            this._userManager = new UserManager(_userStore);
            this._log = new Log();
        }

        /// <summary>
        /// 分页查询获取账号
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [UserPermission("SystemUser")]
        [HttpGet, Route("list")]
        public IHttpActionResult GetListByPage()
        {
            return Json(GetPageResult<User>(_userRepository.Entities).ToGridData());
        }

        /// <summary>
        /// 设置账号的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPost, Route("setUserRole")]
        public async Task<IHttpActionResult> SetUserRoles(SetUserRole model)
        {
            int userId = model.UserId;
            int[] roleIds = model.RoleIds;
            //获取当前账号所以角色
            int[] userRoles = _userRoleRepository.Entities.Where(o => o.UserId == userId).Select(o => o.RoleId).ToArray();
            int[] addList = roleIds.Except(userRoles).ToArray();//获取添加
            int[] removeList = userRoles.Except(roleIds).ToArray();//获取删除
            foreach (var role in addList)
            {
                var entity = new UserRoleMap() { UserId = userId, RoleId = role };
                await _userRoleRepository.InsertAsync(entity);
            }
            _userRoleRepository.Remove(o => o.UserId == userId && removeList.Contains(o.RoleId));//删除
            await _unitOfWork.CommitAsync();
            return Ok(new AjaxResponse());
        }

        /// <summary>
        /// 设置用户模块权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("setUserModule")]
        public async Task<IHttpActionResult> SetUserModules(SetUserModule model)
        {
            //获取所有账号模块权限
            var userModules = await _userModuleRepository.QueryAsync(o => o.UserId == model.UserId);
            var list = new List<UserModuleMap>();
            foreach (var module in model.Modules)
            {
                foreach (var operation in module.Operations)
                {
                    int id = 0;
                    var entity = userModules.FirstOrDefault(o => o.UserId == model.UserId && o.ModuleId == module.Id && o.OperationId == operation.Id);
                    if (entity != null)
                        id = entity.Id;
                    list.Add(new UserModuleMap()
                    {
                        Id = id,
                        UserId = model.UserId,
                        ModuleId = module.Id,
                        OperationId = operation.Id
                    });
                }
            }
            var addList = list.Except(userModules);//获取新增
            var delList = userModules.Except(list);//获取删除
            await _userModuleRepository.InsertAsync(addList);
            _userModuleRepository.Remove(delList);
            await _unitOfWork.CommitAsync();
            return Json(new AjaxResponse());
        }

        /// <summary>
        /// 批量删除账号
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost,Route("delete")]
        public async Task<IHttpActionResult> DeleteUser(int[] ids)
        {
            foreach(int id in ids)
            {
                _userRoleRepository.Remove(o => o.UserId == id);
                _userModuleRepository.Remove(o => o.UserId == id);
                _userRepository.Remove(id);
            }
            await _unitOfWork.CommitAsync();
            return Json(new AjaxResponse());
        }
        
    }
}
