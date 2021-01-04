
$(document).ready(function () {  
    $("#Time_Actual").val(Time());
    $("#Bar_Kam").focus(); 
    $('#date_Plan .input-group.date').datepicker({
        format: 'dd/mm/yyyy',
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true
    });
    $("#Save").click(function () { //Process_Speed
        if ($("#User").val() != "") {

          //  var selectobject = document.getElementById("TagLis");
           // for (var i = 0; i < selectobject.length; i++) {

                $.post(baseUrl + "Planner/SaveChangePlan", {
                    TAG: $("#Bar_Kam").val(),
                    MACHIN_O: $("#Mach_Old").val(),
                    MACHIN_N: $("#Mach_New").val(),
                    USER: $("#User").val() 
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
                    else {
                        var nFrom = "bottom";
                        var nAlign = "center";
                        var nIcons = $(this).attr('data-icon');
                        var nType = "success";
                        var nAnimIn = $(this).attr('data-animation-in');
                        var nAnimOut = $(this).attr('data-animation-out');
                        var mEss = "ไม่สามารถบันทึกข้อมูลได้";
                        notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                        setTimeout(
                            function () {
                                window.location = baseUrl + "Home/Index";
                            }, 2000); 
                    }
                });
          //  }

           
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
       // document.getElementById("Mach_Old").val();
       // $("#Mach_Old").val([]);
        $('#Mach_Old').empty();
        $.post(baseUrl + "Planner/CheckMachine", {
            BAR_KAN: $("#Bar_Kam").val() 
        }).done(function (data) {
            
            var pr = $.parseJSON(data);
          //  alert(pr[0]["PL_Machine"]);
            if (data == "[]") {
                var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "ข้อมูล TAG ไม่ถูกต้อง";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#Bar_Kam").val("").focus();
                //$("#Machin").val("")
            }
            else { 
              //  $("#Bar_Kam").val("").focus();
                $.each(JSON.parse(data), function (i, obj) {
                    if (pr[i]["PL_Machine"] != null) {
                        $('#Mach_Old')
                            .append($("<option></option>")
                                .attr("value", pr[i]["PL_Machine"])
                                .text(pr[i]["PL_Machine"]));
                    }
                });
             /*  $('#TagLis').val($("#Bar_Kam").val());
                $("#Qty").val(pr[0]["PL_Qty"]);
                $("#Qty_T").val(pr[0]["PL_Qty_T"]);
                $("#Bar_Kam").val("").focus();
                var sel = document.getElementById('TagLis');
                var strUser = sel.options[sel.selectedIndex].value;             
                */
               // for (var i = 1, len = sel.options.length; i <= len; i++) {
                //    countries.push(sel.options[i.value]);
               // }
               // alert(countries)
                // $("#Use").val(pr[0]["Use"])

                $("#User").focus();
            }
        });
    });
    $("#Qty").change(function (e) {
        $("#Process_Speed").focus();
    });
    $("#Process_Speed").change(function (e) {
        $("#User").focus();
    });

    $("#Date_Plan").change(function (e) {
        $("#Time_Plan").focus();
    });
    $("#Time_Plan").change(function (e) {
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
