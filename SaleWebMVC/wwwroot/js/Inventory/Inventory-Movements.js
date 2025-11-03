$(document).ready(function () {
    console.log("Inventory-Movements.js CARREGADO!")
    loadMovementHistory();
    tablePlugins();
});

//#region DataTable Plugins
function tablePlugins() {
    $('.table').DataTable({
        dom: '<"d-flex justify-content-between mb-6"lfB>rtip',
        buttons: [
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-success btn-sm'
            },
            {
                extend: 'pdf',
                text: 'Export to PDF',
                className: 'btn btn-danger btn-sm'
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-secondary btn-sm'
            }
        ],
        columnDefs: [
            { orderable: true}
        ],
        language: {
            search: "",
            searchPlaceholder: "🔍 Search...",
            lengthMenu: "Show _MENU_ records",
            paginate: {
                previous: "← Prev",
                next: "Next →"
            },
            info: "Showing _START_ to _END_ of _TOTAL_ records"
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        lengthChange: true,
        searching: true,
        info: true,
        rowReorder: true,
        initComplete: function () {
            $('.dataTables_filter input')
                .addClass('form-control form-control-sm d-inline-block ms-2')
            // ...
            $('.dataTables_length select')
                .addClass('form-select form-select-sm d-inline-block ms-2');
        }
    });
}
//#endregion



//#region load Movement History

function loadMovementHistory() {
    $.ajax({
        url: '/Inventory/GetMovementHistoryTable',
        type: 'GET',
        dataType: 'html',
        success: function (data) {
            console.log("Deu certo o AJAX History")
            $('#movementHistoryTableContainer').html(data); 
            tablePlugins();
        },
        error: function (xhr, status, error) {
            $('#movementHistoryTableContainer').html('<div class="alert alert-danger"> Erro at:' + error + '</div >'); 
        }
    });
    console.log("History AJAX disparado."); // NOVO log de controle
}
//#endregion