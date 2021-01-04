$(document).ready(function () {
    $("#All").DataTable({
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
            "url": "../CreateBar/LoadData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "PL_SO" },
            { "data": "PL_Machine" },
            { "data": "PL_Process" },
            { "data": "PL_Tag" },
            { "data": "PL_Qty_T" },
            { "data": "PL_Type" },
            { "data": "PL_Reel_Size" },
            { "data": "PL_Time" }, 
            {
                "data": "ID",
                render: function (file_id) {
                    return file_id ?
                        '<a href="../CreateBar/PrintViewToPdf?id=' + file_id + '" target="_blank" width="100"> <p hidden>' + file_id + '</p> พิมพ์</a>' :
                          null; 
                }
            }

            // ], "order": [[0, "asc"]]
        ], "order": [[8, "desc"]]
    });  
}); 