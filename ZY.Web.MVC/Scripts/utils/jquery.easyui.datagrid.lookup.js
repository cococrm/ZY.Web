//查找带回
(function ($) {
    $.fn.lookup = function (options, param) {
        if (typeof options == "string") {
            var methods = $.fn.lookup.methods[options];
            if (methods) {
                return methods(this, param);
            } else {
                return this.combo(options, param);
            }
        }
        return this.each(function () {
            initOptions();
            var control = $(this);
            createCombo();

            //初始化参数
            function initOptions() {
                options = $.extend({
                    title: '查找带回',
                    onShowPanel: showDialog,
                    closeCallback: removeControl
                }, options || {});

                //弹出窗口
                function showDialog() {
                    //$.easyui.addItem("lookups", control);
                    control.combo('hidePanel');
                    $.easyui.dialog(options);
                };

                //移除控件
                function removeControl() {
                    //var item = $.easyui.getItem("lookups");
                    //if (item === control)
                        //$.easyui.getArray("lookups").pop();
                }
            }

            //创建组合控件
            function createCombo() {
                control.combo(options);
                control.combo('textbox').unbind("keydown");
                control.combo('textbox').blur(function () {
                    control.lookup("setValue", control.combo('textbox').val());
                });
                control.data('combo').combo.addClass("combo-lookup");
            }
        });
    };

    $.fn.lookup.methods = {
        //设置值
        setValue: function (target, value) {
            return target.each(function () {
                if (!value)
                    return;
                var control = $(this);
                control.combo('setValue', value);
                control.combo('setText', value);
            });
        }
    };

    //扩展datagrid的查找带回
    $.extend($.fn.datagrid.defaults.editors, {
        lookup: {
            init: function (container, options) {
                var input = $('<input type="text" class="datagrid-editable-input"/>').appendTo(container);
                return input.lookup(options);
            },
            destroy: function (target) {
                $(target).lookup('destroy');
            },
            getValue: function (target) {
                return $(target).lookup('getValue');
            },
            setValue: function (target, value) {
                $(target).lookup('setValue', value);
            },
            resize: function (target, width) {
                $(target).lookup('resize', width);
            }
        }
    });
})(jQuery);