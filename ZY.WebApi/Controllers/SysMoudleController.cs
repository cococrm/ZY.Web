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
        private readonly IRepository<RoleModuleMap, int> _roleModuleRrpository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public SysModuleController(
            IRepository<SysModule, Guid> moduleRepository,
            IRepository<ModuleOperationMap, int> moduleOperationRepository,
            IRepository<Operation, int> operationRepository,
            IRepository<RoleModuleMap, int> roleModuleRepository,
            IUnitOfWork unitOfWork
            )
        {
            this._moduleRepository = moduleRepository;
            this._moduleOperationRepository = moduleOperationRepository;
            this._operationRepository = operationRepository;
            this._roleModuleRrpository = roleModuleRepository;
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
            var roleModule = await _roleModuleRrpository.QueryAsync(o => o.RoleId == id);
            var modules = (await _moduleRepository.QueryAsync(o => o.IsLock == false)).OrderBy(o => o.Sort).ToList();
            var operations = _operationRepository.Entities.ToList();
            var json = from m in modules
                       select new SetModuleOperationViewModel
                       {
                           Id = m.Id,
                           Name = m.Name,
                           ParentId = m.ParentId == Guid.Empty ? null : m.ParentId.ToString(),
                           Operations = GetRoleModuleOperation(m.Operations, roleModule, operations)
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
        public async Task<IHttpActionResult> SaveModuleOperation(SaveModuleOperationViewModel model)
        {
            var roleModule = await _roleModuleRrpository.QueryAsync(o => o.RoleId == model.Id);
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

            await _roleModuleRrpository.InsertAsync(addRoleModule);
            _roleModuleRrpository.Remove(deleteRoleModule);
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
        /// <summary>
        /// 获取角色也有菜单权限
        /// </summary>
        /// <param name="list"></param>
        /// <param name="roleModule"></param>
        /// <param name="operations"></param>
        /// <returns></returns>
        private IList<OperationViewModel> GetRoleModuleOperation(ICollection<ModuleOperationMap> list, IList<RoleModuleMap> roleModule, IList<Operation> operations)
        {
            return (from m in list
                    join o in operations
                    on m.OperationId equals o.Id
                    select new OperationViewModel
                    {
                        Id = o.Id,
                        Name = o.Name,
                        IsCheck = roleModule.Where(r => r.ModuleId == m.ModuleId && r.OperationId == m.OperationId).Any()
                    }).ToList<OperationViewModel>();
        }
        #endregion
    }
}
