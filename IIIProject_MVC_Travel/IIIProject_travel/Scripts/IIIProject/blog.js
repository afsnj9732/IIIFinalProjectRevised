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