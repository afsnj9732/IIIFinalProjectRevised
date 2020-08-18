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
        tab[panelIndex].style.display="block";
    }
    tabs(0);

    let bio = document.querySelector(".bio");

    function bioText(){
        bio.oldText = bio.innerText;
        bio.innerText = bio.innerText.substring(0,100)+"...";
        bio.innerHTML += "&nbsp;" + '<span onclick="addLength()" id="see-more-bio">See More</span>';
    }
    bioText();

    function addLength(){
        bio.innerHTML = bio.oldText;
        bio.innerHTML += "&nbsp;" + '<span onclick="bioText()" id="see-less-bio">See More</span>';
    }
   