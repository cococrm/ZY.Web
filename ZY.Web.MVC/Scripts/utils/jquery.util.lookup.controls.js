(function ($) {
    //角色查找带回控件
	$.easyui.lookupControl = function () {
		return {
			role: function (id, options) {
				var _lookup = $("#" + id);
				initOptions();
				_lookup.lookup(options);
				function initOptions() {
					options = $.extend({
						title: '选择角色',
						method: "get",
						url: "roles/list",
						idField: "id",
						textField: "name",
						panelWidth: 500,
						panelHeight: 300,
						multiple: true,
						singleSelect: false,
						pagination: true,
						pageSize: 20,
						columns: [[
							{ field: 'id', checkbox: true, width: 100 },
							{ field: 'name', title: '角色名称', width: 150 },
						]]
					}, options || {});
				}
			}
		};
	}();
})(jQuery);