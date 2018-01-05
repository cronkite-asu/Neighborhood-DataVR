# DataVR
Unity asset for visualizing geo spatial data in Virtual Reality.

## Components Involved:

- MapBox Unity SDK 1.2.0
- GoogleVR sdk 1.70.0


## API configuration:

This unity asset required API keys from Mapbox for rendering map and Google's geolocation API for geocoding address to latitude and longitude.

##### API Key can be obtained from here

Mapbox - https://www.mapbox.com/install/unity/

Google GeoLocation API Key -  https://developers.google.com/maps/documentation/geolocation

## Getting Started:
Once the API keys are obtained, they can be configured in the asset.

Open MapScene from `Assets->Scenes->MapScene`

Click on the GameObject named 'Configuration' and in the Inspector panel under Scripts sections, you will find the text box named 'Google_Maps_API_KEY' and provide the Google Geo Location API Key here.

For adding mapbox API key, Choose `Mapbox->Configure` and provide the mapbox API key in the access token field and save it.

Geolocation data (preferably in .csv file) should be placed in the `Assets->StreamingAssets` folder of the project and this data will be packaged along with the application.
 
Name of the file should be given in the previous configuration game object.

Also, configure the initial location in the configuration game object for starting.


#### For changing markers displayed:

1. For changing the default marker, provide the suitable prefab in the Default Marker field of the Configuration game object.

3. For customizing different game objects for different types of markers
- First, provide the number of the different objects in the 'Size' field of the Configuration game object.
- Second, provide the name of the type and its corresponding prefab object that should be generated for this particular game object. If a particular type of marker is not defined here, then it will take the default game object. 


`Important: So it is advisable to never leave default marker field empty
`
