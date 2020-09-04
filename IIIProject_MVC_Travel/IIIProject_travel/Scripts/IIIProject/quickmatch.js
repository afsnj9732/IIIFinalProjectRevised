(function () {

    $('.resultTrigger').click(function () {
        $('section').removeClass('active');
        $('section').addClass('active');
        $('html,body').animate({ scrollTop: $(document).height() }, 1000);
    });

    $('#f1').click(function () {
        alert('f1');
    });

    $('#f2').click(function () {
        alert('f2');
    });

})();