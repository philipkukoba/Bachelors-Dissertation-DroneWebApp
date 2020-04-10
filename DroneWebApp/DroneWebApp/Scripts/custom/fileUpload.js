

console.log("new script 4");

//$.noConflict();  //idk lol

let doneParsing = false; 

jQuery(document).ready(function ($) {
$('#Myform').ajaxForm({
    beforeSend: function () {
        console.log("beforesend");

        //init 
        $("#progressbar").progressbar({ value: 0 });
        $("#progressbar").progressbar("enable");
        console.log("progress bar initialised");
        $("#uploadstatus").text("beforesend");
    },
    uploadProgress: function (event, position, total, percentComplete) {
        console.log("uploadprogress");
        $("#uploadstatus").text("Uploading the file.. (" + percentComplete + "%)");
        $("#progressbar").progressbar("value", percentComplete);
        //if (percentComplete == 100) {   
        //    doneParsing = true; 
        //}
    },
    complete: function () {   //complete wordt pas opgeroepen als parsing ook gedaan is
        doneParsing = true; 
        console.log("complete");
        $("#uploadstatus").text("Upload complete.");
    }
});
});

//every 0,25sec updates progressbar
//if (doneParsing) {
    var intervalID = setInterval(updateProgress, 250);
    $("#progressbar").progressbar({ value: 0 });
//}

function updateProgress() {
    $.ajax({
        type: "GET",
        url: "/Files/getUploadStatus/",  //eigenlijk progressStatus
        //data: "{}",
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (progressValue) {       
            //update the progress bar
            console.log("parsing progress: " + progressValue );
            $("#uploadstatus").text("Parsing the file...");
            $("#progressbar").progressbar("value", Math.round(progressValue));
        }
    });
}

