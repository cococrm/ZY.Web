using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZY.Web.MVC.Filter;

namespace ZY.Web.MVC.EasyUI
{
    public class LinkButton
    {
        private readonly TagBuilder tagBuilder;

        public LinkButton() { }

        public LinkButton(string text)
            : this(Guid.NewGuid().ToString(), text)
        { }

        public LinkButton(string id, string text)
        {
            tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.AddCssClass("easyui-linkbutton");
            tagBuilder.SetInnerText(text);
        }
        /// <summary>
        /// 添加data-options属性
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public LinkButton Options(string options)
        {
            tagBuilder.MergeAttribute("data-options", options, true);

            return this;
        }
        //添加单击事件
        public LinkButton Click(string click)
        {
            tagBuilder.MergeAttribute("onclick", click);
            return this;
        }
        //判断权限
        public LinkButton Authorize(string module, string operation)
        {
            //判断权限
            if (!UserAuthorize.IsAuthorized(module, operation))
            {
                return new LinkButton();
            }
            return this;
        }

        public override string ToString()
        {
            if (tagBuilder == null)
                return null;
            return MvcHtmlString.Create(tagBuilder.ToString()).ToHtmlString();
        }

        //创建控件
        public MvcHtmlString Create()
        {
            if (tagBuilder == null)
                return null;
            return MvcHtmlString.Create(tagBuilder.ToString());
        }
    }
}
