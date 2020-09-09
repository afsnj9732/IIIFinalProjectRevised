$('.randCoupon').click(function () {
    var time = ['中午12點', '下午1點', '下午2點', '下午3點', '下午4點', '下午5點', '晚上6點', '晚上7點', '晚上8點'];
    var discount = ['單品五折', '單品六折', '單品七折', '單品八折', '單品九折', '折價券200元', '折價券500元', '禮券1000元', '禮券2000元', '禮券5000元'];
    //location = [
    //    { name: '台北101', lat: 25.034, lng: 121.562 },
    //    { name: '京華城', lat: 25.048, lng: 121.561 },
    //    { name: '資策會', lat: 25.034, lng: 121.543 },
    //    { name: '台北新光三越', lat: 25.046, lng: 121.515 },
    //    { name: '台北小巨蛋', lat: 25.051, lng: 121.549 }
    //];
    var location = ['台北101', '京華城', '資策會', '台北新光三越', '台北小巨蛋'];
    var store = ['星巴克', '家樂福', '王品', '陶板屋', 'COSTCO', 'IKEA', '新光三越百貨'];

    const randTime = Math.floor(Math.random() * time.length);
    const randDiscount = Math.floor(Math.random() * discount.length);
    const randLocation = Math.floor(Math.random() * location.length);
    const randStore = Math.floor(Math.random() * store.length);
    //console.log(time[randTime],discount[randDiscount],location[randLocation],store[randStore]);

    $('#content').text('優惠資訊：' + time[randTime] + '在' + location[randLocation] + '方圓三公里範圍內，送' + store[randStore] + discount[randDiscount] + '優惠，收到通知的會員趕快去檢查簡訊領取吧~!');
    $('#sendBtn').html(`<button type="button" class="btn btn-primary ml-3 sendBtn" onclick="sendCoupon()">送出優惠券</button>`);

    $.ajax({
        url: "/優惠發送/List",
        type: "POST",
        data: {
            "randLocation": randLocation
        },
        dataType: "json",
        async: false,
        cache: true,
        success: function (data) {
            //如果有撈到資料
            if (data) {
                $('#tbody').html('<tr><td id="member">' + data[0].mMemberNum + '</td><br>' +
                    '<td>' + data[0].mAccount + '</td><br>' +
                    '<td>' + data[0].mName + '</td><br>' +
                    '<td>' + data[0].mRating + '</td><br>' +
                    '<tr><td id="member">' + data[1].mMemberNum + '</td><br>' +
                    '<td>' + data[1].mAccount + '</td><br>' +
                    '<td>' + data[1].mName + '</td><br>' +
                    '<td>' + data[1].mRating + '</td><br>' +
                    '<tr><td id="member">' + data[2].mMemberNum + '</td><br>' +
                    '<td>' + data[2].mAccount + '</td><br>' +
                    '<td>' + data[2].mName + '</td><br>' +
                    '<td>' + data[2].mRating + '</td><br>'
                );
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});


//發送優惠券
var targetMember;

function sendCoupon() {
    for (i = 0; i < 3; i++) {
        targetMember = document.querySelectorAll('#member')[i].innerHTML;
        console.log('in '+targetMember);
    }

    console.log('out ' + targetMember);

    $('.sendBtn').attr('disabled', '');
    $('.sendBtn').attr('onclick', '');

    if (!('Notification' in window)) {
        console.log('本瀏覽器不支援推播通知');
    }

    if ("Notification" in window) {
        let ask = Notification.requestPermission();
        ask.then(permission => {
            if (permission === "granted") {
                let msg = new Notification("發送成功", {
                    body: "已將優惠資訊成功發送給所有中獎會員!",
                    icon: "../Content/images/joutalogo.png"
                });
                msg.addEventListener("click", event => {
                    alert("點擊接受");
                });
            }
        });
    }
}