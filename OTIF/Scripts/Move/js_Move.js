 
$(document).ready(function () {
    $("#Time_Actual").val(Time());//Hidden
    $("#Save").click(function () { 

        var base;
        if (document.getElementById("Packing1").checked == true) {
            base = "P1";
        }
        if (document.getElementById("Packing2").checked == true) {
            base = "P2";
        }
        if ($("#Bar_Kam").val() != "" && $("#Location").val() != "" && $("#User").val() != "") {
            $.post(baseUrl + "Move/SaveMove", { 
                BAR_KAN: $("#Bar_Kam").val(),
                LOCATION: $("#Location").val(),
                USER: $("#User").val(),
                TIME_ACTUAL: $("#Time_Actual").val(),
                OPTION: $("#Status").html(),
                BASE: base
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
                            window.location = baseUrl + "Move/Move";
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
    $("#Start").click(function () {
        //window.location = baseUrl + "Move/Move";
        Clear(); 
        document.getElementById('Show_Move').style.display = '';
        $("#Bar_Kam").val("").focus();
        $("#Status").html("รับ");
        document.getElementById('CheckBox').style.display = 'none';
        document.getElementById("Packing1").checked = false;
        document.getElementById("Packing2").checked = false;
        $("form").submit(function () { return false; });//ไม่ให้FromLoadหลังกด Enter Input
    });
    $("#End").click(function () {
        Clear();
        document.getElementById('Show_Move').style.display = '';
        $("#Bar_Kam").val("").focus();
        $("#Status").html("ส่ง");
        
        $("form").submit(function () { return false; });//ไม่ให้FromLoadหลังกด Enter Input
    });
    document.getElementById('Packing1').onclick = function (e) {
        if (this.checked) {
            document.getElementById("Packing2").checked = false;
            // document.getElementById('Fail_F').style.display = 'none';
        }

    };
    document.getElementById('Packing2').onclick = function (e) {
        if (this.checked) {
            document.getElementById("Packing1").checked = false;
            //document.getElementById('Fail_F').style.display = '';
        }

    };

    $("#Bar_Kam").change(function (e) {
        $.post(baseUrl + "Move/CheckBar", {
            BAR: $("#Bar_Kam").val()
        }).done(function (data) {
          //  alert(data);
            var pr = $.parseJSON(data);
            if (data == "[]") {
          /*      var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "ไม่มีข้อมูล Barcode นี้";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Bar_Kam").val("").focus();
               

                if ($("#Status").html() == "รับ") {
                    $("#Location").focus();
                }
                else {
                    alertMess("ไม่สามารถส่งได้")
                    $("#Bar_Kam").val("").focus();
                }
                 //*/
             //   $("#Location").focus();
              //  document.getElementById('CheckBox').style.display = '';

            }
            else {

                $("#Location").focus();
                if ($("#Status").html() == "รับ" && (pr[0]["Move_Status"] == null || pr[0]["Move_Status"] == "F")) {
                   // alert("สามารถรับได้");
                     // $("#Status").html(pr[0]["ID"]);
                }
                else if ($("#Status").html() == "รับ" && pr[0]["Move_Status"] == "T") {  
                    alertMess("ไม่สามารถรับได้")
                    $("#Bar_Kam").val("").focus();
                }

                if ($("#Status").html() == "ส่ง" && pr[0]["Move_Status"] == "T") {  
                    //  $("#Status").html("ข้อมูลล้อ (รับ)");
                }
                if ($("#Status").html() == "ส่ง" && pr[0]["PL_Process_Status"] == "Yes") {
                    document.getElementById('CheckBox').style.display = '';
                }

            //    else if ($("#Status").html() == "ส่ง" && pr[0]["Move_Status"] != "T") { 
             //       alertMess("ไม่สามารถส่งได้") 
             //       $("#Bar_Kam").val("").focus();
             //   }
                //  $("#Bar_Kam_De").html(pr[0]["Bar_Kan"]);
                //  $("#QTY_De").html(pr[0]["QTY"]);
                //  document.getElementById('Show_Detail').style.display = '';
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