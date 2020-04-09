
//init 
$("#progressbar").progressbar({ value: 0 });
$("#progressbar").progressbar("enable");
console.log("progress bar initialised");

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
            //$("#progressbar").progressbar("value", uploadValue); 

            //testing 
            $("#progressbar").progressbar("value", Math.floor((Math.random() * 100) + 1)); 
        }
    });
}

//probably solve with viewbag 
$("#uploadstatus").text = "UPLOADING!!!";