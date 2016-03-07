using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZY.Core.Repositories;
using ZY.Identity;
using ZY.Model;
using ZY.Core.Web.Model;
using ZY.WebApi.ViewModels;
using ZY.WebApi.Filter;
using ZY.Core.Logging;

namespace ZY.WebApi.Controllers
{
    /// <summary>
    /// 系统模块接口
    /// </summary>
    [RoutePrefix("sysModules")]
    public class SysModuleController : ApiControllerBase
    {
        private readonly IRepository<SysModule, Guid> _moduleRepository;
        private readonly IRepository<ModuleOperationMap, int> _moduleOperationRepository;
        private readonly IRepository<Operation, int> _operationRepository;
        private readonly IRepository<RoleModuleMap, int> _roleModuleRepository;
        private readonly IRepository<UserModuleMap, int> _userModuleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public SysModuleController(
            IRepository<SysModule, Guid> moduleRepository,
            IRepository<ModuleOperationMap, int> moduleOperationRepository,
            IRepository<Operation, int> operationRepository,
            IRepository<RoleModuleMap, int> roleModuleRepository,
            IRepository<UserModuleMap, int> userModuleRepository,
            IUnitOfWork unitOfWork
            )
        {
            this._moduleRepository = moduleRepository;
            this._moduleOperationRepository = moduleOperationRepository;
            this._operationRepository = operationRepository;
            this._roleModuleRepository = roleModuleRepository;
            this._userModuleRepository = userModuleRepository;
            this._unitOfWork = unitOfWork;
            _log = new Log();
        }

        /// <summary>
        /// 获取系统导航列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getSysNav")]
        public async Task<IHttpActionResult> GetSysNav()
        {
            IList<SysModule> _list = (await _moduleRepository.QueryAsync(o => o.IsLock == false)).OrderBy(o => o.Sort).ToList();
            var trees = new List<TreeNode>();
            var parents = _list.Where(o => o.ParentId.Equals(Guid.Empty));
            foreach (var model in parents)
            {
                if (!UserAuthorize.IsAuthorized(model.Code, OperationCode.Show)) continue; //判断权限
                TreeNode node = new TreeNode();
                node.Id = model.Id;
                node.Text = model.Name;
                node.IconCls = model.Icon;
                GetTree(node, _list, model.Id);
                trees.Add(node);
            }
            return Json(trees);
        }
        /// <summary>
        /// 获取全部导航菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getModules")]
        [UserPermission(Module = "SystemModule")]
        public async Task<IHttpActionResult> GetModules()
        {
            var modules = await _moduleRepository.QueryAsync(o => 1 == 1);
            return Json(new
            {
                total = modules.Count,
                rows = modules.OrderBy(m => m.Sort).Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Code = m.Code,
                    _parentId = m.ParentId == Guid.Empty ? null : m.ParentId.ToString(),
                    Url = m.Url,
                    Icon = m.Icon,
                    IsLock = m.IsLock,
                    Sort = m.Sort,
                    CreateTime = m.CreateTime
                })
            });
        }
        /// <summary>
        /// 获取导航Tree
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getComboTree")]
        public async Task<IHttpActionResult> GetComboTree()
        {
            IList<SysModule> _list = (await _moduleRepository.QueryAsync(o => o.IsLock == false)).OrderBy(o => o.Sort).ToList();
            var trees = new List<TreeNode>();
            trees.Add(new TreeNode() { Id = Guid.Empty, Text = "无父节点" });
            var parents = _list.Where(o => o.ParentId.Equals(Guid.Empty));
            foreach (var model in parents)
            {
                TreeNode node = new TreeNode();
                node.Id = model.Id;
                node.Text = model.Name;
                GetTree(node, _list, model.Id);
                trees.Add(node);
            }
            return Json(trees);
        }
        /// <summary>
        /// 根据Id获取导航
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("get")]
        public async Task<IHttpActionResult> GetModuleById(Guid id)
        {
            return Json(await _moduleRepository.GetByKeyAsync(id));
        }
        /// <summary>
        /// 获取角色系统菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("getRoleModule")]
        public async Task<IHttpActionResult> GetRoleModule(int id)
        {
            var roleModule = await _roleModuleRepository.QueryAsync(o => o.RoleId == id);
            var modules = (await _moduleRepository.QueryAsync(o => o.IsLock == false)).OrderBy(o => o.Sort).ToList();
            var operations = _operationRepository.Entities.ToList();
            var json = from m in modules
                       select new
                       {
                           Id = m.Id,
                           Name = m.Name,
                           _parentId = m.ParentId == Guid.Empty ? null : m.ParentId.ToString(),
                           Operations = (from mo in m.Operations
                                         join o in operations
                                         on mo.OperationId equals o.Id
                                         select new
                                         {
                                             Id = o.Id,
                                             Name = o.Name,
                                             IsCheck = roleModule.Where(u => u.ModuleId.Equals(m.Id) && u.OperationId == o.Id).Any()
                                         })
                       };
            return Json(new
            {
                total = json.Count(),
                rows = json
            });
        }
        /// <summary>
        /// 账号权限查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("getUserModule")]
        public async Task<IHttpActionResult> GetUserModule(int id)
        {
            var userModule = await _userModuleRepository.QueryAsync(o => o.UserId == id);
            var modules = (await _moduleRepository.QueryAsync(o => o.IsLock == false)).OrderBy(o => o.Sort).ToList();
            var operations = _operationRepository.Entities.ToList();
            var json = from m in modules
                       select new
                       {
                           Id = m.Id,
                           Name = m.Name,
                           _parentId = m.ParentId == Guid.Empty ? null : m.ParentId.ToString(),
                           Operations = (from mo in m.Operations
                                         join o in operations
                                         on mo.OperationId equals o.Id
                                         select new
                                         {
                                             Id = o.Id,
                                             Name = o.Name,
                                             IsCheck = userModule.Where(u => u.ModuleId.Equals(m.Id) && u.OperationId == o.Id).Any()
                                         }
                                      )
                       };
            return Json(new
            {
                total = json.Count(),
                rows = json
            });
        }
        /// <summary>
        /// 保存角色模块权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("saveRoleModule")]
        public async Task<IHttpActionResult> SaveRoleModuleOperation(SaveRoleModuleOperationViewModel model)
        {
            var roleModule = await _roleModuleRepository.QueryAsync(o => o.RoleId == model.Id);
            var postRoleModule = new List<RoleModuleMap>();
            foreach (var m in model.Module)
            {
                postRoleModule.Add(new RoleModuleMap()
                {
                    RoleId = model.Id,
                    ModuleId = m.Id,
                    OperationId = m.operation
                });
            }
            //新增
            var addRoleModule = postRoleModule.Except(roleModule, new RoleModuleEquality());
            //删除
            var deleteRoleModule = roleModule.Except(postRoleModule, new RoleModuleEquality());

            await _roleModuleRepository.InsertAsync(addRoleModule);
            _roleModuleRepository.Remove(deleteRoleModule);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
        /// <summary>
        /// 保存账号模块权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("saveUserModule")]
        public async Task<IHttpActionResult> SaveUserModuleOperation(SaveUserModuleOperationViewModel model)
        {
            var userModule = await _userModuleRepository.QueryAsync(o => o.UserId == model.Id);
            var postuUserModule = new List<UserModuleMap>();
            foreach (var m in model.Module)
            {
                postuUserModule.Add(new UserModuleMap()
                {
                    UserId = model.Id,
                    ModuleId = m.Id,
                    OperationId = m.operation
                });
            }
            //新增
            var addUserModule = postuUserModule.Except(userModule, new UserModuleEquality());
            //删除
            var deleteUserModule = userModule.Except(postuUserModule, new UserModuleEquality());

            await _userModuleRepository.InsertAsync(addUserModule);
            _userModuleRepository.Remove(deleteUserModule);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,Route("save")]
        public async Task<IHttpActionResult> Save(SysModule model)
        {
            if (!ModelState.IsValid)
            {
                return ValidError();
            }
            var module = await _moduleRepository.GetByKeyAsync(model.Id);
            if (module == null)
            {
                //添加
                await _moduleRepository.InsertAsync(model);
            }
            else
            {
                module.Name = model.Name;
                module.Code = model.Code;
                module.Url = model.Url;
                module.Icon = model.Icon;
                module.IsLock = model.IsLock;
                module.ParentId = model.ParentId;
                module.Sort = model.Sort;
                module.Remark = model.Remark;
                //修改
                await _moduleRepository.UpdateAsync(module);
            }
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public async Task<IHttpActionResult> Delete([FromBody]Guid id)
        {
            _userModuleRepository.Remove(o => o.ModuleId.Equals(id));
            _roleModuleRepository.Remove(o => o.ModuleId.Equals(id));
            _moduleOperationRepository.Remove(o => o.ModuleId.Equals(id));
            _moduleRepository.Remove(id);
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        #region 私有方法
        /// <summary>
        /// 递归获取树菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        private void GetTree(TreeNode parent, IList<SysModule> list, Guid parentId)
        {
            var query = list.Where(m => m.ParentId == parentId);
            if (query.Any())
            {
                if (parent.Children == null)
                {
                    parent.Children = new List<TreeNode>();
                }
                foreach (var model in query)
                {
                    if (!UserAuthorize.IsAuthorized(model.Code, OperationCode.Show)) continue; //判断权限
                    TreeNode child = new TreeNode()
                    {
                        Id = model.Id,
                        Text = model.Name,
                        IconCls = model.Icon,
                        Attributes = new { url = model.Url }
                    };
                    parent.Children.Add(child);
                    this.GetTree(child, list, model.Id);
                }
            }
        }
        #endregion
    }
}
