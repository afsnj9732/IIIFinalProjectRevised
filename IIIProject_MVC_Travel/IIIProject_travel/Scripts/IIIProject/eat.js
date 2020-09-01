(function () {
    $(".filterbtn").click(function () {
        $(".btn-outline-warning").toggle('fast');
    });

    $(".popularMemberbtn").click(function () {
        $(".popularAvatar").toggle('fast');
    });

    function getAJAX() {
        p = JSON.stringify({
            "order": order, "background_color": background_color, "contain": contain
            , "category": category, "label": label
        });

        $.ajax({
            async: false,
            url: "/Eat/eatArticleAjax",
            type: "POST",
            data: {
                "p": p
            },
            success: function (data) {
                $("#eatArticleAjax div").remove();
                $("#eatArticleAjax").append(data);
            }
        });
    };

})();