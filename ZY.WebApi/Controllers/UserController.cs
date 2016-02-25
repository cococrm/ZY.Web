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
using ZY.Identity;
using ZY.Model;
using ZY.Core.Web.Model;
using ZY.WebApi.ViewModels;
using ZY.WebApi.Filter;
using ZY.Core.Extensions;
using ZY.Core.Logging;

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
        private readonly string defaultPassword = "******";

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
            return Json(GetPageResult(_userRepository.Entities).ToGridData());
        }

        /// <summary>
        /// 根据Id获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public async Task<IHttpActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetByKeyAsync(id);
            return Json(new
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = defaultPassword,
                NickName = user.NickName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Roles = from r in user.Roles
                        select new
                        {
                            roleId = r.RoleId,
                            roleName = _roleRepository.GetByKey(r.RoleId).Name
                        }
            });
        }

        /// <summary>
        /// 保存账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public async Task<IHttpActionResult> SaveUser(SaveUserViewModel model)
        {
            if (!ModelState.IsValid)
                return ValidError();
            if (model.Id > 0)
            {
                var user = await _userRepository.GetByKeyAsync(model.Id);
                if (user == null)
                    return NotFound();

                if (model.Password != defaultPassword)
                    user.Password = new PasswordHasher().HashPassword(model.Password);

                user.UserName = model.UserName;
                user.NickName = model.NickName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                await _userRepository.UpdateAsync(user);
                await SetUserRoles(user.Id, model.Roles.Split(",").ToInt());
            }
            else
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    Password = new PasswordHasher().HashPassword(model.Password),
                    NickName = model.NickName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };
                var result = await _userRepository.InsertAsync(user);
                await SetUserRoles(result.Id, model.Roles.Split(",").ToInt());
            }
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        /// <summary>
        /// 设置账号的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        private async Task SetUserRoles(int userId, int[] roleIds)
        {
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
        [HttpPost, Route("delete")]
        public async Task<IHttpActionResult> DeleteUser(DeleteViewModel model)
        {
            _userRoleRepository.Remove(o => model.Ids.Contains(o.UserId));
            _userModuleRepository.Remove(o => model.Ids.Contains(o.UserId));
            _userRepository.Remove(model.Ids);
            await _unitOfWork.CommitAsync();
            return Ok();
        }

    }
}
