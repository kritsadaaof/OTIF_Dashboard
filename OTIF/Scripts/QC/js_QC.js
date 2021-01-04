
$(document).ready(function () { 
    $("#Bar_Kam").val("").focus();
    $("#Status").html("ดึง");

    
    
    $("#Time_Actual").val(Time());//Hidden
    $("#Save").click(function () {
        var Status;
        if (document.getElementById("CheckboxP1").checked == true) {
            Status = "T";
        }
        else {
            Status = "R";
        }
        if ($("#Bar_Kam").val() != ""&& $("#User").val() != "") {
            $.post(baseUrl + "QCTester/SaveQC", {
                TAG: $("#Bar_Kam").val(), 
                USER: $("#User").val(),
                TIME: $("#Time_Actual").val(),
                QC_Status: Status
            }).done(function (data) {
                if (data == "S") {
                    var nFrom = "bottom";
                    var nAlign = "center";
                    var nIcons = $(this).attr('data-icon');
                    var nType = "success";
                    var nAnimIn = $(this).attr('data-animation-in');
                    var nAnimOut = $(this).attr('data-animation-out');
                    var mEss = $("#Status").html() + "Tagล้อสำเร็จ";
                    notify(nFrom, nAlign, nIcons, nType, nAnimIn, nAnimOut, mEss);
                    setTimeout(
                        function () {
                            window.location = baseUrl + "QCTester/QC";
                        }, 2000);
                }
                else if (data == "F") {
                    alert("ไม่มีข้อมูลไหลเข้ามา");
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
        document.getElementById("CheckboxP1").checked = false;
        document.getElementById("QCCheck").style.display = 'none';
       // document.getElementById('QCCheck').style.display = 'none';
        $("#Bar_Kam").val("").focus();
        $("#Status").html("ดึง");
        $("form").submit(function () { return false; });//ไม่ให้FromLoadหลังกด Enter Input
    });
    $("#End").click(function () {
        Clear(); 
        document.getElementById('Show_Move').style.display = '';
        document.getElementById("QCCheck").style.display = '';
        $("#Bar_Kam").val("").focus();
        $("#Status").html("คืน");
        $("form").submit(function () { return false; });//ไม่ให้FromLoadหลังกด Enter Input
    });

    $("#Bar_Kam").change(function (e) {
        $.post(baseUrl + "QCTester/CheckBarQC", {
            TAG: $("#Bar_Kam").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") { 

                if ($("#Status").html() == "ดึง") {
                    $("#User").focus();
                }
                else {
                    alertMess("ไม่สามารถเลืกรายการนี้ได้")
                    $("#Bar_Kam").val("").focus();
                }

            }
            else { 
                $("#User").focus();
                if ($("#Status").html() == "ดึง" && (pr[0]["Pack_Status_QC"] == null || pr[0]["Pack_Status_QC"] == "R")) {
                     // alert("สามารถดึงได้");
                    //  $("#Status").html("ข้อมูลล้อ (รับ)");
                  //  $("#User").focus();
                }
                else if ($("#Status").html() == "ดึง" && (pr[0]["Pack_Status_QC"] == "T" || pr[0]["Pack_Status_QC"] == "F")) {
                    alertMess("ไม่สามารถดึงได้")
                    $("#Bar_Kam").val("").focus();
                }

                if ($("#Status").html() == "คืน" && (pr[0]["Pack_Status_QC"] == "T" || pr[0]["Pack_Status_QC"] == "F")) {
                    //  $("#Status").html("ข้อมูลล้อ (รับ)");
                }

                else if ($("#Status").html() == "คืน" && (pr[0]["Pack_Status_QC"] == null || pr[0]["Pack_Status_QC"] == "R")) {
                    alertMess("ไม่สามารถคืนได้")
                    $("#Bar_Kam").val("").focus();
                }
                //  $("#Bar_Kam_De").html(pr[0]["Bar_Kan"]);
                //  $("#QTY_De").html(pr[0]["QTY"]);
                //  document.getElementById('Show_Detail').style.display = '';
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
    /*
    $("#DataPacking").DataTable({
        "oLanguage": {
            "sLengthMenu": "แสดง _MENU_ เร็คคอร์ด ต่อหน้า",
            "sZeroRecords": "ไม่เจอข้อมูลที่ค้นหา",
            "sInfo": "แสดง _START_ ถึง _END_ ของ _TOTAL_ เร็คคอร์ด",
            "sInfoEmpty": "แสดง 0 ถึง 0 ของ 0 เร็คคอร์ด",
            "sInfoFiltered": "(จากเร็คคอร์ดทั้งหมด _MAX_ เร็คคอร์ด)",
            "sSearch": "ค้นหา :"
        },
        //  "order": [[0, "des"]],
        // "order": [[0, 'desc'], [5, 'desc']],
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "destroy": true,
        "ordering": true,
        "ajax": {
            "url": "../QCTester/LoadData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "Pack_Status" 
            },
            { "data": "Bar_Kan" },
            { "data": "SO" },
            { "data": "QTY", "name": "QTY", "autoWidth": true },
            { "data": "Machine", "name": "Machine", "autoWidth": true },

            //{ "data": "Pack_DateTime", "name": "Pack_DateTime", "autoWidth": true }
            {
                "data": "Pack_DateTime",
                render: function (file_id) {
                    return file_id ? CalTime(file_id) : null;
                }
            }
        ]
    }); */
    $.post(baseUrl + "QCTester/GetDATA", { 
      }).done(function (data) {
          var pr = $.parseJSON(data);
          $.each(JSON.parse(data), function (i, obj) {
              $('#data-table-basic').dataTable().fnAddData([
                  
               //   diff(CalTimeRDT(), CalMinRDT(pr[i]["Pack_QTY"], pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])),
                  diff(CalTimeRDT(), CalMinRDT((pr[i]["Pack_QTY"] - pr[i]["Pack_QTY_Balance"]), pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) <= "00:05" ? '<P style="color:red">' + diff(CalTimeRDT(), CalMinRDT((pr[i]["Pack_QTY"] - pr[i]["Pack_QTY_Balance"]), pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) + '</P>' : '<P style="color:black">' + diff(CalTimeRDT(), CalMinRDT((pr[i]["Pack_QTY"] - pr[i]["Pack_QTY_Balance"]), pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) + '</P>',

                  // pr[i]["ID"],
                  pr[i]["Pack_Machine"],
                  pr[i]["Pack_Tag"],
                  pr[i]["Pack_SO"],
                  pr[i]["Pack_QTY"],
                  pr[i]["Pack_QTY_Balance"],
                  pr[i]["Pack_Weight"] == null || pr[i]["Pack_Weight"] ==0 ? '<P">-</P>' : pr[i]["Pack_Weight"] > 3000 ? '<P style="color:red">'+ pr[i]["Pack_Weight"].toFixed(0) + "Kg"+'</P>' : pr[i]["Pack_Weight"].toFixed(0) + "Kg",
                //  pr[i]["Pack_Speed"],
               //   pr[i]["Pack_QTY"] / pr[i]["Pack_Speed"], 
                 // CalTime(pr[i]["Pack_DateTime"]),
                 // CalMin(pr[i]["Pack_QTY"], pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"]),
                  pr[i]["Pack_Base"],
                  (pr[i]["Pack_Status_QC"] == "T" ? '<P style="color:#E19219">ตรวจสอบ</P>' : pr[i]["Pack_Status_QC"] == "R" ? '<P style="color:#1983E1">ผ่าน</P>' : pr[i]["Pack_Status_QC"] == "F" ? '<P style="color:Red">ไม่ผ่าน</P>' : "-")
              ]);
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
function CalTime(A) { 
    var m = moment(A);
    //check parsed is correct
    if (m.isValid) {
        m.add({ Hours: 0, Minutes: A });
    }
    return m.format("DD/MM/YYYY HH:mm ");
    
}

function CalTimeRDT(A) {
    var m = moment(A);
    //check parsed is correct
    if (m.isValid) {
        m.add({ Hours: 0, Minutes: A });
    }
    return m;

}

function CalMin(A,B,C) {
  
        var CalTime = A / B 
        var m = moment(C);
        //check parsed is correct
        if (m.isValid) {
            m.add({ Hours: 0, Minutes: CalTime });
        }
        return m.format("DD/MM/YYYY HH:mm ");  
}
function CalMinRDT(A, B, C) {

    var CalTime = A / B
    var m = moment(C);
    //check parsed is correct
    if (m.isValid) {
        m.add({ Hours: 0, Minutes: CalTime });
    }
    return m;
}
function diff(start, end) {   
    var diff = end - start; 
    var days = Math.floor(diff / (1000 * 60 * 60 * 24)); 
    var hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)); 
    diff -= hours * 1000 * 60 * 60; 
    var minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    return (days > 0 ? days + " วัน " : "") + (hours <= 9 && hours >= 0 ? "0" : "") + (hours >= 0 ? hours + ":" : "00:") + (minutes <= 9 && hours >= 0 ? "0" : "") + (hours >= 0 ? minutes : "00") + (hours == 0 ? " นาที" : " ชม.");
}

/*
function diff(start, end) {
    // alert(start + end); 
    var difference = 93234543;
    start = start.split(":");
    end = end.split(":");
    var startDate = new Date(0, 0, 0, start[0], start[1], 0);
    var endDate = new Date(0, 0, 0, end[0], end[1], 0);
    // alert(startDate + "<>" + endDate);
    var diff = endDate.getTime() - startDate.getTime();
    alert(diff);
    var days = Math.floor(diff / (1000 * 60 * 60 * 24));
    alert(days);
    // var hours = Math.floor(diff / 1000 / 60 / 60);
    var hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    alert(hours);
    diff -= hours * 1000 * 60 * 60;
    //  alert(diff);
    // var minutes = Math.floor(diff / 1000 / 60);
    var minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    // var seconds = Math.floor((diff % (1000 * 60)) / 1000);
    //  alert(minutes);
    // return (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;

    return days + "D " + (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;
}


function diff(start, end) {
   alert(start +"AA"+ end);  // 01:42 02:00 // 17/07/2020 01:45 18/07/2020 02:00
    var difference = 93234543;
    start = start.split(":");
    end = end.split(":");
    alert(start + end); //01,42 02,00 // 17/07/2020 01,45 18/07/2020 02,00
    var startDate = new Date(0, 0, 0, start[0], start[1], 0);
    var endDate = new Date(0, 0, 0, end[0], end[1], 0);
    alert(startDate + endDate); //Sun Dec 31 1899 01:42:00 GMT+0642 (เวลาอินโดจีน)Sun Dec 31 1899 02:00:00 GMT+0642 (เวลาอินโดจีน)
    alert(startDate.getTime() + "<>" + endDate.getTime()); 
var diff = end - start;
var difference = diff;
// alert(diff);
var days = Math.floor(diff / (1000 * 60 * 60 * 24));
// alert(days);
// var hours = Math.floor(diff / 1000 / 60 / 60);
var hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
// alert(hours);
//alert(diff +".."+ hours);
diff -= hours * 1000 * 60 * 60;
//alert(diff);
// var minutes = Math.floor(diff / 1000 / 60);
var minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
// var seconds = Math.floor((diff % (1000 * 60)) / 1000);
// alert(minutes);
// return (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;

return days + "D " + (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;
}

*/
 
 