$(document).ready(function () {

    applyPlugins();
    Graphs();

    
});












function applyPlugins() {
    $('#sellerTable').DataTable({
        dom: '<"d-flex justify-content-between mb-6"lfB>rtip',
        buttons: [],
        columnDefs: [
            { orderable: true}
        ],
        pageLength: 5,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        lengthChange: true,
        searching: false,
        info: true,
        rowReorder: true
    });
};


function Graphs() {

    const dataScript = document.getElementById("chart-data");
    if (!dataScript) {
        console.warn("⚠️ Dados de gráfico (#chart-data) não encontrados.");
        return;
    }

    let chartData;
    try {
        chartData = JSON.parse(dataScript.textContent);
    } catch (error) {
        console.error("❌ Erro ao fazer parse do JSON:", error);
        return;
    }

    // Extrai os dados
    const monthLabels = chartData.monthLabels || [];
    const sellerLabels = chartData.sellerLabels || [];
    const statusLabels = chartData.statusLabels || [];
    const deptLabels = chartData.deptLabels || [];

    const salesByMonth = chartData.salesByMonth || {};
    const salesBySeller = chartData.salesBySeller || {};
    const salesByDept = chartData.salesByDept || {};
    const statusTotals = chartData.statusTotals || {};

    console.log("📊 Dados carregados:", chartData);
    console.log(Object.keys(chartData));


    console.log(salesByDept);
    //console.log(dataElement.dataset);

    // Paleta de cores
    const colors = [
        "rgba(0, 0, 255, 1)",
        "rgba(0, 255, 0, 1)",
        "rgba(255, 0, 0, 1)"
    ];

    //#region --- 🗓️ Sales by Month ---

    function buildMonthDatasets(statusLabels, salesData) {
        return statusLabels.map((status, i) => ({
            label: status,
            data: salesData[status] || [],
            borderColor: colors[i % colors.length],
            backgroundColor: colors[i % colors.length].replace("1)", "0.3)"),
            fill: true,
            tension: 0
        }));
    }
    const totalByMonth = monthLabels.map((_, i) =>
        statusLabels.reduce((sum, s) => sum + (salesByMonth[s]?.[i] || 0), 0)
    );

    const monthDatasets = buildMonthDatasets(statusLabels, salesByMonth);
    console.log("Antes do push", monthDatasets);
    // Adiciona o total como uma linha preta opcional
    monthDatasets.push({
        label: "Total Sales",
        data: totalByMonth,
        borderColor: "rgba(0, 0, 0, 0.9)",
        backgroundColor: "rgba(0, 0, 0, 0.2)",
        borderWidth: 2,
        type: "line",
        fill: true,
        tension: 0
    });
    console.log("Após o push", monthDatasets);


    const ctxMonth = document.getElementById("salesByMonthChart").getContext("2d");
    new Chart(ctxMonth, {
        type: "line",
        data: {
            labels: monthLabels,
            datasets: monthDatasets
        },
        options: {
            responsive: true,
            plugins: {
                title: { display: true, text: "📈 Sales by Month" }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: value => "R$ " + value.toLocaleString()
                    }
                }
            }
        }
    });
    //#endregion


    //#region --- 👤 Sales by Seller ---
    function buildSellerDatasets(statusLabels, salesData) {
        return statusLabels.map((status, i) => ({
            label: status,
            data: salesData[status] || [],
            borderColor: colors[i % colors.length],
            backgroundColor: colors[i % colors.length].replace("1)", "0.7)"),
            fill: true,
            tension: 0
        }));
    }


    const ctxSeller = document.getElementById("salesBySellerChart").getContext("2d");

    new Chart(ctxSeller, {
        type: "bar",
        data: {
            labels: sellerLabels,
            datasets: buildSellerDatasets(statusLabels, salesBySeller)
        },
        options: {
            plugins: {
                title: { display: true, text: " 📈 Sales by Seller" }
            },
            responsive: true,
            scales: {
                x: { stacked: true },
                y: { stacked: true, beginAtZero: true }
            }
        }
    });
    //#endregion


    //#region --- 👤 Sales by Department ---
    function buildDeptDatasets(statusLabels, salesData) {
        return statusLabels.map((status, i) => ({
            label: status,
            data: salesData[status] || [],
            borderColor: colors[i % colors.length],
            backgroundColor: colors[i % colors.length].replace("1)", "0.7)"),
            fill: true,
            tension: 0
        }));
    }


    const ctxDept = document.getElementById("salesByDeptChart").getContext("2d");

    new Chart(ctxDept, {
        type: "bar",
        data: {
            labels: deptLabels,
            datasets: buildSellerDatasets(statusLabels, salesByDept)
        },
        options: {
            plugins: {
                title: { display: true, text: " 📈 Sales by Department" }
            },
            responsive: true,
            scales: {
                x: { stacked: true },
                y: { stacked: true, beginAtZero: true }
            }
        }
    });
    //#endregion
}
