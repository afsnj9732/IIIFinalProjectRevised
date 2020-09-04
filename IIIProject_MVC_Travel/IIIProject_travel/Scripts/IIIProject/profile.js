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




//$(document).ready(function () {

//    // get yourid parameter
//    var contractId = '@Model.Item2.ContractID';
//    var params = { id: contractId };

//    // just change these based on the tab....
//    var url = '';
//    var target = '';

//    // capture the on tab click and see which tab you are showing
//    $('a[data-toggle="tab"]').on('shown.bs.tab',
//        function (e) {
//            var tabId = $(e.target).attr("href");
//            switch (tabId) {
//                case "#myProfile":
//                    var url = '/Profile/ProfileIndex';
//                    var target = 'oneContent';
//                    break;
//                case "#myPost":
//                    var url = '/Profile/myPost/';
//                    var target = 'twoContent';
//                    break;
//                case "#myActivity":
//                    var url = '/Profile/index/';
//                    var target = 'threeContent';
//                    break;
//                case "#myFollow":
//                    var url = '/invoices/index/';
//                    var target = 'fourContent';
//                    break;
//            }
//            //    e.target; // newly activated tab
//            //   e.relatedTarget // previous active tab
//        });

//    //now get the view and load it to the tab content id that you named above
//    getView(url, params, target);
//});