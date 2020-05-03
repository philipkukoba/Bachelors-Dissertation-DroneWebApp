let intervalID;
let totalFilesToParse;
let filesLeftToParse;
let amountParsed;
let currentFile;
let my_form;
let firstCheck = true;

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
                $("#endField").hide(); // hide endField ending message 
                $("#progressField").hide(); // hide progress of reading files
                totalFilesToParse = $('#file').get(0).files.length;
                filesLeftToParse = 0;
                firstCheck = true;
            }
            console.log("beforeSend! :)");
        },
        uploadProgress: function (event, position, total, percentComplete) {
            $("#uploadstatus").text("Uploading files... (" + percentComplete + "%)");
            $("#progressbar").progressbar("value", percentComplete);
            $("#endField").hide();
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
        },
        // Return values for error-handling: 
        // 1: success
        // 0: no files submitted
        // 2: no drone flight specified
        // 3: drone flight does not exist
        // 4: someone else is already uploading
        // 5: invalid file type
        success: function (errorCode) {
            // to do: print message on screen for user
            // except when it is == 1
            if (errorCode != 1) {
                $("#progressField").hide();
                $("#endField").hide();
                let text;
                if (errorCode == 0) {
                    text = "No files were submitted.";
                }
                else if (errorCode == 2) {
                    text = "Please specify a Drone Flight.";
                }
                else if (errorCode == 3) {
                    text = "This Drone Flight does not exist.";
                }
                else if (errorCode == 4) {
                    text = "Someone else is already uploading. Please try again in a few minutes.";
                }
                else if (errorCode == 4) {
                    text = "An invalid file type was submitted.";
                }
                $("#errorField").show();
                $("#errorMessage").text(text);
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
    $.get("/Files/GetStatus/", function (result) {
        currentFile = result.currFileName // set the current file
        console.log(result.currFilesLeft);
        filesLeftToParse = result.currFilesLeft;
        if (firstCheck) {
            if (filesLeftToParse > 0) {
                firstCheck = false;
            }
            intervalID = setTimeout(parse, 500);
            return;
        }

        amountParsed = totalFilesToParse - filesLeftToParse;
        // set View
        $(".amountParsed").text(amountParsed);
        // Update the amount of files that still have to be parsed
        console.log("Files left to parse: " + filesLeftToParse);
        //update the progress bar
        let progress = Math.round((amountParsed / totalFilesToParse + (result.currProgress / 100) / totalFilesToParse) * 100);
        $("#uploadstatus").text("Parsing file: " + currentFile + " (" + progress + "%)");
        $("#progressbar").progressbar("value", progress);

        if (amountParsed == totalFilesToParse) { // ending procedure
            console.log("Parsed: " + amountParsed + " (had to parse: " + totalFilesToParse + ")");
            clearTimeout(intervalID); // clear
            $(".amountParsed").text(amountParsed);
            $(".totalParsed").text(totalFilesToParse);
            $("#uploadstatus").text("Parsing complete.");
            $("#progressField").hide();
            $("#totalFailed").text(result.failedFiles.length);
            console.log("Done.")
            // Display the list of files that were parsed unsuccessfully, if any
            // **todo**
            if (result.failedFiles.length == 0) { // no failed files
                //document.getElementById("endField").classList.add(" alert-success");
                //document.getElementById("endField").classList.remove("alert-warning");
                $("#endField").show();
            }
            else { // failed files
                console.log("array length else: " + result.failedFiles.length);
                //document.getElementById("endField").classList.add(" alert-warning");
                //document.getElementById("endField").classList.remove("alert-success");
                $("#endField").show();
                document.getElementById("failedFilesList").appendChild(makeUL(result.failedFiles));
                $("#failedFilesList").show()
            }
        }
        else { // fire another parse
            intervalID = setTimeout(parse, 500);
        }
    });

}

var options = [
    set0 = ['Option 1', 'Option 2'],
    set1 = ['First Option', 'Second Option', 'Third Option']
];

// Make the HTML for the failed files
function makeUL(array) {
    // Create the list element:
    var list = document.createElement('ul');

    for (var i = 0; i < array.length; i++) {
        // Create the list item:
        var item = document.createElement('li');

        // Set its contents:
        item.appendChild(document.createTextNode(array[i]));

        // Add it to the list:
        list.appendChild(item);
    }

    // Finally, return the constructed list:
    return list;
}

