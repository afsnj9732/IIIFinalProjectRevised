var x = document.querySelector(".btnLocation");

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerHTML = "此瀏覽器不支援定位!";
    }
}

function showPosition(position) {
    // x.innerHTML = "Latitude: " + position.coords.latitude +
    //     "<br>Longitude: " + position.coords.longitude;

    console.log("Latitude: " + position.coords.latitude +
        "\r\nLongitude: " + position.coords.longitude);

    x.innerHTML = "LatLng(" + (position.coords.latitude).toFixed(6) + " , " + (position.coords.longitude).toFixed(6) + ")";

}


var mymap = L.map('mapid').setView([25.0335601, 121.542634], 15);

L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
    attribution: '&copy; Jouta',
    maxZoom: 18,
    id: 'mapbox/streets-v11',
    tileSize: 512,
    zoomOffset: -1,
    accessToken: 'pk.eyJ1IjoiZGV5byIsImEiOiJja2VqY3JheTQyM3JsMndzNDBsODF2b214In0.1f7T-kY2Uz1F5FZ_WsLpaA'
}).addTo(mymap);

var popup = L.popup();



function onMapClick(e) {
    popup
        .setLatLng(e.latlng)
        .setContent("以此座標計算"/* + e.latlng.toString()*/)
        .openOn(mymap);
    
    x.innerHTML = e.latlng;

    //console.log(e.latlng.lat);
    //console.log(e.latlng.lng);
}

mymap.on('click', onMapClick);