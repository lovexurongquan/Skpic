function Do(method,vals, showAffirm) {
    if (showAffirm && !confirm(showAffirm)) return false;
    $.blockUI({ message: '<h3>操作中...</h3>', showOverlay: false, border: 'none' });
    $.post($("#myaction").val() + "Do", { method: method, vals: vals }, function (data) {
        $.unblockUI();
        GoToPage(this, 'data', pageCallback);
    });
    return false;
}
function DoAndShow(ctr, pName, id, vals, isSubmit) {
    $('#DoAndShow').css("top", $(ctr).offset().top + 0);
    $('#DoAndShow').css("left", $(ctr).offset().left - 355);
    $("#DoAndShow").show();
    $("#DoAndShow").load(pName + "?id=" + id + "&isSubmit=" + isSubmit + "&vals=" + vals);
    return false;
}
function myWebShow(ctr, url, subName) {
    $('#WebShow').css("top", $(ctr).offset().top + 0);
    $('#WebShow').css("left", $(ctr).offset().left - 555);
    $("#WebShow").show();
    $("#WebShowSub").load("LoadUrl?url=" + url + "&dd=" + Math.random() + " " + subName, function () {

    });
    return false;
}
function CheckAll(ctr, name) {
    if ($(ctr).attr('checked'))
        $("input[name=" + name + "]").attr('checked', true);
    else
        $("input[name=" + name + "]").attr('checked', false);
}
function GetCheckVales(name) {
    var values = '';
    $("input[name=" + name + "]").each(function (i, ctr) {
        if ($(ctr).attr('checked')) values += ',' + $(ctr).val();
    });
    return values;
}
//城市组件
function displayHotelCities(data, template) {

    var htmlStringBuffer = [],
        hoties = [],
        cities = [],
        item,
        itemCity,
        i = 0,
        k = 0;
    if (!(hoties = setupHotCities(hotelHotCities, data)).length) {
        return;
    }
    if (!(cities = sortByAlphabet(data)).length) {
        return;
    }
    template = template.replace("{tabs-id}", "mNotice-mTab-" + this[0].id);
    htmlStringBuffer.push('<div class="mNotice-mTab-content clearfix">');
    while (item = hoties[i++]) {
        htmlStringBuffer.push('<span class="mNotice-normal" title="' + item.name + '" cid="' + item.id + '" ctype="'
            + item.type + '">' + item.name + '</span>');
    }
    htmlStringBuffer.push('</div><div class="mNotice-mTab-content none">');
    i = 0;
    while (item = cities[i++]) {
        htmlStringBuffer.push('<dl class="clearfix mNotice-block"><dt class="mNotice-title">' + item.letter
            + '</dt><dd class="mNotice-def">');
        while (itemCity = item.cities[k++]) {
            htmlStringBuffer.push('<span class="mNotice-normal" title="' + itemCity.name + '" cid="'
                + itemCity.id + '" ctype="' + itemCity.type + '">' + itemCity.name + '</span>');
        }
        k = 0;
        htmlStringBuffer.push('</dd></dl>');
        if (item.letter === "F" || item.letter === "M" || item.letter === "S") {
            htmlStringBuffer.push('</div><div class="mNotice-mTab-content none">');
        }
        if (item.letter === "Z") {
            htmlStringBuffer.push('</div>');
        }
    }
    return template.replace("{contents}", htmlStringBuffer.join(""));
    // data 按字母排序
    function sortByAlphabet(data) {
        var ordered = [];
        var readyForSort = [];
        var alphabet = [
            "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "W", "X", "Y", "Z"
        ];
        // letterAndCities = {},
        var i = 0;
        var j = 0;
        var item;
        var tmpLetter;
        var tmpCity;
        while (item = data[i++]) {
            // 判断是否是城市
            if (item[2] === "0") {
                readyForSort.push(
                    {
                        // 城市名
                        name: item[1],
                        // 城市简拼
                        alphabet: item[4],
                        // 城市或县级市或大景区 id
                        id: item[5].split("#")[0],
                        // type
                        type: item[2]
                    }
                );
            }
        }
        // 下标复位
        i = 0;
        // 注意 item 外循环 和 内循环 不一样的指代
        while (item = alphabet[i++]) {
            tmpLetter = item;
            tmpCity = [];
            while (item = readyForSort[j++]) {
                if (item.alphabet.indexOf(tmpLetter.toLowerCase()) === 0) {
                    tmpCity.push(
                        {
                            name: item.name,
                            type: item.type,
                            id: item.id
                        }

                    );
                }
            }
            ordered.push(
                {
                    letter: tmpLetter,
                    cities: tmpCity
                }
            );
            j = 0;
        }
        // console.log(readyForSort);
        return ordered;
    }
    // 热门城市传 id, type, name
    function setupHotCities(hotCities, data) {
        var setuped = [];
        var i = 0;
        var j = 0;
        var itemHotCities;
        var itemData;
        while (itemHotCities = hotCities[i++]) {
            while (itemData = data[j++]) {
                if (itemHotCities === itemData[1]) {
                    setuped.push(
                        {
                            name: itemHotCities,
                            id: itemData[5].split("#")[0],
                            type: itemData[2]
                        }
                    );
                }
            }
            j = 0;
        }
        return setuped;
    }
}
