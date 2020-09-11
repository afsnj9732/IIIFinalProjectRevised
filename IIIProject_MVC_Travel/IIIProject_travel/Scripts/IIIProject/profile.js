//  此處不可用立即函式，會無法讀取
    $(".user").click(function(){
        $(this).addClass("active").siblings().removeClass("active");
    });
    const tabBtn = document.querySelectorAll(".user");
    const tab = document.querySelectorAll(".tab_info");

    function tabs(panelIndex){
        tab.forEach(function (node) {
            node.style.display ="none";
        });
        tab[panelIndex].style.display = "block";
        //tabNum = panelIndex;
        //return panelIndex;
    }
    tabs(0);

$("#p1").click(function () {
    console.log(this.id);
    $.ajax({
        url: "/Profile/otherprofile",
        type: "POST",
        data: {
            "tabId": this.id
        },
        dataType: "json",
        cache: true,
        success: function (data) {
            if (data) {
                $("#post_img").html("<img class='card-img-top' src='../../Content/images/" + data[0].memberImg + "' alt='@t.f會員編號'>");
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});