﻿var intervalID;

$(document).ready(function () {
$('#Myform').ajaxForm({
    beforeSend: function () {
        //init 
        $("#progressbar").progressbar("value", 0); // reset to 0%
        $("#progressbar").progressbar("enable");
        $("#successfullyAdded").hide();
    },
    uploadProgress: function (event, position, total, percentComplete) {
        $("#uploadstatus").text("Uploading the file.. (" + percentComplete + "%)");
        $("#progressbar").progressbar("value", percentComplete);

        //start parsing
        if (percentComplete == 100) { 
            startParsing();
        }
    },
    complete: function () {   //complete is called once parsing is also done
        clearInterval(intervalID);
        $("#uploadstatus").text("Parsing complete.");
        // update old ViewBag fields
        $.ajax({
            type: "GET",
            url: "/Files/getParseResult/", 
            success: function (parseResult) {
                $("#progressbar").progressbar("value", 100); // ensures that the value at the end is definitely 100% in case a file is uploaded very quickly
                $("#initialMessage").hide();
                $("#successfullyAdded").show();
                if (parseResult) {
                    $("#continuationMessage").show();
                }
                else {
                    $("#alreadyExists").show();
                }
            }
        });
    }
});
});


function startParsing() {
    $("#progressbar").progressbar({ value: 0 });
    intervalID = setInterval(updateProgressBar, 250);  //every 0,25sec the progress bar updates
}

function updateProgressBar() {
    $.ajax({
        type: "GET",
        url: "/Files/getProgressStatus/",
        success: function (progressValue) {       
            //update the progress bar
            $("#uploadstatus").text("Parsing file.. (" + Math.round(progressValue) + "%)");
            $("#progressbar").progressbar("value", Math.round(progressValue));
        }
    });
}

