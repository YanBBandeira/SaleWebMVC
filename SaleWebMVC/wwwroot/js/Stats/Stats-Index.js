$(document).ready(function () {

    applyPlugins();

    $('.select2').select2({
        allowClear: true,
        width: '150%'
    });
    $('#minDate, #maxDate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const dataElement = document.getElementById("all-data");

    const monthLabels = JSON.parse(dataElement.dataset.monthlabels);
    const sellerLabels = JSON.parse(dataElement.dataset.sellerlabels);
    const statusLabels = JSON.parse(dataElement.dataset.statuslabels);
    const salesByMonth = JSON.parse(dataElement.dataset.salesbymonth);
    const salesBySeller = JSON.parse(dataElement.dataset.salesbyseller);

    const statusTotals = JSON.parse(dataElement.dataset.statustotals);

    console.log(salesByMonth)


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
});







$(document).on('click', '#limparCampos', function () {
    LimparCampos();
});

function LimparCampos() {
    $('#departmentIds').val(null).trigger('change');
    $('#sellerIds').val(null).trigger('change');
    $('#minDate').val('');
    $('#maxDate').val('');
}



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
