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
    "esri/layers/FeatureLayer",
    "esri/widgets/AreaMeasurement2D",
    "esri/widgets/Feature"
], function (Map, MapView, Graphic, GraphicsLayer, SpatialReference, LayerList, Search, Legend, PopupTemplate, FeatureLayer, AreaMeasurement2D, Feature) {

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

                    },
                    {
                        "fieldName": "Inside",
                        "label": "Inside pointcloud"
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
                    z: CTRLPoint.Z,
                    inside: CTRLPoint.Inside
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
                    },
                    {
                        name: "inside",
                        type: "string"
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
});