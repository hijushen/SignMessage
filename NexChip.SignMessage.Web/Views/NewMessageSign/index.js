﻿
$(function () {
    $("#btn-query").click(function () { query(); });
    //$("#btnDelete").click(function () { deleteMulti(); });
    $("#btnSave").click(function () { save(); });
    //$("#checkAll").click(function () { checkAll(this) });
    loadDataTable();




});

function reloadTables() {
    exampleTable.ajax.reload(null, true);
}

/// 初始化日期控件
function setDataPicker() {
    var start = moment().subtract(29, 'days');
    var end = moment();
    function cb(start, end) {
        $('#reportrange span').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        format: 'YYYY-MM-DD', //控件中from和to 显示的日期格式
        locale: {
            applyLabel: '确定',
            cancelLabel: '取消',
            fromLabel: '起始时间',
            toLabel: '结束时间',
            customRangeLabel: '自定义',
            daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                '七月', '八月', '九月', '十月', '十一月', '十二月'],
            firstDay: 1
        }
        , ranges: {
            '今日': [moment(), moment()],
            //'昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            '过去7天': [moment().subtract(6, 'days'), moment()],
            '一个月内': [moment().subtract(29, 'days'), moment()],
            '本月': [moment().startOf('month'), moment().endOf('month')],
            '上个月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);
    cb(start, end);
}

//提示信息
var lang = {
    "lengthMenu": "每页 _MENU_  项",
    "processing": "处理中...",
    "zeroRecords": "没有匹配结果",
    "info": "当前显示第 _START_ 到 _END_ 项, 共 _TOTAL_ 项",
    "infoEmpty": "暂无数据",
    "infoFiltered": "(由 _MAX_ 项结果过滤)", //当使用搜索功能后，表格主要信息出追加的字符
    "loadingRecords": "加载中...",
    "processing": "加载中， 请稍后...",
    "search": "搜索:",
    "infoPostFix": "", //追加到所有其他主要信息字符串之后
    "url": "",
    "emptyTable": "表中数据为空",
    "infoThousands": ",",
    "paginate": {
        "first": "首页",
        "last": "末页",
        "next": "下页",
        "previous": "上页"
    }
};

var partshowtool = {
    //默认现实的字符串长度
    remarkShowLength: 30
    ,
    changeShowRemarks: function (obj) {//obj是td
        debugger;
        var content = $(obj).attr("content");
        if (content != null && content != '') {
            if ($(obj).attr("isDetail") == 'true') {//当前显示的是详细备注，切换到显示部分
                //$(obj).removeAttr('isDetail');//remove也可以
                $(obj).attr('isDetail', false);
                $(obj).html(partshowtool.getPartialRemarksHtml(content));
            } else {//当前显示的是部分备注信息，切换到显示全部
                $(obj).attr('isDetail', true);
                $(obj).html(partshowtool.getTotalRemarksHtml(content));
            }
        }
    }
    ,
    //切换显示备注信息，显示部分或者全部
    getPartialRemarksHtml: function (remarks) {
        return remarks.substr(0, partshowtool.remarkShowLength) + '&nbsp;&nbsp;<a href="javascript:void(0);" ><b>...</b></a>';
    }
    ,
    //全部备注信息
    getTotalRemarksHtml: function (remarks) {
        return remarks + '&nbsp;&nbsp;<a href="javascript:void(0);" >收起</a>';
    }

}

//
function createEditToolColumn(item) {
    return "<button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" + item.oid + "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" + item.oid + "\")'><i class='fa fa-trash-o'></i> 删除 </button> "
}



//新增
function add() {
    $("#Id").val("");
    $("#appname").val("");
    $("#appnamechs").val("");
    $("#reservedkey1").val("");
    $("#Title").text("新增应用程序");
    //弹出新增窗体
    $("#editModal").modal("show");
};

//保存
function save() {
    var postData = { "dto": { "OID": $("#Id").val(), "appname": $("#appname").val(), "appnamechs": $("#appnamechs").val(), "reservedkey1": $("#reservedkey1").val() } };
    $.ajax({
        type: "Post",
        url: "/NewMessageSign/EditSave",
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
//编辑
function edit(id) {
    $.ajax({
        type: "Get",
        url: "/NewMessageSign/GetS?OID=" + id + "&_t=" + new Date().getTime(),
        success: function (res) {
            var data = res.data || {};
            $("#Id").val(id);
            $("#appname").val(data.appname);
            $("#appnamechs").val(data.appnamechs);
            $("#reservedkey1").val(data.reservedkey1);

            $("#Remarks").val(data.remarks);

            $("#Title").text("编辑应用程序")
            $("#editModal").modal("show");
        }
    })
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
            url: "/NewMessageSign/Delete",
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

//初始化表格
function loadDataTable() {
    exampleTable = $("#example").DataTable({
        language: lang, //提示信息
        stateSave: true, //状态保存，再次加载页面时还原表格状态
        autoWidth: false, //禁用自动调整列宽
        stripeClasses: ["odd", "even"], //为奇偶行加上样式，兼容不支持CSS伪类的场合
        processing: true, //隐藏加载提示,自行处理
        serverSide: true, //启用服务器端分页
        searching: false, //禁用原生搜索
        orderMulti: false, //启用多列排序
        order: [], //取消默认排序查询,否则复选框一列会出现小箭头
        deferRender: true, //延迟渲染可以提高Datatables的加载速度
        lengthMenu: [
            [10, 25, 50, 100, 300, -1],
            [10, 25, 50, 100, 300, "所有"]
        ], //每页多少项，第一个数组是表示的值，第二个数组用来显示
        renderer: "bootstrap", //渲染样式：Bootstrap和jquery-ui
        pagingType: "full_numbers", //分页样式：simple,simple_numbers,full,full_numbers
        scrollY: 800, //表格的固定高
        scrollCollapse: true, //开启滚动条
        pageLength: 10, //首次加载的数据条数
        columnDefs: [
            {
                targets: 'nosort', //列的样式名
                orderable: false //包含上样式名‘nosort'的禁止排序
            }
        ],
        //对应列表表头字段
        columns: [{
            "title": "createtime", "data": "createtime", "orderable": false, "visible": false
        }, {
            "title": "fromempname", "data": "fromempname"
        }, {
            "title": "appname", "data": "appname", "orderable": false,
        }, {
            "title": "substance", "data": "substance", "orderable": false
        }, {
            "title": "操作",
            "data": null,
            "orderable": false,
            "render": function (data, type, full, meta) {
                return createEditToolColumn(data)
            }
        }
        ],
        "initComplete": function (settings, json) {
            //$('div.loading').hide();
        },
        "createdRow": function (row, data, dataIndex) {
            //debugger;
            if (data.reservedkey2.length > partshowtool.remarkShowLength) {
                //只有超长，才有td点击事件
                //$(row).children('td').eq(4).attr('onclick', 'javascript:partshowtool.changeShowRemarks(this);');
                //$(row).children('td').eq(4).attr('content', data.reservedkey2);
                //$(row).children('td').eq(4).attr('isDetail', false);
            }
        },
        ajax: function (data, callback, settings) {
            //封装请求参数
            var param = {};
            param.length = data.length; //页面显示记录条数，在页面显示每页显示多少项的时候
            param.start = data.start; //开始的记录序号
            //param.page = (data.start / data.length) + 1; //当前页码

            //ajax请求数据
            $.ajax({
                url: "/NewMessageSign/DataTableList",
                type: "POST",
                cache: false, //禁用缓存
                data: data, //传入组装的参数
                dataType: "json",
                contentType: 'application/x-www-form-urlencoded;charset=utf-8',  //application/json
                //beforeSend: function (request) {
                //    request.setRequestHeader("token", localStorage.token);
                //},
                success: function (result) {
                    setTimeout(function () {
                        if ($('#example_processing').hide) {
                            $('#example_processing').hide();
                        }
                        callback(result);
                    }, 200);
                }
            });
        },

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