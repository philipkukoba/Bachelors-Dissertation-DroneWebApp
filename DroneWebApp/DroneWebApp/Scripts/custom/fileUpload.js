let intervalID;
let totalFilesToParse;
let filesLeftToParse;
let amountParsed;
let currentFile;

$(document).ready(function () {
$('#Myform').ajaxForm({
    beforeSend: function () {
        //init 
        $("#progressbar").progressbar({ value: 0 }); // reset to 0%
        $("#progressbar").progressbar("enable");
        $("#successfullyAdded").hide();
        $("#progressField").hide();
        totalFilesToParse = 0;
        filesLeftToParse = 0;
    },
    uploadProgress: function (event, position, total, percentComplete) {
        $("#uploadstatus").text("Uploading files... (" + percentComplete + "%)");
        $("#progressbar").progressbar("value", percentComplete);

        // Once uploading is complete...
        if (percentComplete == 100) {
            currentFile = "";
            $(".amtRead").text(0);
            $.ajax({
                type: "GET",
                url: "/Files/GetTotalFiles/",
                success: function (result) {
                    totalFilesToParse = result;
                    $("#progressField").show();
                    $(".totalRead").text(result);
                },
                // on complete of this ajax-call, start tracking the progress of the parsing
                complete: function () {
                    console.log("Total files that must be parsed:" + totalFilesToParse);
                    // begin parsing
                    startParsing();

                    // todo: check the list of files & booleans and tell the user what was not uploaded + how many were successfully uploaded
                    // ajax call

                }
            });
        }
    },
    complete: function() {
        $("#uploadstatus").text("Upload complete.");
        $("#initialMessage").hide();
    }
});
});

// changes the progress bar value at an interval
function startParsing() {
    $("#progressbar").progressbar({ value: 0 });
    intervalID = setInterval(parse, 250);  //every 0.25 sec the progress bar updates
}

// called in startParsing to change the progress bar value through an ajax call
function parse() {
    $.ajax({
        type: "GET",
        url: "/Files/GetStatus/",
        dataType: 'json',
        success: function (result) {       
            currentFile = result.currFileName // set the current file
            filesLeftToParse = result.currFilesLeft
            amountParsed = totalFilesToParse - filesLeftToParse;
            if (amountParsed < 0) { // ******
                amountParsed = 0;
            }
            // set View
            $(".amtRead").text(amountParsed);
            // Update the amount of files that still have to be parsed
            console.log("Files left to parse: " + filesLeftToParse);
            //update the progress bar
            $("#uploadstatus").text("Parsing file " + currentFile + " (" + Math.round(result.currProgress) + "%)");
            $("#progressbar").progressbar("value", Math.round(result.currProgress));

            if (filesLeftToParse == 0) {
                console.log("parsed: " + amountParsed);
                console.log("tot: " + totalFilesToParse);
                clearInterval(intervalID); // not being applied correctly
                $(".amtRead").text(amountParsed); // wrong
                $(".totalRead").text(totalFilesToParse); // wrong
                $("#uploadstatus").text("Parsing complete.");
                $("#progressField").hide();
                $("#successfullyAdded").show();
            }
        },
        complete: function () {

        }
    });
}

