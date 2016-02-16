using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Core.Web.Model
{
    /// <summary>
    /// Ajax响应对象
    /// </summary>
    public class AjaxResponse
    {
        public AjaxResponse()
            : this("操作成功！")
        { }
        public AjaxResponse(string Message)
            : this(AjaxResponseStatus.Success, Message)
        { }
        public AjaxResponse(int Status, string Message)
            : this(Status, Message, null)
        { }
        public AjaxResponse(int Status, string Message, object Data)
        {
            this.Status = Status;
            this.Message = Message;
            this.Data = Data;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }
}
