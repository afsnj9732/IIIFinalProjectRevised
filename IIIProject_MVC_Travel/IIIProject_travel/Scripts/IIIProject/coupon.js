//var lat = null;
//var lng = null;
//var loc1 = { name: '台北101', lat: 25.034, lng: 121.562 };
//var loc2 = { name: '京華城', lat: 25.048, lng: 121.561 };
//var loc3 = { name: '資策會', lat: 25.034, lng: 121.543 };
//var loc4 = { name: '台北新光三越', lat: 25.046, lng: 121.515 };
//var loc5 = { name: '台北小巨蛋', lat: 25.051, lng: 121.549 };

$('.randCoupon').click(function () {
    var time = ['中午12點', '下午1點', '下午2點', '下午3點', '下午4點', '下午5點', '晚上6點', '晚上7點', '晚上8點'];
    var discount = ['全品項五折', '全品項六折', '全品項七折', '全品項八折', '全品項九折', '購物金20元', '購物金50元', '購物金100元', '購物金200元', '購物金500元', '購物金1000元', '購物金2000元', '購物金5000元'];
    //location = [
    //    { name: '台北101', lat: 25.034, lng: 121.562 },
    //    { name: '京華城', lat: 25.048, lng: 121.561 },
    //    { name: '資策會', lat: 25.034, lng: 121.543 },
    //    { name: '台北新光三越', lat: 25.046, lng: 121.515 },
    //    { name: '台北小巨蛋', lat: 25.051, lng: 121.549 }
    //];
    var location = ['台北101', '京華城', '資策會', '台北新光三越', '台北小巨蛋'];
    var store = ['星巴克', '家樂福', '王品', '陶板屋', '頤宮', 'COSTCO', 'IKEA', '新光三越百貨'];

    const randTime = Math.floor(Math.random() * time.length);
    const randDiscount = Math.floor(Math.random() * discount.length);
    const randLocation = Math.floor(Math.random() * location.length);
    const randStore = Math.floor(Math.random() * store.length);
    //console.log(time[randTime],discount[randDiscount],location[randLocation],store[randStore]);

    $('#content').text(time[randTime] + '在' + location[randLocation] + '方圓三公里範圍內，送' + store[randStore] + discount[randDiscount] + '優惠券，凡定位於此處者皆有機會獲得~!');

    $.ajax({
        url: "/優惠發送/List",
        type: "POST",
        data: {
            "location":location
        },
        dataType: "json",
        async: false,
        cache: true,
        success: function (data) {
            //如果有撈到資料
            if (data.length !== 0) {
                console.log(data);
                $('#mAchieve').text(data[0].mAchieve);
                $('#mAvatar').html('<img src="/Content/images/' + data[0].mAvatar + '" id="mAvatar" class="col-auto ArticlePic">');
                $('#mName').text('使用者名稱:' + data[0].mName);
                $('#mRating').text('使用者評分:' + data[0].mRating);
                //$('#mTimeleft').text(''); //controller待修改
                $('.content').text(data[0].content);
                $('#share').attr('hidden', false);
            }
            //沒撈到資料 (還可調整)
            else {
                $('#mAchieve').text('');
                $('#mAvatar').html('');
                $('#mName').text('');
                $('#mRating').text('');
                $('#mTimeleft').text('');
                $('.content').text('找不到符合條件的結果');
                $('#share').attr('hidden', true);
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});

//$('.sendCoupon').click(function () {
    //$.ajax({
    //    url: "/優惠發送/List",
    //    type: "POST",
    //    data: {
    //        "Lat": currentLat,
    //        "curLng": currentLng,
    //        "tabNum": tabNum
    //    },
    //    dataType: "json",
    //    async: false,
    //    cache: true,
    //    success: function (data) {
    //        //如果有撈到資料
    //        if (data.length !== 0) {
    //            console.log(data);
    //            $('#mAchieve').text(data[0].mAchieve);
    //            $('#mAvatar').html('<img src="/Content/images/' + data[0].mAvatar + '" id="mAvatar" class="col-auto ArticlePic">');
    //            $('#mName').text('使用者名稱:' + data[0].mName);
    //            $('#mRating').text('使用者評分:' + data[0].mRating);
    //            //$('#mTimeleft').text(''); //controller待修改
    //            $('.content').text(data[0].content);
    //            $('#share').attr('hidden', false);
    //        }
    //        //沒撈到資料 (還可調整)
    //        else {
    //            $('#mAchieve').text('');
    //            $('#mAvatar').html('');
    //            $('#mName').text('');
    //            $('#mRating').text('');
    //            $('#mTimeleft').text('');
    //            $('.content').text('找不到符合條件的結果');
    //            $('#share').attr('hidden', true);
    //        }
    //    },
    //    error: function (xhr, status, error) {
    //        console.log(error);
    //    }
    //});
//});