$(document).ready(function () {
    console.log("Carregou a o js!")
    applyPlugins();
    $('.select2').select2();

    // Evento de Mudança no Dropdown de Estados
    $('#StateId').change(function () {
        var stateId = $(this).val();
        var cityDropdown = $('#Supplier_CityId');

        cityDropdown.empty().attr('disabled', 'disabled');
        cityDropdown.append('<option value="">Carregando cidades...</option>');
        console.log("cCLIQUE FUNCIONA, Id do estado", stateId)

        if (stateId) {
            // 1. Chamar um novo endpoint AJAX no Controller
            $.ajax({
                url: '/api/Location/CitiesByState', // Você precisará criar este endpoint
                type: 'GET',
                data: { stateId: stateId },
                success: function (cities) {
                    console.log("SUCESSO!",cities)
                    cityDropdown.empty().removeAttr('disabled');
                    cityDropdown.append('<option value="">-- Choose the city --</option>');

                    $.each(cities, function (i, city) {
                        cityDropdown.append($('<option>', {
                            value: city.id,
                            text: city.name
                        }));
                    });
                    cityDropdown.select2(); // Re-inicializa o Select2
                },
                error: function () {
                    cityDropdown.empty().append('<option value="">Error at searching for cities.</option>');
                }
            });
        }
    });
});


function applyPlugins() {
    $('#suppliersTable').DataTable({
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
            { orderable: false, targets: 6 }
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
};