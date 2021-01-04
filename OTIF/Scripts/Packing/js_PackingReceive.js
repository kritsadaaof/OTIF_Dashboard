﻿ 
$(document).ready(function () {
    $("#Time_Actual").val(Time());//Hidden
    $("#Bar_Kam").focus();
    $("#Save").click(function () { 
 
        if ($("#Bar_Kam").val() != "" && $("#ReelNo").val() != "" && $("#User").val() != "" && $("#Gross").val() != "") {
            $.post(baseUrl + "Packing/SaveDataByPacking", { 
                BAR_KAN: $("#Bar_Kam").val(), 
                USER: $("#User").val(),
                REELNO: $("#ReelNo").val(),
                GROSS: $("#Gross").val(),
                TIME: $("#Time_Actual").val()
            }).done(function (data) {
                if (data == "S") {
                    var nFrom = "bottom";
                    var nAlign = "center";
                    var nIcons = $(this).attr('data-icon');
                    var nType = "success";
                    var nAnimIn = $(this).attr('data-animation-in');
                    var nAnimOut = $(this).attr('data-animation-out');
                    var mEss = "บันทึกข้อมูลสำเร็จ";
                    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                    setTimeout(
                        function () {
                            window.location = baseUrl + "Packing/PackingReceive";
                        }, 2000);
                } 
            });
        }
        else {
            var nFrom = "bottom";
            var nAlign = "center";
            var nIcons = $(this).attr('data-icon');
            var nType = "danger";
            var nAnimIn = $(this).attr('data-animation-in');
            var nAnimOut = $(this).attr('data-animation-out');
            var mEss = "กรุณากรอกข้อมูลให้ถูกต้อง";
            notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
        } 
    });     

    $("#Bar_Kam").change(function (e) {
        $.post(baseUrl + "Packing/CheckBar", {
            BAR: $("#Bar_Kam").val()
        }).done(function (data) {
            //alert(data);
            var pr = $.parseJSON(data);
            if (data == "null") {
               var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "Barcode นี้ Panner ยังไม่โยนเข้าระบบ";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Bar_Kam").val("").focus(); 
            }
            else {
                $("#User").focus();
               // alert(pr["PL_SO"]);
               //$("#User").val(pr["PL_SO"])
            }
        });
    });
    $("#Location").change(function (e) {
        $.post(baseUrl + "Class/CheckLoc", {
            LOC: $("#Location").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "ไม่มีข้อมูล Location นี้";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Location").val("").focus();
                $("#LocationDes").html(""); 
            }
            else {  
                $("#User").focus();
                $("#Location").val(pr[0]["Lo_Name"]);
                $("#LocationDes").html(pr[0]["Lo_Des"]); 

            }
        });
    });  
     
    $("#User").change(function (e) { 
        $.post(baseUrl + "Class/CheckUser", {
            USER: $("#User").val() 
        }).done(function (data) {
            var pr = $.parseJSON(data); 
            if (data == "[]") {
                var nFrom = "bottom";
                var nAlign = "center"; 
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "ไม่มีUserนี้ / Userไม่มีสิทธิ์";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#User").val("").focus();
            }
            else {
                $("#User").val(pr[0]["Mem_Name"]);
                $("#ReelNo").focus()
                // $("#Use").val(pr[0]["Use"])
            }
        });
    }); 
    $("#ReelNo").change(function (e) {

        $("#Gross").focus()
    }); 
});
function dateFormat() {
    var d = new Date();
    month = '' + (d.getMonth() + 1), day = '' + d.getDate(), year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var val = day + "/" + month + "/" + year;
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
function Clear() {

    $("#Bar_Kam").val("");
    $("#Location").val("");
    $("#User").val("");
}
function alertMess(mEss) {
    var nFrom = "bottom";
    var nAlign = "center";
    var nIcons = $(this).attr('data-icon');
    var nType = "danger";
    var nAnimIn = $(this).attr('data-animation-in');
    var nAnimOut = $(this).attr('data-animation-out');
    var mEss = mEss;
    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
}