var x = document.querySelector(".btnLocation");
var tabNum;
var currentLat = null;
var currentLng = null;
var tabBtn = document.querySelectorAll(".resultTrigger");

console.log("var: " + currentLat);

//取得定位授權 2
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerHTML = "此瀏覽器不支援定位!";
    }
}

function showPosition(position) {
    currentLat = position.coords.latitude;
    currentLng = position.coords.longitude;
    x.innerHTML = "座標 (" + currentLat.toFixed(3) + " , " + currentLng.toFixed(3) + ")";

    mymap = L.map('mapid').setView([currentLat, currentLng], 15);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: '&copy; Jouta',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiZGV5byIsImEiOiJja2VqY3JheTQyM3JsMndzNDBsODF2b214In0.1f7T-kY2Uz1F5FZ_WsLpaA'
    }).addTo(mymap);

    //畫出marker和圓圈範圍
    var marker = L.marker([currentLat, currentLng]).addTo(mymap);
    var circle = L.circle([currentLat, currentLng], {
        color: '#0080ff',
        fillColor: '#c4e1ff',
        fillOpacity: 0.5,
        radius: 1500
    }).addTo(mymap);
    marker.bindPopup("你在這裡!").openPopup();
}

//翻牌給tab
function tabs(panelIndex) {
    tabNum = panelIndex;
    return panelIndex;
}
tabs(0);

$('.resultTrigger').click(function () {

    console.log('pi: ' + tabNum);
    console.log("lat: " + currentLat);

    $.ajax({
        url: "/Home/QuickMatch",
        type: "POST",
        data: {
            "curLat": currentLat,
            "curLng": currentLng,
            "tabNum": tabNum
        },
        dataType: "json",
        async: false,
        cache: true,
        success: function (data) {
            //alert(data);
            console.log(data);
            console.log(z);
            //JSON Data
            //$('#mName').text(data);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});












// old shit


//取得使用者當前座標 3
//function showPosition(position) {
//    currentLat = position.coords.latitude;
//    currentLng = position.coords.longitude;
//    console.log("showPosition  " + currentLat);
//    x.innerHTML = "座標 (" + currentLat.toFixed(3) + " , " + currentLng.toFixed(3) + ")";
//}

//按下btnLocation觸發繪圖 1
//$('.btnLocation').click(function () {
//    console.log("('.btnLocation').click1   " + currentLat);
//    getLocation();
//    console.log("('.btnLocation').click2   " + currentLat);
//    drawMap(currentLat, currentLng);
//});

//繪製地圖
//function drawMap(currentLat, currentLng) {
//    //根據目前座標畫出地圖
//    var mymap = L.map('mapid').setView([currentLat, currentLng], 15);
//    console.log("drapm " + currentLat);

//    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
//        attribution: '&copy; Jouta',
//        maxZoom: 18,
//        id: 'mapbox/streets-v11',
//        tileSize: 512,
//        zoomOffset: -1,
//        accessToken: 'pk.eyJ1IjoiZGV5byIsImEiOiJja2VqY3JheTQyM3JsMndzNDBsODF2b214In0.1f7T-kY2Uz1F5FZ_WsLpaA'
//    }).addTo(mymap);

//    //畫出marker和圓圈範圍
//    var marker = L.marker([currentLat, currentLng]).addTo(mymap);
//    var circle = L.circle([currentLat, currentLng], {
//        color: '#0080ff',
//        fillColor: '#c4e1ff',
//        fillOpacity: 0.5,
//        radius: 1500
//    }).addTo(mymap);
//    marker.bindPopup("你在這裡!").openPopup();
//}

//變json格式
//var theLocation = {
//    curLat: currentLat,
//    curLng: currentLng
//};
//var jsonLocation = JSON.stringify(theLocation);

//console.log("jsonlocaion " + jsonLocation);

//抽活動按鈕
//$('.resultTrigger').click(function () {
//    $.ajax({
//        type: "GET",
//        data: {
//            "currentLat": currentLat,
//            "currentLng": currentLng
//        },
//        dataType: "json",
//        url: "/Home/QuickMatch_Api",
//        async: false,
//        cache: true,
//        success: function () {
//            alert("success!");
//        },
//        error: function (xhr, status, error) {
//            console.log(error);
//        }
//    });
//});





