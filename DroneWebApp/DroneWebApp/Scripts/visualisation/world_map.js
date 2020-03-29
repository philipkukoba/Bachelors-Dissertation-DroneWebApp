﻿require([
    "esri/Map",
    "esri/views/MapView",
    "esri/layers/FeatureLayer",
    "esri/Graphic",
    "esri/layers/GraphicsLayer",
    "esri/geometry/SpatialReference"
], function (Map, MapView, FeatureLayer, Graphic, GraphicsLayer, SpatialReference) {

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

    // #region Example labels: trails near LA

    var trailheadsRenderer = {
        type: "simple",
        symbol: {
            type: "picture-marker",
            url: "http://static.arcgis.com/images/Symbols/NPS/npsPictograph_0231b.png",
            width: "18px",
            height: "18px"
        }
    }

    var trailheadsLabels = {
        symbol: {
            type: "text",
            color: "#FFFFFF",
            haloColor: "#5E8D74",
            haloSize: "2px",
            font: {
                size: "12px",
                family: "Noto Sans",
                style: "italic",
                weight: "normal"
            }
        },
        labelPlacement: "above-center",
        labelExpressionInfo: {
            expression: "$feature.TRL_NAME"
        }
    };


    var trailheads = new FeatureLayer({
        url:
            "https://services3.arcgis.com/GVgbJbqm8hXASVYi/arcgis/rest/services/Trailheads/FeatureServer/0",
        renderer: trailheadsRenderer,
        labelingInfo: [trailheadsLabels]
    });

    map.add(trailheads);

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

    //Add Graphics Layer
    var graphicsLayer = new GraphicsLayer();
    map.add(graphicsLayer);

    //#region AJAX Testing AND XYZ VISUALISATION
    //var belgianLambertWKT = PROJCS["Belge 1972 / Belgian Lambert 72",GEOGCS["Belge 1972",DATUM["D_Belge_1972",SPHEROID["International_1924",6378388,297]],PRIMEM["Greenwich",0],UNIT["Degree",0.017453292519943295]],PROJECTION["Lambert_Conformal_Conic"],PARAMETER["standard_parallel_1",51.16666723333333],PARAMETER["standard_parallel_2",49.8333339],PARAMETER["latitude_of_origin",90],PARAMETER["central_meridian",4.367486666666666],PARAMETER["false_easting",150000.013],PARAMETER["false_northing",5400088.438],UNIT["Meter",1]];

    var sr = { // autocasts to esri/geometry/SpatialReference
        //imageCoordinateSystem: { id: imageId }
        wkid: 31370
    };

    $.ajax({
        type: "GET",
        url: "/api/PointCloudXYZs/1", // the URL of the controller action method
        data: null, // optional data
        success: function (result) {
            console.log("AJAX: SUCCESS");
            for (let i = 0; i < 999; i++) {
                console.log(result[i]);
                var point = {
                    type: "point",
                    x: result[i].X,
                    y: result[i].Y,
                    z: result[i].Z,
                    //!!! spatial reference instellen 
                    spatialReference: sr
                };
                console.log(point);

                var simpleMarkerSymbol = {
                    type: "simple-marker",
                    color: [result[i].Red, result[i].Green, result[i].Blue],
                    size: "7px",
                    outline: {
                        color: [255, 255, 255], // white
                        width: 1
                    }
                };
                console.log(simpleMarkerSymbol);

                var pointGraphic = new Graphic({
                    geometry: point,
                    symbol: simpleMarkerSymbol
                });
                console.log(pointGraphic);

                graphicsLayer.add(pointGraphic);
            }
        },
        error: function (req, status, error) {
            console.log("AJAX: FAIL");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
    //#endregion 

    //#region XYZ Visualisation testing 

    //#endregion
});


