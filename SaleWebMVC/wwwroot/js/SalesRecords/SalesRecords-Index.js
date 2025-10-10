$(document).ready(function () {
    // Função genérica para envio via AJAX
    function submitFormAjax(formId) {
        let form = $(formId);
        let url = form.attr('action');
        let data = form.serialize();

        $.ajax({
            type: 'GET',
            url: url,
            data: data,
            success: function (partialViewResult, status, xhr) {
                $('#salesResults').html(partialViewResult);
            },
            error: function () {
                alert('Erro ao buscar resultados.');
            }
        });
    }

    // Captura o submit dos dois formulários
    $('#simpleSearchForm').submit(function (e) {
        e.preventDefault();
        submitFormAjax('#simpleSearchForm');
    });

    $('#groupSearchForm').submit(function (e) {
        e.preventDefault();
        submitFormAjax('#groupSearchForm');
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


