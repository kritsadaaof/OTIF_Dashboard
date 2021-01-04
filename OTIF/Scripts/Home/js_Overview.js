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
            "url": "../Home/LoadData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "Bar_Kan" },
            { "data": "QTY", "name": "QTY", "autoWidth": true },
            { "data": "Location", "name": "Location", "autoWidth": true },
            { "data": "Machine", "name": "Machine", "autoWidth": true } 
        ]
    }); 
}); 