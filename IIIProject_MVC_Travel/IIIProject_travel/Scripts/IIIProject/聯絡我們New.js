function validateEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!re.test(String(email).toLowerCase())) {
        alert("請輸入正確的信箱");
        document.idform.txtMail.value = "";
    }
}
$("#fordemo").click(function () {
    $("#title").val("行程太過於攏長");
    $("#post聯絡人").val("十元瑞");
    $("#phone").val("0976703669");
    $("#comment").val("今天我們去六福村的時候，前前後後帶了我們進去禮品店購物了3次，真的是夠了!!");
   
});
