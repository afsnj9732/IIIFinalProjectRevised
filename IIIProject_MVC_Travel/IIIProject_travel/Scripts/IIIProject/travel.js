(function () {
    var order, background_color, contain, category, label,page ,p;
    //Bootstrap Modal 關閉觸發事件
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open'); //疊加互動視窗 Scroll Debug
        $(".combine_readmore").addClass("show"); //顯示隱藏的第一個視窗
    });

    $(document).on('click', '[data-toggle = modal]', function () {
        if ($('.modal-backdrop').eq(0).css('background-color') !== null) {
            $(".combine_readmore").removeClass("show"); //隱藏第一個視窗
        }          
        $('.modal-backdrop').eq(1).css('background-color', 'white'); 
    });

    //書籤回最上
    $("#labelTop").on("click", function (e) {
        e.preventDefault();
        window.document.body.scrollTop = 0;
        window.document.documentElement.scrollTop = 0;
    });

    //揪團時間限制   
    $("#ActivityStart").on("change", function () {
        //$("#ActivityStart").attr("disabled","");
        $("#ActivityStartTo").attr("hidden", "");
        $("#ActivityEnd").val("");
        $("#ActivityFindEnd").val("");
        $("#ActivityEnd").removeAttr("disabled");
        $("#ActivityFindEnd").removeAttr("disabled");
        $("#ActivityEnd").attr("min", $("#ActivityStart").val());
        $("#ActivityFindEnd").attr("max", $("#ActivityStart").val());
    });

    //星星評分頭
     
    $("body").on('mouseover', '.Score img', function () {
        let amount = $(this).attr("ScoreID");
        let target = $(this).attr("ScoreTarget");
        if ($("[ScoreTarget =" + target + "]").attr("isclick") === "false") {
            for (let i = 0; i < amount; i++) {
                $("[ScoreTarget =" + target + "]").eq(i).attr('src', '/Content/images/ChangeStar.png');
            }
        }
    });

    $("body").on('mouseout', '.Score img', function () {
        let amount = $(this).attr("ScoreID");
        let target = $(this).attr("ScoreTarget");
        if ($("[ScoreTarget =" + target + "]").attr("isclick") === "false") {
            for (let i = 0; i < amount; i++) {
                $("[ScoreTarget =" + target + "]").eq(i).attr('src', '/Content/images/Star.png');
            }
        }

    });

    $("body").on('click', '.Score img', function () {   
        let target = $(this).attr("ScoreTarget");
        $(this).addClass("GetScore");
        for (let i = 0; i < 5; i++) {
            $("[ScoreTarget =" + target + "]").eq(i).attr('isclick', 'true');
        }
    });
    $("body").on('click', '.resetScore', function () {  
        let target = $(this).attr("resetScore");

        for (let i = 0; i < 5; i++) {
            $("[ScoreTarget =" + target + "]").eq(i).attr('isclick', 'false');
            $("[ScoreTarget =" + target + "]").eq(i).attr('src', '/Content/images/Star.png');
            $("[ScoreTarget =" + target + "]").removeClass("GetScore");
        }
    });
    
    $("body").on('click', '.leaveScore', function () { //送出評分
        let target = $(this).attr("leaveScore");
        let Score = $(".GetScore").attr("ScoreID");
        $.ajax({
            url: "/Travel/ScoreAdd",
            type: "POST",
            data: { "target": target , "Score":Score},
            success: function (data) {
                if (data === "0") {
                    window.confirm("有參加的會員且須於活動結束以後才可評分");
                } else if (data === "1") {
                    window.confirm("你已經評分過了哦!");
                } else {
                    window.confirm("評分成功!");
                }
            }
        });
    });
    //星星評分尾



    var theMonth, theDay;
    $("#ActivityEnd").on("change", function () {
        $("#ActivityEndTo").attr("hidden", "");
        //let d = new Date($("#ActivityEnd").val());
        //date = new Date(d.setDate(d.getDate() - 1));
        nowdate = new Date(Date.now());
        //theMonth = date.getMonth() + 1;
        //theDay = date.getDate();
        //FormatTime();
        //$("#ActivityStart").attr("max", date.getFullYear() + "-" + theMonth + "-" + theDay);
        theMonth = nowdate.getMonth() + 1;
        theDay = nowdate.getDate();
        FormatTime();
        $("#ActivityStart").attr("min", nowdate.getFullYear() + "-" + theMonth + "-" + theDay);
    });
    //時間格式轉換
    function FormatTime() {
        if (theMonth.toString().length < 2) {
            theMonth = "0" + theMonth;
        }
        if (theDay.toString().length < 2) {
            theDay = "0" + theDay;
        }
    }
    //揪團欄位限制
    $("#NeedAT").on("change", function () {
        $("#NeedATTo").attr("hidden", "");
    });
    $("#ActivityFindEnd").on("change", function () {
        $("#ActivityFindEndTo").attr("hidden", "");
    });
    $("#NeedAC").on("change", function () {
        $("#NeedACTo").attr("hidden", "");
    });
    $("#NeedAP").on("change", function () {
        $("#NeedAPTo").attr("hidden", "");
    });
    $("#NeedAL").on("change", function () {
        $("#NeedALTo").attr("hidden", "");
    });
    //揪團欄位限制
    $("#JoutaSend").on("click", function (e) {
        if ($("#NeedAT").val().length < 8) {
            e.preventDefault();
            $("#NeedATTo").removeAttr("hidden");
        } else if ($("#ActivityStart").val() === "") {
            e.preventDefault();
            $("#ActivityStartTo").removeAttr("hidden");
        } else if ($("#ActivityEnd").val() === "") {
            e.preventDefault();
            $("#ActivityEndTo").removeAttr("hidden");
        } else if ($("#ActivityFindEnd").val() === "") {
            e.preventDefault();
            $("#ActivityFindEndTo").removeAttr("hidden");
        } else if ($("#NeedAC").val() === "") {
            e.preventDefault();
            $("#NeedACTo").removeAttr("hidden");
        } else if ($("#NeedAP").val() === "") {
            e.preventDefault();
            $("#NeedAPTo").removeAttr("hidden");
        } else if ($("#NeedAL").val().length < 100) {
            e.preventDefault();
            $("#NeedALTo").removeAttr("hidden");
        }
    });



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

    //收藏按鈕，抓活動編號
    $('body').on('click', '.likeIt', function () {
        var target = $(this).attr("likeIndex");
        var combine = "[likeIndex=" + target + "]";
        var ActivityID = target;
        if ($(combine).attr("src") === "/Content/images/14.png") {
            $(combine).attr("src", "/Content/images/11.png");           
            $.ajax({
                url: "/Travel/likeIt",
                type: "POST",
                data: { "ActivityID": ActivityID }
            });
        } else {
            $(combine).attr("src", "/Content/images/14.png");
            $.ajax({
                url: "/Travel/likeIt",
                type: "POST",
                data: { "ActivityID": ActivityID }
            });
        }
        //console.log("現在src "+$(combine).attr("src"));
    });

    //入團
    function joinAct() {
        let target = $(this).attr("joinAct");
        $.ajax({
            url: "/Travel/ActAdd",
            type: "POST",
            data: { "target": target,"isAdd":true},
            success: function (data) {
                if (data === "1") {
                    window.confirm("已是團主不用入團");
                }
                else if (data === "") {
                    window.confirm("你已經入團了哦!");
                } else {
                    $("[ActAdd=" + target + "]").html(data);
                }
            }
        });
    }
    //退團
    function leaveAct() {
        let target = $(this).attr("leaveAct");
        $.ajax({
            url: "/Travel/ActAdd",
            type: "POST",
            data: { "target": target, "isAdd": false},
            success: function (data) {
                if (data === "1") {
                    window.confirm("團主不可退團");
                }
                else if (data === "") {
                    window.confirm("你沒有入團哦!");                    
                } else {
                    $("[ActAdd=" + target + "]").html(data);
                }              
            }
        });
    }
    //留言
    function leaveMsg() {
        let target = $(this).attr("leaveMsg");
        let sentMsg = $("[sentMsg=" + target + "]").val();
        //console.log(sentMsg);
        $.ajax({
            url: "/Travel/MsgAdd",
            type: "POST",
            data: { "target": target, "sentMsg": sentMsg},
            success: function (data) {
                $("[MsgAdd=" + target + "]").html(data); 
                $("[sentMsg=" + target + "]").val("");
            }
        });
        
    }


    //增加讚
    function getGoodCounts() {         
        let target = $(this).attr("target");
        let FeelGood = $("[ToUpdateGC =" + target + "]").html();
        $.ajax({
            url: "/Travel/FeelGood",
            type: "POST",
            data: { "target": target},
            success: function (data) {
                if (data === "0") {
                    window.confirm("這篇文章你按過讚囉!");
                }
                else {
                    $("[ToUpdateGC =" + target + "]").html(parseInt(FeelGood)+1);
                }
            }
        });
    }

    //增加瀏覽次數
    function getViewCounts() {        
        let target = $(this).attr("updateVC");
        var combine = "[ToUpdateVC=" + target + "]";
        var getCounts = parseInt($(combine).html()) + 1;
        $(combine).html(getCounts);
        $.ajax({
            url: "/Travel/ViewCounts",
            type: "POST",
            data: { "ActivityID": target}
            }
        );
    }
    //因為文章項目是ajax動態產生，因此事件必須使用氣泡動態綁定寫法
    $("body").on('click', ".ViewCounts", getViewCounts );
    $("body").on('click', ".FeelGood", getGoodCounts );
    $("body").on('click', ".joinAct", joinAct);
    $("body").on('click', ".leaveAct", leaveAct);
    $("body").on('click', ".leaveMsg", leaveMsg);

    $("body").on('click', ".MyPage", function () {
        $(this).addClass("NowPage");
        $(this).siblings().removeClass("NowPage");
        getAJAX();
    });
    function getAJAX() {
        //console.log("我是AJAX，拿到的背景顏色是" + background_color);
        order = $(".using").attr("order");
        document.querySelector(".using").style.backgroundColor;
        contain = $("#contain").val();
        category = $("#category").val();
        label = $("#label").val();
        page = $(".NowPage").attr("page");
        p = JSON.stringify({
            "order": order, "background_color": background_color, "contain": contain
            , "category": category, "label": label , "page":page
        });
 
        $.ajax({
            url: "/Travel/article_AJAX",
            type: "POST",
            data: { "p": p },
            success: function (data) {
                $("#article_ajax").html(data);
                //$("#article_ajax div").remove();
                //$("#article_ajax").append(data);
                Travel_RWD();
                //getViewCounts();
                //getGoodCounts();
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
            type: "POST",
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
        page = 1;
        $(".page").eq(0).addClass("NowPage")
        $(".page").eq(0).siblings().removeClass("NowPage");//回彈到第一頁
        getAJAX();
    });

    $("#category").on('change', function () {
        page = 1;
        $(".page").eq(0).addClass("NowPage")
        $(".page").eq(0).siblings().removeClass("NowPage");
        getAJAX();
    });
    $("#label").on('change', function () {
        page = 1;
        $(".page").eq(0).addClass("NowPage")
        $(".page").eq(0).siblings().removeClass("NowPage");
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
        page = 1;
        $(".page").eq(0).addClass("NowPage")
        $(".page").eq(0).siblings().removeClass("NowPage");//回彈到第一頁
        getAJAX();
        //this.childNodes[1].childNodes[1].click();//連動點擊圖片觸發排序和ajax
        //注意，子元素特定事件觸發會連帶觸發所有父元素的該事件       
    });

    ////預設最新被選為排序
    $("#travel_sort .sort li").eq(0).click();                               



})(); 