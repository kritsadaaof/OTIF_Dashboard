﻿$(document).ready(function () {
    $("#StartDate").focus();
    $.post(baseUrl + "Home/SelectMachines", {
        MAC: $("#SelectMac").val(),
        STARTDATE: $("#StartDate").val(),
        ENDDATE: $("#EndDate").val()
    }).done(function (data) {
        var pr = $.parseJSON(data);
        $.each(JSON.parse(data), function (i, obj) {

            $('#data-table-basic').dataTable().fnAddData([
                (i + 1),
                pr[i]["Date_Actual"],
                pr[i]["Pro_SO"],
                pr[i]["PL_Type"],
                pr[i]["Bar_Kan"] == "-" ? "-" : pr[i]["SF_Remain_Qty"] == null ? "ยังไม่รายงานล้อรับ" : pr[i]["Bar_Kan"],
                pr[i]["SF_Remain_Qty"] == null ? "-" : pr[i]["QTY"],
                pr[i]["SF_Remain_Qty"] == null ? "-" : pr[i]["RT_QTY"],
                pr[i]["User"],
                pr[i]["Stop_Process"]
            ]);
        });

    });
});

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
