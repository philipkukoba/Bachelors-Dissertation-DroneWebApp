

console.log("new script 4");

//$.noConflict();  //idk lol

jQuery(document).ready(function ($) {
$('#Myform').ajaxForm({
    beforeSend: function () {
        console.log("beforesend");

        //init 
        $("#progressbar").progressbar({ value: 0 });
        $("#progressbar").progressbar("enable");
        console.log("progress bar initialised");
        $("#uploadstatus").text("beforesend");
        //status.empty();
        //var percentVal = '0%';
        //bar.width(percentVal);
        //percent.html(percentVal);
    },
    uploadProgress: function (event, position, total, percentComplete) {
        console.log("uploadprogress");
        $("#uploadstatus").text("Uploading the file.. (" + percentComplete + "%)");
        $("#progressbar").progressbar("value", percentComplete);
       // var percentVal = percentComplete + '%';
       // bar.width(percentVal);
        //percent.html(percentVal);
    },
    complete: function () {   //complete wordt nooit opgeroepen??
        console.log("complete");
        $("#uploadstatus").text("Upload complete.");
        //status.html(xhr.responseText);
    }
});
});

let amountOfLines;

$.ajax({
    type: "GET",
    url: "/Files/getUploadStatus/",
    async: true,
    success: function (uploadValue) {
        amountOfLines = uploadValue;
    }
});

console.log("amount of lines in js: " + amountOfLines);


//every 0,25sec updates progressbar
var intervalID = setInterval(updateProgress, 250);

function updateProgress() {
    $.ajax({
        type: "GET",
        url: "/Files/getUploadStatus/",
        //data: "{}",
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        async: true,
        success: function (uploadValue) {

            console.log("successful ajax get!!");
            console.log("upload status val " + uploadValue);

            //update the progress bar
            $("#progressbar").progressbar("value", uploadValue);

            //testing
            //$("#progressbar").progressbar("value", Math.floor((Math.random() * 100) + 1));
        }
    });
}

//probably solve with viewbag
$("#uploadstatus").text = "UPLOADING!!!";


