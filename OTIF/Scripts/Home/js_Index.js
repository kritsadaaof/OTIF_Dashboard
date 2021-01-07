$(document).ready(function () {
    // alert("Index");
    $("#Time_Actual").html("(" + dateFormat() + ")");//Hidden
    $.post(baseUrl + "Home/DataIndexMach", {
        DATE: dateFormat()
    }).done(function (data) {
        // alert(data);
        var pr = $.parseJSON(data);
        $.each(JSON.parse(data), function (i, obj) {

            $('#data-table-basic').dataTable().fnAddData([
                (i + 1),
                //       '<a class="SalesEdit" id="' + pr[i]["ID"] + '" href="#">' + pr[i]["Prefix"] + '</a>',
                pr[i]["Machine"],
                pr[i]["Pro_SO"],
                // pr[i]["Date_Actual"],

                pr[i]["PL_Type"],
                pr[i]["Bar_Kan"] == "-" ? "-" : pr[i]["SF_Remain_Qty"] == null ? "ยังไม่รายงานล้อรับ" : pr[i]["Bar_Kan"],
                pr[i]["SF_Remain_Qty"] == null ? "-" : pr[i]["QTY"],
                pr[i]["SF_Remain_Qty"] == null ? "-" : pr[i]["RT_QTY"],
                pr[i]["User"],
                pr[i]["Stop_Process"]
                //   pr[i]["Use"],
                //   '<img src="../Content/img/Item/' + pr[i]["Item_Img"] + '" title="' + pr[i]["Item_Img"] + '" border="0" alt="" class="IMGS"/>'

            ]);
        });

    });
});
function dateFormat() {
    var d = new Date();
    month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var val = year + "-" + month + "-" + day;
    return val;
}
function Time() {
    var dt = new Date();
    var Hou = '' + dt.getHours();
    var Min = '' + dt.getMinutes();
    var Sec = '' + dt.getSeconds();
    if (Hou.length < 2) Hou = '0' + Hou;
    if (Min.length < 2) Min = '0' + Min;
    if (Sec.length < 2) Sec = '0' + Sec;
    var times = Hou + ":" + Min + ":" + Sec;
    return times;
}


function notify(from, align, icon, type, animIn, animOut, mEssage) { //Notify
    $.growl({
        icon: icon,
        title: ' แจ้งเตือน ',
        message: mEssage,

        url: ''
    }, {
        element: 'body',
        type: type,
        allow_dismiss: true,
        placement: {
            from: from,
            align: align
        },
        offset: {
            x: 20,
            y: 85
        },
        spacing: 10,
        z_index: 1031,
        delay: 2500,
        timer: 2000,
        url_target: '_blank',
        mouse_over: false,
        animate: {
            enter: animIn,
            exit: animOut
        },
        icon_type: 'class',
        template: '<div data-growl="container" class="alert" role="alert">' +
            '<button type="button" class="close" data-growl="dismiss">' +
            '<span aria-hidden="true">&times;</span>' +
            '<span class="sr-only">Close</span>' +
            '</button>' +
            '<span data-growl="icon"></span>' +
            '<span data-growl="title"></span>' +
            '<span data-growl="message"></span>' +
            '<a href="#" data-growl="url"></a>' +
            '</div>'
    });
};