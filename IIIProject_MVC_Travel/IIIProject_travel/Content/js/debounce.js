; (function () {
  let imgs = document.querySelectorAll("img");
  function scrollHandler() {
    let windowTop = window.scrollY;
    let windowBottom = windowTop + window.innerHeight;

    imgs.forEach(img => {
      // if (img.offsetTop < windowBottom) { //  照片最頂端進入畫面時，圖片就進入
      //   img.classList.add("active");
      // }

      // if (img.offsetTop + img.height < windowBottom) { //  照片最末端進入畫面時，圖片就進入
      //   img.classList.add("active");
      // }
      let imgMid = img.offsetTop + img.height / 2;
      if (imgMid < windowBottom && imgMid > windowTop) { //  照片中間段進入畫面時，圖片就進入
        img.classList.add("active");
      } else {
        img.classList.remove("active");
      }
    });
  }
  window.addEventListener("scroll", scrollHandler);
})();