 
$(document).ready(function () {
    $("#Time_Actual").val(Time());//Hidden
    $("#Save").click(function () { 
        if ($("#Bar_Kam").val() != "" && $("#Qty").val() != "" && $("#User").val() != "") {
            $.post(baseUrl + "PDReturn/SaveRT", { 
                BAR_KAN: $("#Bar_Kam").val(),
                RT_QTY: $("#Qty").val(),
                USER: $("#User").val(),
                TIME_ACTUAL: $("#Time_Actual").val()
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
                            window.location = baseUrl + "Home/Index";
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
        $.post(baseUrl + "Class/CheckBar", {
            BAR: $("#Bar_Kam").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "ไม่มีข้อมูล Barcode นี้";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Bar_Kam").val("").focus();
            }
            else { 
                $("#Qty").focus();
                $("#QTY_DE").html("("+pr[0]["QTY"]+")");
            }
        });
    }); 
    $("#Qty").change(function (e) {
        $("#User").focus();
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
                $("#User").val(pr[0]["Mem_Name"])
                // $("#Use").val(pr[0]["Use"])
            }
        });
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
