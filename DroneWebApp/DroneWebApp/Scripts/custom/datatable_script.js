// These scripts change the regular tables selected by id into DataTables that have searchability, sortability and paging

$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#dftable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [5, 6, 7, 8],
                "searchable": false, "targets": [5, 6, 7, 8]
            }
        ]
      }); 
});

$(document).ready(function () {
    $.fn.dataTable.moment('DD/MM/YYYY');

    $('#pdftable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": [4, 5],
                "searchable": false, "targets": [4, 5]
            }
        ]
    });
});

$(document).ready(function () {
    $('#dtable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": 5,
                "searchable": false, "targets": 5
            }
        ]
    });
});

$(document).ready(function () {
    $('#ptable').dataTable({
        "columnDefs": [
            {
                "orderable": false, "targets": 6,
                "searchable": false, "targets": 6
            }
        ]
    });
});

$(document).ready(function () {
    $('#ctrlptable').dataTable();
});

$(document).ready(function () {
    $('#gcpptable').dataTable();
});