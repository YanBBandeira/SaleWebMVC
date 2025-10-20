$(document).ready(function () {
    const collapseElement = document.getElementById('collapseOne');

    if (collapseElement) {
        const shouldOpen = collapseElement.getAttribute('data-open') === 'true';

        if (shouldOpen) {
            const bsCollapse = new bootstrap.Collapse(collapseElement, {
                toggle: false
            });
            bsCollapse.show();
        }
    }

    $('#salesTable').DataTable({
        dom: '<"d-flex justify-content-between mb-3"lfB>rtip',
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
                .attr('placeholder', '🔍 Buscar...');

            $('.dataTables_length select')
                .addClass('form-select form-select-sm d-inline-block ms-2');
        },
        drawCallback: function () {
            $('.dataTables_paginate .paginate_button')
                .addClass('btn btn-sm btn-outline-primary mx-1');

            $('.dataTables_paginate .paginate_button.current')
                .removeClass('btn-outline-primary')
                .addClass('btn-primary text-white');
        }
    });
    $('.select2').select2({
        placeholder: "Select departments",
        allowClear: true,
        width: '150%'
    });
});




$(function () {
    $("#minDate").datepicker();
    $("#maxDate").datepicker();
});



$(document).on('click', '#limparCampos', function () {
    LimparCampos();
});


// Evento de click para abrir modal de detalhes
$(document).on('click', '.view-details', function (e) {
    e.preventDefault();

    const saleId = $(this).data('id');

    $.ajax({
        url: detailsUrl,//'@Url.Action("Details", "SalesRecords")', // Ajuste o controller se necessário
        data: { id: saleId },
        type: 'GET',
        success: function (result) {
            $('#detailsModalBody').html(result);
            $('#detailsModal').modal('show');
        },
        error: function () {
            alert('Erro ao carregar os detalhes.');
        }
    });
});

function LimparCampos() {
    $('#Seller').val('');
    $('#minDate').val('');
    $('#maxDate').val('');
    $('#DepartmentIds').val(null).trigger('change');
    $('#salesStatusIds').val(null).trigger('change');
}








