 
$(document).ready(function () {
    $("#Machin").focus(); 
    //$("#Date_Plan").val(dateFormat(new Date()));
   // $("#Time_Plan").val(Time());
    $("#Time_Actual").val(Time());
    $('#date_Plan .input-group.date').datepicker({
        format: 'dd/mm/yyyy',
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true
    }); 
    $('#Delivery_Date .input-group.date').datepicker({
        format: 'dd/mm/yyyy',
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true
    }); 
    $("#Save").click(function () { 
        if ($("#SO").val() != "" && $("#Qty").val() != "" && $("#User").val() != "" && $("#Delivery_DateT").val() != "") {
            $.post(baseUrl + "Planner/SaveData", {
                SO: $("#SO").val(), 
                QTY: $("#Qty").val(),
                USER: $("#User").val(),
                Date_Metal: $("#Date_Metal").val(),
                Time_Metal: $("#Time_Metal").val(),
                Date_Compound: $("#Date_Compound").val(),
                Time_Compound: $("#Time_Compound").val(),
                Date_Oh_RM: $("#Date_Oh_RM").val(),
                Time_Oh_RM: $("#Time_Oh_RM").val(),
                Date_CutL: $("#Date_CutL").val(),
                Time_CutL: $("#Time_CutL").val(),
                CUTL: $("#CutL").val(),
                Date_Mg: $("#Date_Mg").val(),
                Time_Mg: $("#Time_Mg").val(),
                DATE_PLAN: $("#Date_PlanT").val(),
                TIME_PLAN: $("#Time_Plan").val(),
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
    $("#SO").change(function (e) { 
        $.post(baseUrl + "Class/CheckSO", {
            SO: $("#SO").val()
        }).done(function (data) {
          //  alert(data);
            var pr = $.parseJSON(data);
            if (data == "[]") {
                var nFrom = "bottom";
                var nAlign = "center";
                var nIcons = $(this).attr('data-icon');
                var nType = "danger";
                var nAnimIn = $(this).attr('data-animation-in');
                var nAnimOut = $(this).attr('data-animation-out');
                var mEss = "ไม่มีข้อมูล SO นี้";
                notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                $("#SO").val("").focus();
            }
            else {

                $("#Cus").val(pr[0]["B_Customer"]);// 
                $("#Create_Date").val(pr[0]["Date_Create"]);// Create_Date
                $("#PFD").val(pr[0]["Date_PFD"]);//  
                $("#Qty").val(pr[0]["B_Order_QTY"]);//     + ' ' + pr[0]["B_Order_Unit"]
                $("#QTY_Unit").html("  หน่วย : " + pr[0]["B_Order_Unit"]);// 
                $("#Mat").val(pr[0]["B_Material"]);//  
                $("#Delivery_DateT").val(pr[0]["Date_Delivery"]);//  
                $("#Mat_Des").val(pr[0]["B_Material_Des"]);//  
                if (pr[0]["B_Cutting_Length"] == "" || pr[0]["B_Cutting_Length"] == null) {
                    document.getElementById('De_CutL').style.display = '';
                    document.getElementById('Check_CutL').checked = false;
                }
                else { 
                    $("#CutL_De_Label").html(" : "+pr[0]["B_Cutting_Length"]);
                }

                $("#User").focus();
            }
        });
    });

    $("#Bar_Kam").change(function (e) {
        $("#Qty").focus();
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
     

    document.getElementById('Check_Matal').onclick = function (e) {
        if (this.checked) {

            document.getElementById('De_Metal').style.display = 'none';
            $("#Time_Metal").val("");
            $("#Date_Metal").val("");
        }
        else {   
            document.getElementById('De_Metal').style.display = ''; 
           // $("#Date_Metal").val(dateFormat(new Date()));
           // $("#Time_Metal").val(Time());
            $('#data_1 .input-group.date').datepicker({
                format: 'dd/mm/yyyy',
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true
            });  
        } 
    };
    document.getElementById('Check_Compound').onclick = function (e) {
        if (this.checked) {
            document.getElementById('De_Compound').style.display = 'none';
            $("#Time_Compound").val("");
            $("#Date_Compound").val("");
        }
        else {
            document.getElementById('De_Compound').style.display = '';
          //  $("#Date_Compound").val(dateFormat(new Date()));
          //  $("#Time_Compound").val(Time());
            $('#data_2 .input-group.date').datepicker({
                format: 'dd/mm/yyyy',
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true
            }); 
        }
    };
    document.getElementById('Check_Oh_RM').onclick = function (e) {
        if (this.checked) {
            document.getElementById('De_Oh_RM').style.display = 'none';
            $("#Time_Oh_RM").val("");
            $("#Date_Oh_RM").val("");
        }
        else {
            document.getElementById('De_Oh_RM').style.display = '';
          //  $("#Date_Oh_RM").val(dateFormat(new Date()));
          //  $("#Time_Oh_RM").val(Time());
            $('#data_3 .input-group.date').datepicker({
                format: 'dd/mm/yyyy',
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true
            }); 
        }
    }; 


    document.getElementById('Check_CutL').onclick = function (e) {
        if (this.checked) {
            document.getElementById('De_CutL').style.display = 'none';
            $("#Time_CutL").val("");
            $("#Date_CutL").val("");
        }
        else {
            document.getElementById('De_CutL').style.display = '';
         //   $("#Date_CutL").val(dateFormat(new Date()));
          //  $("#Time_CutL").val(Time());
            $('#data_4 .input-group.date').datepicker({
                format: 'dd/mm/yyyy',
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true
            }); 
        }
    };
    document.getElementById('Check_Mg').onclick = function (e) {
        if (this.checked) {
            document.getElementById('De_Mg').style.display = 'none';
            $("#Date_Mg").val("");
            $("#Time_Mg").val("");
        }
        else {
            document.getElementById('De_Mg').style.display = ''; 
          //  $("#Date_Mg").val(dateFormat(new Date()));
          //  $("#Time_Mg").val(Time());
            $('#data_5 .input-group.date').datepicker({
                format: 'dd/mm/yyyy',
                todayBtn: "linked",
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true
            }); 
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
