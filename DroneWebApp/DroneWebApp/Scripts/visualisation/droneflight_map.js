require([
    "esri/Map",
    "esri/views/MapView",
    "esri/Graphic",
    "esri/layers/GraphicsLayer",
    "esri/geometry/SpatialReference",
    "esri/widgets/LayerList",
    "esri/PopupTemplate"
], function (Map, MapView, FeatureLayer, Graphic, GraphicsLayer, SpatialReference, LayerList, PopupTemplate) {

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

    //#region LAYERLIST TESTING
    var layerList = new LayerList({
        view: view
    });

    // Adds widget below other elements in the top right corner of the view
    view.ui.add(layerList, "top-right");

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

    //ID van de droneflight, nodig voor visualisation
    var id = $("#viewDiv").data("id");
    console.log(id);

    //#region XYZ VISUALISATION
    //var belgianLambertWKT = PROJCS["Belge 1972 / Belgian Lambert 72",GEOGCS["Belge 1972",DATUM["D_Belge_1972",SPHEROID["International_1924",6378388,297]],PRIMEM["Greenwich",0],UNIT["Degree",0.017453292519943295]],PROJECTION["Lambert_Conformal_Conic"],PARAMETER["standard_parallel_1",51.16666723333333],PARAMETER["standard_parallel_2",49.8333339],PARAMETER["latitude_of_origin",90],PARAMETER["central_meridian",4.367486666666666],PARAMETER["false_easting",150000.013],PARAMETER["false_northing",5400088.438],UNIT["Meter",1]];

    //popup for XYZ
    //werkt niet
    var popupXYZ = new PopupTemplate({
        "title": "XYZ Information",
        "content": "SAMPLE CONTENT"
    });

    //add graphics layer
    var XYZGraphicsLayer = new GraphicsLayer({
        title: "XYZ",
        //outFields: ["PointcloudXYZId", "X", "Y", "Z"],
        popupTemplate: popupXYZ
    });
    map.add(XYZGraphicsLayer);

    var sr = {
        wkid: 31370
        };

    let displayXYZ = (ajaxresult) => {
        var point = {
            type: "point",
            x: ajaxresult.X,
            y: ajaxresult.Y,
            z: ajaxresult.Z,
            //!!! spatial reference instellen
            spatialReference: sr
        };

        var simpleMarkerSymbol = {
            type: "simple-marker",
            color: [ajaxresult.Red, ajaxresult.Green, ajaxresult.Blue],
            size: "7px",
            outline: {
                color: [255, 255, 255], // white
                width: 1
            }
        };

        var pointGraphic = new Graphic({
            geometry: point,
            symbol: simpleMarkerSymbol
        });

        XYZGraphicsLayer.add(pointGraphic);
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/PointCloudXYZs/" + id, // the URL of the controller action method
        //data: null, // optional data
        success: function (result) {
            console.log("AJAX: SUCCESS");
            for (let i = 0; i < 2000; i++) {
                displayXYZ(result[i]);
            }
        },
        error: function (req, status, error) {
            console.log("AJAX: FAIL");
            console.log(req + status + error);
        }
    });
    //#endregion

    //#region CTRLPoints VISUALISATION
    var CTRLPointsLayer = new GraphicsLayer({
        title: "CTRL"
    });
    map.add(CTRLPointsLayer);

    let displayCTRL = (ajaxresult) => {
        var point = {
            type: "point",
            x: ajaxresult.X,
            y: ajaxresult.Y,
            z: ajaxresult.Z,
            //!!! spatial reference instellen
            spatialReference: sr
        };

        var simpleMarkerSymbol = {
            type: "simple-marker",
            color: [255, 255, 255], //white
            size: "12px",
            outline: {
                color: [0, 0, 0], // black
                width: 1
            }
        };

        var pointGraphic = new Graphic({
            geometry: point,
            symbol: simpleMarkerSymbol
        });

        CTRLPointsLayer.add(pointGraphic);
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/CTRLPoints/" + id, // the URL of the controller action method
        //data: null, // optional data
        success: function (result) {
            console.log("AJAX: SUCCESS");
            var count = Object.keys(result).length;
            for (let i = 0; i < count; i++) {
                displayCTRL(result[i]);
            }
        },
        error: function (req, status, error) {
            console.log("AJAX: FAIL");
            console.log(req + status + error);
        }
    });
    //#endregion

    //#region GCP VISUALISATION
    var urlGCP = "/WebAPI/api/GCP/" + id;

    //add graphics layer
    var GCPGraphicsLayer = new GraphicsLayer({
        title: "GCP",
    });
    map.add(GCPGraphicsLayer);

    function displayGCP(gcp) {
        var point = {
            type: "point",
            x: gcp.X,
            y: gcp.Y,
            z: gcp.Z,
            spatialReference: sr
        };

        var simpleMarkerSymbol = {
            type: "simple-marker",
            color: "red",
            size: "7px",
            outline: {
                color: [255, 255, 255], // white
                width: 1
            }
        };

        var pointGraphic = new Graphic({
            geometry: point,
            symbol: simpleMarkerSymbol
        });

        GCPGraphicsLayer.add(pointGraphic);
    }
    $.ajax({
        type: "GET",
        url: urlGCP, // the URL of the controller action method
        data: null, // optional data
        success: function (result) {
            console.log("AJAX: SUCCESS");
            result.forEach(displayGCP);
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
