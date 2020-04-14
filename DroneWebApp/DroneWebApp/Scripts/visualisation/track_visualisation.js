﻿require([
    "esri/Map",
    "esri/views/MapView",
    "esri/Graphic",
    "esri/layers/GraphicsLayer",
    "esri/geometry/SpatialReference",
    "esri/widgets/LayerList",
    "esri/PopupTemplate"
], function (Map, MapView, Graphic, GraphicsLayer, SpatialReference, LayerList, PopupTemplate) {

    // Create the map
    var map = new Map({
        basemap: "topo-vector"
    });

    // Create the View (MapView)
    var view = new MapView({
        container: "viewDiv",
        map: map,
        center: [3.30120924, 50.85590007],
        zoom: 20
    });

    var graphicsLayer = new GraphicsLayer();

    map.add(graphicsLayer);

    //ID van de droneflight, nodig voor visualisation 
    var id = $("#viewDiv").data("id");
    console.log(id);

    //#region Track Visualisation

    var urlTrack = "/WebAPI/api/DroneGPs/" + id;
    console.log(urlTrack);

    var path = [];

    function makePath(gp) {
        var point = [gp.Long, gp.Lat, gp.HeightMSL];
        path.push(point);
    }

    function displayTrack(path) {
        var track = {
            type: "polyline",
            hasM: "false",
            hasZ: "true",
            paths: path,
            //spatialReference: sr
        };

        var simpleMarkerSymbol = {
            type: "simple-line",
            color: "yellow",
            width: 4
        };

        var popupTemplate = {
            title: "Flight " + id
        };

        var trackGraphic = new Graphic({
            geometry: track,
            popupTemplate: popupTemplate,
            symbol: simpleMarkerSymbol
        });

        graphicsLayer.add(trackGraphic);

        view.goTo({
            target: trackGraphic
        });
    }

    $.ajax({
        type: "GET",
        url: urlTrack, // the URL of the controller action method
        data: null, // optional data
        success: function (result) {
            console.log("AJAX: SUCCESS");
            result.forEach(makePath);
            console.log(path);
            displayTrack(path);
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


