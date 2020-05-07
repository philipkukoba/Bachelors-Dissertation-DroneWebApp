require([
    "esri/Map",
    "esri/views/MapView",
    "esri/geometry/SpatialReference",
    "esri/widgets/Search",
    "esri/PopupTemplate",
    "esri/layers/FeatureLayer"
], function (Map, MapView, SpatialReference, Search, PopupTemplate, FeatureLayer) {

    //#region Basic setup: Map and View

    // Create the map
    let map = new Map({
        basemap: "topo-vector"
    });

    // Create the View (MapView)
    let view = new MapView({
        container: "droneFlightMap",
        map: map,
        center: [4.3, 50.9],
        zoom: 9
    });

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

    //#region get all droneflights 

    const popup = {
        "title": "Drone Flight Information",
        "content": [{
            "type": "fields",
            "fieldInfos": [
                {
                    "fieldName": "PilotName", //result.PilotName,   
                    "label": "Pilot Name",

                },
                {
                    "fieldName": "DroneName",
                    "label": "Drone Name",

                },
                {
                    "fieldName": "DepartureUTC",
                    "label": "Departure Time (UTC)",

                },
                {
                    "fieldName": "DepartureLatitude",
                    "label": "Departure Latitude",

                },
                {
                    "fieldName": "DepartureLongitude",
                    "label": "Departure Longitude",

                },
                {
                    "fieldName": "DestinationUTC",
                    "label": "Destination Time (UTC)",

                },
                {
                    "fieldName": "DestinationLatitude",
                    "label": "Destination Latitude",

                },
                {
                    "fieldName": "DestinationLongitude",
                    "label": "Destination Longitude",
                }
            ]
        }]
    };

    let readFlightpoint = (fp) => {
        let pointGraphic = {             //type graphic (autocasts)
            geometry: {
                type: "point",
                x: fp.DepartureLongitude,
                y: fp.DepartureLatitude
            },
            attributes: {
                FlightId: fp.FlightId,
                PilotName: fp.PilotName,
                DroneName: fp.DroneName,
                DepartureUTC: fp.DepartureUTC,
                DepartureLatitude: fp.DepartureLatitude,
                DepartureLongitude: fp.DepartureLongitude,
                DestinationUTC: fp.DestinationUTC,
                DestinationLatitude: fp.DestinationLatitude,
                DestinationLongitude: fp.DestinationLongitude
            }
        };
        console.log(pointGraphic);
        return pointGraphic;
    }

    $.ajax({
        type: "GET",
        url: "/WebAPI/api/DroneFlightsAPI/",
        success: (result) => {

            let flightpoints = [];
            for (let i = 0; i < result.length; i++) {
                flightpoints.push(readFlightpoint(result[i]));
            }

            flightsFeatureLayer = new FeatureLayer({
                source: flightpoints,                                  
                fields: [
                    {
                        name: "FlightId",
                        type: "integer"
                    },
                    {
                        name: "PilotName",
                        type: "string"
                    },
                    {
                        name: "DroneName",
                        type: "string"
                    },
                    {
                        name: "DepartureUTC",
                        type: "string"
                    },
                    {
                        name: "DepartureLatitude",
                        type: "double"
                    },
                    {
                        name: "DepartureLongitude",
                        type: "double"
                    },
                    {
                        name: "DestinationUTC",
                        type: "string"
                    },
                    {
                        name: "DestinationLatitude",
                        type: "double"
                    },
                    {
                        name: "DestinationLongitude",
                        type: "double"
                    }
                ],
                objectIdField: "FlightId",   
                geometryType: "point",                      
                popupTemplate: popup,
                renderer: {
                    type: "simple",  // autocasts as new SimpleRenderer()
                    symbol: {
                        type: "simple-marker",
                        size: 25,
                        color: [26, 102, 255], // light blue
                        outline: {
                            color: [255, 255, 255], // white
                            width: 1
                        }
                    }
                }
            });

            console.log(flightsFeatureLayer);

            map.add(flightsFeatureLayer);

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