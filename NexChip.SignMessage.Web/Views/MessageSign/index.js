﻿
$(function () {
    $("#btn-query").click(function () { reloadTables(); });
    //$("#btnDelete").click(function () { deleteMulti(); });
    $("#btnSave").click(function () { save(); });
    //$("#checkAll").click(function () { checkAll(this) });
    loadDataTable();

    setDataPicker();
    setSelect();
    setModalCenter();
});

///设置select控件样式（select2组件）
function setSelect() {
    $("#formTypeSelect").select2({
        width: "200px",
        language: "zh-CN"
    });

    $("#handleStatusSelect").select2({
        width: "80px",
        language: "zh-CN",
        minimumResultsForSearch: Infinity
    });
}

///设置弹窗居中
function setModalCenter() {
    var $modal = $('#editModal');
    // 测试 bootstrap 居中
    $modal.on('shown.bs.modal', function () {
        var $this = $(this);
        var $modal_dialog = $this.find('.modal-dialog');
        var m_top = ($(window).height() - $modal_dialog.height()) / 2;
        $modal_dialog.css({ 'margin': m_top + 'px auto' });
    })
}

Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        //月份

        "d+": this.getDate(),
        //日

        "h+": this.getHours(),
        //小时

        "m+": this.getMinutes(),
        //分

        "s+": this.getSeconds(),
        //秒

        "q+": Math.floor((this.getMonth() + 3) / 3),
        //季度

        "S": this.getMilliseconds() //毫秒

    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

function reloadTables() {
    if (exampleTable) {
        exampleTable.ajax.reload(null, true);
    }
}

function getDefaultDateSpanStr() {

    var start = moment().subtract(29, 'days');
    var end = moment();

    return start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD');
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

    //cb(start, end);
    $('#reportrange span').html(getDefaultDateSpanStr());
}


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
        //scrollY: 800, //表格的固定高
        scrollCollapse: true, //开启滚动条
        pageLength: 10, //首次加载的数据条数
        columnDefs: [
            {
                targets: 'nosort', //列的样式名
                orderable: false //包含上样式名‘nosort'的禁止排序
            }
        ],
        //对应列表表头字段
        columns: [
            {
                "title": "收到日期", width: "10%", "data": "createtime", "orderable": false, "text-align": "center",
                render: function (data, type, row, meta) {
                    return (new Date(data)).Format("yyyy-MM-dd");
                    //debugger;
                }
            }, {
                "title": "寄件人", width: "10%", "data": "fromempname", "orderable": false
            }, {
                "title": "AppName", width: "15%", "data": "appname", "orderable": false,
            }, {
                "title": "主旨", "data": "substance", "orderable": false
            }, {
                "title": "操作", width: "15%",
                "data": null,
                "orderable": false,
                "render": function (data, type, full, meta) {
                    return createEditToolColumn(data)
                }
            }
        ],
        "fnRowCallback": function (row, data, index) {
            // debugger;
            if (!data.msgstatus) {
                $(row).addClass('highlight');
            }
        },

        "initComplete": function (settings, json) {
            //$('div.loading').hide();
        },
        "createdRow": function (row, data, dataIndex) {
            //debugger;
            //if (data.reservedkey2.length > partshowtool.remarkShowLength) {
            //只有超长，才有td点击事件
            //$(row).children('td').eq(4).attr('onclick', 'javascript:partshowtool.changeShowRemarks(this);');
            //$(row).children('td').eq(4).attr('content', data.reservedkey2);
            //$(row).children('td').eq(4).attr('isDetail', false);
            //}
        },
        ajax: function (data, callback, settings) {
            //封装请求参数
            var param = {};
            param.length = data.length; //页面显示记录条数，在页面显示每页显示多少项的时候
            param.start = data.start; //开始的记录序号
            param.timespan = $('#reportrange span').html() || getDefaultDateSpanStr(); //日期区间(初次加载为默认）)
            param.formtype = $('#formTypeSelect').val(); //表单类型
            param.handlestatus = $('#handleStatusSelect').val(); //处理状况

            //param.page = (data.start / data.length) + 1; //当前页码

            //ajax请求数据
            $.ajax({
                url: "/MessageSign/DataTableList",
                type: "POST",
                cache: false, //禁用缓存
                data: param, //传入组装的参数
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
        }

    });

    $('#example tbody').on('click', 'td', function () {
        var table = exampleTable;
        var idx = table.cell(this).index().column;
        var title = table.column(idx).header();


        if ($(title).html() === '操作') {
            return;
        } else { //行点击事件
            var data = table.row($(this).index().row).data();
            rowClickHandler(table.row($(this).index().row));
            window.open(data.callbackurl, '_blank');

            ///更新已读状态
            $.ajax({
                type: "POST",
                url: "/MessageSign/UpdateRead",
                data: { "OID": data.oid },
                success: function (data) {
                    if (data.success) {
                       
                    }
                    else {
                        layer.alert(data.msg);
                    }
                }
            });
        } //行点击事件结束
    });
};

//样式变更.
function rowClickHandler(row) {
    ///单选行加样式
    //exampleTable.$('tr.selected').removeClass('selected');
    //row.addClass('selected');
    //row.removeClass('highlight');

    //$(row).removeClass('selected');
    //$(row).addClass('selected');
    //$(row).removeClass('highlight');

    var row = exampleTable.$('tr.selected');
    row.removeClass('selected');
    row.removeClass('highlight');
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

///操作列
function createEditToolColumn(item) {
    return "<button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" + item.oid
        + "\")'><i class='fa fa-edit'></i> 详细 </button>"
        //+ " <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" + item.oid
        //+ "\")'><i class='fa fa-trash-o'></i> 删除 </button> "
        + "<button class='btn btn-success btn-xs' href='javascript:;' onclick='testsend(\"" + item.oid
        + "\",1)'<i class='fa fa-edit'></i>测试发送-更新</button>"
        + "<button class='btn btn-warning btn-xs' href='javascript:;' onclick='testsend(\"" + item.oid
        + "\",2)'<i class='fa fa-edit'></i>测试发送-新增</button>"
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
        url: "/MessageSign/EditSave",
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
    Array.prototype.findObj = function (callback) {
        for (var i = 0, length = this.length; i < length; i++) {
            var item = this[i];
            if (callback(item)) {
                return item;
            }
        }
        return null;
    };

    var sourceArray = exampleTable.data();
    var arrayD = [];
    for (var i = 0; i < sourceArray.length; i++) {
        arrayD.push(sourceArray[i]);
    }
    var findItem = arrayD.findObj(function (item) {
        return item.oid == id;
    });

    if (findItem) {
        $("#detail-substance").val(findItem.substance);
        $("#detail-showmsg").val(findItem.showmsg);
        $("#detail-createtime").val(findItem.createtime);
        $("#detail-updatetime").val(findItem.updatetime);

    }

    $("#editModal").modal("show");

    //$.ajax({
    //    type: "Get",
    //    url: "/MessageSign/GetS?OID=" + id + "&_t=" + new Date().getTime(),
    //    success: function (res) {
    //        var data = res.data || {};
    //        $("#Id").val(id);
    //        $("#appname").val(data.appname);
    //        $("#appnamechs").val(data.appnamechs);
    //        $("#reservedkey1").val(data.reservedkey1);

    //        $("#Remarks").val(data.remarks);

    //        $("#Title").text("编辑应用程序")
    //        $("#editModal").modal("show");
    //    }
    //})
};

///测试发送
function testsend(id,type) {
    $.ajax({
        type: "POST",
        url: "/MessageSign/testSend",
        data: { "OID": id,"type":type },
        success: function (data) {
            if (data.success) {
                //reloadTables()
                //layer.closeAll();
                layer.alert("测试成功！");
            }
            else {
                layer.alert("测试失败！");
            }
        }
    });
}


//删除单条数据
function deleteSingle(id) {
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        var ids = [];
        ids.push(id);
        $.ajax({
            type: "POST",
            url: "/MessageSign/Delete",
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
