$(document).ready(function () {

    function DataPL(d) {
        return d.Material;
    }
    function DataActual(d) {
        return d.SO;
    }
    function format(d, a, c) {
        return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:100px;">' +
            '<tr>' +
            '<td width="300PX">PLAN</td>' +
            '<td width="300PX">WIP</td>' +
            '<td width="300PX">FG</td>' +
            '</tr>' +
            '<tr >' +
            '<td>' + d + '</td>' +
            '<td>' + a + '</td>' +
            '<td>' + c + '</td>' +
            '</tr>' +
            '</table>';
    }

    function QTY(d) {
        return d.Order_QTY;
    }
    function QTYPL(d) {
        return '<i style="color:#ff6a00;font-size:16px"> PLAN Order: ' + d.Order_QTY + ' ' + d.Unit + '</i>';
    }
    function QTYAC(d) {
        return '<i style="color:#0094ff;font-size:16px"> Operation Order: ' + d.Order_QTY + ' ' + d.Unit + '</i>';
    }
    function QTYFG(d) {
        return '<i style="color:green;font-size:16px"> Finished goods Order: ' + d.Order_QTY + ' ' + d.Unit + '</i>';
    }

    function CheckSOPl(Material) {

        // alert("aa");
        $.post(baseUrl + "SO_PL_AC/CheckSOPL", {
            SO: Material.Material
        }).done(function (data) {
            return data;

            var pr = $.parseJSON(data);
            alert(data);
            alert(pr);
            $.each(JSON.parse(data), function (i, obj) {
                pr[i]["Machine"]
            });

        });
    }
    var dt = $('#Report').DataTable({
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
            "url": "../SO_PL_AC/LoadData",
            "type": "POST",
            "datatype": "json"

        },
        "columns": [

            {
                "data": "SO"
            },
            { "data": "Customer" },
            //{ "data": "Material" },
            { "data": "Material_Des" },
            { "data": "Date_Delivery" },
            {
                "class": "details-control",
                "orderable": false,
                "data": null,
                "defaultContent": ""
            },
            /* {
                 data: null,
                 render: function (row) {
                     return "<a href='#' class='btn btn-success' onclick=Show('" + row.SO + "'); >Sh</a>";
                 }
             }, */

        ], "order": [[1, "desc"]],
        "pageLength": 10000,
        "lengthMenu": [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]]
    });

    // Array to track the ids of the details displayed rows
    var detailRows = [];

    $('#Report tbody').on('click', 'tr td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#Report').DataTable().row(tr);
        var idx = $.inArray(tr.attr('id'), detailRows);

        if (row.child.isShown()) {
            tr.removeClass('details');
            row.child.hide();
            // Remove from the 'open' array
            detailRows.splice(idx, 1);
        }
        else { 
            //   alert(row.data(Material));
            tr.addClass('details');
            var PLAN = QTYPL(row.data());
            var ACTUAL = QTYAC(row.data());
            var FG = QTYFG(row.data());
            $.post(baseUrl + "SO_PL_AC/CheckSOPL", {
                MAT: DataPL(row.data())
            }).done(function (data) {
                var pr = $.parseJSON(data);
                $.each(JSON.parse(data), function (i, obj) {
                    PLAN += '<br>' + (i + 1) + "). " + pr[i]["Operation"];
                });
                // row.child(format(PLAN,"aaa")).show();
                $.post(baseUrl + "SO_PL_AC/CheckActual", {
                    SO: DataActual(row.data())
                }).done(function (data) {
                    var pr = $.parseJSON(data);
                    $.each(JSON.parse(data), function (i, obj) {
                        ACTUAL += '<br> -' + pr[i]["MasterProcess"] + " QTY : " + (pr[i]["QTY"] < QTY(row.data()) ? '<i style="color:red">' + pr[i]["QTY"] + '</i>' : pr[i]["QTY"]);
                    });
                    $.post(baseUrl + "SO_PL_AC/CheckFG", {
                        SO: DataActual(row.data())
                    }).done(function (data) {
                        var pr = $.parseJSON(data);
                        $.each(JSON.parse(data), function (i, obj) {
                            FG += '<br> TAG ' + pr[i]["PRO_Tag_No"] + " QTY : " + pr[i]["PRO_Quantity"];
                        });
                        row.child(format(PLAN, ACTUAL, FG)).show();
                    });
                });
            });


            // alert(result);
            // row.child(format(row.data())).show();
            // Add to the 'open' array
            if (idx === -1) {
                detailRows.push(tr.attr('id'));
            }
        }
    });

    // On each draw, loop over the `detailRows` array and show any child rows
    dt.on('draw', function () {
        $.each(detailRows, function (i, id) {
            $('#' + id + ' td.details-control').trigger('click');
        });
    });




});
function Show(Session_ID) {
    //  alert(Session_ID);
    // if (confirm("ข้อมูล" + Session_ID)) {
    // Delete(Session_ID);
    //  } else {
    //      return false;
    //  }
}
function dateFormat(d) {
    month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var val = day + "/" + month + "/" + year;
    return val;
}