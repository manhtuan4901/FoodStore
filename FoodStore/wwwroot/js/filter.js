document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.filter-tope-group button').forEach(function (button) {
        button.addEventListener('click', function () {
            var categoryId = this.getAttribute('data-filter');
            fetch(`/Product/FilterProducts?categoryId=${categoryId}`)
                .then(response => response.json())
                .then(data => {
                    updateProductDisplay(data);
                });
        });
    });
});

function updateProductDisplay(products) {
    var container = document.querySelector('.row.isotope-grid');
    container.innerHTML = '';
    products.forEach(product => {
        container.innerHTML += `
            <div class="col-sm-6 col-md-4 col-lg-3 p-b-35 isotope-item">
                <div class="block2">
                    <div class="block2-pic hov-img0">
                        <img src="${product.images}" alt="IMG-PRODUCT">
                    </div>
                    <div class="block2-txt flex-w flex-t p-t-14">
                        <div class="block2-txt-child1 flex-col-l">
                            <a href="/product/detail/${product.id}" class="stext-104 cl4 hov-cl1 trans-04 js-name-b2 p-b-6">${product.name}</a>
                            <span class="stext-105 cl3">${product.price}</span>
                        </div>
                    </div>
                </div>
            </div>
        `;
    });
}

var currentPage = 1;
var initialPageSize = 8;
$('.js-load-more').click(function (e) {
    e.preventDefault();
    currentPage++;
    $.ajax({
        url: '/product/LoadMoreProducts/',
        type: 'GET',
        data: { currentPage: currentPage },
        success: function (data) {
            $('.isotope-grid').append(data);
            if (currentPage * 3 >= initialPageSize) {
                $('.js-load-more').hide();
            }
        }
    });
});