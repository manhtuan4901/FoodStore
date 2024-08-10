$(document).ready(function () {
    function updateCart() {
        var quantities = [];
        $('.num-product').each(function () {
            quantities.push($(this).val());
        });

        console.log("Quantities sent to server:", quantities); // Log quantities

        $.ajax({
            url: '/Cart/UpdateCart',
            type: 'POST',
            data: { quantities: quantities },
            success: function (response) {
                console.log("AJAX response:", response); // Log response for debugging
                // Update the total amount in the view
                $('.mtext-110.cl2').text(response.totalAmount + " VND");
                // Optionally, update individual item totals if needed
                $('.table_row .column-5').each(function (index) {
                    $(this).text(response.individualTotals[index] + " VND");
                });
            },
            error: function (error) {
                console.log("Error updating cart: ", error);
            }
        });
    }

    // Event listener for input change
    $('.num-product').on('change', function () {
        updateCart();
    });

    // Event listeners for the + and - buttons
    $('.btn-num-product-down').on('click', function () {
            updateCart();
        
    });

    $('.btn-num-product-up').on('click', function () {
        updateCart();
    });


});
