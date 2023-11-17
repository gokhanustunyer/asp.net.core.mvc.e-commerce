class AdminStatistics {

    constructor()
    {
        this.loadCharts();
    }

    loadCharts() {
        var date = new Date();
        var month = date.getMonth();
        var year = date.getFullYear();
        var formData = new FormData();
        formData.append("month", month+1);
        formData.append("year", year);
        $.ajax({
            type: "POST",
            data: formData,
            url: "/admin/GetStatistics",
            contentType: false,
            processData: false,
            success: (response) => {
                document.getElementById("dailyRegister").innerHTML = response["dailyRegisterCount"];
                document.getElementById("dailtyVisitCount").innerHTML = `${response["dailyVisitCount"]}`;
                document.getElementById("dailyOrderCount").innerHTML = `${response["dailyOrderCount"]}`;
                document.getElementById("dailySalary").innerHTML = `${response["dailySalary"]} TL`;
                drawLineChartByJson("Toplam Satış Tutarı", "salePrices", response.totalSalesPriceByDay);
                drawLineChartByJson("Toplam Satış Miktari", "saleCounts", response.totalSalesCountByDay, "#00b0c7");
                drawBarChartByJson("Ziyaret Sayısı", "visitCountWeek", response.visitCountForaWeek);
                drawPieChartByJson("Toplam Satın Alım (Adet)",
                    response.orderCountByCategoryName, "salesByCategoryCount");
                drawPieChartByJson("Toplam Satın Alım (Ciro)",
                    response.orderPricesByCategoryName, "salesByCategoryPrice");
            }
        });
    };
}


const url = 'https://unpkg.com/world-atlas@2.0.2/countries-50m.json';
fetch(url).then((result) => result.json()).then((datapoint) => {
    const countries = ChartGeo.topojson.feature(datapoint, datapoint.objects.countries).features;
    const data = {
        labels: countries.map(country => country.properties.name),
        datasets: [{
            label: 'Countries',
            data: countries.map(country => ({ feature: country, value: (country.properties.name == "Turkey") ? 108 : 0 })),
        }]
    };

    const config = {
        type: 'choropleth',
        data,
        options: {
            showGraticule: true,
            showOutline: true,
            scales: {
                xy: {
                    projection: 'equalEarth'
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    };
    new Chart(
        document.getElementById('mapChart'),
        config
    );
});