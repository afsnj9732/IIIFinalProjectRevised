function validateEmail(email) {
    const re = /([a-zA-Z0-9]+)([\.{1}])?([a-zA-Z0-9]+)\@gmail([\.])com/;

    if ($("#email").val()=="") {
        alert("必填欄位");
    }
    else if (!re.test(String(email).toLowerCase())) {
        alert("請使用gmail信箱");
        $(document).ready(function () {
            $('#email').val('');
        });
        
    }
}
$("#fordemo").click(function () {
    var female = "";
    $("#sexbtn2").attr("checked", "checked");
    $("#title").val("行程太過於攏長");
    $("#post聯絡人").val("十元瑞");
    $("#phone").val("0976703669");
    $("#comment").val("今天我們去六福村的時候，前前後後帶了我們進去禮品店購物了3次，真的是夠了!!");
});
