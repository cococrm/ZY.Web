(function ($) {
    $.easyui.treegrid = (function () {
        editIndex = undefined;
        return {
            //判断编辑
            endEditing: function (gridId) {
                if (editIndex == undefined) {
                    return true;
                }
                var grid = $("#" + gridId);
                if (grid.treegrid('validateRow', editIndex)) {
                    grid.treegrid('endEdit', editIndex);
                    editIndex = undefined;
                    return true;
                } else {
                    return false;
                }
            },
            //添加并编辑
            addRow: function (gridId, url) {
                if ($.easyui.treegrid.endEditing(gridId)) {
                    var grid = $("#" + gridId);
                    //var row= { ID: $.newGuid('-'), MenuName: "", MenuCode: "", ParentId: $.newEmptyGuid(), SortId: 99, isNewRecord: true };
                    $.getJSON(url, function (row) {
                        grid.treegrid('append', { parent: '', data: [row] });
                        grid.treegrid('select', row.ID);
                        grid.data("datagrid").insertedRows.push(row);
                        $.easyui.treegrid.edit(gridId);
                    });
                }
            },
            //编辑行
            edit: function (gridId) {
                var grid = $("#" + gridId);
                var row = grid.treegrid('getSelected');
                if (!row) {
                    $.easyui.warn("请选择待编辑的记录！");
                    return;
                }
                if (editIndex != row.ID) {
                    if ($.easyui.treegrid.endEditing(gridId)) {
                        editIndex = row.ID;
                        grid.treegrid('selectRow', editIndex);
                        grid.treegrid('beginEdit', editIndex);
                    } else {
                        grid.treegrid('selectRow', editIndex);
                    }
                }
            },
            //取消编辑
            cancel: function (gridId) {
                if (editIndex == undefined) { return; }
                var grid = $("#" + gridId);
                grid.treegrid('cancelEdit', editIndex);
                editIndex = undefined;
            },
            //删除节点
            remove: function (gridId) {
                var grid = $("#" + gridId);
                var row = grid.treegrid('getSelected');
                if (!row) {
                    $.easyui.warn("请选择待删除的记录！");
                    return;
                }
                grid.treegrid('remove', row.ID);
            },
            //刷新
            refresh: function (gridId) {
                var grid = $("#" + gridId);
                grid.treegrid('reload');
            },
            //保存
            save: function (gridId, url) {
                var grid = $("#" + gridId);
                if ($.easyui.treegrid.endEditing(gridId)) {
                    if (grid.treegrid('getChanges').length == 0) {
                        return;
                    }
                    var addList = grid.treegrid('getChanges', 'inserted');
                    var updateList = grid.treegrid('getChanges', 'updated');
                    var deleteList = grid.treegrid('getChanges', 'deleted');
                    var data = { addList: $.toJSON(addList), updateList: $.toJSON(updateList), deleteList: $.toJSON(deleteList) };
                    $.easyui.ajax('Save', data, function () {
                        grid.treegrid('acceptChanges');
                        $.easyui.treegrid.refresh(gridId);
                    });
                }
            }
        };
    })();
})(jQuery);