﻿require([
    "esri/Map",
    "esri/views/MapView",
    "esri/Graphic",
    "esri/layers/GraphicsLayer",
    "esri/geometry/SpatialReference",
    "esri/widgets/LayerList",
    "esri/widgets/Search",
    "esri/widgets/Legend",
    "esri/PopupTemplate",
    "esri/layers/FeatureLayer",
    "esri/widgets/AreaMeasurement2D",
    "esri/widgets/Feature"
], function (Map, MapView, Graphic, GraphicsLayer, SpatialReference, LayerList, Search, Legend, PopupTemplate, FeatureLayer, AreaMeasurement2D, Feature) {

    let trackFeatureLayer; // needs to be declared here so we can switch visual variable with key 


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

    //#region LAYERLIST
    let layerList = new LayerList({
        view: view
    });

    // Adds widget below other elements in the top right corner of the view
    view.ui.add(layerList, "top-right");
    //#endregion 

    //#region LEGEND Widget
    let legend = new Legend({
        view: view,
        style: "classic", // other styles include 'card'
        label: {
            title: "DEFAULT TITLE"
        }
    });
    view.ui.add(legend, "top-right");
    //#endregion

    //#region Search widget
    const searchWidget = new Search({
        view: view
    });
    // Adds the search widget below other elements in
    // the top left corner of the view
    view.ui.add(searchWidget, {
        position: "bottom-right",
        //index: 2
    });
    //#endregion

    // #region Coordinate Widget
    // Create a coordinate widget
    let coordsWidget = document.createElement("div");
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

    //#region Area Measurement Widget
    // To add the AreaMeasurement2D widget to your map
    let measurementWidget = new AreaMeasurement2D({
        view: view
    });
    view.ui.add(measurementWidget, "bottom-left");
    //#endregion

    //#region Visualisation Const Values 
    //ID van de droneflight 
    const id = $("#viewDiv").data("id");

    //Lambert spacial reference (needed for all featurelayers except track visualisation)
    const LambertSR = { wkid: 31370 };

    //#endregion 

    //#region XYZ VISUALISATION


    const XYZPopup = {
        "title": "XYZ Point with ID: {XYZId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X",

                },
                {
                    "fieldName": "y",
                    "label": "Y",

                },
                {
                    "fieldName": "z",
                    "label": "Z",

                },
                {
                    "fieldName": "RedValue",
                    "label": "Red",

                },
                {
                    "fieldName": "GreenValue",
                    "label": "Green",

                },
                {
                    "fieldName": "BlueValue",
                    "label": "Blue",

                }
            ]
        }]
    };

    let readXYZ = (XYZPoint) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: XYZPoint.X,
                y: XYZPoint.Y,
                z: XYZPoint.Z,
                spatialReference: LambertSR   //needs to be defined here
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                XYZId: XYZPoint.PointCloudXYZId,
                x: XYZPoint.X,
                y: XYZPoint.Y,
                z: XYZPoint.Z,
                RedValue: XYZPoint.Red,
                GreenValue: XYZPoint.Green,
                BlueValue: XYZPoint.Blue,
                Intensity: XYZPoint.Intensity
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/PointCloudXYZs/" + id, // the URL of the controller action method
        //data: null, // optional data
        success: (result) => {

            let XYZPoints = [];
            for (let i = 0; i < result.length; i++) {
                XYZPoints.push(readXYZ(result[i]));
            }

            let XYZFeatureLayer = new FeatureLayer({
                title: "XYZ Points",
                source: XYZPoints,

                fields: [{                //repeat the fields for visual variables here
                    name: "x",
                    type: "double"
                },
                {
                    name: "y",
                    type: "double"
                },
                {
                    name: "z",
                    type: "double"
                },
                {
                    name: "RedValue",
                    type: "double"
                },
                {
                    name: "GreenValue",
                    type: "double"
                },
                {
                    name: "BlueValue",
                    type: "double"
                },
                {
                    name: "Intensity",
                    type: "double"
                }],

                objectIdField: "XYZId",                             //needed to uniquely identify each object

                //renderer defines how everything will be visualised inside this layer
                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        color: ["RedValue", "GreenValue", "BlueValue"]
                        //outline: {
                        //    color: [255, 255, 255], // white
                        //    width: 1
                        //}
                    }
                },
                popupTemplate: XYZPopup
            });

            map.add(XYZFeatureLayer);
        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: XYZ");
            console.log(req + status + error);
        }
    });
    //#endregion 

    //#region CTRLPoints VISUALISATION 

    const CTRLTemplate = {
        "title": "Control Point with ID: {CTRLId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X",

                },
                {
                    "fieldName": "y",
                    "label": "Y",

                },
                {
                    "fieldName": "z",
                    "label": "Z",

                }
            ]
        }]
    };

    let readCTRL = (CTRLPoint) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: CTRLPoint.X,
                y: CTRLPoint.Y,
                z: CTRLPoint.Z,
                spatialReference: LambertSR   //needs to be defined here
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                CTRLId: CTRLPoint.CTRLId,
                x: CTRLPoint.X,
                y: CTRLPoint.Y,
                z: CTRLPoint.Z
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/CTRLPoints/" + id, // the URL of the controller action method
        //data: null, // optional data
        success: (result) => {

            let CTRLPoints = [];
            for (let i = 0; i < result.length; i++) {
                CTRLPoints.push(readCTRL(result[i]));
            }

            let CTRLfeatureLayer = new FeatureLayer({
                title: "CTRL Points",
                source: CTRLPoints,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features)
                fields: [{                //repeat the fields for visual variables here
                    name: "x",
                    type: "double"
                },
                {
                    name: "y",
                    type: "double"
                },
                {
                    name: "z",
                    type: "double"
                }],
                objectIdField: "CTRLId",                             //needed to uniquely identify each object

                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        color: [26, 102, 255], // light blue
                        outline: {
                            color: [255, 255, 255], // white
                            width: 1
                        }
                    }
                },
                popupTemplate: CTRLTemplate
            });

            map.add(CTRLfeatureLayer);
        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: CTRL POINTS");
            console.log(req + status + error);
        }
    });
    //#endregion

    //#region GCP VISUALISATION 

    const GCPTemplate = {
        "title": "Ground Control Point with ID: {GCPId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X",

                },
                {
                    "fieldName": "y",
                    "label": "Y",

                },
                {
                    "fieldName": "z",
                    "label": "Z",

                }
            ]
        }]
    };

    let readGCP = (gcp) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: gcp.X,
                y: gcp.Y,
                z: gcp.Z,
                spatialReference: LambertSR   //needs to be defined here??
            },
            attributes: {               //additional attributes (battery etc) will go here 
                GCPId: gcp.GCPId,
                x: gcp.X,
                y: gcp.Y,
                z: gcp.Z
            }
        };
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/GCP/" + id, // the URL of the controller action method
        data: null, // optional data
        success: (result) => {
            let GCPs = [];
            for (let i = 0; i < result.length; i++) {
                GCPs.push(readGCP(result[i]));
            }

            let GCPFeatureLayer = new FeatureLayer({
                title: "Ground Control Points",
                source: GCPs,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features) 
                fields: [{                //repeat the fields for visual variables here
                    name: "x",
                    type: "double"
                },
                {
                    name: "y",
                    type: "double"
                },
                {
                    name: "z",
                    type: "double"
                }],
                objectIdField: "GCPId",                             //needed to uniquely identify each object

                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        color: [0, 102, 0], // dark green
                        outline: {
                            color: [255, 255, 255], // white
                            width: 1
                        }
                    }
                },

                popupTemplate: GCPTemplate

            });

            map.add(GCPFeatureLayer);
        },
        error: (req, status, error) => {
            console.log("AJAX GCP FAIL");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });
    //#endregion

    //#region TRACK VISUALISATION 
    //#region Visual Variable, custom Renderer and popup

    //visual variable HeightMSL
    let colorVar_HeightMSL = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "HeightMSL",     //specify which field to use
        stops: [{ value: 2, color: "#FF0000" }, { value: 8, color: "#0000FF" }]
    };

    //visual variable VelComposite
    let colorVar_VelComposite = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "VelComposite",     //specify which field to use
        stops: [{ value: 0, color: "#436480" }, { value: 7, color: "#afbac4" }, { value: 14, color: "#ebe6df" }]
    };

    //visual variable BatteryPercentage
    let colorVar_BatteryPercentage = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "BatteryPercentage",     //specify which field to use
        stops: [{ value: 0, color: "red" }, { value: 50, color: "yellow" }, { value: 100, color: "green" }]
    };

    //specify visualisation here 
    let customRenderer = {
        type: "simple",                 // autocasts as new SimpleRenderer()
        symbol: { type: "simple-marker", size: 5 }, // autocasts as new SimpleMarkerSymbol()
        visualVariables: [colorVar_HeightMSL]
    };

    const GPTemplate = {
        "title": "Track Point with ID: {GPSId}",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "x",
                    "label": "X (WGS84)",
                },
                {
                    "fieldName": "y",
                    "label": "Y (WGS84)",
                },
                {
                    "fieldName": "HeightMSL",
                    "label": "Height (Mean Sea Level)",
                },
                {
                    "fieldName": "VelComposite",
                    "label": "Velocity (Composite)",
                },
                {
                    "fieldName": "BatteryPercentage",
                    "label": "Battery Percentage",
                }
            ]
        }]
    };

    //#endregion

    let readTrackPoint = (gp) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: gp.Long,
                y: gp.Lat
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                GPSId: gp.GPSId,

                HeightMSL: gp.HeightMSL,
                BatteryPercentage: gp.BatteryPercentage,
                VelComposite: gp.VelComposite,

                x: gp.Long,
                y: gp.Lat
            }
        };
        return pointGraphic;
    }

    //MAIN TRACK VISUALISATION AJAX
    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneGPs/" + id, // the URL of the controller action method
        success: (result) => {

            let trackpoints = [];
            for (let i = 0; i < result.length; i++) {
                trackpoints.push(readTrackPoint(result[i]));
            }

            trackFeatureLayer = new FeatureLayer({
                title: "Track",
                source: trackpoints,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features)
                spatialReference: SpatialReference.WGS84,           // autocasts to wgs84 if not set 
                fields: [{                                          //repeat the fields for visual variables here!!! 
                    name: "HeightMSL",
                    type: "double"
                },
                {                                          //repeat the fields for visual variables here!!! 
                    name: "BatteryPercentage",
                    type: "double"
                },
                {                                          //repeat the fields for visual variables here!!! 
                    name: "VelComposite",
                    type: "double"
                },
                {                                          //repeat the fields for visual variables here!!! 
                    name: "x",
                    type: "double"
                },
                {                                          //repeat the fields for visual variables here!!! 
                    name: "y",
                    type: "double"
                }],
                objectIdField: "GPSId",                             //needed to uniquely identify each object
                renderer: customRenderer,
                popupTemplate: GPTemplate
            });

            map.add(trackFeatureLayer);

        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: TRACK VISUALISATION");
            console.log(req);
            console.log(status);
            console.log(error);
        }
    });

    //#endregion 


    //#region FEATURE (LEGENDE) 

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneFlightsAPI/" + id, // the URL of the controller action method
        success: (result) => {
            let feature = new Feature({
                id: 'infoFeature',
                graphic: new Graphic({
                    attributes: {
                        pilotName: result.PilotName,
                        droneName: result.DroneName,

                        departureUTC: result.DepartureUTC,
                        departureLatitude: result.DepartureLatitude,
                        departureLongitude: result.DepartureLongitude,

                        destinationUTC: result.DestinationUTC,
                        destinationLatitude: result.DestinationLatitude,
                        destinationLongitude: result.DestinationLongitude
                    },
                    popupTemplate: {
                        "title": "Drone Flight Information",
                        "content": [{
                            "type": "fields",
                            "fieldInfos": [
                                {
                                    "fieldName": "pilotName", //result.PilotName,   
                                    "label": "Pilot Name",

                                },
                                {
                                    "fieldName": "droneName",
                                    "label": "Drone Name",

                                },
                                {
                                    "fieldName": "departureUTC",
                                    "label": "Departure Time (UTC)",

                                },
                                {
                                    "fieldName": "departureLatitude",
                                    "label": "Departure Latitude",

                                },
                                {
                                    "fieldName": "departureLongitude",
                                    "label": "Departure Longitude",

                                },
                                {
                                    "fieldName": "destinationUTC",
                                    "label": "Destination Time (UTC)",

                                },
                                {
                                    "fieldName": "destinationLatitude",
                                    "label": "Destination Latitude",

                                },
                                {
                                    "fieldName": "destinationLongitude",
                                    "label": "Destination Longitude",
                                }
                            ]
                        }]
                    }
                }),
                map: map
            });

            view.ui.add(feature, "top-left");
        },
        error: (req, status, error) => {
            console.log("AJAX FAIL: FEATURE (LEGENDE)");
        }
    });
    //#endregion 

    //#region KEY LISTENER 
    let featureHidden = false;
    view.on("key-down", (event) => {
        if (event.key == "b") {  //battery
            customRenderer.visualVariables = [colorVar_BatteryPercentage];
            trackFeatureLayer.renderer = customRenderer;
        }
        else if (event.key == "h") {  //heightmsl
            customRenderer.visualVariables = [colorVar_HeightMSL];
            trackFeatureLayer.renderer = customRenderer;
        }
        else if (event.key == "v") {  //velocity
            customRenderer.visualVariables = [colorVar_VelComposite];
            trackFeatureLayer.renderer = customRenderer;
        }
        else if (event.key == "l") {  //toggle visibility of feature component
            if (!featureHidden) {
                document.querySelector('.esri-component.esri-feature.esri-widget').style.display = 'none';
                featureHidden = true;
            }
            else {
                document.querySelector('.esri-component.esri-feature.esri-widget').style.display = 'block';
                featureHidden = false;
            }

        }
    });
    //#endregion 

    //#region POINT CLOUD CONTROL TOOL 
        // .. 
    //#endregion 

});


