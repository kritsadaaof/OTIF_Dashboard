 
$(document).ready(function () {
    var Stop_Process = "F", Stop_Code = null;
    $("#Time_Actual").val(Time());//Hidden
    $("#Time_SP_Fst").val(Time());//Hidden
    $("#Save").click(function () {
        if ($("#Bar_Kam").val() != "" && $("#User").val() != "" && $("#Actual_Speed").val() != "") {
            $.post(baseUrl + "ShopFloor/SaveSHFFst", {
                BAR_KAN: $("#Bar_Kam").val(),
                MACHINE: $("#Machin").val(),
                QTY: 0,
                TRAGET: $("#Pro_Traget").html(),
                USER: $("#User").val(),
                TIME_ACTUAL: $("#Time_Actual").val(),
                REMAIN: $("#Pro_Remain").html(),
                ACTUAL_SPEED: $("#Actual_Speed").val(),
                STP: Stop_Process,
                STC: $("#Reason_Code").val(),
                TIME_SF_FST: $("#Time_SP_Fst").val()
               // STC: Stop_Code
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
                            window.location = baseUrl + "ShopFloor/ShopFloor";
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
    var Pro_Remain;

    $("#Machin").change(function (e) {

        $.post(baseUrl + "Class/CheckMACH", {
            MACH: $("#Machin").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "เครื่องจักรไม่ถูกต้อง";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Machin").val("").focus();
            }
            else {
                $("#Machin").val(pr[0]["MC_Machine"])
                // $("#Use").val(pr[0]["Use"])

                $("#Bar_Kam").focus();
            }
        });
    });

    $("#Bar_Kam").change(function (e) {
        $.post(baseUrl + "ShopFloor/CheckBarSF", {
            MACHINE: $("#Machin").val(),
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
                var mEss = "ข้อมูลเครื่อง / ข้อมูล TAG ไม่ถูกต้อง";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Bar_Kam").val("");
                $("#Machin").val("").focus();
            }
            else {
                $("#Actual_Speed").val('').focus();
              //  $("#Actual_Speed").val(0)
                $("#Pro_Traget").html(pr[0]["QTY"]) 
                if (pr[0]["SF_Remain_Qty"] == null) {
                    $("#Pro_Remain").html(pr[0]["QTY"]);
                    Pro_Remain = pr[0]["QTY"];
                }
                else {
                    $("#Pro_Remain").html(pr[0]["SF_Remain_Qty"])
                    Pro_Remain = pr[0]["SF_Remain_Qty"];
                }
                document.getElementById('Show_QTY').style.display = '';

            }
        });
    });

     
    $("#Actual_Speed").change(function (e) {
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
   // $('#Reason_Code').change(function () {
    //    Stop_Code =$(this).val(); 
    //}); 

    document.getElementById('Check_Reason').onclick = function (e) {
        if (this.checked) {
            document.getElementById('Reason').style.display = '';
            Stop_Process = "T"; 
        }
        else {
            document.getElementById('Reason').style.display = 'none'; 
            Stop_Process = "F";
            Stop_Code = null; 
        }
    };
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
