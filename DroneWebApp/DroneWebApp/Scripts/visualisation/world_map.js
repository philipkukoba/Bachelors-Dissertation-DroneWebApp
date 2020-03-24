require([
    "esri/Map",
    "esri/views/MapView",
    "esri/layers/FeatureLayer"
], function (Map, MapView, FeatureLayer) {

    // Create the map
    var map = new Map({
        basemap: "topo-vector"
    });

    // Create the View (MapView)
    var view = new MapView({
        container: "viewDiv",
        map: map,
        center: [3.2, 51.2],
        zoom: 11
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

    //#region AJAX Testing
    $.ajax({
        type: "GET",
        url: "/api/CTRLPoints/1/", // the URL of the controller action method
        data: null, // optional data
        success: function (result) {
            console.log(result);
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
    //#endregion 
});


