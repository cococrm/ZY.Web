(function ($) {
    //ajax请求扩展，错误统一处理
    var _ajax = $.ajax;
    $.ajax = function (opt) {
        var url = $.webApiUrl + opt.url;
        var fn = {
            error: function (XMLHttpRequest, textStatus, errorThrown) { },
            success: function (data, textStatus) { }
        }
        if (opt.error) {
            fn.error = opt.error;
        }
        if (opt.success) {
            fn.success = opt.success;
        }
        if (opt.dataType == "html") {
            url = opt.url;
        }
        var _opt = $.extend(opt, {
            url: url,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                fn.error(XMLHttpRequest, textStatus, errorThrown)
            },
            success: function (data, textStatus) {
                $.easyui.removeLoading();
                try {
                    if (data.status == "401") {
                        location.href = "/Account/Login";
                    } else if (data.status == "403") {
                        $.easyui.warn("你没有权限操作！");
                        return;
                    } else if (data.status == "500") {
                        $.easyui.warn(data.message + "," + data.data);
                        return;
                    } else if (data.status == "100") {
                        $.easyui.warn(data.message + "," + data.data);
                        return;
                    } else if (data.status == "404") {
                        $.easyui.warn("404错误！");
                        return;
                    }
                } catch (e) {

                }
                fn.success(data, textStatus);
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + $.cookie("token"));
            }
        });
        _ajax(_opt);
    };
})(jQuery)