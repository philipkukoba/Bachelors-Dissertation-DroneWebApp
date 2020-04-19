require([
    "esri/Map",
    "esri/views/MapView"
], function (Map, MapView) {

        //#region Basic setup: Map and View

        // Create the map
        let map = new Map({
            basemap: "topo-vector"
        });

        // Create the View (MapView)
        let view = new MapView({
            container: "viewDiv",
            map: map,
            center: [3.30120924, 50.85590007],
            zoom: 20
        });

        //#endregion

        //#region Visualisation Const Values 
        //ID van de droneflight 
        const id = $("#viewDiv").data("id");

        //Lambert spacial reference (needed for all featurelayers except track visualisation)
        const LambertSR = { wkid: 31370 };

        //#endregion 

        $.ajax({
            type: "GET",
            url: "/WebAPI/api/PointCloudControlTool/" + id, // the URL of the controller action method
            //data: null, // optional data
            success: (result) => {

                let pointcloudControls = [];

                for (let i = 0; i < result.length; i++) {
                    pointcloudControls.push(result[i]);
                }
                console.log("AJAX SUCCES: PointcloudControlTool");

                for (let j = 0; j < pointcloudControls.length; j++) {
                    console.log(pointcloudControls[j].CTRLName);
                    console.log(pointcloudControls[j].Inside);
                }
            },
            error: (req, status, error) => {
                console.log("AJAX FAIL: PointcloudControlTool");
                console.log(req + status + error);
            }
        });

        
});