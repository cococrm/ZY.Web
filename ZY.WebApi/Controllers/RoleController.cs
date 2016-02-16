using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using ZY.Core.Repositories;
using ZY.Utils;
using ZY.Identity;
using ZY.Model;
using ZY.Core.Web.Model;
using ZY.WebApi.ViewModels;

namespace ZY.WebApi.Controllers
{
    [RoutePrefix("roles")]
    public class RoleController : ApiControllerBase
    {
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<UserRoleMap, int> _userRoleRepository;
        private readonly IRepository<RoleModuleMap, int> _roleModuleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public RoleController(
            IRepository<Role, int> roleRepository,
            IRepository<UserRoleMap, int> userRoleRepository,
            IRepository<RoleModuleMap, int> roleModuleRepository,
            IUnitOfWork unitOfWork)
        {
            this._roleRepository = roleRepository;
            this._userRoleRepository = userRoleRepository;
            this._roleModuleRepository = roleModuleRepository;
            this._unitOfWork = unitOfWork;
            this._log = new Log();
        }

        /// <summary>
        /// 分页查询获取角色
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public IHttpActionResult GetListByPage()
        {
            return Json(GetPageResult<Role>(_roleRepository.Entities));
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public async Task<IHttpActionResult> SaveRole(SaveRoleViewModel model)
        {
            if (model.AddList.Count() > 0)
            {
                foreach(var m in model.AddList)
                {
                    await _roleRepository.InsertAsync(new Role() { Name = m.Name, Remark = m.Remark, CreatedTime = DateTime.Now });
                }
            }
            if (model.UpdateList.Count > 0)
            {
                foreach (var m in model.UpdateList)
                {
                    await _roleRepository.UpdateAsync(new Role() { Id = m.Id, Name = m.Name, Remark = m.Remark });
                }
            }
            if (model.DeleteList.Count>0)
            {
                foreach (var m in model.DeleteList)
                {
                    _userRoleRepository.Remove(o => o.RoleId == m.Id);//删除角色相关信息
                    _roleModuleRepository.Remove(o => o.RoleId == m.Id);//删除角色相关信息
                    _roleRepository.Remove(m.Id);
                }
            }
            await _unitOfWork.CommitAsync();
            return Json(new AjaxResponse());
        }

        /// <summary>
        /// 设置角色模块权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,Route("setRoleModule")]
        public async Task<IHttpActionResult> setRoleModule(SetRoleModule model)
        {
            //获取所有账号模块权限
            var roleModules = await _roleModuleRepository.QueryAsync(o => o.RoleId == model.RoleId);
            var list = new List<RoleModuleMap>();
            foreach (var module in model.Modules)
            {
                foreach (var operation in module.Operations)
                {
                    int id = 0;
                    var entity = roleModules.FirstOrDefault(o => o.RoleId == model.RoleId && o.ModuleId == module.Id && o.OperationId == operation.Id);
                    if (entity != null) id = entity.Id;
                    list.Add(new RoleModuleMap()
                    {
                        Id = id,
                        RoleId = model.RoleId,
                        ModuleId = module.Id,
                        OperationId = operation.Id
                    });
                }
            }
            var addList = list.Except(roleModules);//获取新增
            var delList = roleModules.Except(list);//获取删除
            await _roleModuleRepository.InsertAsync(addList);
            _roleModuleRepository.Remove(delList);
            await _unitOfWork.CommitAsync();
            return Json(new AjaxResponse());
        }
    }
}
