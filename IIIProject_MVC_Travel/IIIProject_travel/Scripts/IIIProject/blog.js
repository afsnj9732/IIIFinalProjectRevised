;(function(){
  $(document).ready(function () {
    $("#s1").mouseenter(function () {
      $(".fa-star").css("color", "black");
      $("#s1").css("color", "orange");
    });
    $("#s2").mouseenter(function () {
      $(".fa-star").css("color", "black");
      $("#s1,#s2").css("color", "orange");
    });
    $("#s3").mouseenter(function () {
      $(".fa-star").css("color", "black");
      $("#s1,#s2,#s3").css("color", "orange");
    });
    $("#s4").mouseenter(function () {
      $(".fa-star").css("color", "black");
      $("#s1,#s2,#s3,#s4").css("color", "orange");
    });
    $("#s5").mouseenter(function () {
      $(".fa-star").css("color", "black");
      $("#s1,#s2,#s3,#s4,#s5").css("color", "orange");
    });
    $('.ui.radio.checkbox')
      .checkbox();
  });
})();


(function(){
  $('#file').change(function() {
    var file = $('#file')[0].files[0];
    var reader = new FileReader;
    reader.onload = function(e) {
      $('#demo').attr('src', e.target.result);
    };
    reader.readAsDataURL(file);
  });
})();


$(function () {

    $("body").on("mouseenter", ".a-ellipsis", function () {
        if (!this.title) this.title = $(this).text();
    });

  

//    $("#btnComment").click(function () {

//        var comment = this.querySelector("txtComment").innerHTML

//        $.ajax({
//            url: "/Blog/Comment",
//            type: "POST",
//            data: {

//                "comment" : comment
//            },
//            dataType: "json",
//            async: false,
//            cache: true,
//            success: function (data) {
//                //如果有撈到資料
//                if (data.length !== 0) {
//                    //console.log(data);
//                    $('#mAchieve').text(data[0].mAchieve);
//                    $('#mAvatar').html('<img src="/Content/images/' + data[0].mAvatar + '" id="mAvatar" class="col-auto ArticlePic">');
//                    $('#mName').text('使用者名稱:' + data[0].mName);
//                    $('#mRating').text('使用者評分:' + data[0].mRating);
//                    //$('#mTimeleft').text(''); //controller待修改
//                    $('.content').text(data[0].content);
//                    $('#share').attr('hidden', false);
//                }
//                //沒撈到資料 (還可調整)
//                else {
//                    $('#mAchieve').text('');
//                    $('#mAvatar').html('');
//                    $('#mName').text('');
//                    $('#mRating').text('');
//                    $('#mTimeleft').text('');
//                    $('.content').text('找不到符合條件的結果');
//                    $('#share').attr('hidden', true);
//                }
//            },
//            error: function (xhr, status, error) {
//                console.log(error);
//            }



//    });
//});


