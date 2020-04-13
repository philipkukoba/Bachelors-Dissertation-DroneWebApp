require([
    "esri/Map",
    "esri/views/MapView",
    "esri/Graphic",
    "esri/geometry/SpatialReference",
    "esri/widgets/LayerList",
    "esri/PopupTemplate",
    "esri/layers/FeatureLayer"
], function (Map, MapView, Graphic, SpatialReference, LayerList, PopupTemplate, FeatureLayer) {

    //#region basic setup 
    // Create the map
    var map = new Map({
        basemap: "topo-vector"
    });

    // Create the View (MapView)
    var view = new MapView({
        container: "viewDiv",
        map: map,
        center: [3.30120924, 50.85590007],
        zoom: 16
    });

    //ID van de droneflight, nodig voor visualisation 
    var id = $("#viewDiv").data("id");



    //#endregion

    // #region Coordinate Widget
    // Create a coordinate widget
    var coordsWidget = document.createElement("div");
    coordsWidget.id = "coordsWidget";
    coordsWidget.className = "esri-widget esri-component";
    coordsWidget.style.padding = "7px 15px 5px";

    // Add this widget to the View
    view.ui.add(coordsWidget, "bottom-right");

    // Function to show coordinates
    function showCoordinates(pt) {
        var coords = "Lat/Lon " + pt.latitude.toFixed(3) + " " + pt.longitude.toFixed(3) +
            " | Scale 1:" + Math.round(view.scale * 1) / 1 +
            " | Zoom " + view.zoom;
        coordsWidget.innerHTML = coords;
    }

    view.watch("stationary", function (isStationary) {
        showCoordinates(view.center);
    });

    view.on("pointer-move", function (evt) {
        showCoordinates(view.toMap({ x: evt.x, y: evt.y }));
    });

    // #endregion

    //#region Visual Variable and custom Renderer

    //visual variable
    var colorVisVar = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "HeightMSL",     //specify which field to use
        stops: [{ value: 0.0, color: "#FF0000" }, { value: 55.0, color: "#0000FF" }]
    };

    //specify visualisation here 
    var customRenderer = {
        type: "simple",                 // autocasts as new SimpleRenderer()
        symbol: { type: "simple-marker" }, // autocasts as new SimpleMarkerSymbol()
        visualVariables: [colorVisVar]
    };

    //#endregion

    //#region readPoint function and Main AJAX call 

    function readPoint(gp) {
        var pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: gp.Long,
                y: gp.Lat
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                GPSId: gp.GPSId,
                HeightMSL: gp.HeightMSL
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneGPs/" + id, // the URL of the controller action method
        success: function (result) {
            console.log("AJAX: SUCCESS");

            var features = [];
            for (let i = 0; i < result.length; i++) {
                features.push(readPoint(result[i]));
            }

            var featurelayer = new FeatureLayer({
                source: features,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features)
                spatialReference: SpatialReference.WGS84,           // autocasts to wgs84 if not set 
                fields: [{                                          //repeat the fields for visual variables here!!! 
                    name: "HeightMSL",   
                    type: "double"
                }],
                objectIdField: "GPSId",                             //needed to uniquely identify each object
                renderer: customRenderer
            });

            map.add(featurelayer);

        },
        error: function (req, status, error) {
            console.log("AJAX: FAIL");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
    //#endregion 

});


