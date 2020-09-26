(function () {  
    var order, background_color, contain, category, label, page, condition, readmore_target;
    var timeLimit = new Array();
    var calendarEl = document.getElementById('calendar');
    $("#Travel_Demo_Click").on("click", function () {
        $(".NeedAT").eq(0).val("陽明山國家公園");
        $(".NeedAC").eq(0).val("20000");
        CKEDITOR.instances.AddAct.setData(`
一般人印象裡的陽明山，春季裡有好花綻放，盛開如畫；炎夏時，
它是躲開襲人熱浪的最佳避暑之地；秋天，起伏於山谷間的芒花與枝頭零落的紅葉為大地染上蕭瑟的色彩；寒冬中的疾風勁雨，
使偶爾露臉的冬陽或突如其來的一陣瑞雪都特別的令人驚喜；這豐富多變的面貌呈現出陽明山國家公園鮮明的四季景觀。`)
    });

    //動態生成行事曆
    function GetCalendar() {
        $.ajax({
            url: "/Travel/GetCalendar",
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
    GetCalendar();
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
            url: "/Travel/AutoComplete",
            type: "POST",
            success: function (data) {
                var getautoComplete = data.split(',');
                $("#contain").autocomplete({
                    source: getautoComplete
                });
            }
        });
    });


    //開團+編輯時間限制
    function TheDatePicker(index, dateLimitID) {       
        $.ajax({
            url: "/Travel/GetDateLimit",
            type: "POST",
            data: { "actID": dateLimitID },
            success: function (data) {
                if (data !== "")
                    timeLimit = data.split(',');
            }
        });
        $(".ActivityStart").eq(index).datepicker(
            {
                dateFormat: 'yy-mm-dd',
                monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                beforeShowDay: function (date) {
                    var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                    return [timeLimit.indexOf(string) === -1];
                },
                minDate: '0'
            }
        );
        if (index === "0") {
            $(".ActivityEnd").eq(index).datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                    beforeShowDay: function (date) {
                        var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                        return [timeLimit.indexOf(string) === -1];
                    },
                    minDate: '0'
                }
            );
            $(".ActivityFindEnd").eq(index).datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                    minDate: '0'
                }
            );
        } else {
            let NowDate = new Date(Date.parse($(".ActivityStart").eq(index).val()));
            theMonth = NowDate.getMonth() + 1;
            theDay = NowDate.getDate() /*NowDate.getDate() - 1*/;
            FormatTime();
            let deadLine = NowDate.getFullYear() + "-" + theMonth + "-" + theDay;

            let endLimit;
            let temp1, temp2;
            let i = 0;
            for (let item of timeLimit) {
                let itemDate = new Date(Date.parse(item));
                if (itemDate > NowDate) {
                    temp1 = itemDate;
                    i++;
                    if (temp2 > temp1 || i === 1) {
                        temp2 = temp1;
                    }
                }
            }
            endLimit = temp2;
            $(".ActivityEnd").eq(index).datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                    beforeShowDay: function (date) {
                        var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                        return [timeLimit.indexOf(string) === -1];
                    },
                    minDate: $(".ActivityStart").eq(index).val(), //因為有當天行程所以允許在同一天
                    maxDate: endLimit //maxDate 找最接近minDate的日期
                }
            );

            $(".ActivityFindEnd").eq(index).datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                    minDate: '0',
                    maxDate: deadLine //變為活動開始前一天，要原本的值-1
                }
            );
        }

    }
    

    //ajax取得readmore
    function get_ajax_readmore() {
        $.ajax({            
            url: "/Travel/GetReadMore",  
            async: false,         
            type: "POST",
            data: { "actID": readmore_target },
            success: function (data) {
                $("#add_ajax_readmore").html(data); //更新readmore項目     
                //$('#ajax_readmore').modal("show");
                TheDatePicker(1, readmore_target);
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
        $("#calendarEventGo").attr("act_id", readmore_target);
        $("#calendarEventGo").click();
        var combine = "[ToUpdateVC=" + readmore_target + "]";
        var getCounts = parseInt($(combine).html()) + 1;
        $(combine).html(getCounts);

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




    ////時間格式轉換
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
    });


    //揪團時間限制   
    $("body").on("change", ".ActivityStart", function () {
        $(".ActivityStartTo").attr("hidden", "");
        $(".ActivityEnd").val("");
        $(".ActivityFindEnd").val("");
        $(".ActivityEnd").removeAttr("disabled");
        $(".ActivityFindEnd").removeAttr("disabled");
        //$(".ActivityEnd").attr("min", $(this).val());
        //$(".ActivityFindEnd").attr("max", $(this).val());
        let index = $(this).attr("listNumber");

        //let themax = new Date(Date.parse($(this).val().replace(/-/g, "/"))); 日期字串dash轉斜線
        let NowDate = new Date(Date.parse($(this).val()));
        theMonth = NowDate.getMonth() + 1;
        theDay = NowDate.getDate() /*NowDate.getDate() - 1*/;
        FormatTime();
        let deadLine = NowDate.getFullYear() + "-" + theMonth + "-" + theDay;

        let endLimit;
        let temp1, temp2;
        let i = 0;
        for (let item of timeLimit) {            
            let itemDate = new Date(Date.parse(item));           
            if (itemDate > NowDate) {
                temp1 = itemDate;
                i++;
                if (temp2 > temp1 || i === 1) {
                    temp2 = temp1;
                }                   
            }
        }
        endLimit = temp2;
        $(".ActivityEnd").eq(index).datepicker('destroy');//重新建立
        $(".ActivityEnd").eq(index).datepicker(
            {
                dateFormat: 'yy-mm-dd',
                monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                beforeShowDay: function (date) {
                    var string = jQuery.datepicker.formatDate('yy-mm-dd', date);
                    return [timeLimit.indexOf(string) === -1];
                },
                minDate: $(this).val(), //因為有當天行程所以允許在同一天
                maxDate: endLimit //maxDate 找最接近minDate的日期
            }
        );

        $(".ActivityFindEnd").eq(index).datepicker('destroy');//重新建立
        $(".ActivityFindEnd").eq(index).datepicker(
            {
                dateFormat: 'yy-mm-dd',
                monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                minDate: '0',
                maxDate: deadLine //變為活動開始前一天，要原本的值-1
            }
        );
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
            TheDatePicker(0, 0);

        } else {
            //文字編輯器
            CKEDITOR.replace('f活動內容2', { height: 400, width: 1100 });
            TheDatePicker(1, readmore_target);
        }
        
        $(".NeedATTo").attr("hidden", "");
        $(".ActivityStartTo").attr("hidden", "");
        $(".ActivityEndTo").attr("hidden", "");
        $(".ActivityFindEndTo").attr("hidden", "");
        //nowdate = new Date(Date.now());
        //theMonth = nowdate.getMonth()+1; //js時間月份是索引值，所以現在月份要+1
        //theDay = nowdate.getDate()+2;
        //FormatTime();
        //$(".ActivityStart").attr("min", nowdate.getFullYear() + "-" + theMonth + "-" + theDay);        
        //$(".ActivityFindEnd").attr("max", "");
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
        
        if ($(".NeedAT").eq(target).val().length < 5) {
            e.preventDefault();
            $(".NeedATTo").eq(target).removeAttr("hidden");
        }
        if ($(".ActivityStart").eq(target).val() === "") {
            e.preventDefault();
            $(".ActivityStartTo").eq(target).removeAttr("hidden");
        }
        if ($(".ActivityEnd").eq(target).val() === "") {
            e.preventDefault();
            $(".ActivityEndTo").eq(target).removeAttr("hidden");
        }
        if ($(".ActivityFindEnd").eq(target).val() === "") {
            e.preventDefault();
            $(".ActivityFindEndTo").eq(target).removeAttr("hidden");
        }
        if ($(".NeedAC").eq(target).val() === "") {
            e.preventDefault();
            $(".NeedACTo").eq(target).removeAttr("hidden");
        }
        if ($(".NeedAP").eq(target).val() === "") {
            e.preventDefault();
            $(".NeedAPTo").eq(target).removeAttr("hidden");
        }
        if ($(".NeedAL").eq(target).val().length < 100) {
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
                url: "/Travel/LikeIt",
                type: "POST",
                data: { "actID": target }
            });
        } else {
            $(combine).attr("src", "/Content/images/14.png");
            $.ajax({
                url: "/Travel/LikeIt",
                type: "POST",
                data: { "actID": target }
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
        console.log("1")
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
    $("body").on('click', '[data-target="#ajax_msg"]', leaveMsg)
    //踢人
    function kickAct() {
        let target = $(this).attr("member_id");
        let id = $(this).attr("act_id");
        $.ajax({
            url: "/Travel/ActKick",
            type: "POST",
            data: { "targetMemberID": target,"actID":id },
            success: function (data) {
                if (data === "") {
                    window.confirm("不可以踢自己!");
                } else {
                    $("[ActAdd=" + id + "]").html(data);
                    window.confirm("踢除成功!");
                //GetCalendar(); 踢人行事曆不用動
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
                    GetCalendar();
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
            url: "/Travel/GetFeelGood",
            type: "POST",
            data: { "actID": target },
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


    //增加瀏覽次數，前後端分離，前端為假數字，後端直接計數
    function getViewCounts() {
        let target = $(this).attr("updateVC");
        var combine = "[ToUpdateVC=" + target + "]";
        var getCounts = parseInt($(combine).html()) + 1;
        $(combine).html(getCounts);
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
            "Order": order, "BackgroundColor": background_color, "Contain": contain
            , "Category": category, "Label": label , "Page":page
        });
        
        $.ajax({
            url: "/Travel/GetTravelList",
            type: "POST",
            data: { "condition": condition },
            success: function (data) {
                $("#article_ajax").html(data); 
                travel_RWD(); 
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
                $(this).append("<span id='temp'>近 → 遠</span>");
            }
            else if ($(this).attr('id') === "08") {
                $(this).append("<span id='temp'>快 → 慢</span>");
            }
            else if ($(this).attr('id') === "03") {
                $(this).append("<span id='temp'>北 → 南</span>");
            }
            else if ($(this).attr('id') === "02") {
                $(this).append("<span id='temp'>舊 → 新</span>");
            }
            else {
                $(this).append("<span id='temp'>低 → 高</span>");
            }
        }
        else {            
            this.style.backgroundColor = 'rgb(250, 224, 178)';
   
            $("#temp").remove();
            if ($(this).attr('id') === "07") {
                $(this).append("<span id='temp'>遠 → 近</span>");
            }
            else if ($(this).attr('id') === "08") {
                $(this).append("<span id='temp'>慢 → 快</span>");
            }
            else if ($(this).attr('id') === "03") {
                $(this).append("<span id='temp'>南 → 北</span>");
            }
            else if ($(this).attr('id') === "02") {
                $(this).append("<span id='temp'>新 → 舊</span>");
            }
            else {
                $(this).append("<span id='temp'>高 → 低</span>");
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

    TheDatePicker(0,0);

    travel_sort_scrollHandler();
})(); 