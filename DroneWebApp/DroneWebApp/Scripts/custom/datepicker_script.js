// Datepicker date format
// unused because of bugs: it formats properly, but then jquery won't parse dd/mm/yy correctly and throws errors.

$(document).ready(function () {
    $("#date").datepicker({
        dateFormat: 'dd/mm/yy',
        todayHighlight: true
    });
})