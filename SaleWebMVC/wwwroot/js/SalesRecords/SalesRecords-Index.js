$(document).ready(function () {
    applyPlugins();
});


$(document).on('click', '#limparCampos', function () {
    LimparCampos();
});

$(document).on('submit', '#search-form', function (e) {
    e.preventDefault(); // Evita o envio padrão do formulário

    const form = $(this);
    const url = form.attr('action') || window.location.pathname + '/Filter'; // Usa a URL do formulário ou a URL atual

    const data = form.serialize(); // Serializa os dados do formulário

    $.ajax({
        url: url,
        type: 'GET',
        data: data,
        success: function (partialViewHtml) {
            $('#salesResults').html(partialViewHtml); // Atualiza a seção de resultados com a Partial View retornada
            applyPlugins();

            setTimeout(function () {
                const $table = $('#salesTable');

                if ($table.length) {
                    $('html, body').animate({
                        scrollTop: $table.offset().top - 100 // rola até um pouco acima 
                    }, 400, function () {
                        $table.focus(); // foco visual
                    });
                }
            }, 100);

        },
        error: function () {
            alert('Erro ao buscar os registros de vendas.');
        }
    });
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


function applyPlugins() {
    $('#salesTable').DataTable({
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
                .attr('placeholder', '🔍 Search...');

            $('.dataTables_length select')
                .addClass('form-select form-select-sm d-inline-block ms-2');
        },
        drawCallback: function () {
            $('.dataTables_paginate .paginate_button')
                .addClass('btn btn-sm btn-outline-info mx-1');

            $('.dataTables_paginate .paginate_button.current')
                .removeClass('btn-outline-info')
                .addClass('btn-primary text-white');
        }
    });
    $('.select2').select2({
        allowClear: true,
        width: '150%'
    });
    $("#minDate").datepicker();
    $("#maxDate").datepicker();
};





