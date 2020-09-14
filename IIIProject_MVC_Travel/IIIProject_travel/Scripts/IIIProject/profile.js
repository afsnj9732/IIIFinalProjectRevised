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
    }
    tabs(0);
