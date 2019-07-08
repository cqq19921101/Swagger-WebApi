/// <reference path="jquery-1.10.2.min.js" />
$(document).ready(function () {
    //$.post("ajax/MealCar.ashx", { "cmd": "" }, function (data) {
    //    var data = eval("(" + data + ")");//解析Json
    //    if (data.Success == true) {
            //for (i in data.Info)
            //{
            //    var tr;
            //    tr = '<td>' + data.Info[i].POSNAME + '</td>' + '<td>' + data.Info[i].CONSTYPE + '</td>' + '<td>' + data.Info[i].CONSFARE + '</td>' + '<td>' + data.Info[i].CONSTIME + '</td>';
            //    $("#test").append('<tr>' + tr + '</tr>');
            //}
        //}
        //else {

        //}

    //})

    layui.use('table', function () {
        var table = layui.table;

        table.render({
            elem: '#test'
          , url: 'liteonApi/SmartMeter/GetMealCar'
                    //    , response: {
                    //        statusName: 'status' //规定数据状态的字段名称，默认：code
                    //, statusCode: 200 //规定成功的状态码，默认：0
                    //, msgName: 'hint' //规定状态信息的字段名称，默认：msg
                    //, countName: 'total' //规定数据总数的字段名称，默认：count
                    //, dataName: 'Info' //规定数据列表的字段名称，默认：data
                    //    }

          , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
          , cols: [[
            { field: 'POSNAME', width: 80, title: '機器名', }
            //, { field: 'username', width: 80, title: '用户名' }
            //, { field: 'sex', width: 80, title: '性别', }
            //, { field: 'city', width: 80, title: '城市' }
          ]]
        });
    });



})