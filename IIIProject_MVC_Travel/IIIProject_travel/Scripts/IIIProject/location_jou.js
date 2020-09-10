(function () {
    var mymap;
    var theMarker = {};
    var evtLat = $('#evtLat').val();
    var evtLng = $('#evtLng').val();

    console.log('lat out ' + evtLat);
    console.log('lng out ' + evtLng);

    //如果已經有畫地圖就刪掉重畫
    if (mymap !== undefined) {
        mymap.remove();
    }

    //畫出地圖
    mymap = L.map('mapid').setView([24.9, 121.3], 9);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: '&copy; Jouta',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiZGV5byIsImEiOiJja2VqY3JheTQyM3JsMndzNDBsODF2b214In0.1f7T-kY2Uz1F5FZ_WsLpaA'
    }).addTo(mymap);

    //點擊顯示icon
    mymap.on('click', function (e) {
        lat = e.latlng.lat;
        lng = e.latlng.lng;
        if (theMarker !== undefined) {
            mymap.removeLayer(theMarker);
        }
        theMarker = L.marker([lat, lng]).addTo(mymap);
        //console.log("Lat, Lon : " + lat + ", " + lng);
        evtLat = lat;
        evtLng = lng;
        console.log('lat in ' + evtLat);
        console.log('lng in ' + evtLng);
    });
})();
