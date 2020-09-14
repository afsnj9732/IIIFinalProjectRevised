(function () {
    var mymap;
    var theMarker = {};
    var getMapNumber;
    var lat, lng; 

function callmap() {
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
    }

    //如果已經有畫地圖就刪掉重畫
    $("body").on("click", ".btn_get_map", function () {
        getMapNumber = $(this).attr("getMapNumber");
        if (mymap !== undefined) {
            mymap.remove();
        }
        callmap();
        getmarker();
    });

    function getmarker() {
    //點擊顯示marker
    mymap.on('click', function (e) {
        lat = e.latlng.lat;
        lng = e.latlng.lng;

        //如果地圖上已經有marker就刪掉重畫
        if (theMarker !== undefined) {
            mymap.removeLayer(theMarker);
            //evtLat = null;
            //evtLng = null;
        }
        theMarker = L.marker([lat, lng]).addTo(mymap);
        //console.log("Lat, Lon : " + lat + ", " + lng);        
        console.log($('.evtLng').eq(getMapNumber).val());
    });
    }

    $("body").on("click", ".getMapValue", function () {
        $('.evtLat').eq(getMapNumber).val(lat);
        $('.evtLng').eq(getMapNumber).val(lng);
        $('.btn_get_map_msg').eq(getMapNumber).html("設定成功");
        //window.confirm("地點設定成功!");
    });

    callmap();
    getmarker();

})();
