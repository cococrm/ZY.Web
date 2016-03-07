using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using ZY.Core.Repositories;
using ZY.Identity;
using ZY.Model;
using ZY.WebApi.ViewModels;
using ZY.Core.Extensions;
using ZY.Core.Logging;
using ZY.WebApi.Filter;

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
            return Json(GetPageResult(_roleRepository.Entities, o => new { o.Id, o.Name }).ToGridData());
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,Route("add")]
        [UserPermission(Module = "SystemRole",Operation = "Add")]
        public async Task<IHttpActionResult> Add(Role model)
        {
            if (!ModelState.IsValid)
            {
                return ValidError();
            }
            await _roleRepository.InsertAsync(model);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,Route("update")]
        [UserPermission(Module = "SystemRole", Operation = "Update")]
        public async Task<IHttpActionResult> Update(Role model)
        {
            if (!ModelState.IsValid)
            {
                return ValidError();
            }
            await _roleRepository.UpdateAsync(model);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        [UserPermission(Module = "SystemRole", Operation = "Delete")]
        public async Task<IHttpActionResult> Delete(DeleteRoleViewModel model)
        {
            _userRoleRepository.Remove(o => model.Ids.Contains(o.RoleId));
            _roleRepository.Remove(model.Ids);
            await _unitOfWork.CommitAsync();
            return Ok();
        }


    }
}
