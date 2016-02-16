(function ($) {
    $.project = (function () {
        return {
            //加载导航菜单
            loadNavMenu: function (url, menuId) {
                $.easyui.ajax(url, '', function (data) {
                    $.each(data, function () {
                        $.project.addToAccordion(menuId, this.text, this.icon, false, this.children);
                    });
                    $("#" + menuId).accordion('select', 0);//默认选择第一个
                }, 'get')
            },
            //加载手风琴
            addToAccordion: function (id, title, icon, select, treedata) {
                if (!id && !title)
                    return;
                $("#" + id).accordion('add', {
                    title: title,
                    content: $.project.addToNavTree(treedata),
                    selected: select
                });
            },
            //加载导航tree
            addToNavTree: function (treedata) {
                var treeid = $.newGuid("");
                var str = '<ul id="' + treeid + '" class="easyui-tree" data-options="onClick:$.project.clickMainMenuNode"></ul>';
                str += '<script>$(function(){$("#' + treeid + '").tree({data:' + $.toJSON(treedata) + '})})</script>';
                return str;
            },
            //单击左侧菜单树
            clickMainMenuNode: function (node) {
                if (!node.attributes)
                    return;
                if (!node.attributes.url)
                    return;
                $.project.addToMainTabs(node.text, node.attributes.url, node.iconCls, true);
            },
            //添加我的桌面
            addDeskTopTab: function () {
                $.project.addToMainTabs("我的桌面", "/Home/Home/Welcome", "icon-house", false);
            },
            //添加Tabs面板
            addToMainTabs: function (title, url, icon, closable) {
                $.easyui.addIframeToTabs("divMainTabs", title, url, icon, closable);
                var tabs = $('#divMainTabs');
                bindTabsMenu();
                bindTabsDbClick();

                //绑定选项卡右键菜单
                function bindTabsMenu() {
                    tabs.tabs({
                        onContextMenu: function (e, title, index) {
                            $.easyui.showMenu(getMenuId(), e);
                            tabs.tabs('select', index);

                            //获取选项卡菜单Id
                            function getMenuId() {
                                return index === 0 ? "divDesktopTabsMenu" : "divTabsMenu";
                            }
                        }
                    });
                }

                //绑定选项卡双击事件
                function bindTabsDbClick() {
                    $(".tabs-inner").unbind("dblclick");
                    $(".tabs-inner").dblclick(function () {
                        var selectedTab = tabs.tabs('getSelected');
                        var selectedIndex = tabs.tabs('getTabIndex', selectedTab);
                        if (selectedIndex === 0)
                            return;
                        tabs.tabs('close', selectedIndex);
                    });
                }
            },
            //绑定选项卡菜单单击事件
            bindTabsMenuClick: function () {
                var tabs = $('#divMainTabs');
                bindMenuClick('divTabsMenu');
                bindMenuClick('divDesktopTabsMenu');

                function bindMenuClick(contextMenuId) {
                    $('#' + contextMenuId).menu({
                        onClick: function (item) {
                            var allTabs = tabs.tabs('tabs');
                            var selectedTab = tabs.tabs('getSelected');
                            var selectedIndex = tabs.tabs('getTabIndex', selectedTab);
                            command(item.id);
                            //执行命令
                            function command(id) {
                                switch (id) {
                                    case "menuItem_Refresh":
                                        return refresh();
                                    case "menuItem_CloseCurrent":
                                        return closeCurrent();
                                    case "menuItem_CloseOther":
                                        return closeOther();
                                    case "menuItem_CloseAll":
                                        return closeAll();
                                }
                                return true;
                            }

                            //刷新选项卡
                            function refresh() {
                                $.easyui.refreshTabs("divMainTabs");
                            }

                            //关闭当前
                            function closeCurrent() {
                                tabs.tabs('close', selectedIndex);
                            }

                            //关闭其它
                            function closeOther() {
                                close(function (i) {
                                    return i === 0 || i === selectedIndex;
                                });
                                tabs.tabs('select', 1);
                            }

                            //关闭窗口
                            function close(ignore) {
                                $(allTabs).each(function (i, tab) {
                                    if (!ignore(i)) {
                                        var index = tabs.tabs('getTabIndex', tab);
                                        tabs.tabs('close', index);
                                    }
                                });
                            }

                            //关闭全部
                            function closeAll() {
                                close(function (i) {
                                    return i === 0;
                                });
                            }
                        }
                    });
                }
            }
        };
    })();
})(jQuery);
$(function () {
    $.project.addDeskTopTab();
    $.project.bindTabsMenuClick();
});