;(function(){
    function c檢查聯絡人() {
        let the聯絡人物件 = document.getElementById("聯絡人");
        let the聯絡人的值 = the聯絡人物件.value;
        let the聯絡人 = document.getElementById("聯絡人").value;
        let span聯絡人 = document.getElementById("idcp");
        let the聯絡人長度 = the聯絡人.length;
        let flagac1 = false;
    
        if (the聯絡人 == "")
            span聯絡人.innerHTML = "*請輸入";
    }
    function c檢查帳號() {
        let the帳號物件 = document.getElementById("帳號");
        let the帳號的值 = the帳號物件.value;
        let the帳號 = document.getElementById("帳號").value;
        let span帳號 = document.getElementById("idac");
        let the帳號長度 = the帳號.length;
        let flagac1 = false;
    
        if (the帳號 == "")
            span帳號.innerHTML = "*請輸入";
    }
    function c檢查電話() {
        let the電話物件 = document.getElementById("電話");
        let the電話的值 = the電話物件.value;
        let the電話 = document.getElementById("電話").value;
        let span電話 = document.getElementById("idtal");
        let the電話長度 = the電話.length;
        let flagac1 = false;
    
        if (the電話 == "")
            span電話.innerHTML = "*請輸入";
    }
    function c檢查mail() {
        let themail物件 = document.getElementById("mail");
        let themail的值 = themail物件.value;
        let themail = document.getElementById("mail").value;
        let spanmail = document.getElementById("idmail");
        let themail長度 = themail.length;
        let flagac1 = false;
    
        if (themail == "")
            spanmail.innerHTML = "*請輸入";
    }
    function c檢查意見() {
        let the意見物件 = document.getElementById("意見");
        let the意見的值 = the意見物件.value;
        let the意見 = document.getElementById("意見").value;
        let span意見 = document.getElementById("idop");
        let the意見長度 = the意見.length;
        let flagac1 = false;
    
        if (the意見 == "")
            span意見.innerHTML = "*請輸入";
    }
})();
