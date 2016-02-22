(function ($) {
    $.easyui.submit = function (formId, fnBefore, fnSuccess) {
        ///	<summary>
        ///	提交更新表单
        ///	</summary>
        ///	<param name="fnBefore" type="Function">
        ///	提交前操作
        ///	</param>
        ///	<param name="fnSuccess" type="Function">
        ///	成功操作
        ///	</param>
        ///	<param name="formId" type="String">
        ///	表单Id
        ///	</param>
        var form = $("#" + formId);
        if (!validate())
            return;
        if (!submitBefore())
            return;
        ajaxSubmit();

        //验证表单
        function validate() {
            return form.form('validate');
        }

        //提交前操作
        function submitBefore() {
            if (!fnBefore)
                return true;
            return fnBefore(form);
        }

        //提交
        function ajaxSubmit() {
            $.easyui.ajax(form.attr("url"), form.serializeArray(), ajaxCallback);
            //回调
            function ajaxCallback(result) {
                if (fnSuccess)
                    fnSuccess(result);
            }
        }
    };

})(jQuery);

