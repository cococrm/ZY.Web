(function ($) {
    function create(target) {
        var state = $.data(target, "lookup");
        opts = state.options;
        t = $(target).combobox($.extend({}, opts, {
            onClickButton: function () {
                t.combobox("panel").hide();
                var dialogId = $.newGuid();
                var gridId = $.newGuid();
                var dialog = parent.$("<div id='" + dialogId + "'></div>").appendTo('body');
                var grid = parent.$("<table id='" + gridId + "'></table>").appendTo(dialog);
                grid.datagrid({
                    url: opts.url,
                    method: opts.method,
                    idField: opts.idField,
                    fit: true,
                    rownumbers: opts.rownumbers,
                    pagination: opts.pagination,
                    pageSize: opts.pageSize,
                    singleSelect: opts.singleSelect,
                    toolbar: opts.toolbar,
                    columns: opts.columns,
                    onLoadSuccess: function (data) {
                        var values = t.combo('getValues');
                        if (values.length > 0) {
                            $.each(values[0].split(','), function (i, v) {
                                grid.datagrid("selectRecord", v);
                            })
                        }
                    }
                });
                if (opts.pagination) {
                    grid.pagination({
                        pageSize: opts.pageSize,
                        pageList: [5, 10, 15],
                        beforePageText: '第',
                        afterPageText: '页    共 {pages} 页',
                        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
                    });
                }
                dialog.dialog({
                    title: opts.title,
                    iconCls: opts.icon,
                    width: $.isNumber(opts.panelWidth) ? opts.panelWidth : grid.width,
                    height: $.isNumber(opts.panelHeight) ? opts.panelHeight : grid.height,
                    closed: false,
                    maximizable: true,
                    resizable: true,
                    cache: false,
                    modal: true,
                    buttons: [{
                        text: '确定',
                        iconCls: 'icon-ok',
                        handler: function () {
                            var row = grid.datagrid("getSelections");
                            var values = [],text = [];
                            $(row).each(function () {
                                values.push(this[opts.idField]);
                                text.push(this[opts.textField]);
                            });
                            if (row) {
                                t.combobox('setValue', values).combobox('setText', text);
                            }
                            parent.$("#" + dialogId).dialog('destroy');
                        }
                    }, {
                        text: '取消',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            parent.$("#" + dialogId).dialog('destroy');
                        }
                    }],
                    onClose: function () {
                        if (opts.closeCallback)
                            opts.closeCallback();
                        parent.$("#" + dialogId).dialog('destroy');
                    }
                })
            }
        }));
    };

    $.fn.lookup = function (options, param) {
        options = options || {};
        return this.each(function () {
            var state = $.data(this, "lookup");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "lookup", { options: $.extend({}, $.fn.lookup.defaults, $.fn.lookup.parseOptions(this), options) });
                create(this);
            }
        });
    };

    $.fn.lookup.parseOptions = function (target) {
        return $.extend({},
            $.fn.combobox.parseOptions(target),
            $.fn.datagrid.parseOptions(target),
            $.parser.parseOptions(target, ["text", "selector", "iconCls"])
        );
    };

    //lookup 默认参数
    $.fn.lookup.defaults = $.extend({}, $.fn.combobox.defaults, {
        required: true, //textbox是否验证
        text: null, //textbox默认文本
        prompt: '', //textbox提示
        data: null, //combo data
        autoShowPanel: false, //隐藏combo panel
        hasDownArrow: false, //不显示combo 下拉图标
        buttonText: '选择', //按钮文本
        panelWidth: "auto", //dialog width
        panelHeight: "auto", //dialog height
        icon: "icon-search", //dialog icon
        url: null, //datagrid url
        toolbar: null, //datagrid toolbar
        rownumbers: true, //datagrid 是否显示行号
        pagination: false, //datagrid 是否分页
        pageSize: 10, //datagrid pageSize
        singleSelect: true, //datagrid 单选，多选
        idField: "id", //datagrid主键列 input的值
        textField: "name",//datagrid返回显示的文本列  input文本显示值
        onLoadSuccess:function(){

        }
    });

    $.parser.plugins.push("lookup");

})(jQuery);