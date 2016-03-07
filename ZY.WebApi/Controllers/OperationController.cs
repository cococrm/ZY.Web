using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [RoutePrefix("operations")]
    public class OperationController : ApiControllerBase
    {
        private readonly IRepository<Operation, int> _operationRepository;
        private readonly IRepository<ModuleOperationMap, int> _moduleOperationRepository;
        private readonly IRepository<RoleModuleMap, int> _roleModuleRepository;
        private readonly IRepository<UserModuleMap, int> _userModuleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OperationController(
            IRepository<Operation,int> operationRepository,
            IRepository<ModuleOperationMap,int> moduleOperationRepository,
            IRepository<RoleModuleMap, int> roleModuleRepository,
            IRepository<UserModuleMap, int> userModuleRepository,
            IUnitOfWork unitOfWork)
        {
            this._operationRepository = operationRepository;
            this._moduleOperationRepository = moduleOperationRepository;
            this._roleModuleRepository = roleModuleRepository;
            this._userModuleRepository = userModuleRepository;
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getList")]
        public async Task<IHttpActionResult> GetList(Guid id)
        {
            var moduleOperations = await _moduleOperationRepository.QueryAsync(o => o.ModuleId.Equals(id));
            var json = from o in _operationRepository.Entities.ToList()
                       select new
                       {
                           Id = o.Id,
                           Name = o.Name,
                           Code = o.Code,
                           Remark = o.Remark,
                           IsCheck = moduleOperations.Any(m => m.OperationId == o.Id)
                       };
            return Json(json);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,Route("add")]
        public async Task<IHttpActionResult> Add(Operation model)
        {
            if (!ModelState.IsValid)
            {
                return ValidError();
            }
            await _operationRepository.InsertAsync(model);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,Route("update")]
        public async Task<IHttpActionResult> Update(Operation model)
        {
            if (!ModelState.IsValid)
            {
                return ValidError();
            }
            await _operationRepository.UpdateAsync(model);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost,Route("delete")]
        public async Task<IHttpActionResult> Delete(OperationDeleteViewModel model)
        {
            _roleModuleRepository.Remove(o => model.Ids.Contains(o.OperationId));
            _userModuleRepository.Remove(o => model.Ids.Contains(o.OperationId));
            _moduleOperationRepository.Remove(o => model.Ids.Contains(o.OperationId));
            _operationRepository.Remove(model.Ids);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
        /// <summary>
        /// 保存模块按钮
        /// </summary>
        /// <param name="id">模块ID</param>
        /// <param name="ids">按钮列表Id</param>
        /// <returns></returns>
        [HttpPost,Route("saveModuleOperation")]
        public async Task<IHttpActionResult> SaveModuleOperation(SaveModuleOperationViewModel model)
        {
            var moduleOperations = (await _moduleOperationRepository.QueryAsync(o => o.ModuleId.Equals(model.Id))).Select(o => o.OperationId).ToArray();
            var addList = model.Ids.Except(moduleOperations).ToArray();
            var deleteList = moduleOperations.Except(model.Ids).ToArray();
            foreach (var i in addList)
            {
                _moduleOperationRepository.Insert(new ModuleOperationMap() { ModuleId = model.Id, OperationId = i });
            }
            _moduleOperationRepository.Remove(o => deleteList.Contains(o.OperationId) && o.ModuleId.Equals(model.Id));
            await _unitOfWork.CommitAsync();
            return Ok();
        }
    }
}
