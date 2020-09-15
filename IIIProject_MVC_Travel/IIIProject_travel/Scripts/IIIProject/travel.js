(function () {  
    var order, background_color, contain, category, label, page, condition, readmore_target;
    var calendarEl = document.getElementById('calendar');
    //動態生成行事曆
    function getCalendar() {
        $.ajax({
            url: "/Travel/getCalendar",
            type: "POST",
            success: function (data) {
                if (data === "") {
                    //行事曆                   
                    let calendar = new FullCalendar.Calendar(calendarEl, {
                        initialView: 'dayGridMonth',
                        displayEventTime: false,
                        locale: 'zh-tw',
                        height: 750
                    });
                    calendar.render();
                } else if (data !== "1") {
                    let calendar = new FullCalendar.Calendar(calendarEl, {
                        initialView: 'dayGridMonth',
                        displayEventTime: false,
                        locale: 'zh-tw',
                        height: 750,
                        events: JSON.parse(data),
                        eventClick: function () {
                        }
                    });
                    calendar.render();
                }
            }
        });
    }
    getCalendar();
    //排序固定   
    let header = document.querySelector(".header");
    let travel_sort = document.querySelector("#travel_sort");
    function travel_sort_scrollHandler() {
        travel_sort.classList.add("fix");
        calendarEl.classList.remove("calendar_relative");
        calendarEl.classList.add("calendar_fix");               
        travel_sort.style.top = header.offsetHeight + "px";
        document.querySelector("#replace").classList.add("col-3");
        travel_sort.classList.remove("col-3");

    }

    //RWD 
    function travel_RWD() {
        if (document.body.clientWidth > 1500 || document.body.clientWidth < 970) {
            $(".popguys").attr('class', "mr-2 ml-2 popguys");
            $("#labelTop").removeAttr("hidden");           
        }
        else {
            $(".popguys").attr('class', "mr-1 ml-1 popguys");
            $("#labelTop").attr("hidden", "");
            
        }

        if (document.body.clientWidth < 970) {
            $("#labelTop").css("bottom", "0%");
        } else {
            $("#labelTop").css("bottom", "5%");
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
            calendarEl.classList.remove("calendar_fix");
            calendarEl.classList.add("calendar_relative");            
            travel_sort.style.top = 0 + "px";
            document.querySelector("#replace").classList.remove("col-3");
            travel_sort.classList.add("col-3");
        }
    }

    //視窗變化觸發RWD
    window.addEventListener("resize", function () {
        travel_RWD();
    });

    //搜尋推薦
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

    //ajax取得readmore
    function get_ajax_readmore() {
        $.ajax({            
            url: "/Travel/get_ajax_readmore",  
            async: false,         
            type: "POST",
            data: { "act_id": readmore_target },
            success: function (data) {
                $("#add_ajax_readmore").html(data); //更新readmore項目     
                //$('#ajax_readmore').modal("show");
            }
        });  
    }
  
    //文章列表觸發readmore
    $("body").on('click', '[data-target="#ajax_readmore"]', function () {
        readmore_target = $(this).attr("act_id");
        get_ajax_readmore();                
    });

    //文章列表直接看留言
    $("body").on('click', '.call_ajax_msg', function () {
        readmore_target = $(this).attr("act_id");
        get_ajax_readmore();
    });

    //行事曆觸發readmore
    $("body").on("click", ".CalendarEvent", function () {
        let targetclass = this.classList.item(this.classList.length-1);
        readmore_target = targetclass.substring(10, targetclass.length);        
        get_ajax_readmore();
        $("#calendarEventGo").attr("act_id", readmore_target);
        $("#calendarEventGo").click();
    });




    //Bootstrap Modal 關閉觸發事件 
    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open'); //疊加互動視窗 Scroll Debug        
        if ($(".get_map").hasClass("getMap_show")) { //若目前是要關閉設定地區視窗   
            $(".combine_edit").addClass("show");
            $(".JoutaStart").addClass("show");
            $("#getMap").removeClass("getMap_show");
        } else {             
            $(".combine_readmore").addClass("show"); //顯示視窗
            
        }
    });

    $("body").on('click', '[data-target="#getMap"]', function () {
        $("#getMap").addClass("getMap_show");
    });


    $(document).on('click', '[data-toggle = modal]', function () {
        if ($('.modal-backdrop').eq(0).css('background-color') !== null) {
            $(".JoutaStart").removeClass("show");
            $(".combine_readmore").removeClass("show"); //隱藏視窗
            $(".combine_edit").removeClass("show");
        }          
        $('.modal-backdrop').eq(1).css('background-color', 'white'); 
    });

 


    //書籤回最上
    $("#labelTop").on("click", function (e) {
        e.preventDefault();
        window.document.body.scrollTop = 0;
        window.document.documentElement.scrollTop = 0;
    });


    //---------------------------------------------下方問題區塊

    //時間格式轉換
    var theMonth, theDay;
    function FormatTime() {
        if (theMonth.toString().length < 2) {
            theMonth = "0" + theMonth;
        }
        if (theDay.toString().length < 2) {
            theDay = "0" + theDay;
        }
    }
    //刪除
    $("body").on("click", ".delete_act", function (e) {       
        if (!window.confirm("確定要刪除?")) {
            e.preventDefault();
        }
    })
    //揪團時間限制   
    $("body").on("change", ".ActivityStart", function () {
        $(".ActivityStartTo").attr("hidden", "");
        $(".ActivityEnd").val("");
        $(".ActivityFindEnd").val("");
        $(".ActivityEnd").removeAttr("disabled");
        $(".ActivityFindEnd").removeAttr("disabled");
        $(".ActivityEnd").attr("min", $(this).val());
        $(".ActivityFindEnd").attr("max", $(this).val());
    });

    //動態活動結束時間限制
    $("body").on("change", ".ActivityEnd", function () {
        $(".ActivityEndTo").attr("hidden", "");
        //nowdate = new Date(Date.now());
        //theMonth = nowdate.getMonth() + 1;
        //theDay = nowdate.getDate();
        //FormatTime();
        //$(".ActivityStart").attr("min", nowdate.getFullYear() + "-" + theMonth + "-" + theDay);
    });

    //欄位限制動態檢驗
    $("body").on("change", ".NeedAT", function () {
        $(".NeedATTo").attr("hidden", "");
    });
    $(".ActivityFindEnd").on("change", function () {
        $(".ActivityFindEndTo").attr("hidden", "");
    });
    $(".NeedAC").on("change", function () {
        $(".NeedACTo").attr("hidden", "");
    });
    $(".NeedAP").on("change", function () {
        $(".NeedAPTo").attr("hidden", "");
    });
    $(".NeedAL").on("change", function () {
        $(".NeedALTo").attr("hidden", "");
    });


    //文字編輯器因套件與bootsrap modal設計上有衝突，因此編輯器有關對話框的功能會無法使用
                                                  //但其他功能可以正常運作
    //按下開團/編輯的按鈕，進入開團/編輯頁面，重置所有欄位的驗證
    $("body").on("click", ".JoutaEdit", function () {
        //文字編輯器重置
        if (CKEDITOR.instances.AddAct) {   
            CKEDITOR.instances.AddAct.destroy();
        }
        if (CKEDITOR.instances.EditAct) {
            CKEDITOR.instances.EditAct.destroy();
        }
        let target = $(this).attr("limitNumber");
        if (target === "0") {
            //文字編輯器
            CKEDITOR.replace('f活動內容', { height: 400, width: 1100 });

        } else {
            //文字編輯器
            CKEDITOR.replace('f活動內容2', { height: 400, width: 1100 });
        }
        
        $(".NeedATTo").attr("hidden", "");
        $(".ActivityStartTo").attr("hidden", "");
        $(".ActivityEndTo").attr("hidden", "");
        $(".ActivityFindEndTo").attr("hidden", "");
        nowdate = new Date(Date.now());
        theMonth = nowdate.getMonth()+1; //js時間月份是索引值，所以現在月份要+1
        theDay = nowdate.getDate()+2;
        FormatTime();
        $(".ActivityStart").attr("min", nowdate.getFullYear() + "-" + theMonth + "-" + theDay);        
        $(".ActivityFindEnd").attr("max", "");
        if (target === "0") {
            $(".ActivityStart").val("");
            $(".ActivityEnd").val("");
            $(".ActivityFindEnd").val("");
        }
        $(".ActivityEnd").eq(0).attr("disabled","");
        $(".ActivityFindEnd").eq(0).attr("disabled", "");
        $(".btn_get_map_msg").html("");
        $(".NeedACTo").attr("hidden", "");
        $(".NeedAPTo").attr("hidden", "");
        $(".NeedALTo").attr("hidden", "");
    });



    //取得文字編輯器值+欄位檢驗
    $("body").on("click", ".JoutaSend", function (e) {
        let target = $(this).attr("limitNumber");
        if (target === "0") {
            let data = CKEDITOR.instances.AddAct.getData();
            $('#AddAct').val(data);
        } else {
            let data2 = CKEDITOR.instances.EditAct.getData();
            $('#EditAct').val(data2);
        }       
        
        if ($(".NeedAT").eq(target).val().length < 8) {
            e.preventDefault();
            $(".NeedATTo").eq(target).removeAttr("hidden");
        } else if ($(".ActivityStart").eq(target).val() === "") {
            e.preventDefault();
            $(".ActivityStartTo").eq(target).removeAttr("hidden");
        } else if ($(".ActivityEnd").eq(target).val() === "") {
            e.preventDefault();
            $(".ActivityEndTo").eq(target).removeAttr("hidden");
        } else if ($(".ActivityFindEnd").eq(target).val() === "") {
            e.preventDefault();
            $(".ActivityFindEndTo").eq(target).removeAttr("hidden");
        } else if ($(".NeedAC").eq(target).val() === "") {
            e.preventDefault();
            $(".NeedACTo").eq(target).removeAttr("hidden");
        } else if ($(".NeedAP").eq(target).val() === "") {
            e.preventDefault();
            $(".NeedAPTo").eq(target).removeAttr("hidden");
        } else if ($(".NeedAL").eq(target).val().length < 100) {
            e.preventDefault();
            $(".NeedALTo").eq(target).removeAttr("hidden");
        }
        $(".modal-body").scrollTop(0);//回彈最上方
    });

    //modal進入自動回彈
    $('body').on('shown',".modal", function () {
        $(".modal-body").scrollTop(0);
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
            data: { "target": target, "Score": Score },
            success: function (data) {
                if (data === "5") {
                    window.confirm("團主不可自行評分");
                }
                else if (data === "3") {
                    window.confirm("活動結束後才可評分");
                }
                else if (data === "0") {
                    window.confirm("有參加的會員才可評分");
                } else if (data === "1") {
                    window.confirm("你已經評分過了哦!");
                } else {
                    window.confirm("評分成功!");
                }
            }
        });
    });
    //星星評分尾

    //收藏按鈕，抓活動編號
    $('body').on('click', '.likeIt', function () {
        var target = $(this).attr("likeIndex");
        var combine = "[likeIndex=" + target + "]";
        if ($(combine).attr("src") === "/Content/images/14.png") {
            $(combine).attr("src", "/Content/images/11.png");           
            $.ajax({
                url: "/Travel/likeIt",
                type: "POST",
                data: { "ActivityID": target }
            });
        } else {
            $(combine).attr("src", "/Content/images/14.png");
            $.ajax({
                url: "/Travel/likeIt",
                type: "POST",
                data: { "ActivityID": target }
            });
        }
    });

    //黑名單人
    function addBlackList() {
        let target = $(this).attr("member_id");
        let id = $(this).attr("act_id");
        let act_target = $(this).attr("act_target");
        $.ajax({
            url: "/Travel/addBlackList",
            type: "POST",
            data: { "target_member": target, "act_id": id, "act_target": act_target},
            success: function (data) {
                if (data === "0") {
                    window.confirm("不可以自己黑單自己")
                    return;
                } else if (data === "1") {
                    window.confirm("對象已經在黑名單內")
                    return;
                }

                if (act_target === "msg") {
                    $("[MsgAdd=" + id + "]").html(data);
                    window.confirm("黑單成功!")
                } else {
                    $("[ActAdd=" + id + "]").html(data);
                    window.confirm("黑單成功!")
                }                                             
            }
        });
    }
    $("body").on('click', ".jouta_black_list", addBlackList);

    //留言
    function leaveMsg() {
        let target = $(this).attr("leaveMsg");
        let sentMsg = $("[sentMsg=" + target + "]").val();
        $.ajax({
            url: "/Travel/MsgAdd",
            type: "POST",
            data: { "target": target, "sentMsg": sentMsg },
            success: function (data) {
                $("[MsgAdd=" + target + "]").html(data);
                $("[sentMsg=" + target + "]").val("");
            }
        });

    }
    $("body").on('click', ".leaveMsg", leaveMsg);

    //踢人
    function kickAct() {
        let target = $(this).attr("member_id");
        let id = $(this).attr("act_id");
        $.ajax({
            url: "/Travel/ActKick",
            type: "POST",
            data: { "target_member": target,"act_id":id },
            success: function (data) {
                if (data === "") {
                    window.confirm("不可以踢自己!");
                } else {
                    $("[ActAdd=" + id + "]").html(data);
                    window.confirm("踢除成功!");
                //getCalendar(); 踢人行事曆不用動
                }
            }
        });
        }    
    $("body").on('click', ".jouta_kick", kickAct);

    //入團審核通過
    function agreeAct() {
        let target = $(this).attr("member_id");
        let id = $(this).attr("act_id");
        let act = $(this).attr("act_target");
        $.ajax({
            url: "/Travel/agree_add",
            type: "POST",
            data: { "target_member": target, "act_id": id , "act":act },
            success: function (data) {
                if (data === "6") {
                    window.confirm("活動時間與對象既有活動時間衝突!");
                } else {
                    $("[ActAdd=" + id + "]").html(data);
                }
 
            }
        });
    }
    $("body").on('click', ".jouta_agree", agreeAct);


    //想要入團
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
                else if (data === "7") {
                    window.confirm("慘遭團主黑單，不予入團!");
                } else if (data === "8") {
                    window.confirm("團主審核中，請靜待佳音!");
                }
                else if (data === "0") {
                    window.confirm("你已經入團了哦!");
                }
                else if (data === "6") {
                    window.confirm("與既有活動時間衝突!");
                } else {
                    $("[ActAdd=" + target + "]").html(data);
                }
            }
        });
    }
    $("body").on('click', ".joinAct", joinAct);

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
                    getCalendar();
                }              
            }
        });
    }
    $("body").on('click', ".leaveAct", leaveAct);    



    //增加讚
    function getGoodCounts() {
        let target = $(this).attr("target");
        let FeelGood = $("[ToUpdateGC =" + target + "]").html();
        $.ajax({
            url: "/Travel/FeelGood",
            type: "POST",
            data: { "target": target },
            success: function (data) {
                if (data === "0") {
                    window.confirm("這篇文章你按過讚囉!");
                }
                else {
                    $("[ToUpdateGC =" + target + "]").html(parseInt(FeelGood) + 1);
                    window.confirm("按讚成功!");
                }
            }
        });
    }
    //因為文章項目是ajax動態產生，因此事件必須使用氣泡動態綁定寫法
    $("body").on('click', ".FeelGood", getGoodCounts);


    //增加瀏覽次數
    function getViewCounts() {
        let target = $(this).attr("updateVC");
        var combine = "[ToUpdateVC=" + target + "]";
        var getCounts = parseInt($(combine).html()) + 1;
        $(combine).html(getCounts);
        $.ajax({
            url: "/Travel/ViewCounts",
            type: "POST",
            data: { "ActivityID": target }
        }
        );
    }
    //因為文章項目是ajax動態產生，因此事件必須使用氣泡動態綁定寫法
    $("body").on('click', ".ViewCounts", getViewCounts);


    function getAJAX() {
        order = $(".using").attr("order");
        document.querySelector(".using").style.backgroundColor;
        contain = $("#contain").val();
        category = $("#category").val();
        label = $("#label").val();
        page = $(".NowPage").attr("page");
        condition = JSON.stringify({
            "order": order, "background_color": background_color, "contain": contain
            , "category": category, "label": label , "page":page
        });
        
        $.ajax({
            url: "/Travel/article_AJAX",
            type: "POST",
            data: { "condition": condition },
            success: function (data) {
                $("#article_ajax").html(data); 
                travel_RWD(); 
                readmore_target = $('.ViewCounts').eq(0).attr("act_id");
                if (readmore_target !== undefined) {
                    get_ajax_readmore();
                    
                }
            }
        });

    }

    //取得頁數
    $("body").on('click', ".MyPage", function () {
        $(this).addClass("NowPage");
        $(this).siblings().removeClass("NowPage");
        getAJAX();
    });


    function get_now_condition() {
        page = 1;
        $(".page").eq(0).addClass("NowPage")
        $(".page").eq(0).siblings().removeClass("NowPage");//回彈到第一頁
        getAJAX();
    }

    //點選搜尋
    $("#contain_pic").on('click', function () {
        get_now_condition();
    });

    $("#category").on('change', function () {
        get_now_condition();
    });
    $("#label").on('change', function () {
        get_now_condition();
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
        get_now_condition();
        //this.childNodes[1].childNodes[1].click();//連動點擊圖片觸發排序和ajax
        //注意，子元素特定事件觸發會連帶觸發所有父元素的該事件       
    });

    //首頁搜尋
    let HSCategory = $("#category").attr("HomeSearch");
    $("#category [value=所有]").removeAttr("selected");
    $("#category [value=" + HSCategory + "]").attr("selected","");

    let HSLabel = $("#label").attr("HomeSearch");
    $("#label [value=全部]").removeAttr("selected");
    $("#label [value=" + HSLabel + "]").attr("selected", "");

    //進入旅遊業面預設最新被選為排序
    $("#travel_sort .sort li").eq(0).click(); 

    

    
})(); 