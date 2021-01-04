$(document).ready(function () {
    Load();
    //  CalTimeRDT();
    //   diff(CalTimeRDT(), CalTimeRDT());
    //  $(".C").click(function () {
    //      alert(this.title);
    //  alert($("#Machine_0").html());
    // });
    setTimeout(
        function () {
            //location.reload();
            window.location = baseUrl + "Monitor/MonitorTV4";
        }, 15000);                                                 // 1 นาที Refesh หน้อจอ
    setInterval(Load, 5000);                                        // 5 วินาที ดึงข้อมูล
    //Load();
});
function Load() {
    $.post(baseUrl + "Monitor/Machine", {
        START: 18,
        END: 9
    }).done(function (dataM) {
        // var prM = $.parseJSON(dataM); 

        $.each(JSON.parse(dataM), function (i, obj) {
            $("#TimeM_" + i).html(Time());
            //  Time();
            $.post(baseUrl + "Monitor/CheckMach", {
                MACH: $("#Machine_" + i).html()
            }).done(function (data) {
                if (data == "[]") {
                }
                else {
                    var pr = $.parseJSON(data);
                    var Count = pr.length;
                    var Total_F = 0;
                    var Total_P = 0;
                    $.each(JSON.parse(data), function (ii, obj) {
                        Total_F += pr[ii]["RT_QTY"]
                    });
                    //  alert(CalTimeRDT() -CalTimeRDT(pr[0]["TR_DateTime"]));Time_Actual
                    // alert(CalTimeRDT(pr[0]["Time_Actual"]));
                    $("#Sum_TimeD_" + i).html(diff(CalTimeRDT(pr[0]["TR_DateTime"]), CalTimeRDT()));
                    $('#P_Order_M_' + i).html(pr[Count - 1]["B_Order_QTY"]);
                    $('#P_Unit_M_' + i).html(pr[Count - 1]["B_Order_Unit"]);
                    $('#Finish_Order_M_' + i).html(pr[Count - 1]["CalTotalQTYs"]);
                    $('#Balance_Order_M_' + i).html(pr[Count - 1]["B_Order_Unit"] == 'KM'
                        && Metric_Convers(pr[Count - 1]["B_Order_QTY"], pr[Count - 1]["CalTotalQTYs"], pr[Count - 1]["B_Order_Unit"]) > 20000 ?
                        Metric_ConversM(Metric_Convers(pr[Count - 1]["B_Order_QTY"], pr[Count - 1]["CalTotalQTYs"], pr[Count - 1]["B_Order_Unit"]))
                        : Metric_Convers(pr[Count - 1]["B_Order_QTY"], pr[Count - 1]["CalTotalQTYs"], pr[Count - 1]["B_Order_Unit"]));
                    $('#Convers_Unit_M_' + i).html(pr[Count - 1]["B_Order_Unit"] == 'KM' && Metric_Convers(pr[Count - 1]["B_Order_QTY"],
                        pr[Count - 1]["CalTotalQTYs"], pr[Count - 1]["B_Order_Unit"]) <= 20000 ? 'M' : pr[Count - 1]["B_Order_Unit"]);
                    Total_P = diffMIN(CalTimeRDT(pr[Count - 1]["TR_DateTime"]), CalTimeRDT()) * (pr[Count - 1]["Process_Speed"] / 60) < pr[Count - 1]["QTY"] ?
                        diffMIN(CalTimeRDT(pr[Count - 1]["TR_DateTime"]), CalTimeRDT()) * ((pr[Count - 1]["Process_Speed"] / 60)) : pr[Count - 1]["QTY"];
                    $('#Target_QTY_Plan_By_Mach_' + i).html(Total_P.toFixed(1));

                    $('#User_' + i).html(pr[Count - 1]["User"]);
                    $('#ItemM_' + i).html(pr[Count - 1]["PL_Type"].substring(33, 0));
                    $('#SOM_' + i).html(pr[Count - 1]["Pro_SO"]);
                    $('#TagM_' + i).html(pr[Count - 1]["Bar_Kan"]);
                    $('#PlanM_' + i).html(pr[Count - 1]["QTY"]);
                    $('#ActualM_' + i).html(pr[Count - 1]["RT_QTY"]);
                    $('#CusM_' + i).html(pr[Count - 1]["B_Customer"].substring(33, 0));
                    $('#DiffM_' + i).html(pr[Count - 1]["QTY"] - pr[Count - 1]["RT_QTY"]);
                    $('#Total_FinsM_' + i).html(Total_F);
                    if (pr[Count - 1]["QTY"] > pr[Count - 1]["RT_QTY"]) {
                        // document.getElementById("MST_" + i).style.backgroundColor = "lime";
                        // document.getElementById("StartM_" + i).checked = true;
                        //document.getElementById("StopM_" + i).checked = false; 
                        $('#Mach_Status_' + i).html("เครื่องเดินปกติ").css("color", "lime");
                        document.getElementById("BG_" + i).style.backgroundColor = "black";
                    }
                    if (pr[Count - 1]["QTY"] <= pr[Count - 1]["RT_QTY"]) {
                        // document.getElementById("StartM_" + i).checked = false;
                        // document.getElementById("StopM_" + i).checked = true;
                        // document.getElementById("PBM_" + i).checked = false;
                        $('#Mach_Status_' + i).html("เครื่องจอด").css("color", "yellow");
                        document.getElementById("BG_" + i).style.backgroundColor = "grey";
                    }
                    if (pr[Count - 1]["Stop_Process"] == "T") {
                        // document.getElementById("StartM_" + i).checked = false;
                        // document.getElementById("StopM_" + i).checked = false;
                        // document.getElementById("PBM_" + i).checked = true;
                        $('#Mach_Status_' + i).html("เครื่องมีปัญหา").css("color", "yellow");
                        document.getElementById("BG_" + i).style.backgroundColor = "maroon";
                    }
                }
            });
        });
    });
}

function Metric_Convers(Val1, Val2, Unit) {
    var KM, M;
    if (Unit == 'KM') {
        M = (Val1 * 1000) - Val2;
    }
    else {
        M = Val1 - Val2;
    }
    return M;
}
function Metric_ConversM(Val1) {

    var M = (Val1 / 1000);
    return M;
}
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
    var times = Hou + ":" + Min + " น.";
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
function CalTimeRDT(A) {
    var m = moment(A);
    // alert(m);
    //check parsed is correct
    if (m.isValid) {
        m.add({ Hours: 0, Minutes: A });
    }
    //  alert(m);
    return m;
}
function diff(start, end) {
    var diff = end - start; //
    //  alert(start);
    //  alert(end);
    //  alert(diff);
    var days = Math.floor(diff / (1000 * 60 * 60 * 24));
    var hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutesAll = Math.floor((diff / (1000 * 60))); //80
    // alert(diff + "===" + end + "-" + start + "Minaa" + minutesAll)

    diff -= hours * 1000 * 60 * 60;
    var minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60)); //1.20 ชม
    //alert(diff + "===" + end + "-" + start + "Min" + minutes)
    return (days > 0 ? days + " วัน " : "") + (hours <= 9 && hours >= 0 ? "0" : "") + (hours >= 0 ? hours + ":" : "00:") + (minutes <= 9 && hours >= 0 ? "0" : "") + (hours >= 0 ? minutes : "00");// + (hours == 0 ? " นาที" : " ชม.");
}


function diffMIN(start, end) {
    var diff = end - start;
    var minutesAll = Math.floor((diff / (1000 * 60)) * 60);
    return minutesAll;
}
