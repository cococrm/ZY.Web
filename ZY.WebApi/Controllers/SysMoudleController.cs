using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZY.Core.Repositories;
using ZY.Utils;
using ZY.Identity;
using ZY.Model;
using ZY.Core.Web.Model;
using ZY.WebApi.ViewModels;
using ZY.WebApi.Filter;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public SysModuleController(
            IRepository<SysModule, Guid> moduleRepository,
            IRepository<ModuleOperationMap, int> moduleOperationRepository,
            IUnitOfWork unitOfWork
            )
        {
            this._moduleRepository = moduleRepository;
            this._moduleOperationRepository = moduleOperationRepository;
            this._unitOfWork = unitOfWork;
            _log = new Log();
        }

        /// <summary>
        /// 获取系统导航列表
        /// </summary>
        /// <returns></returns>
        //[UserPermission("moudle", "show")]
        [HttpGet, Route("getSysNav")]
        public async Task<IHttpActionResult> GetSysNav()
        {
            IList<SysModule> _list = await _moduleRepository.QueryAsync(o => o.IsLock == false);
            Guid parentId = Guid.Empty;
            var trees = new List<TreeNode>();
            var parents = _list.Where(o => o.ParentId.Equals(parentId)).OrderBy(o => o.Sort);
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
        /// 设置模块权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> SetOperation(SetOperationViewModel model)
        {
            int[] operationIds = model.OperationIds;
            //获取当前账号所以角色
            int[] moduleOperations = _moduleOperationRepository.Entities.Where(o => o.ModuleId == model.Id).Select(o => o.OperationId).ToArray();
            int[] addList = operationIds.Except(moduleOperations).ToArray();//获取添加
            int[] removeList = moduleOperations.Except(operationIds).ToArray();//获取删除
            foreach (var id in addList)
            {
                var entity = new ModuleOperationMap() { ModuleId = model.Id, OperationId = id };
                await _moduleOperationRepository.InsertAsync(entity);
            }
            _moduleOperationRepository.Remove(o => o.ModuleId == model.Id && removeList.Contains(o.OperationId));//删除
            await _unitOfWork.CommitAsync();
            return Ok(new AjaxResponse());
        }

        [Authorize]
        [UserPermission("moudle", "show")]
        [HttpGet, Route("loadNav")]
        public async Task<IHttpActionResult> LoadNav()
        {

            //var Module1 = new SysModule()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "系统管理",
            //    Code = "SystemManager",
            //    ParentId = Guid.Empty,
            //    IsLock = false,
            //    Sort = 99
            //};
            //Module1 = await _ModuleRepository.InsertAsync(Module1);
            var Module2 = new SysModule()
            {
                Id = Guid.NewGuid(),
                Name = "菜单管理",
                Code = "SystemNav",
                ParentId = new Guid("3E2D25B9-23C3-4249-AD91-01B80F90C463"),
                IsLock = false,
                Sort = 99
            };
            await _moduleRepository.InsertAsync(Module2);
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
        #endregion
    }
}
