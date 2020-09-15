var x = document.querySelector(".btnLocation");
var tabBtn = document.querySelectorAll(".resultTrigger");
var mymap;
var tabNum;
var currentLat = null;
var currentLng = null;

//console.log("var: " + currentLat);

//取得定位授權
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

    //如果已經有畫地圖就刪掉重畫
    if (mymap !== undefined) {
        mymap.remove();
    }



    //畫出地圖
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

//demo if needed
$('.demoLocation').click(function () {
    currentLat = 25.034012;
    currentLng = 121.543333;
    x.innerHTML = "座標 (" + 25.034 + " , " + 121.543 + ")"

    if (mymap !== undefined) {
        mymap.remove();
    }

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
});

//翻牌給tab
function tabs(panelIndex) {
    tabNum = panelIndex;
    return panelIndex;
}
tabs(0);

//click翻牌
$('.resultTrigger').click(function () {

    //console.log('pi: ' + tabNum);
    //console.log("lat: " + currentLat);
    //console.log(typeof (currentLat));

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
            //如果有撈到資料
            if (data.length !== 0) {
                $('section').removeClass('active');
                console.log(data);
                $('.secY').addClass('active');

                $('.mImg').attr('src', '../Content/images/' + data[0].mImg);
                $('.mContent').text(data[0].mContent);
                $('.mSort').text(data[0].mSort);
                $('.mPlace').text(data[0].mPlace);
                $('.mEstimate').text(data[0].mEstimate);
                $('.mView').text(data[0].mView);
                $('.mLike').text(data[0].mLike);
                $('.mTitle').text(data[0].mTitle);
                $('.mName').text(data[0].mName);
                $('.mDeadline').text(data[0].mDeadline);
            }
            //沒撈到資料 (還可調整)
            else {
                $('section').removeClass('active');
                $('.secN').addClass('active');
            }
            $('html,body').animate({ scrollTop: $(document).height() }, 1000);

        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});