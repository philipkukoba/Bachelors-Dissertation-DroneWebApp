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
                $("#successfullyAdded").hide();
                $("#progressField").hide();
                totalFilesToParse = $('#file').get(0).files.length;
                filesLeftToParse = 0;
            }
        
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
}); // end of $(document).ready

// Helper-function for AjaxForm: changes the progress bar value at an interval
function startParsing() {
    $("#progressbar").progressbar({ value: 0 });
    intervalID = setInterval(parse, 250);  //every 0.25 sec the progress bar updates
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

