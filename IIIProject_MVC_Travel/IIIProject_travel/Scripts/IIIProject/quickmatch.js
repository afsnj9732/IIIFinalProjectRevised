; (function () {
    $('.resultTrigger').click(function () {
        var scrollBottom = $(document).height() - $(window).height() - $(window).scrollTop();

        $('section').removeClass('active');
        $('section').addClass('active');
        $('html,body').animate({ scrollTop: $(document).height() }, 1000);
    });
})();