let intervalID;
let totalFilesToParse;
let filesLeftToParse;
let amountParsed;
let currentFile;
let my_form;

$(document).ready(function () {
    // File extension verification; upload button will be disabled if the user tries to upload an extension that is not allowed
    $("#file").change(function () {
        var fileExtension = ['pdf', 'dat', 'txt', 'csv', 'xyz', 'tfw', 'jpg']; // allowed extensions
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            alert("Only formats are allowed : " + fileExtension.join(', '));
            $("#uploadbtnSubmit").prop('disabled', true); // disable upload button
        }
        else {
            $("#uploadbtnSubmit").prop('disabled', false); // enable upload button
        }
    });

    // AjaxForm
    $('#Myform').ajaxForm({
        beforeSend: function (xhr) {
            // If the user did not submit any files, abort the ajaxForm
            if ($('#file').get(0).files.length === 0) {
                xhr.abort();
            }
            else {
                //init 
                $("#progressbar").progressbar({ value: 0 }); // reset to 0%
                $("#progressbar").progressbar("enable");
                $("#successfullyAdded").hide(); // hide successfully added ending message 
                $("#progressField").hide(); // hide progress of reading files
                totalFilesToParse = $('#file').get(0).files.length;
                filesLeftToParse = 0;
            }
            console.log("beforeSend! :)");
        },
        uploadProgress: function (event, position, total, percentComplete) {
            $("#uploadstatus").text("Uploading files... (" + percentComplete + "%)");
            $("#progressbar").progressbar("value", percentComplete);
            $("#successfullyAdded").hide();
            // Once uploading is complete...
            if (percentComplete == 100) {
                $("#uploadstatus").text("Upload complete.");
                $("#initialMessage").hide();
                console.log("Attempting to parse: " + totalFilesToParse + " files.");
                currentFile = "";
                $(".amountParsed").text(0);
                $(".totalParsed").text(totalFilesToParse);
                $("#progressField").show();
                // begin parsing
                console.log("startParsing");
                startParsing();
            }
        }
    });

}); // end of $(document).ready

// Helper-function for AjaxForm: changes the progress bar value at an interval
function startParsing() {
    $("#progressbar").progressbar({ value: 0 });
    intervalID = setTimeout(parse, 500);  //every 0.5 sec the progress bar updates
}

// Helper-function for startParsing: called in startParsing to change the progress bar value through an ajax call
function parse() {
    $.ajax({
        type: "GET",
        url: "/Files/GetStatus/",
        dataType: 'json',
        success: function (result) {       
            currentFile = result.currFileName // set the current file
            filesLeftToParse = result.currFilesLeft
            amountParsed = totalFilesToParse - filesLeftToParse;
            // set View
            $(".amountParsed").text(amountParsed);
            // Update the amount of files that still have to be parsed
            console.log("Files left to parse: " + filesLeftToParse);
            //update the progress bar
            $("#uploadstatus").text("Parsing file: " + currentFile + " (" + Math.round(result.currProgress) + "%)");
            $("#progressbar").progressbar("value", Math.round(result.currProgress));

            if (amountParsed == totalFilesToParse) { // ending procedure
                console.log("Parsed: " + amountParsed + " (had to parse: " + totalFilesToParse + ")");
                clearTimeout(intervalID); // clear
                $(".amountParsed").text(amountParsed);
                $(".totalParsed").text(totalFilesToParse);
                $("#uploadstatus").text("Parsing complete.");
                $("#progressField").hide();
                $("#successfullyAdded").show();
                console.log("Done.")
                // Display the list of files that were parsed (un)successfully
                // **todo**

            }
            else { // fire another parse
                setTimeout(parse, 500); 
            }
        },
        complete: function () {
        }
    });
}

