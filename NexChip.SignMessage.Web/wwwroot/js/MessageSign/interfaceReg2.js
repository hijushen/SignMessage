var selectedRole = 0;
$(function () {
    $("#btnAdd").click(function () { add(); });
    //$("#btnDelete").click(function () { deleteMulti(); });
    $("#btnSave").click(function () { save(); });
    $("#checkAll").click(function () { checkAll(this) });
    reloadTables();
});

function reloadTables() {
    loadTables(1, 5);
}


function loadTables(startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checked", false);
    $.ajax({
        type: "GET",
        url: "/InterfaceReg/List?offset=" + startPage + "&limit=" + pageSize + "&_t=" + new Date().getTime(),
        success: function (data) {
            $.each(data.rows, function (i, item) {
                var tr = "<tr>";
                tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.oid + "'/></td>";
                tr += "<td>" + (item.appname == null ? "" : item.appname) + "</td>";
                tr += "<td>" + item.appnamechs + "</td>";
                tr += "<td>" + item.reservedkey1 + "</td>";
                tr += "<td class=\"reserverkey2\">" + item.reservedkey2 + "</td>";
                tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" + item.oid + "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" + item.oid + "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>"
                tr += "</tr>";
                $("#tableBody").append(tr);
            })
            var elment = $("#grid_paging_part"); //分页插件的容器id
            if (data.total > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.total, //总数
                    totalPages: getTotalPages(data.total, pageSize), //data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件
                        loadTables(newPage, pageSize);
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
            $("table > tbody > tr").click(function () {
                $("table > tbody > tr").removeAttr("style")
                $(this).attr("style", "background-color:#beebff");
                selectedRole = $(this).find("input").val();
                //loadPermissionByRole(selectedRole);
            });
        }
    })
}
//全选
function checkAll(obj) {
    $(".checkboxs").each(function () {
        if (obj.checked == true) {
            $(this).prop("checked", true)

        }
        if (obj.checked == false) {
            $(this).prop("checked", false)
        }
    });
};
//新增
function add() {
    if ($("#appname").attr("readonly") == true) {
        $("#appname").removeAttr("readonly");
    };

    $("#Id").val("");
    $("#appname").val("");
    $("#appnamechs").val("");
    $("#reservedkey1").val("");
    $("#Title").text("新增应用程序");
    //弹出新增窗体
    $("#editModal").modal("show");
};
//编辑
function edit(id) {
    $.ajax({
        type: "Get",
        url: "/InterfaceReg/GetS?OID=" + id + "&_t=" + new Date().getTime(),
        success: function (res) {
            var data = res.data || {};
            $("#Id").val(id);
            $("#appname").attr("readonly", "readonly");
            $("#appname").val(data.appname);
            $("#appnamechs").val(data.appnamechs);
            $("#reservedkey1").val(data.reservedkey1);

            $("#Remarks").val(data.remarks);

            $("#Title").text("编辑应用程序")
            $("#editModal").modal("show");
        }
    })
};
//保存
function save() {
    var postData = { "dto": { "OID": $("#Id").val(), "appname": $("#appname").val(), "appnamechs": $("#appnamechs").val(), "reservedkey1": $("#reservedkey1").val() } };
    $.ajax({
        type: "Post",
        url: "/InterfaceReg/EditSave",
        data: postData,
        success: function (data) {
            if (data.success) {
                reloadTables();
                $("#editModal").modal("hide");
            } else {
                layer.tips(data.msg, "#btnSave");
            };
        }
    });
};
//批量删除
function deleteMulti() {
    var ids = "";
    $(".checkboxs").each(function () {
        if ($(this).prop("checked") == true) {
            ids += $(this).val() + ","
        }
    });
    ids = ids.substring(0, ids.length - 1);
    if (ids.length == 0) {
        layer.alert("请选择要删除的记录。");
        return;
    };
    //询问框
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        var sendData = { "ids": ids };
        $.ajax({
            type: "Post",
            url: "/Role/DeleteMuti",
            data: sendData,
            success: function (data) {
                if (data.result == "Success") {
                    reloadTables()
                    layer.closeAll();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        });
    });
};
//删除单条数据
function deleteSingle(id) {
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        var ids = [];
        ids.push(id);
        $.ajax({
            type: "POST",
            url: "/InterfaceReg/Delete",
            data: { "OIDs": ids },
            success: function (data) {
                if (data.success) {
                    reloadTables()
                    layer.closeAll();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        })
    });
};


//
var format = {
    status: function (value) {
        if (value) {
            return '有效'
        } else {
            return '无效'
        }
    }
};

function getTotalPages(total, pagesize) {
    if (total == 0) return 1;
    return Math.ceil(parseFloat(total, 0) / parseFloat(pagesize, 0));
}
