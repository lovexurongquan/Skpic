function GoToPage(ctr, refName, callback) {
    $("#DoAndShow").hide();
    var items = new Array();
    $.each($('form').formSerialize().split('&'), function(i, datastr) {
        var keyValue = datastr.split('=');
        if (typeof(items[keyValue[0]]) != "undefined")
            items[keyValue[0]] = items[keyValue[0]] + ',' + keyValue[1];
        else
            items[keyValue[0]] = keyValue[1];
    });
    var hrefStr = $(ctr).attr('href');//分页按键页码优先
    if (hrefStr) {
        hrefStr = hrefStr.split("?")[1];
        $.each(hrefStr.split('&'), function(i, datastr) {
            var keyValue = datastr.split('=');
            items[keyValue[0]] = keyValue[1];
        });
    }
    var newUrl = "";
    for (var item in items) {
        newUrl += "&"+item + "=" + items[item];
    }
    newUrl = "?dd=" + Math.random()+"&"+ newUrl.substr(1, newUrl.length - 1);
    $.blockUI({ message: '<h3>数据加载中...</h3>', showOverlay: false, border: 'none' });
    var loadUrl = $("#myaction").val() + newUrl + " #" + refName;
    $("#" + refName).load(loadUrl, function (responseText, status, res) {
        $.unblockUI();
        if (status == "error") $.blockUI({ message: '加载出错,请刷新后重试' + res.status + " " + loadUrl });
        if (callback) callback([responseText, status, res]);
        //隔行样式的处理
        $(".list_cut tr:even").addClass("alt");//给class为stripe的表格的偶数行添加class值为alt
        $(".list_cut tr").mouseover(function() { //如果鼠标移到class为stripe的表格的tr上时，执行函数
            $(this).addClass("over");
        }).mouseout(function() { //给这行添加class值为over，并且当鼠标一出该行时执行函数
            $(this).removeClass("over");
        }); //移除该行的class
    });
    return false;
}
$(document).ready(function () {  //这个就是传说的ready
    $(".list_cut tr").mouseover(function() { //如果鼠标移到class为stripe的表格的tr上时，执行函数
        $(this).addClass("over");
    }).mouseout(function() { //给这行添加class值为over，并且当鼠标一出该行时执行函数
        $(this).removeClass("over");
    });  //移除该行的class
    $(".list_cut tr:even").addClass("alt");//给class为stripe的表格的偶数行添加class值为alt
});


//$(function () { //读取title法给input加注释
//    $('.input').example(function () {
//        return $(this).attr('title');
//    });
//});