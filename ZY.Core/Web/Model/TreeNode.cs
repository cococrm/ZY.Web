using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;

namespace ZY.Core.Web.Model
{
    /// <summary>
    /// 树形菜单
    /// </summary>
    [DataContract]
    public class TreeNode
    {

        public TreeNode() { }
        public TreeNode(Guid id, string text, string icon)
        {
            this.Id = id;
            this.Text = text;
            this.IconCls = icon;
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        [DataMember]
        public Guid Id { get; set; }
        /// <summary>
        /// 菜单文本
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        [DataMember]
        public string Text { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        [DataMember]
        public string State { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty(PropertyName = "iconCls", NullValueHandling = NullValueHandling.Ignore)]
        [DataMember]
        public string IconCls { get; set; }
        /// <summary>
        /// Attributes
        /// </summary>
        [JsonProperty(PropertyName = "attributes", NullValueHandling = NullValueHandling.Ignore)]
        [DataMember]
        public object Attributes { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        [DataMember]
        public IList<TreeNode> Children { get; set; }
    }
}
