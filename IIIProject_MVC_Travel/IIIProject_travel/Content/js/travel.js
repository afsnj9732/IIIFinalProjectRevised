; (function () {

    //$("#showCharts").on('click', function () {
    //    console.log("123");
    //    $.ajax({
    //        url: "/Travel/Charts",
    //        success: function (data) {
    //            $("#showChartsTag").append(data.write());
    //        }
    //    });
        
    //});


    //排序用點擊事件(因為li太大)，已測試可運行
    $("#travel_sort .sort li img").on('click',function (e) {
        e.stopPropagation(); //停止氣泡，防止子元素點擊父元素
        //console.log("test");
        let order = $(this).attr("order");
        let background_color = $(this).parent().parent().css("background-color");
        let contain = $("#contain").val();
        //console.log(contain);
        let p = JSON.stringify({ "order": order, "background_color": background_color, "contain": contain });
        $.ajax({
            async: false, 
            url: "/Travel/article_AJAX",
            type: 'POST',
            data: {"p":p},
          success: function (data) {
              $("#article_ajax div").remove();
              $("#article_ajax").append(data);
            }          
        });
    });
    //搜尋ajax

    //增加讚
    //$("body").on('click',".FeelGood", function () {
    //    //console.log("123"); 
    //    let target = $(".FeelGood").attr("target");
    //    //console.log(target);
    //    $.ajax({
    //        url: "/Travel/FeelGood",
    //        data: { "target": target},
    //        success: function (data) {
    //            if (data == "0") {
    //                window.confirm("請先登入");
    //            }
    //            else if (data == "1") {
    //                window.confirm("讚");
    //            }
 
    //        }   
    //    });


    //});



    // 排序固定    
    // TravelSortFix();
    // function TravelSortFix() {
    let header = document.querySelector(".header");
    let travel_sort = document.querySelector("#travel_sort");
    function travel_sort_scrollHandler() {
        // if ((window.scrollY + header.offsetHeight) >= travel_sort.offsetTop) {
        travel_sort.classList.add("fix");
        travel_sort.style.top = header.offsetHeight + "px";
        document.querySelector("#replace").classList.add("col-3");
        travel_sort.classList.remove("col-3");
        //  $("#search").append("</div>");
        // } else {
        //     travel_sort.classList.remove("fix");
        //     document.querySelector("#replace").classList.remove("col-3");
        //     travel_sort.classList.add("col-3");
        //     travel_sort.style.top = 0 + "px";
        // }
    }
    travel_sort_scrollHandler();
    // window.addEventListener("scroll", travel_sort_scrollHandler);
    // }

    //搜尋，應該也可以在不提供排序條件的情況下提交ajax，
    $("#contain_pic").on('click', function (e) {
        e.stopPropagation;
        $("#travel_sort .sort li").eq(0).click();                     
    });


    //排序變化，注意JS.CSS好像不吃Hex碼
    $("#travel_sort .sort li").on('click', function () {
        //console.log("HI");
        $("#row span").remove();
        $("#row div").append($(this).html());
        $("#row #temp").remove();
        if ($(this).css('background-color') === 'rgb(250, 224, 178)') {    //第2n-1次點擊           
            $(this).css('background-color', 'rgb(250, 224, 177)');
            $("#temp").remove();
            if ($(this).attr('id') === "07") {
                $(this).append("<span id='temp'>近－>遠</span>");
            }
            else if ($(this).attr('id') === "08") {
                $(this).append("<span id='temp'>慢－>快</span>");
            }
            else if ($(this).attr('id') === "03") {
                $(this).append("<span id='temp'>北－>南</span>");
            }
            else if ($(this).attr('id') === "02") {
                $(this).append("<span id='temp'>舊－>新</span>");
            }
            else {
                $(this).append("<span id='temp'>低－>高</span>");
            }
        }
        else {
            $(this).css('background-color', 'rgb(250, 224, 178)');    //第2*n次點擊
            $("#temp").remove();
            if ($(this).attr('id') === "07") {
                $(this).append("<span id='temp'>遠－>近</span>");
            }
            else if ($(this).attr('id') === "08") {
                $(this).append("<span id='temp'>快－>慢</span>");
            }
            else if ($(this).attr('id') === "03") {
                $(this).append("<span id='temp'>南－>北</span>");
            }
            else if ($(this).attr('id') === "02") {
                $(this).append("<span id='temp'>新－>舊</span>");
            }
            else {
                $(this).append("<span id='temp'>高－>低</span>");
            }
        }
        //border - color: rgb(250, 224, 178);
        $(this).css('border-color', 'rgb(250, 224, 110)');
        $(this).siblings().css('background-color', 'transparent');    //剔除未選取排序   
        $(this).siblings().css('border-color', 'transparent');    //剔除未選取排序   
        //$("img",this).click(); //會無限迴圈
        this.childNodes[1].childNodes[1].click();//連動點擊圖片觸發排序和ajax
        //子元素預設點下去會再次觸發父元素點擊事件
 
    });
    $("#travel_sort .sort li").eq(0).click();                               //預設熱門被選為排序


})(); 