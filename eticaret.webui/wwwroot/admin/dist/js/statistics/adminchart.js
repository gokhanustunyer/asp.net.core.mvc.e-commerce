function dynamicColors() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgba(" + r + "," + g + "," + b + ", 0.5)";
}

function poolColors(a) {
    var pool = [];
    for (i = 0; i < a; i++) {
        pool.push(dynamicColors());
    }
    return pool;
}

function drawBarChartByJson(title, canvasId, jsonData) {
    const ctx = document.getElementById(canvasId);
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: Object.keys(jsonData),
            datasets: [{
                label: title,
                data: Object.values(jsonData),
                backgroundColor: "#36bea6bd",
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            plugins: {
                title: {
                    display: true,
                }
            },
        }
    });
}

function drawLineChartByJson(title, canvasId, jsonData, color = "#7460ee") {
        const ctx = document.getElementById(canvasId);
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: Object.keys(jsonData),
                datasets: [{
                    label: title,
                    data: Object.values(jsonData),
                    fill: true,
                    backgroundColor: color+"63",
                    borderColor: color,
                    tension: 0.2,
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                    },
                    suggestedMin: -10,
                    suggestedMax: 200
                },
                plugins: {
                    title: {
                        display: true,
                    }
                },

            }
        });
}

function drawPieChartByJson(title, jsonData, canvasId) {
    const ctx = document.getElementById(canvasId);
    new Chart(ctx, {
        type: 'polarArea',
        data: {
            labels: Object.keys(jsonData),
            datasets: [{
                label: title,
                data: Object.values(jsonData),
                borderWidth: 1,
                backgroundColor: poolColors(Object.keys(jsonData).length),
            }],
            hoverOffset: 4,
        },
    });
}
