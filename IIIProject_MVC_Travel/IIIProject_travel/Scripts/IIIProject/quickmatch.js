(function () {

    //$('.resultTrigger').click(function () {
        //$('section').removeClass('active');
        //$('section').addClass('active');
        //$('html,body').animate({ scrollTop: $(document).height() }, 1000);
    //});

    $('#f1').click(function () {
        $(this).removeClass('clicked');
        $(this).addClass('clicked');
        $('#f2').removeClass('clicked');
    });

    $('#f2').click(function () {
        $(this).removeClass('clicked');
        $(this).addClass('clicked');
        $('#f1').removeClass('clicked');
    });

})();