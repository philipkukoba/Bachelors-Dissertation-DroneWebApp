require([
    "esri/Map",
    "esri/views/MapView",
    "esri/Graphic",
    "esri/layers/GraphicsLayer",
    "esri/geometry/SpatialReference",
    "esri/widgets/LayerList",
    "esri/widgets/Search",
    "esri/widgets/Legend",
    "esri/PopupTemplate",
    "esri/layers/FeatureLayer"
], function (Map, MapView, Graphic, GraphicsLayer, SpatialReference, LayerList, Search, Legend, PopupTemplate, FeatureLayer) {

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
    view.ui.add(layerList, "bottom-left");

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

    //#region Visualisation Const Values 
    //ID van de droneflight 
    const id = $("#viewDiv").data("id");

    //Lambert spacial reference (needed for all featurelayers except track visualisation)
    const LambertSR = { wkid: 31370 };

    //#endregion 

    //#region XYZ VISUALISATION


    //popup for XYZ
    //werkt niet want dit is enkel voor featurelayers
    //var popupXYZ = new PopupTemplate({
    //    "title": "XYZ Information",
    //    "content": "SAMPLE CONTENT"
    //});

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

                fields: [{                                        
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
                    }],

                objectIdField: "XYZId",                             //needed to uniquely identify each object

                //renderer defines how everything will be visualised inside this layer
                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        color: ["RedValue", "GreenValue", "BlueValue"], 
                        outline: {
                            color: [255, 255, 255], // white
                            width: 1
                        }
                    }
                }
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
                CTRLId: CTRLPoint.CTRLId
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
                CTRLPoints.push(readGCP(result[i]));
            }

            let CTRLfeatureLayer = new FeatureLayer({
                title: "CTRL Points",
                source: CTRLPoints,                                   //THIS needs to be set        (autocast as a Collection of new Graphic())
                geometryType: "point",                              //normaal niet nodig (kan hij afleiden uit features)
                //spatialReference: LambertSR,           // autocasts to wgs84 if not set 
                /*fields: [{                                          //repeat the fields for visual variables here!!! 
                    name: "HeightMSL",
                    type: "double"
                }], */
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
                }
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
    let readGCP = (gcp) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: gcp.X,
                y: gcp.Y,
                z: gcp.Z,
                spatialReference: LambertSR   //needs to be defined here??
            },
            attributes: {               //additional attributes (battery etc) will go here !!!
                GCPId: gcp.GCPId
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
                //spatialReference: LambertSR,           // autocasts to wgs84 if not set 
                /*fields: [{                                          //repeat the fields for visual variables here!!! 
                    name: "HeightMSL",
                    type: "double"
                }], */
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
                }
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
    //#region Visual Variable and custom Renderer

    //visual variable
    var colorVisVar = {
        type: "color",          //specify that its based on color (not size or rotation etc)
        field: "HeightMSL",     //specify which field to use
        stops: [{ value: 0.0, color: "#FF0000" }, { value: 55.0, color: "#0000FF" }]
    };

    //specify visualisation here 
    let customRenderer = {
        type: "simple",                 // autocasts as new SimpleRenderer()
        symbol: { type: "simple-marker", size: 5 }, // autocasts as new SimpleMarkerSymbol()
        visualVariables: [colorVisVar]
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
                HeightMSL: gp.HeightMSL
            }
        };
        return pointGraphic;
    }

    //MAIN TRACK VISUALISATION AJAX
    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneGPs/" + id, // the URL of the controller action method
        success: (result) => {

            let features = [];
            for (let i = 0; i < result.length; i++) {
                features.push(readTrackPoint(result[i]));
            }

            let trackFeatureLayer = new FeatureLayer({
                title: "Track",
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
});


