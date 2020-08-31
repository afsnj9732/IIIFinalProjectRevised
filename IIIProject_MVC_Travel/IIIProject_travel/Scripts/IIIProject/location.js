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
}