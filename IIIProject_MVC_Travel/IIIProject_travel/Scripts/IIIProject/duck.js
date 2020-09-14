gsap.registerPlugin(CSSPlugin);

var maintl;

(function () {

    maintl = new TimelineMax({ repeat: -1 });

    var width = 1220;
    var height = 200;
    var increments = 50;
    var incrementSize = width / increments;
    var speed = .2;

    var props = {};
    props.var1 = 0;
    props.var2 = -10;
    props.var3 = 50;
    props.var4 = -15;
    props.var5 = 10;
    props.var6 = 3;

    var path;
    var path2;

    maintl.add("frame0", 0)
        .add(TweenMax.delayedCall(speed, updatePath))
        .seek(0);


    function updatePath() {
        path = "M";
        path2 = "M";
        path += "-10," + height * .5;
        path2 += "-10," + height * .5;

        var points = [];
        var points2 = [];
        var i;
        for (i = -1; i < increments + 2; i++) {
            points[i] = {};
            points2[i] = {};
            points[i].x = i * incrementSize;
            points2[i].x = i * incrementSize;

            var variant1 = (Math.sin(i * .14 + props.var1) * 9);
            var variant2 = (Math.cos(i * .2 + props.var2) * 13) - 9;
            var variant3 = (Math.cos(i * .15 + props.var3) * 10) - 5;
            var variant4 = (Math.sin(i * .25 + props.var4) * 8) - 3;
            var variant5 = (Math.cos(i * .21 + props.var5) * 6) - 5;
            var variant6 = (Math.cos(i * .17 + props.var6) * 11) - 7;

            points[i].y = height * .5 + variant4 - variant5 + variant6;
            points2[i].y = height * .5 + variant1 - variant2 + variant3;

            if (i == 25) {
                var rotate = 1;
                if (points[i - 1].y < points[i].y) {
                    rotate = -1;
                }
                TweenMax.to(jQuery('.duck'), speed, { rotation: "+=" + rotate, y: points[i].y - 150, ease: Power0.easeNone });
            }
        }

        for (i = 0; i < increments + 2; i++) {
            var anchor1 = {};
            anchor1.x = (points[i - 1].x + points[i].x) * .5;
            anchor1.y = (points[i - 1].y + points[i].y) * .5;
            path += "Q" + points[i - 1].x + "," + points[i - 1].y + " " + anchor1.x + "," + anchor1.y;

            var anchor2 = {};
            anchor2.x = (points2[i - 1].x + points2[i].x) * .5;
            anchor2.y = (points2[i - 1].y + points2[i].y) * .5;
            path2 += "Q" + points2[i - 1].x + "," + points2[i - 1].y + " " + anchor2.x + "," + anchor2.y;
        }

        path += "L" + width + "," + height + "L-10," + height;
        path2 += "L" + width + "," + height + "L-10," + height;



        TweenMax.set(props, { var1: "+=" + .8 * speed, var2: "+=" + 1.4 * speed, var3: "-=" + 1.2 * speed, var4: "+=" + .8 * speed, var5: "+=" + 1.4 * speed, var6: "-=" + 1.3 * speed }, "frame0");
        TweenMax.to('.wave', speed, { morphSVG: { d: path }, ease: Power0.easeNone }, "frame0");
        TweenMax.to('.wave2', speed, { morphSVG: { d: path2 }, ease: Power0.easeNone }, "frame0");

    }
    updatePath();

})();