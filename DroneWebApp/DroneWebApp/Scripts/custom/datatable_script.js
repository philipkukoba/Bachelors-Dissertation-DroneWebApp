$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#dftable').dataTable({
        "columnDefs": [
            { "orderable": false, "targets": [5,6] }
        ]
    });
    
});

$(document).ready(function () {
    $('#dtable').dataTable({
        "columnDefs": [
            { "orderable": false, "targets": 3 }
        ]
    });
});

$(document).ready(function () {
    $('#ptable').dataTable({
        "columnDefs": [
            { "orderable": false, "targets": 9 }
        ]
    });
});