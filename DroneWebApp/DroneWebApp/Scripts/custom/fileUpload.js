
//$.noConflict();  //caused problems
var intervalID;

jQuery(document).ready(function ($) {
$('#Myform').ajaxForm({
    beforeSend: function () {
        //init 
        $("#progressbar").progressbar({ value: 0 });
        $("#progressbar").progressbar("enable");
    },
    uploadProgress: function (event, position, total, percentComplete) {
        $("#uploadstatus").text("Uploading the file.. (" + percentComplete + "%)");
        $("#progressbar").progressbar("value", percentComplete);

        //start parsing
        if (percentComplete == 100) { 
            startParsing();
        }
    },
    complete: function () {   //complete wordt pas opgeroepen als parsing ook gedaan is
        clearInterval(intervalID);
        $("#uploadstatus").text("Parsing complete.");
    }
});
});

function startParsing() {
    $("#progressbar").progressbar({ value: 0 });
    intervalID = setInterval(updateProgress, 250);  //every 0,25sec updates progressbar
}

function updateProgress() {
    $.ajax({
        type: "GET",
        url: "/Files/getUploadStatus/",  //eigenlijk progressStatus
        success: function (progressValue) {       
            //update the progress bar
            $("#uploadstatus").text("Parsing the file.. (" + Math.round(progressValue) + "%)");
            $("#progressbar").progressbar("value", Math.round(progressValue));
        }
    });
}

