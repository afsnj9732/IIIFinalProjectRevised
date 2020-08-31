; (function () {
    var order;
    var background_color;
    var contain;
    var category;
    var label;
    var p;
    //$("#showCharts").on('click', function () {
    //    console.log("123");
    //    $.ajax({
    //        url: "/Travel/Charts",
    //        success: function (data) {
    //            $("#showChartsTag").append(data.write());
    //        }
    //    });
        
    //});

    //增加讚
    function getGoodCounts() {
        var j = 0;
        let target = $(".FeelGood").attr("target");
        //console.log(target);
        $.ajax({
            url: "/Travel/FeelGood",
            type: 'POST',
            data: { "target": target, "p": p },
            success: function (data) {
                $(".GoodCountTemp").remove();//刪除html原有資料
                for (var i of data) {
                    $(".updateGoodCounts").eq(j).after("<span class='GoodCountTemp'>" + i + "</span>");
                    j++;
                    $(".updateGoodCounts").eq(j).after("<span class='GoodCountTemp'>" + i + "</span>");
                    j++;
                }

            }

        });
    };

    //瀏覽次數
    function getViewCounts() {
        var j = 0;         
        let target = $(this).attr("updateVC");
        //console.log("1231")
        $.ajax({
            url: "/Travel/CountView",
            type: 'POST',
            data: { "target":target,"p":p },
            success: function (data) {
                $(".ViewCountTemp").remove();//刪除html原有資料
                for (var i of data) {                            
                    $(".updateViewCounts").eq(j).after("<span class='ViewCountTemp'>" + i + "</span>");
                    j++;
                    $(".updateViewCounts").eq(j).after("<span class='ViewCountTemp'>" + i + "</span>");
                    j++;
                }

            }

        });
    }
    //瀏覽次數，注意因為文章項目是ajax動態產生，因此事件必須使用氣泡動態綁定寫法
    $("body").on('click', ".ViewCounts", getViewCounts );
    $("body").on('click', ".FeelGood", getGoodCounts );

    //即時性搜尋
    //$("#contain").on('keyup', function () {
    //    $("#travel_sort .sort li").eq(0).click();   //用這樣搜尋剛搜尋完第一下會有BUG
    //});
    $("#category").on('change', function () {
        $("#travel_sort .sort li img").eq(0).click(); 
    })
    $("#label").on('change', function () {
        $("#travel_sort .sort li img").eq(0).click();
    })

    //排序用點擊事件(因為li太大)，已測試可運行
    $("#travel_sort .sort li img").on('click',function (e) {
        e.stopPropagation(); //停止氣泡，防止子元素點擊父元素
        //console.log("test");
        order = $(this).attr("order");
        background_color = $(this).parent().parent().css("background-color");
        contain = $("#contain").val();
        category = $("#category").val();
        label = $("#label").val();
        //console.log(category);
        //console.log(label);
        p = JSON.stringify({
            "order": order, "background_color": background_color, "contain": contain
            , "category": category, "label": label
        });
        $.ajax({
            async: false, 
            url: "/Travel/article_AJAX",
            type: 'POST',
            data: {"p":p},
          success: function (data) {
              $("#article_ajax div").remove();
              $("#article_ajax").append(data);
              Travel_RWD();
              getViewCounts();
            }          
        });
    });
    //搜尋ajax





    // 排序固定RWD    
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

    function Travel_RWD() {
        if (document.body.clientWidth > 1500 || document.body.clientWidth < 970) {
            $(".popguys").attr('class', "mr-2 ml-2 popguys");
        }
        else
        {
            $(".popguys").attr('class', "mr-1 ml-1 popguys");
        }
        
        if (document.body.clientWidth < 1850) {
            $(".RWD_1850").css('display', '');
            $(".RWD_1851").css('display', 'none');

        } else {
            $(".RWD_1850").css('display', 'none');
            $(".RWD_1851").css('display', '');
        }

        if (document.body.clientWidth >= 975) {
            travel_sort_scrollHandler();
        } else {
            travel_sort.classList.remove("fix");
            travel_sort.style.top = 0 + "px";
            document.querySelector("#replace").classList.remove("col-3");
            travel_sort.classList.add("col-3");
        }
    };
    Travel_RWD();
     

    window.addEventListener("resize", function () {
        Travel_RWD();
    });    
    // window.addEventListener("scroll", travel_sort_scrollHandler);
    // }

    //window.setInterval(function () { $("#contain_pic").click(); console.log("123"); }, 3000);

    //點選搜尋
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