
$(document).ready(function () {
    setTimeout(
        function () {
            location.reload();
        }, 30000);
    document.getElementById('Menu').style.display = 'none';
    document.getElementById('Bran').style.display = 'none';
    $("#Key").html("รายการกำลังจะออกจากผลิต");
    $.post(baseUrl + "Packing/GetDATAQCPass", {
        BASE: "P2"
    }).done(function (data) {
        var pr = $.parseJSON(data);
        $.each(JSON.parse(data), function (i, obj) {
            $('#data-table-basic').dataTable().fnAddData([

                //(i + 1),
                // pr[i]["ID"],
                // (diff(CalTimeRDT(), CalMinRDT(pr[i]["Pack_QTY"], pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) <= "00:05" ? '<P style="color:red">' + diff(CalTimeRDT(), CalMinRDT(pr[i]["Pack_QTY"], pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) + '</P>' : '<P style="color:black">' + diff(CalTimeRDT(), CalMinRDT(pr[i]["Pack_QTY"], pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) + '</P>'),
                (diff(CalTimeRDT(), CalMinRDT((pr[i]["Pack_QTY"] - pr[i]["Pack_QTY_Balance"]), pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) <= "00:05" ? '<P style="color:red">' + diff(CalTimeRDT(), CalMinRDT((pr[i]["Pack_QTY"] - pr[i]["Pack_QTY_Balance"]), pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) + '</P>' : '<P style="color:black">' + diff(CalTimeRDT(), CalMinRDT((pr[i]["Pack_QTY"] - pr[i]["Pack_QTY_Balance"]), pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"])) + '</P>'),
                pr[i]["Pack_SO"],
                pr[i]["Pack_Tag"],
                //  pr[i]["Pack_SO"], 
                //  '<a style="color:black" href="#">' + pr[i]["Pack_SO"] + '</a>',
                pr[i]["Pack_QTY"],
                (pr[i]["Pack_QTY_Balance"] == 0 ? '<P style="color:blue">เริ่มผลิต</P>' : pr[i]["Pack_QTY_Balance"] && pr[i]["Pack_QTY_Balance"] < pr[i]["Pack_QTY"] ? '<P style="color:Red">' + pr[i]["Pack_QTY_Balance"] + '</P>' : pr[i]["Pack_QTY_Balance"]),
                // pr[i]["Pack_Weight"] == null ? '<P">-</P>' : pr[i]["Pack_Weight"].toFixed(0) + "Kg",
                pr[i]["Pack_Weight"] == null || pr[i]["Pack_Weight"] == 0 ? '<P">-</P>' : pr[i]["Pack_Weight"].toFixed(0) + "Kg",
               // pr[i]["Reel_Weight"] == null ? '<P">-</P>' : (pr[i]["Pack_Weight"] + pr[i]["Reel_Weight"]).toFixed(0) > 3000 ? '<P style="background-color:red;color:aliceblue">' + (pr[i]["Pack_Weight"] + pr[i]["Reel_Weight"]).toFixed(0) + " Kg" + '</P>' : (pr[i]["Pack_Weight"] + pr[i]["Reel_Weight"]).toFixed(0) + " Kg",
                pr[i]["Pack_Gross"],
                pr[i]["Pack_ReelNo"],
                pr[i]["Pack_Reel_Size"],

                (pr[i]["Drum_High"] != null ? pr[i]["Drum_High"] + "x" + pr[i]["Drum_Length"] : "-"),
                (pr[i]["Size"] != null ? pr[i]["Size"] : "-"),
                (pr[i]["Q_Unit"] != null ? pr[i]["Q_Unit"] + "ชิ้น หนา" + pr[i]["Thickness"] : "-"),
                // pr[i]["Pack_Speed"],
                //  pr[i]["Pack_QTY"] / pr[i]["Pack_Speed"], 
                //  (CalTime(pr[i]["Pack_Update"]) != "Invalid date" ? CalTime(pr[i]["Pack_Update"]):"-"),
                CalMin(pr[i]["Pack_QTY"], pr[i]["Pack_Speed"], pr[i]["Pack_DateTime"]),
                (pr[i]["Pack_Status_Shop"] == "T" ? '<P  style="background-color:forestgreen;color:white">เตรียมเสร็จ</P>' : "-")
                //(pr[i]["Pack_Status_QC"] == "F" ? '<i style="color:Red" class="fa fa-times"></i>' : '<i style="color:Green" class="fa fa-check"></i>')
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

function CalTime(A) {
    var m = moment(A);
    //alert(m);
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
function CalMin(A, B, C) {

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

