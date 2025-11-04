$(document).ready(function () {
    // Inicialização do Select2 e Datepickers (assumindo que você usa)
    console.log("SalesRecords-Index CARREGADO!")
    $('.select2').select2();
    $('#minDate, #maxDate').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });
    tablePlugins()

    // 1. Lógica de Filtro (Busca de Vendas)
    $('#search-form').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var url = form.attr('action') || window.filterUrl;
        var data = form.serialize();

        $('#loading-spinner').removeClass('d-none');

        $.ajax({
            type: 'GET',
            url: url,
            data: data,
            success: function (partialViewHtml) {
                $('#salesResults').html(partialViewHtml);
                $('#loading-spinner').addClass('d-none');
                tablePlugins()
                // Abre o acordeão se o filtro foi aplicado (se tiver ViewBag.ShouldOpenAccordion)
                var accordion = $('#collapseOne');
                if (accordion.data('open') === 'true') {
                    accordion.collapse('show');
                }
            },
            error: function () {
                alert('Erro ao realizar a busca de vendas.');
                $('#loading-spinner').addClass('d-none');
            }
        });
    });


    let currentSaleId = null;
    let currentActionUrl = null;
    let currentActionName = null;
    let currentToken = null;

    // 2. Lógica de Ação AJAX (Bill/Cancel)
    $('#salesResults').on('click', '.action-bill, .action-cancel', function () {
        console.log("Botão Bill/Cancel clicado");
        var button = $(this);

        currentSaleId = button.data('id');
        var isBill = button.hasClass('action-bill');
        currentActionName = isBill ? 'Bill' : 'Cancel';
        currentActionUrl = isBill ? '/SalesRecords/Bill' : '/SalesRecords/Cancel';
        currentToken = $('input[name="__RequestVerificationToken"]').val();


        // Preenche o modal de confirmação
        $('#confirmationModal .modal-body #confirmationMessage').text(`Are you sure you want to ${currentActionName} the sell Nº ${currentSaleId}?`);

        // Muda a cor do botão 'Yes' dependente da ação
        $('#btnConfirmAction').removeClass('btn-success btn-danger')
            .addClass(isBill ? 'btn-success' : 'btn-danger')
            .text(`Yes, ${currentActionName}`);

        // Mostra o modal
        var confirmationModal = new bootstrap.Modal(document.getElementById('confirmationModal'));
        confirmationModal.show();
    });

    // Adicionar o evento de clique ao botão de confirmação no modal
    $('#btnConfirmAction').off('click').on('click', function () {
        //Esconde o modal de confirmação
        bootstrap.Modal.getInstance(document.getElementById('confirmationModal')).hide();
        console.log("Confirmação recebida para ação:", currentActionName, "na venda ID:", currentSaleId);
        // Só prossegue se todas as variáveis estiverem definidas
        if (currentSaleId && currentActionUrl && currentActionName) {
            executeActionAjax(currentSaleId, currentActionUrl, currentActionName, currentToken);
        };

    });




    function executeActionAjax(saleId, actionUrl, actionName, token) {
        $('#loading-spinner').removeClass('d-none');

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: {
                id: saleId,
                __RequestVerificationToken: token
            },
            success: function (response) {
                console.log('Sucesso no AJAX')
                if (response.success) {
                       showNotificationModal('Sucesso', response.message, 'success'); 

                    // RECARREGA OS RESULTADOS DO FILTRO ATUAL (Para atualizar a tabela)
                    $('#search-form').trigger('submit');
                    tablePlugins()

                } else {
                    showNotificationModal('Falha na Ação', `Falha ao ${actionName}: ${response.message}`,
                        'error'); 
                }
            },
            error: function (xhr) {
                var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : `Erro desconhecido ao ${actionName}.`; 
            },
            complete: function () {
                $('#loading-spinner').addClass('d-none');
            }
        });
    }

    // 3. Lógica de Limpar Campos (já existente, mantida)
    $('#limparCampos').click(function () {
        $('#search-form')[0].reset();
        $('.select2').val(null).trigger('change');
    });
});


function showNotificationModal(title, message, type) {
    const modalTitle = $('#notificationModalLabel');
    const modalBody = $('#notificationMessage');
    const modalHeader = $('#notificationModal .modal-header');

    // 1. Limpa classes e define o conteúdo 
    modalTitle.html(title);
    modalBody.text(message);
    modalHeader.removeClass('bg-success bg-danger bg-warning');

    // 2. Aplica cores e ícones baseados no tipo 
    if (type === 'success') {
        modalTitle.prepend('<i class="fas fa-check-circle me-2"></i>');
        modalHeader.addClass('bg-success text-white');
    } else if (type === 'error') {
        modalTitle.prepend('<i class="fas fa-times-circle me-2"></i>');
        modalHeader.addClass('bg-danger text-white');
    } else {
        modalTitle.prepend('<i class="fas fa-exclamation-triangle me-2"></i>');
        modalHeader.addClass('bg-warning text-dark');
    }

    // 3. Mostra o modal 
    var notificationModal = new bootstrap.Modal(document.getElementById('notificationModal'));
    notificationModal.show();
}
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
            { orderable: false, targets: 5 }
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