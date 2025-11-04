
// Função para atualizar os índices dos campos (crucial para o Model Binder)
function updateItemIndexes() {
    var container = $('#order-items-container');
    var rows = container.find('.order-item-row');

    rows.each(function (index) {
        // Atualiza o índice nos atributos 'name' e 'data-valmsg-for'
        $(this).find(':input, span[data-valmsg-for]').each(function () {
            $.each(this.attributes, function (i, attr) {
                if (attr.specified) {
                    // Regex mais seguro: substitui OrderItems[NOME_DO_CAMPO] por OrderItems[index].NOME_DO_CAMPO
                    attr.value = attr.value.replace(/OrderItems\[\d*\]./g, 'OrderItems[' + index + '].');
                }
            });
        });
    });
    $('#item-count').text(rows.length);
}

function addNewItem() {
    var newIndex = $('#order-items-container').find('.order-item-row').length;
    var template = getOrderItemTemplate();
    var newRowHtml = template.replace(/_INDEX_/g, newIndex);

    $('#order-items-container').append(newRowHtml);

    // Revalida o formulário para incluir os novos campos
    var form = $('#createForm');
    form.data('validator', null);
    $.validator.unobtrusive.parse(form);

    updateItemIndexes();
}

$(document).ready(function () {

    // 1. Inicializa: Garante que os índices estejam corretos e a data atual
    updateItemIndexes();

    let today = new Date().toISOString().split('T')[0];
    $('input[name="SalesRecord.Date"]').val(today);

    // Garante que o status é PENDING (regra do Service)
    $('select[name="SalesRecord.Status"]').val('@SalesStatus.Pending').prop('disabled', true);



    // Se não há itens, adiciona o primeiro
    if ($('#order-items-container').find('.order-item-row').length === 0) {
        addNewItem();
    }

    // Ouvinte para Adicionar Item
    $('#add-item-btn').on('click', addNewItem);

    // Ouvinte para Remover Item (usa event delegation)
    $('#order-items-container').on('click', '.remove-item-btn', function () {
        $(this).closest('.order-item-row').remove();
        updateItemIndexes();
    });




    // 1. Ouvinte para alteração de Produto (apenas 'change')
    $('#order-items-container').on('change', '.item-product-select', function () {
        var $row = $(this).closest('.order-item-row');
        var productId = $row.find('.item-product-select').val();

        updatePriceDisplay($row, productId);
        calculateGrandTotal();
    });

    // 2. Ouvinte específico para Quantidade (com 'input' e 'change')
    $('#order-items-container').on('input change', '.item-quantity-input', function () {
        var $row = $(this).closest('.order-item-row');
        // Precisamos do ID do produto para recalcular o total do ITEM
        var productId = $row.find('.item-product-select').val();

        updatePriceDisplay($row, productId);
        calculateGrandTotal();
    });


    // 3. Lógica de Filtro de Vendedor (existente)
    $('#DepartmentId').change(function () {
        var selectedDepartmentId = $(this).val();
        var sellerSelect = $('#SellerId');

        sellerSelect.empty();
        sellerSelect.append('<option value="">-- Carregando Vendedores --</option>');

        if (selectedDepartmentId) {
            $.ajax({
                url: '/Sellers/GetSellersByDepartment',
                type: 'GET',
                data: { departmentId: selectedDepartmentId },
                success: function (sellers) {
                    sellerSelect.empty();
                    sellerSelect.append('<option value="">-- Select a seller --</option>');

                    $.each(sellers, function (index, seller) {
                        sellerSelect.append('<option value="' + seller.id + '">' + seller.userFullName + '</option>');
                    });
                },
                error: function () {
                    sellerSelect.empty();
                    sellerSelect.append('<option value="">Erro ao carregar vendedores</option>');
                }
            });
        } else {
            sellerSelect.empty();
            sellerSelect.append('<option value="">-- Select a seller --</option>');
        }
    });

    // 4. Submissão AJAX (mantida)
    $('#createForm').submit(function (e) {
        e.preventDefault();

        // Validação mínima de itens
        if ($('#order-items-container').find('.order-item-row').length === 0) {
            alert("A venda deve conter pelo menos um item.");
            return;
        }

        var form = $(this);
        var url = '/SalesRecords/Create';
        var data = form.serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                var modal = new bootstrap.Modal(document.getElementById('successModal'));
                modal.show();
                form[0].reset();
                // Limpa os itens dinâmicos após sucesso
                $('#order-items-container').empty();
                updateItemIndexes();
            },
            error: function (xhr) {
                var msg = "Erro ao criar. Verifique os dados e tente novamente.";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = xhr.responseJSON.message;
                }
                alert(msg);
            }
        });
    });

    // 5. Redirecionamento da modal (mantida)
    $('#goToIndexBtn').click(function () {
        window.location.href = '/SalesRecords/Index';
    });
});


// 1. CARREGA E PARSEA OS DADOS JSON
var productsDataElement = document.getElementById('products-data');
if (productsDataElement) {
    // Lê o conteúdo da tag e o transforma em um objeto JavaScript
    window.productsData = JSON.parse(productsDataElement.textContent);
} else {
    window.productsData = [];
}


// 2. CARREGA O HTML DAS OPÇÕES
var optionsHtmlElement = document.getElementById('product-options-html');
if (optionsHtmlElement) {
    // Lê o conteúdo HTML diretamente
    window.productOptionsHtml = optionsHtmlElement.textContent;
} else {
    window.productOptionsHtml = '';
}

// --- FUNÇÕES DE TEMPLATE E DOM (Template HTML Simplificado) ---

// 2. TEMPLATE HTML DO ITEM (DEVE SER DUPLICADO DO SEU ANTIGO EditorTemplate)
function getOrderItemTemplate() {
    var html = `
                <div class="order-item-row p-3 mb-3 border border-secondary rounded">

                <input type="hidden" name="SalesRecord.OrderItems[_INDEX_].Id" value="0" />
                <input type="hidden" name="SalesRecord.OrderItems[_INDEX_].UnitPrice" value="0.00" />
                                       
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <label class="control-label">Product</label>
                            <select name="SalesRecord.OrderItems[_INDEX_].ProductId" class="form-control item-product-select">
                                ${window.productOptionsHtml} 
                            </select>
                     
                            <span data-valmsg-for="SalesRecord.OrderItems[_INDEX_].ProductId" class="text-danger"></span>
                        </div>

                        <div class="col-md-3 form-group">
                            <label class="control-label">Qty</label>
                            <input name="SalesRecord.OrderItems[_INDEX_].Quantity" type="number" value="1" min="1" class="form-control item-quantity-input" />
                            <span data-valmsg-for="SalesRecord.OrderItems[_INDEX_].Quantity" class="text-danger"></span>
                        </div>

                        <div class="col-md-3 form-group align-self-end">
                            <button type="button" class="btn btn-danger w-100 remove-item-btn"><i class="fas fa-trash"></i> Remove</button>
                        </div>
                    </div>
                    <div class="row">
                <div class="col-6">
                    <small>Price: <strong class="item-unit-price-display">0.00</strong></small>
                </div>
                <div class="col-6 text-end">
                    <small>Total: <strong class="item-total-display">0.00</strong></small>
                </div>
            </div>
                </div>
            `;
    return html;
}

// --- FUNÇÕES DE PREÇO E TOTAL (no SalesRecords-Create.js) ---

// REQUISITO 3: Adicionar a função para buscar e exibir o preço unitário
function updatePriceDisplay($row, productId) {
    // A lista de DTOs de produtos (com preço) está disponível globalmente no JS (se você usou DTOs)
    // Se você usou o foreach do Razor, a lista não está no JS.
    // Vamos usar a variável global 'productsData' que contém a lista de DTOs, se você usou a solução DTO.

    var productsData = window.productsData; // Ou buscar de forma limpa como DTO
    // Dentro de updatePriceDisplay:


    var $unitPriceHidden = $row.find('input[name*="UnitPrice"]');
    var $unitPriceDisplay = $row.find('.item-unit-price-display');
    var $quantityInput = $row.find('input[name*="Quantity"]');
    var $itemTotalDisplay = $row.find('.item-total-display');
    var quantity = parseFloat($quantityInput.val() || 0);
    console.log(quantity);
    if (productId && productsData) {
        // Encontra o produto na lista DTOs do JavaScript
        var product = productsData.find(p => p.Id == productId);

        if (product) {
            var unitPrice = parseFloat(product.Price);
            var itemTotal = (unitPrice * quantity).toFixed(2);

            // Atualiza os campos
            $unitPriceHidden.val(unitPrice.toFixed(2));
            $unitPriceDisplay.text(unitPrice.toFixed(2));
            $itemTotalDisplay.text(itemTotal);
        } else {
            // Produto não encontrado (deve ser 0 para segurança)
            $unitPriceHidden.val('0.00');
            $unitPriceDisplay.text('0.00');
            $itemTotalDisplay.text('0.00');
        }
    } else {
        // Produto não selecionado
        $unitPriceHidden.val('0.00');
        $unitPriceDisplay.text('0.00');
        $itemTotalDisplay.text('0.00');
    }
}

// REQUISITO 4: Função para calcular o total
function calculateGrandTotal() {
    var totalAmount = 0.00;

    $('#order-items-container').find('.order-item-row').each(function () {
        var quantity = parseFloat($(this).find('input[name*="Quantity"]').val() || 0);
        var unitPrice = parseFloat($(this).find('input[name*="UnitPrice"]').val() || 0);

        totalAmount += (quantity * unitPrice);
    });
    console.log(totalAmount);

    // Atualiza o display do total
    $('#grand-total-display').text(totalAmount.toFixed(2));
}