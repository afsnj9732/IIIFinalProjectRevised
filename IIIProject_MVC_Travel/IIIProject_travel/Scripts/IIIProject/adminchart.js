//線圖
$(document).ready(function () {
    console.log("333");
    $.getJSON("/後台Home/GetLineChartData", function (data) {
        var date = [];
        var Qts = [];
        for (var i = 0; i < data.length; i++) {
            date.push(data[i].name);
            Qts.push(data[i].count);
        }

        Highcharts.chart('chart-line', {
            chart: {
                type: 'line'
            },
            title: {
                text: '瀏覽趨勢'
            },
            subtitle: {
                text: 'Jouta Data'
            },
            xAxis: {
                categories: date
            },
            yAxis: {
                title: {
                    text: '瀏覽次數'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: '瀏覽次數 v.s 時間',
                data: Qts
            }]
        });
    });
});

//圓餅圖
$(document).ready(function () {

    $.getJSON("/後台Home/GetPieChartData", function (data) {
        Highcharts.chart('chart-pie', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: '區域分布'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            accessibility: {
                point: {
                    valueSuffix: '%'
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                    }
                }
            },
            series: [{
                name: '區域比重分布圖',
                colorByPoint: true,
                data: [{
                    name: '北部',
                    y: data.North
                }, {
                    name: '南部',
                    y: data.South
                }, {
                    name: '東部',
                    y: data.East
                }]
            }]
        });
    });
});

//Bar圖
$(document).ready(function () {
    console.log("333");
    $.getJSON("/後台Home/GetBarChartData", function (data) {
        var date = [];
        var Qts = [];
        for (var i = 0; i < data.length; i++) {
            date.push(data[i].name);
            Qts.push(data[i].count);
        }

        Highcharts.chart('chart-bar', {
            chart: {
                type: 'bar'
            },
            title: {
                text: '會員評分'
            },
            subtitle: {
                text: 'Jouta Data'
            },
            xAxis: {
                categories: date
            },
            yAxis: {
                title: {
                    text: '會員名稱'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: '會員名稱 v.s 總分',
                data: Qts
            }]
        });
    });
});


//Column圖
$(document).ready(function () {
    console.log("333");
    $.getJSON("/後台Home/GetColumnChartData", function (data) {
        var date = [];
        var Qts = [];
        for (var i = 0; i < data.length; i++) {
            date.push(data[i].name);
            Qts.push(data[i].count);
        }

        Highcharts.chart('chart-column', {
            chart: {
                type: 'column'
            },
            title: {
                text: '個人頁瀏覽次數'
            },
            subtitle: {
                text: 'Jouta Data'
            },
            xAxis: {
                categories: date
            },
            yAxis: {
                title: {
                    text: '瀏覽次數'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: '瀏覽次數 v.s會員名稱',
                data: Qts
            }]
        });
    });
});