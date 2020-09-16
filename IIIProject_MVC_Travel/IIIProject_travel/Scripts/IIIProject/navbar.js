; (function () {
    let nav = document.querySelector("#main");
    let navPosition = nav.offsetTop;
    console.log(navPosition);
    
    function scrollHandler() {
        if (window.scrollY >= navPosition) {
            //  fixed menu
            document.body.classList.add("fixedMenu");
            document.body.style.paddingTop = nav.offsetHeight + "px";
        }else {
            //  normal
            document.body.classList.remove("fixedMenu");
            document.body.style.paddingTop = "";
        }
    }

    window.addEventListener("scroll", scrollHandler);
    window.addEventListener("resize", () => {
        navPosition = nav.offsetTop;
    });
})();