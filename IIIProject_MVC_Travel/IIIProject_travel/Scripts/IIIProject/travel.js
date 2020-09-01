(function () {
    var order, background_color,contain,category,label,p;

    // 排序固定   
    let header = document.querySelector(".header");
    let travel_sort = document.querySelector("#travel_sort");
    function travel_sort_scrollHandler() {
        travel_sort.classList.add("fix");
        travel_sort.style.top = header.offsetHeight + "px";
        document.querySelector("#replace").classList.add("col-3");
        travel_sort.classList.remove("col-3");

    }

    //RWD 
    function Travel_RWD() {
        if (document.body.clientWidth > 1500 || document.body.clientWidth < 970) {
            $(".popguys").attr('class', "mr-2 ml-2 popguys");
        }
        else {
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
    }

    //視窗變化觸發RWD
    window.addEventListener("resize", function () {
        Travel_RWD();
    });    

    //增加讚
    function getGoodCounts() {
        var g = 0;
        let target = $(this).attr("target");
        $.ajax({
            url: "/Travel/FeelGood",
            type: 'POST',
            data: { "target": target, "p": p },
            success: function (data) {
                $(".GoodCountTemp").remove();//刪除html原有資料
                for (let l of data) {
                    $(".updateGoodCounts").eq(g).after("<span class='GoodCountTemp'>" + l + "</span>");
                    g++;
                    $(".updateGoodCounts").eq(g).after("<span class='GoodCountTemp'>" + l + "</span>");
                    g++;
                }
            }
        });
    }

    //增加瀏覽次數
    function getViewCounts() {
        var j = 0;         
        let target = $(this).attr("updateVC");
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
    //因為文章項目是ajax動態產生，因此事件必須使用氣泡動態綁定寫法
    $("body").on('click', ".ViewCounts", getViewCounts );
    $("body").on('click', ".FeelGood", getGoodCounts );
 

    function getAJAX() {
        //console.log("我是AJAX，拿到的背景顏色是" + background_color);
        order = $(".using").attr("order");
        document.querySelector(".using").style.backgroundColor;
        contain = $("#contain").val();
        category = $("#category").val();
        label = $("#label").val();
        p = JSON.stringify({
            "order": order, "background_color": background_color, "contain": contain
            , "category": category, "label": label
        });
 
        $.ajax({
            url: "/Travel/article_AJAX",
            type: 'POST',
            data: { "p": p },
            success: function (data) {
                $("#article_ajax div").remove();
                $("#article_ajax").append(data);
                Travel_RWD();
                getViewCounts();
                getGoodCounts();
            }
        });
    }


    ////即時性搜尋
    //$("#contain").on('keyup', function (e) {
    //    getAJAX();   
    //});

    $("#contain").on('keyup', function () { 
        $.ajax({
            url: "/Travel/autoComplete",
            success: function (data) {
                var getautoComplete = data.split(',');
                $("#contain").autocomplete({
                    source: getautoComplete
                });
            }
        });
    });

    //點選搜尋
    $("#contain_pic").on('click', function () {
        getAJAX();
    });

    $("#category").on('change', function () {
        getAJAX();
    });
    $("#label").on('change', function () {
        getAJAX();
    });

    //排序特效，注意JS.CSS使用Hex碼有些許狀況，本身不帶ajax，以事件連動觸發ajax
    $("#travel_sort .sort li").on('click', function () {             
        $(this).addClass('using');
        $(this).siblings().removeClass('using');
        $("#row span").remove();
        $("#row div").append($(this).html());
        $("#row #temp").remove();             
 
        if (this.style.backgroundColor === 'rgb(250, 224, 178)') {
            this.style.backgroundColor = 'rgb(250, 224, 177)';

            $("#temp").remove();
            if ($(this).attr('id') === "07") {
                $(this).append("<span id='temp'>近－>遠</span>");
            }
            else if ($(this).attr('id') === "08") {
                $(this).append("<span id='temp'>快－>慢</span>");
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
            this.style.backgroundColor = 'rgb(250, 224, 178)';
   
            $("#temp").remove();
            if ($(this).attr('id') === "07") {
                $(this).append("<span id='temp'>遠－>近</span>");
            }
            else if ($(this).attr('id') === "08") {
                $(this).append("<span id='temp'>慢－>快</span>");
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
        background_color = document.querySelector(".using").style.backgroundColor;
        //$(this).css()抓出來的有點問題，與項目實際情形不符，待查證
        $(this).css('border-color', 'rgb(250, 224, 110)');
        $(this).siblings().css('background-color', 'transparent');    //剔除未選取排序   
        $(this).siblings().css('border-color', 'transparent');    //剔除未選取排序         
        getAJAX();
        //this.childNodes[1].childNodes[1].click();//連動點擊圖片觸發排序和ajax
        //注意，子元素特定事件觸發會連帶觸發所有父元素的該事件       
    });

    ////預設最新被選為排序
    $("#travel_sort .sort li").eq(0).click();                               



})(); 